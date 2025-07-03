using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using GyG.Datos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GyG.Presentacion
{

    public partial class PedidosForm : Form
    {

        public PedidosForm()
        {
            InitializeComponent();
            CargarProveedores();
            ConfigurarGridPedidos();
            CargarHistorialPedidos();
            dgvPedidos.CellClick += dgvPedidos_CellClick;
            dgvPedidos.CellDoubleClick += dgvPedidos_CellDoubleClick;


        }

        private void CargarProveedores()
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT id, nombre FROM proveedor", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Bind a combobox, se recomienda Resetear DataSource antes para evitar problemas
                        cmbProveedores.DataSource = null;
                        cmbProveedores.DataSource = dt;
                        cmbProveedores.DisplayMember = "nombre";
                        cmbProveedores.ValueMember = "id";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
            }
        }


        private void AbrirPDFPedido(int idPedido)
        {
            string nombreArchivo = $"pedido_{idPedido}.pdf";

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string query =
                        "SELECT contenido FROM archivo_pdf WHERE tipo = 'pedido' AND nombre_archivo = @nombre";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreArchivo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] pdfBytes = (byte[])reader["contenido"];
                                string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), nombreArchivo);
                                System.IO.File.WriteAllBytes(tempPath, pdfBytes);
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath)
                                    { UseShellExecute = true });
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el archivo PDF asociado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir PDF: " + ex.Message);
            }
        }

        private void dgvPedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idPedido = Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells["id"].Value);

                try
                {
                    using (var conn = Conexion.ObtenerConexion())
                    {
                        string query = @"
                    SELECT contenido, nombre_archivo 
                    FROM archivo_pdf 
                    WHERE tipo = 'pedido' AND nombre_archivo LIKE @nombre";

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", $"pedido_{idPedido}%");

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    byte[] contenido = (byte[])reader["contenido"];
                                    string nombre = reader["nombre_archivo"].ToString();
                                    string rutaTemp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), nombre);

                                    System.IO.File.WriteAllBytes(rutaTemp, contenido);
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                    {
                                        FileName = rutaTemp,
                                        UseShellExecute = true
                                    });
                                }
                                else
                                {
                                    MessageBox.Show("No se encontró el archivo PDF para este pedido.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir el PDF: " + ex.Message);
                }
            }
        }


   private void ActualizarEstadoPedido(int idPedido)
{
    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                // 1. Actualizar estado y fecha del pedido
                string updatePedidoQuery = @"
                    UPDATE pedido 
                    SET estado = 'recibido', fecha_recibido = CURRENT_TIMESTAMP 
                    WHERE id = @id";
                using (var cmdUpdate = new NpgsqlCommand(updatePedidoQuery, conn, transaction))
                {
                    cmdUpdate.Parameters.AddWithValue("@id", idPedido);
                    cmdUpdate.ExecuteNonQuery();
                }

                // 2. Leer todos los productos del pedido en memoria
                DataTable detalles = new DataTable();
                string detalleQuery = @"
                    SELECT id_producto, cantidad 
                    FROM pedido_detalle 
                    WHERE id_pedido = @id";
                using (var cmdDetalle = new NpgsqlCommand(detalleQuery, conn, transaction))
                {
                    cmdDetalle.Parameters.AddWithValue("@id", idPedido);
                    using (var reader = cmdDetalle.ExecuteReader())
                    {
                        detalles.Load(reader); // ← Lee todo en memoria y cierra reader
                    }
                }

                // 3. Actualizar stock por cada producto
                foreach (DataRow row in detalles.Rows)
                {
                    int idProducto = Convert.ToInt32(row["id_producto"]);
                    int cantidad = Convert.ToInt32(row["cantidad"]);

                    string updateStockQuery = @"
                        UPDATE producto 
                        SET stock = stock + @cantidad 
                        WHERE id = @id_producto";
                    using (var cmdStock = new NpgsqlCommand(updateStockQuery, conn, transaction))
                    {
                        cmdStock.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdStock.Parameters.AddWithValue("@id_producto", idProducto);
                        cmdStock.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        MessageBox.Show("Pedido actualizado a 'recibido' y stock actualizado.");
        CargarHistorialPedidos(); // Recarga el grid actualizado
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al actualizar el pedido y el stock: " + ex.Message);
    }
}

        
        
private void GenerarPDFPedido(int idPedido, DataTable productos, string nombreProveedor, NpgsqlConnection conn)
{
    string nombreArchivo = $"pedido_{idPedido}.pdf";
    string rutaTemp = Path.Combine(Path.GetTempPath(), nombreArchivo);

    Document doc = new Document(PageSize.A4);
    using (var fs = new FileStream(rutaTemp, FileMode.Create, FileAccess.Write, FileShare.None))
    {
        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
        doc.Open();

        doc.Add(new Paragraph("FERRETERÍA G Y G"));
        doc.Add(new Paragraph($"Pedido N°: {idPedido}"));
        doc.Add(new Paragraph($"Fecha: {DateTime.Now.ToShortDateString()}"));
        doc.Add(new Paragraph($"Proveedor: {nombreProveedor}"));
        doc.Add(new Paragraph(" "));

        PdfPTable table = new PdfPTable(5);
        table.WidthPercentage = 100;
        table.AddCell("Producto");
        table.AddCell("Descripción");
        table.AddCell("Cantidad");
        table.AddCell("Precio Compra");
        table.AddCell("Precio Venta");

        foreach (DataRow row in productos.Rows)
        {
            table.AddCell(row["nombre"].ToString());
            table.AddCell(row["descripcion"].ToString());
            table.AddCell(row["cantidad"].ToString());
            table.AddCell(Convert.ToDecimal(row["precio_compra"]).ToString("C2"));
            table.AddCell(Convert.ToDecimal(row["precio_venta"]).ToString("C2"));
        }

        doc.Add(table);

        doc.Add(new Paragraph(" "));
        doc.Add(new Paragraph("__________________________"));
        doc.Add(new Paragraph("Firma de Recepción"));
        doc.Close();
    }

    // Guardar en base de datos
    byte[] pdfBytes = File.ReadAllBytes(rutaTemp);

    string query = @"
        INSERT INTO archivo_pdf (tipo, nombre_archivo, contenido)
        VALUES ('pedido', @nombre, @contenido)";
    using (var cmd = new NpgsqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@nombre", nombreArchivo);
        cmd.Parameters.AddWithValue("@contenido", pdfBytes);
        cmd.ExecuteNonQuery();
    }
}
      
        
        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Ver PDF
                if (dgvPedidos.Columns[e.ColumnIndex].Name == "verPDF")
                {
                    int idPedido = Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells["id"].Value);
                    AbrirPDFPedido(idPedido);
                }

                // Actualizar Estado del Pedido
                if (dgvPedidos.Columns[e.ColumnIndex].Name == "actualizarEstado")
                {
                    int idPedido = Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells["id"].Value);
                    DialogResult result = MessageBox.Show("¿Deseas marcar este pedido como recibido y actualizar el stock?", 
                        "Confirmar recepción", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        ActualizarEstadoPedido(idPedido);
                    }
                }
            }
        }



        private void CargarProductos(int umbral)
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    string query = @"
                SELECT id, nombre, descripcion, stock, precio_inventario
                FROM producto
                WHERE stock <= @umbral
                AND id NOT IN (
                    SELECT id_producto
                    FROM pedido_detalle pd
                    JOIN pedido p ON pd.id_pedido = p.id
                    WHERE p.estado = 'solicitado'
                )
                ORDER BY stock ASC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@umbral", umbral);
                        DataTable dt = new DataTable();
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        dgvProductos.DataSource = null;
                        dgvProductos.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }


        private void btnManejarProveedores_Click(object sender, EventArgs e)
        {
            var form = new ProveedoresForm();
            form.ShowDialog();

            // Recarga proveedores después de cerrar el formulario para actualizar la lista
            CargarProveedores();
        }


        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            int umbral;

            if (!int.TryParse(txtUmbral.Text.Trim(), out umbral) || umbral < 0)
            {
                MessageBox.Show("Por favor ingresa un valor numérico válido para el umbral.", "Valor inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    string query = @"
            SELECT id, nombre, descripcion, stock, precio_inventario
            FROM producto
            WHERE stock <= @umbral AND id NOT IN (
                SELECT id_producto
                FROM pedido_detalle pd
                JOIN pedido p ON pd.id_pedido = p.id
                WHERE p.estado = 'solicitado'
            )
            ORDER BY stock ASC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@umbral", umbral);
                        DataTable dt = new DataTable();
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        dgvProductos.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar productos: " + ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void CargarHistorialPedidos()
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string query = @"
                SELECT 
                    p.id, 
                    pr.nombre AS proveedor, 
                    p.estado, 
                    p.fecha_solicitud,
                    COALESCE(SUM(pd.precio_compra * pd.cantidad), 0) AS total_precio_compra,
                    COALESCE(SUM(pd.precio_venta * pd.cantidad), 0) AS total_precio_venta
                FROM pedido p
                JOIN proveedor pr ON p.id_proveedor = pr.id
                LEFT JOIN pedido_detalle pd ON pd.id_pedido = p.id
                GROUP BY p.id, pr.nombre, p.estado, p.fecha_solicitud
                ORDER BY 
                    CASE WHEN p.estado = 'solicitado' THEN 0 ELSE 1 END,
                    p.fecha_solicitud DESC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvPedidos.DataSource = dt;

                        if (!dgvPedidos.Columns.Contains("actualizarEstado"))
                        {
                            var colActualizar = new DataGridViewButtonColumn
                            {
                                Name = "actualizarEstado",
                                HeaderText = "Actualizar Estado",
                                Text = "Recibir",
                                UseColumnTextForButtonValue = true,
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                            };
                            dgvPedidos.Columns.Add(colActualizar);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar historial de pedidos: " + ex.Message);
            }
        }

        private void ConfigurarGridPedidos()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.Columns.Clear();

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                DataPropertyName = "id",
                HeaderText = "ID",
                Visible = true
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "proveedor",
                DataPropertyName = "proveedor",
                HeaderText = "Proveedor",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "estado",
                DataPropertyName = "estado",
                HeaderText = "Estado",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "fecha_solicitud",
                DataPropertyName = "fecha_solicitud",
                HeaderText = "Fecha",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "total_precio_compra",
                DataPropertyName = "total_precio_compra",
                HeaderText = "Precio Compra",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } // formato moneda
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "total_precio_venta",
                DataPropertyName = "total_precio_venta",
                HeaderText = "Precio Venta",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } // formato moneda
            });
        }


        private void btnBuscarProductos_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUmbral.Text.Trim(), out int umbral))
            {
                MessageBox.Show("Por favor ingresa un número válido para el umbral.");
                return; // Sale si no es válido
            }

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT id, nombre, descripcion, stock, precio_inventario
                        FROM producto
                        WHERE stock <= @umbral
                        AND id NOT IN (
                            SELECT id_producto
                            FROM pedido_detalle pd
                            JOIN pedido p ON pd.id_pedido = p.id
                            WHERE p.estado = 'solicitado'
                        )", conn))
                    {
                        cmd.Parameters.AddWithValue("@umbral", umbral);
                        DataTable dt = new DataTable();
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        dgvProductos.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message);
            }
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            var form = new AgregarProveedorForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarProveedores();
            }
        }

       private void btnGenerarPedido_Click(object sender, EventArgs e)
{
    // Validar que al menos un producto esté seleccionado
    if (dgvProductos.SelectedRows.Count == 0)
    {
        MessageBox.Show("Seleccione al menos un producto para generar el pedido.");
        return;
    }

    // Validar proveedor
    if (cmbProveedores.SelectedValue == null)
    {
        MessageBox.Show("Seleccione un proveedor.");
        return;
    }

    int idProveedor = (int)cmbProveedores.SelectedValue;

    // Crear tabla temporal con columnas necesarias
    DataTable productosSeleccionados = new DataTable();
    productosSeleccionados.Columns.Add("id", typeof(int));
    productosSeleccionados.Columns.Add("nombre", typeof(string));
    productosSeleccionados.Columns.Add("descripcion", typeof(string));
    productosSeleccionados.Columns.Add("stock", typeof(int));
    productosSeleccionados.Columns.Add("precio_inventario", typeof(decimal));
    productosSeleccionados.Columns.Add("cantidad", typeof(int));
    productosSeleccionados.Columns.Add("precio_compra", typeof(decimal));
    productosSeleccionados.Columns.Add("precio_venta", typeof(decimal));

    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                int idPedido;

                // Insertar pedido principal
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO pedido (id_proveedor, estado) VALUES (@prov, 'solicitado') RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("@prov", idProveedor);
                    idPedido = (int)cmd.ExecuteScalar();
                }

                // Insertar detalles y construir la tabla para PDF
                foreach (DataGridViewRow row in dgvProductos.SelectedRows)
                {
                    if (row.DataBoundItem is DataRowView drv)
                    {
                        int idProducto = (int)drv["id"];
                        string nombre = drv["nombre"].ToString();
                        string descripcion = drv["descripcion"].ToString();
                        int stock = Convert.ToInt32(drv["stock"]);
                        decimal precioInventario = Convert.ToDecimal(drv["precio_inventario"]);

                        int cantidad = PedirCantidad(nombre);
                        decimal precioCompra = precioInventario;
                        decimal precioVenta = precioCompra * 1.25m;

                        // Insertar detalle
                        using (var cmd = new NpgsqlCommand(@"
                            INSERT INTO pedido_detalle (id_pedido, id_producto, cantidad, precio_compra, precio_venta)
                            VALUES (@id_pedido, @id_producto, @cantidad, @precio_compra, @precio_venta)", conn))
                        {
                            cmd.Parameters.AddWithValue("@id_pedido", idPedido);
                            cmd.Parameters.AddWithValue("@id_producto", idProducto);
                            cmd.Parameters.AddWithValue("@cantidad", cantidad);
                            cmd.Parameters.AddWithValue("@precio_compra", precioCompra);
                            cmd.Parameters.AddWithValue("@precio_venta", precioVenta);
                            cmd.ExecuteNonQuery();
                        }

                        // Agregar fila para el PDF
                        DataRow nueva = productosSeleccionados.NewRow();
                        nueva["id"] = idProducto;
                        nueva["nombre"] = nombre;
                        nueva["descripcion"] = descripcion;
                        nueva["stock"] = stock;
                        nueva["precio_inventario"] = precioInventario;
                        nueva["cantidad"] = cantidad;
                        nueva["precio_compra"] = precioCompra;
                        nueva["precio_venta"] = precioVenta;
                        productosSeleccionados.Rows.Add(nueva);
                    }
                }

                transaction.Commit();

                // Generar PDF y guardar
                GenerarPDFPedido(idPedido, productosSeleccionados, cmbProveedores.Text, conn);
                MessageBox.Show("Pedido generado correctamente.");
                CargarHistorialPedidos();
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al generar el pedido: " + ex.Message);
    }
}

        private int PedirCantidad(string nombreProducto)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Ingrese la cantidad para el producto '{nombreProducto}':", 
                "Cantidad Producto", "1");

            if (int.TryParse(input, out int cantidad) && cantidad > 0)
            {
                return cantidad;
            }
            else
            {
                MessageBox.Show("Cantidad inválida. Se usará cantidad 1 por defecto.");
                return 1;
            }
        }

    }
}
