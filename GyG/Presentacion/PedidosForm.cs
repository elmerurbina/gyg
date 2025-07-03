using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using GyG.Datos;

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
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        // 1. Actualizar estado y fecha del pedido
                        string updatePedidoQuery = @"
                    UPDATE pedido 
                    SET estado = 'recibido', fecha_recibido = CURRENT_TIMESTAMP 
                    WHERE id = @id";
                        using (var cmdUpdate = new NpgsqlCommand(updatePedidoQuery, conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@id", idPedido);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 2. Obtener productos y cantidades del pedido
                        string detalleQuery = @"
                    SELECT id_producto, cantidad 
                    FROM pedido_detalle 
                    WHERE id_pedido = @id";
                        using (var cmdDetalle = new NpgsqlCommand(detalleQuery, conn))
                        {
                            cmdDetalle.Parameters.AddWithValue("@id", idPedido);
                            using (var reader = cmdDetalle.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int idProducto = reader.GetInt32(0);
                                    int cantidad = reader.GetInt32(1);

                                    // 3. Sumar cantidad al stock
                                    string updateStockQuery = @"
                                UPDATE producto 
                                SET stock = stock + @cantidad 
                                WHERE id = @id_producto";
                                    using (var cmdStock = new NpgsqlCommand(updateStockQuery, conn))
                                    {
                                        cmdStock.Parameters.AddWithValue("@cantidad", cantidad);
                                        cmdStock.Parameters.AddWithValue("@id_producto", idProducto);
                                        cmdStock.ExecuteNonQuery();
                                    }
                                }
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

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPedidos.Columns[e.ColumnIndex].Name == "verPDF")
            {
                int idPedido = Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells["id"].Value);
                AbrirPDFPedido(idPedido);
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
            SELECT id, nombre, descripcion, stock
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
                    p.fecha_solicitud
                FROM pedido p
                JOIN proveedor pr ON p.id_proveedor = pr.id
                ORDER BY 
                    CASE WHEN p.estado = 'solicitado' THEN 0 ELSE 1 END,
                    p.fecha_solicitud DESC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvPedidos.DataSource = dt;

                        // Evita duplicar columnas si ya existen
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
                Visible = false
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "fecha_creacion",
                DataPropertyName = "fecha_creacion",
                HeaderText = "Fecha",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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

            // Validar que haya un proveedor seleccionado
            if (cmbProveedores.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            int idProveedor = (int)cmbProveedores.SelectedValue;

            // Crear una tabla temporal para guardar los productos seleccionados
            DataTable productosSeleccionados = ((DataTable)dgvProductos.DataSource).Clone();

            // Copiar cada fila seleccionada a la tabla temporal
            foreach (DataGridViewRow row in dgvProductos.SelectedRows)
            {
                if (row.DataBoundItem is DataRowView drv)
                {
                    productosSeleccionados.ImportRow(drv.Row);
                }
            }

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        // Insertar el pedido
                        int idPedido;
                        using (var cmd = new NpgsqlCommand(
                                   "INSERT INTO pedido (id_proveedor, estado) VALUES (@prov, 'solicitado') RETURNING id",
                                   conn))
                        {
                            cmd.Parameters.AddWithValue("@prov", idProveedor);
                            idPedido = (int)cmd.ExecuteScalar();
                        }

                        // Insertar los detalles del pedido para cada producto seleccionado
                        foreach (DataRow producto in productosSeleccionados.Rows)
                        {
                            using (var cmd = new NpgsqlCommand(@"
                        INSERT INTO pedido_detalle (id_pedido, id_producto, cantidad, precio_compra, precio_venta)
                        VALUES (@id_pedido, @id_producto, @cantidad, @precio_compra, @precio_venta)", conn))
                            {
                                cmd.Parameters.AddWithValue("@id_pedido", idPedido);
                                cmd.Parameters.AddWithValue("@id_producto", (int)producto["id"]);
                                cmd.Parameters.AddWithValue("@cantidad",
                                    10); // Aquí puedes cambiar la cantidad o pedirla al usuario
                                cmd.Parameters.AddWithValue("@precio_compra", (decimal)producto["precio_inventario"]);
                                cmd.Parameters.AddWithValue("@precio_venta",
                                    (decimal)producto["precio_inventario"] * 1.25m);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Pedido generado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el pedido: " + ex.Message);
            }
        }
    }
}
