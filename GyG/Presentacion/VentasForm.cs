using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using GyG.Datos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextDocument = iTextSharp.text.Document;
using iTextParagraph = iTextSharp.text.Paragraph;
using iTextFontFactory = iTextSharp.text.FontFactory;
using iTextPdfWriter = iTextSharp.text.pdf.PdfWriter;
using iTextPdfPTable = iTextSharp.text.pdf.PdfPTable;
using iTextPageSize = iTextSharp.text.PageSize;
using iTextElement = iTextSharp.text.Element;
using iTextRectangle = iTextSharp.text.Rectangle;
using Npgsql;
using Document = System.Reflection.Metadata.Document;
using Font = System.Drawing.Font;

namespace GyG.Presentacion
{
    public partial class VentasForm : Form
    {
        

        private List<ProductoCarrito> carrito = new List<ProductoCarrito>();
        private Label lblItemsCarrito;
        private Label lblInfoEditarCantidad;
        private Label lblStockProducto;
      

        
        public VentasForm()
        {
           
            InitializeComponent();
            
            txtNombreCliente.Leave += txtNombreCliente_Leave;
            txtNombreCliente.KeyDown += TxtNombreCliente_KeyDown;
           

            
            lblItemsCarrito = new Label();
            lblItemsCarrito.AutoSize = true;
            lblItemsCarrito.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblItemsCarrito.ForeColor = Color.Green;
            lblItemsCarrito.Location = new Point(10, dgvCarrito.Top - 30); // Ajusta la posición

            // Crear lblInfoEditarCantidad
            lblInfoEditarCantidad = new Label();
            lblInfoEditarCantidad.AutoSize = true;
            lblInfoEditarCantidad.ForeColor = Color.DarkBlue;
            lblInfoEditarCantidad.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblInfoEditarCantidad.Text = "💡 Para editar la cantidad: doble clic en la celda, cambie el número y presione ENTER.";
            lblInfoEditarCantidad.Location = new Point(120, dgvCarrito.Top - 30); 

            lblStockProducto = new Label();
            lblStockProducto.AutoSize = true;
            lblStockProducto.ForeColor = Color.DarkRed;
            lblStockProducto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStockProducto.Location = new Point(120, 5); 
            
            
            Button btnEditarCliente = new Button();
            btnEditarCliente.Text = "Modificar información del cliente";
            btnEditarCliente.Location = new Point(420, 1);
            btnEditarCliente.BackColor = Color.LightGoldenrodYellow;
            btnEditarCliente.Width = 220;
            btnEditarCliente.Height = 28;
            btnEditarCliente.Click += BtnEditarCliente_Click;

            this.Controls.Add(btnEditarCliente);

            
            // Agregar los labels al formulario
            this.Controls.Add(lblItemsCarrito);
            this.Controls.Add(lblInfoEditarCantidad);
            this.Controls.Add(lblStockProducto);
            
            CargarProductos();
            CargarClientes();
            ConfigurarColumnasCarrito();
            numCantidad.ValueChanged += (s, e) => CalcularPrecioFinal();
            numIVA.ValueChanged += (s, e) => CalcularPrecioFinal();
            numDescuento.ValueChanged += (s, e) => CalcularPrecioFinal();

        }
        
        
        private void BtnHistorialFacturas_Click(object sender, EventArgs e)
        {
            HistorialFacturasForm historialForm = new HistorialFacturasForm();
            historialForm.ShowDialog(this);
        }


        private void CargarProductos()
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT * FROM sp_get_productos_en_ventas();", conn))

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cbProductos.Items.Add(new Producto
                    {
                        Id = reader.GetInt32(0), // id_producto
                        Nombre = reader.GetString(1), // nombre
                        Descripcion = reader.GetString(2), // descripcion
                        Categoria = reader.IsDBNull(3) ? null : reader.GetString(3), // categoria
                        PrecioVenta = reader.GetDecimal(4), // precio_venta
                        Stock = reader.GetInt32(5), // stock
                        IVA = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                        Descuento = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7)
                    });
                }
            }

            cbProductos.DisplayMember = "Nombre";
            cbProductos.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbProductos.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void CargarClientes()
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT * FROM sp_get_clientes();", conn))
            using (var reader = cmd.ExecuteReader())
            {
                AutoCompleteStringCollection nombres = new AutoCompleteStringCollection();
                AutoCompleteStringCollection telefonos = new AutoCompleteStringCollection();

                while (reader.Read())
                {
                    nombres.Add(reader.GetString(1));    // Nombre
                    telefonos.Add(reader.GetString(2));  // Teléfono
                }

                txtNombreCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtNombreCliente.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtNombreCliente.AutoCompleteCustomSource = nombres;

                txtTelefono.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtTelefono.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtTelefono.AutoCompleteCustomSource = telefonos;
            }
        }

        private void TxtNombreCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Llamar la función que rellena teléfono y ubicación
                txtNombreCliente_Leave(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true; // Evitar sonido 'ding'
            }
        }
        
        private void BtnEditarCliente_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Debe seleccionar un cliente existente antes de modificar.", "Advertencia");
                return;
            }

            var form = new ClienteForm(txtNombreCliente.Text, txtTelefono.Text, txtUbicacion.Text);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Recargar datos si es necesario
                CargarClientes();
                txtNombreCliente_Leave(null, null); // Actualiza campos si hubo cambios
            }
        }



        private void cbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProductos.SelectedItem is Producto producto)
            {
                txtDescripcion.Text = producto.Descripcion;
                txtPrecio.Text = producto.PrecioVenta.ToString("F2");

                numIVA.Value = producto.IVA;
                numDescuento.Value = producto.Descuento;

                lblStockProducto.Text = $"Stock disponible: {producto.Stock}";

                CalcularPrecioFinal();
            }
            else
            {
                txtDescripcion.Clear();
                txtPrecio.Clear();
                numIVA.Value = 0;
                numDescuento.Value = 0;
                txtSubtotal.Clear();
            }
        }


        private void CalcularPrecioFinal()
        {
            if (decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                decimal iva = precio * (numIVA.Value / 100);
                decimal descuento = precio * (numDescuento.Value / 100);
                decimal precioFinal = precio + iva - descuento;
                txtSubtotal.Text = precioFinal.ToString("C2");
            }
            else
            {
                txtSubtotal.Clear();
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cbProductos.SelectedItem is Producto producto)
            {
                if (numCantidad.Value <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "Validación");
                    return;
                }

                if (numCantidad.Value > producto.Stock)
                {
                    MessageBox.Show("No hay suficiente stock para este producto.", "Stock insuficiente");
                    return;
                }

                // Aquí va el bloque para verificar si ya existe y actualizar o agregar nuevo
                var existente = carrito.FirstOrDefault(c => c.Id == producto.Id);
                if (existente != null)
                {
                    existente.Cantidad += (int)numCantidad.Value;
                    existente.IVA = numIVA.Value;
                    existente.Descuento = numDescuento.Value;
                }
                else
                {
                    carrito.Add(new ProductoCarrito
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Cantidad = (int)numCantidad.Value,
                        PrecioUnitario = producto.PrecioVenta,
                        IVA = numIVA.Value,
                        Descuento = numDescuento.Value
                    });
                }

                RefrescarCarrito();
                LimpiarProductoSeleccionado();
            }
        }


        
        
        private void txtNombreCliente_Leave(object sender, EventArgs e)
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT telefono, ubicacion FROM cliente WHERE nombre = @nombre", conn))
            {
                cmd.Parameters.AddWithValue("nombre", txtNombreCliente.Text.Trim());

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtTelefono.Text = reader.GetString(0);
                        txtUbicacion.Text = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    }
                }
            }
        }


        // Cuando cambie valor en una celda editable (cantidad)
        private void DgvCarrito_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = dgvCarrito;
            if (grid.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                var nombre = grid.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                if (int.TryParse(grid.Rows[e.RowIndex].Cells["Cantidad"].Value?.ToString(), out int nuevaCantidad))
                {
                    if (nuevaCantidad <= 0)
                    {
                        MessageBox.Show("La cantidad debe ser mayor a 0.", "Validación");
                        RefrescarCarrito();
                        return;
                    }

                    var producto = carrito.FirstOrDefault(c => c.Nombre == nombre);
                    if (producto != null)
                    {
                        producto.Cantidad = nuevaCantidad;
                        RefrescarCarrito();
                    }
                }
                else
                {
                    MessageBox.Show("Cantidad inválida.", "Error");
                    RefrescarCarrito();
                }
            }
        }

// Para eliminar producto con botón "X"
        private void DgvCarrito_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvCarrito.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                string nombre = dgvCarrito.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"¿Está seguro que desea eliminar \"{nombre}\" del carrito?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var producto = carrito.FirstOrDefault(c => c.Nombre == nombre);
                    if (producto != null)
                    {
                        carrito.Remove(producto);
                        RefrescarCarrito();
                    }
                }
            }
        }


        private void RefrescarCarrito()
        {
            dgvCarrito.CellValueChanged -= DgvCarrito_CellValueChanged;
            dgvCarrito.CellContentClick -= DgvCarrito_CellContentClick;
            dgvCarrito.CellEndEdit -= DgvCarrito_CellEndEdit;

            dgvCarrito.Rows.Clear();

            foreach (var c in carrito)
            {
                dgvCarrito.Rows.Add(c.Id, c.Nombre, c.Descripcion ?? "", 
                    c.PrecioUnitario.ToString("C2"),
                    c.IVA, c.Descuento, c.Cantidad,
                    c.Subtotal.ToString("C2"));
            }

            lblTotal.Text = "Total: " + carrito.Sum(c => c.Subtotal).ToString("C2");

            foreach (DataGridViewRow row in dgvCarrito.Rows)
            {
                var cell = row.Cells["Eliminar"] as DataGridViewButtonCell;
                if (cell != null)
                {
                    cell.Style.BackColor = Color.Red;
                    cell.Style.ForeColor = Color.White;
                    cell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }

            
            lblItemsCarrito.Text = $"🛒 {carrito.Sum(c => c.Cantidad)} item(s)";


            
            dgvCarrito.CellValueChanged += DgvCarrito_CellValueChanged;
            dgvCarrito.CellContentClick += DgvCarrito_CellContentClick;
            dgvCarrito.CellEndEdit += DgvCarrito_CellEndEdit;
        }

        private void ConfigurarColumnasCarrito()
        {
            dgvCarrito.Columns.Clear();
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.EditMode = DataGridViewEditMode.EditOnEnter;

            dgvCarrito.Columns.Add("Id", "Id");
            dgvCarrito.Columns["Id"].Visible = false;

            dgvCarrito.Columns.Add("Nombre", "Nombre");
            dgvCarrito.Columns.Add("Descripcion", "Descripción");
            dgvCarrito.Columns.Add("Precio", "Precio");
            dgvCarrito.Columns.Add("IVA", "IVA");
            dgvCarrito.Columns.Add("Descuento", "Descuento");

            var cantidadCol = new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "Cantidad"
            };
            dgvCarrito.Columns.Add(cantidadCol);

            dgvCarrito.Columns.Add("Subtotal", "Subtotal");

            var eliminarBtn = new DataGridViewButtonColumn
            {
                Name = "Eliminar",
                HeaderText = "Acciones",
                Text = "X",
                UseColumnTextForButtonValue = true
            };
            dgvCarrito.Columns.Add(eliminarBtn);
        }




        private void DgvCarrito_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DgvCarrito_CellValueChanged(sender, e);
        }


        private void LimpiarProductoSeleccionado()
        {
            cbProductos.SelectedIndex = -1;
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtSubtotal.Clear();
            numCantidad.Value = 1;
            numIVA.Value = 0;
            numDescuento.Value = 0;
        }

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {
            RegistrarVenta("contado");
        }

        private void btnCredito_Click(object sender, EventArgs e)
        {
            RegistrarVenta("credito");
        }

        private void RegistrarVenta(string estadoPago)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("Debe agregar productos al carrito.", "Validación");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCliente.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Debe ingresar nombre y teléfono del cliente.", "Validación");
                return;
            }

            int idCliente = ObtenerORegistrarCliente(
                txtNombreCliente.Text.Trim(),
                txtTelefono.Text.Trim(),
                txtUbicacion.Text.Trim());

            decimal total = carrito.Sum(c => c.Subtotal);
            decimal descuento = carrito.Sum(c => c.PrecioUnitario * (c.Descuento / 100));

            // Serializar carrito en formato esperado por el SP
            var detallesJson = JsonSerializer.Serialize(
                carrito.Select(c => new
                {
                    id = c.Id,
                    cantidad = c.Cantidad,
                    precioUnitario = c.PrecioUnitario,
                    IVA = c.IVA,
                    Descuento = c.Descuento
                })
            );

            int idFacturaGenerada;

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand(
                       "SELECT sp_insert_factura(@id_cliente, @estado_pago, @total, @descuento, @detalles::json);", conn))
            {
                cmd.Parameters.AddWithValue("id_cliente", idCliente);
                cmd.Parameters.AddWithValue("estado_pago", estadoPago);
                cmd.Parameters.AddWithValue("total", total);
                cmd.Parameters.AddWithValue("descuento", descuento);
                cmd.Parameters.AddWithValue("detalles", detallesJson);

                // Obtener el ID de la factura generada
                idFacturaGenerada = Convert.ToInt32(cmd.ExecuteScalar());
                // Después de registrar la factura...
                foreach (var item in carrito)
                {
                    using (var cmdStock = new NpgsqlCommand(
                               "UPDATE producto SET stock = stock - @cantidad WHERE id = @id_producto;", conn))
                    {
                        cmdStock.Parameters.AddWithValue("cantidad", item.Cantidad);
                        cmdStock.Parameters.AddWithValue("id_producto", item.Id);
                        cmdStock.ExecuteNonQuery();
                    }
                    int stockActualizado = ObtenerStockProducto(item.Id);
                    ActualizarStockProductoEnCombo(item.Id, stockActualizado);
                }

            }

            MessageBox.Show("Venta registrada exitosamente.", "Venta registrada");

   
            

            
            // Generar y guardar factura en PDF
            using (var conn = Conexion.ObtenerConexion())
            {
                GenerarYGuardarPDFFactura(idFacturaGenerada, conn.ConnectionString);
            }

            LimpiarTodo(); // Limpia carrito, cliente, etc.
        }
        

        private void ActualizarStockProductoEnCombo(int idProducto, int nuevoStock)
        {
            for (int i = 0; i < cbProductos.Items.Count; i++)
            {
                if (cbProductos.Items[i] is Producto producto && producto.Id == idProducto)
                {
                    producto.Stock = nuevoStock;
                    break;
                }
            }
        }

        private void ActualizarStockLabel()
        {
            if (cbProductos.SelectedItem is Producto productoSeleccionado)
            {
                using (var conn = Conexion.ObtenerConexion())
                using (var cmd = new NpgsqlCommand("SELECT stock FROM producto WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", productoSeleccionado.Id);
                    var result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int stockActualizado))
                    {
                        lblStockProducto.Text = $"Stock disponible: {stockActualizado}";
                    }
                }
            }
        }

        
        private int ObtenerStockProducto(int idProducto)
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT stock FROM producto WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", idProducto);
                var result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int stock))
                {
                    return stock;
                }
                return 0;
            }
        }


        private void btnGenerarProforma_Click(object sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("Debe agregar productos al carrito.", "Validación");
                return;
            }

            int idCliente = ObtenerORegistrarCliente(txtNombreCliente.Text.Trim(), txtTelefono.Text.Trim(),
                txtUbicacion.Text.Trim());
            decimal total = carrito.Sum(c => c.Subtotal);
            decimal descuento = carrito.Sum(c => c.PrecioUnitario * (c.Descuento / 100));
            var detalles = JsonSerializer.Serialize(carrito);

            int idProformaGenerada;

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT sp_insert_proforma(@id_cliente, @total, @descuento, @detalles::json);", conn))
            {
                cmd.Parameters.AddWithValue("id_cliente", idCliente);
                cmd.Parameters.AddWithValue("total", total);
                cmd.Parameters.AddWithValue("descuento", descuento);
                cmd.Parameters.AddWithValue("detalles", detalles);
                idProformaGenerada = Convert.ToInt32(cmd.ExecuteScalar());  // Asegúrate que el SP retorna el ID
            }

            // ✅ Usa la conexión directamente con el método que ya tienes
            using (var conn = Conexion.ObtenerConexion())
            {
                bool generado = GenerarYGuardarPDFProforma(idProformaGenerada, conn.ConnectionString);
                if (generado)
                {
                    MessageBox.Show("✅ Proforma generada y PDF guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // No mostramos mensaje de error aquí porque ya se mostró dentro del método si falló
            }


          
            LimpiarTodo();
        }


        private int ObtenerORegistrarCliente(string nombre, string telefono, string ubicacion)
        {
            using (var conn = Conexion.ObtenerConexion())
            {
                // Llama siempre al procedimiento almacenado
                using (var insertCmd = new NpgsqlCommand("SELECT sp_insert_cliente(@nombre, @telefono, @ubicacion);", conn))
                {
                    insertCmd.Parameters.AddWithValue("nombre", nombre);
                    insertCmd.Parameters.AddWithValue("telefono", telefono);
                    insertCmd.Parameters.AddWithValue("ubicacion",
                        string.IsNullOrWhiteSpace(ubicacion) ? DBNull.Value : ubicacion);
                    insertCmd.ExecuteNonQuery();
                }

                // Luego obtén el ID del cliente actualizado o recién creado
                using (var cmd = new NpgsqlCommand("SELECT id FROM cliente WHERE telefono = @telefono;", conn))
                {
                    cmd.Parameters.AddWithValue("telefono", telefono);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }



  public bool GenerarYGuardarPDFProforma(int idProforma, string connectionString)
{
    byte[] pdfBytes;

    using (MemoryStream ms = new MemoryStream())
    {
        iTextDocument doc = new iTextDocument(iTextPageSize.A4, 50, 50, 50, 50);
        iTextPdfWriter writer = iTextPdfWriter.GetInstance(doc, ms);
        doc.Open();

        // Cabecera
        var titulo = new iTextParagraph("Ferretería GyG\nPROFORMA",
            iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_BOLD, 20));
        titulo.Alignment = iTextElement.ALIGN_CENTER;
        doc.Add(titulo);

        doc.Add(new iTextParagraph(" ")); // espacio

        // Obtener info del cliente y productos
        string clienteNombre = "";
        string fecha = "";
        decimal total = 0;

        DataTable productos = new DataTable();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            // Info general proforma y cliente
            using (var cmd = new NpgsqlCommand(@"
                SELECT c.nombre, p.total, p.fecha
                FROM proforma p
                JOIN cliente c ON p.id_cliente = c.id
                WHERE p.id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", idProforma);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        clienteNombre = reader.GetString(0);
                        total = reader.GetDecimal(1);
                        fecha = reader.GetDateTime(2).ToString("dd/MM/yyyy HH:mm");
                    }
                }
            }

            // ✅ Validación: si no hay cliente, salimos
            if (string.IsNullOrWhiteSpace(clienteNombre))
            {
                MessageBox.Show("⚠️ No se puede generar la proforma porque no hay datos del cliente asociados.", "Datos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Productos con descripción incluida
            using (var da = new NpgsqlDataAdapter(@"
                SELECT prod.nombre AS producto, prod.descripcion, d.cantidad, d.precio_unitario, d.subtotal
                FROM proforma_detalle d
                JOIN producto prod ON d.id_producto = prod.id
                WHERE d.id_proforma = @id", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", idProforma);
                da.Fill(productos);
            }
        }

        // Datos generales
        doc.Add(new iTextParagraph($"N° Proforma: {idProforma}"));
        doc.Add(new iTextParagraph($"Cliente: {clienteNombre}"));
        doc.Add(new iTextParagraph($"Fecha: {fecha}"));
        doc.Add(new iTextParagraph(" "));

        // Tabla con descripción
        iTextPdfPTable table = new iTextPdfPTable(5);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 30, 35, 12, 12, 13 });

        table.AddCell("Producto");
        table.AddCell("Descripción");
        table.AddCell("Cantidad");
        table.AddCell("Precio Unitario");
        table.AddCell("Subtotal");

        foreach (DataRow row in productos.Rows)
        {
            table.AddCell(row["producto"].ToString());
            table.AddCell(row["descripcion"].ToString());
            table.AddCell(row["cantidad"].ToString());
            table.AddCell("$" + Convert.ToDecimal(row["precio_unitario"]).ToString("F2"));
            table.AddCell("$" + Convert.ToDecimal(row["subtotal"]).ToString("F2"));
        }

        doc.Add(table);

        // Total
        doc.Add(new iTextParagraph(" "));
        doc.Add(new iTextParagraph($"TOTAL: ${total:F2}", iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_BOLD, 14)));

        doc.Close();
        pdfBytes = ms.ToArray();
    }

    // Guardar PDF en base de datos
    using (var conn = new NpgsqlConnection(connectionString))
    {
        conn.Open();
        using (var cmd = new NpgsqlCommand(@"
            INSERT INTO archivo_pdf(nombre_archivo, tipo, contenido, id_relacionado)
            VALUES (@nombre, @tipo, @contenido, @id)", conn))
        {
            cmd.Parameters.AddWithValue("@nombre", $"proforma_{idProforma}.pdf");
            cmd.Parameters.AddWithValue("@tipo", "proforma");
            cmd.Parameters.AddWithValue("@contenido", pdfBytes);
            cmd.Parameters.AddWithValue("@id", idProforma);
            cmd.ExecuteNonQuery();

            string nombreArchivo = $"proforma_{idProforma}.pdf";
            string rutaTemporal = Path.Combine(Path.GetTempPath(), nombreArchivo);
            File.WriteAllBytes(rutaTemporal, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = rutaTemporal,
                UseShellExecute = true
            });
        }
    }

    return true;
}

   
   public void GenerarYGuardarPDFFactura(int idFactura, string connectionString)
{
    byte[] pdfBytes;

    using (MemoryStream ms = new MemoryStream())
    {
        iTextDocument doc = new iTextDocument(iTextPageSize.A4, 50, 50, 50, 50);
        iTextPdfWriter writer = iTextPdfWriter.GetInstance(doc, ms);
        doc.Open();

        // Cabecera
        var titulo = new iTextParagraph("FERRETERÍA GyG\nFACTURA",
            iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_BOLD, 20));
        titulo.Alignment = iTextElement.ALIGN_CENTER;
        doc.Add(titulo);
        doc.Add(new iTextParagraph(" ")); // Espacio

        // Variables para cliente y factura
        string clienteNombre = "", telefonoCliente = "", fecha = "", estadoPago = "";
        decimal total = 0;
        DataTable productos = new DataTable();

        // Obtener datos de factura y productos
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            // Datos generales factura
            using (var cmd = new NpgsqlCommand(@"
                SELECT c.nombre, c.telefono, f.total, f.fecha, f.estado_pago
                FROM factura f
                JOIN cliente c ON f.id_cliente = c.id
                WHERE f.id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", idFactura);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        clienteNombre = reader.GetString(0);
                        telefonoCliente = reader.GetString(1);
                        total = reader.GetDecimal(2);
                        fecha = reader.GetDateTime(3).ToString("dd/MM/yyyy HH:mm");
                        estadoPago = reader.GetString(4);
                    }
                }
            }

            // Detalle de productos
            using (var da = new NpgsqlDataAdapter(@"
                SELECT 
                    prod.nombre AS producto,
                    prod.descripcion,
                    d.cantidad,
                    d.precio_unitario,
                    prod.iva,
                    prod.descuento,
                    d.subtotal
                FROM factura_detalle d
                JOIN producto prod ON d.id_producto = prod.id
                WHERE d.id_factura = @id", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", idFactura);
                da.Fill(productos);
            }
        }

        // Información del cliente y factura
        doc.Add(new iTextParagraph($"N° Factura: {idFactura}"));
        doc.Add(new iTextParagraph($"Cliente: {clienteNombre}"));
        doc.Add(new iTextParagraph($"Teléfono: {telefonoCliente}"));
        doc.Add(new iTextParagraph($"Fecha: {fecha}"));
        doc.Add(new iTextParagraph($"Estado de Pago: {estadoPago}"));
        doc.Add(new iTextParagraph(" "));

        // Tabla de productos
        iTextPdfPTable table = new iTextPdfPTable(7);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 20, 25, 10, 10, 10, 10, 15 });

        table.AddCell("Producto");
        table.AddCell("Descripción");
        table.AddCell("Cant.");
        table.AddCell("Precio");
        table.AddCell("IVA");
        table.AddCell("Desc.");
        table.AddCell("Subtotal");

        foreach (DataRow row in productos.Rows)
        {
            decimal precio = Convert.ToDecimal(row["precio_unitario"]);
            int cantidad = Convert.ToInt32(row["cantidad"]);
            decimal iva = Convert.ToDecimal(row["iva"]);
            decimal descuento = Convert.ToDecimal(row["descuento"]);
            decimal subtotal = Convert.ToDecimal(row["subtotal"]);

            table.AddCell(row["producto"].ToString());
            table.AddCell(row["descripcion"].ToString());
            table.AddCell(cantidad.ToString());
            table.AddCell($"${precio:F2}");
            table.AddCell($"{iva}%");
            table.AddCell($"{descuento}%");
            table.AddCell($"${subtotal:F2}");
        }

        doc.Add(table);
        doc.Add(new iTextParagraph(" "));

        // Total
        doc.Add(new iTextParagraph($"TOTAL: ${total:F2}", iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_BOLD, 14)));
        doc.Add(new iTextParagraph(" "));

        // Líneas de firma y texto
        PdfContentByte cb = writer.DirectContent;
        float yLinea = doc.BottomMargin + 50;
        float anchoLinea = 60;

        // Línea izquierda - "Entregué Conforme"
        float xEntregue = doc.LeftMargin;
        cb.MoveTo(xEntregue, yLinea);
        cb.LineTo(xEntregue + anchoLinea, yLinea);
        cb.Stroke();

        // Línea derecha - "Recibí Conforme"
        float xRecibi = doc.PageSize.Width - doc.RightMargin - anchoLinea;
        cb.MoveTo(xRecibi, yLinea);
        cb.LineTo(xRecibi + anchoLinea, yLinea);
        cb.Stroke();

        // Texto debajo de líneas
        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        cb.BeginText();
        cb.SetFontAndSize(bf, 10);
        cb.ShowTextAligned(Element.ALIGN_CENTER, "Entregué Conforme", xEntregue + anchoLinea / 2, yLinea - 12, 0);
        cb.ShowTextAligned(Element.ALIGN_CENTER, "Recibí Conforme", xRecibi + anchoLinea / 2, yLinea - 12, 0);
        cb.EndText();

        // Mensaje final
        doc.Add(new iTextParagraph(" "));
        doc.Add(new iTextParagraph("Gracias por su compra.",
            iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_OBLIQUE, 12)));

        doc.Close();
        pdfBytes = ms.ToArray();
    }

    // Guardar en base de datos y abrir el PDF
    using (var conn = new NpgsqlConnection(connectionString))
    {
        conn.Open();
        using (var cmd = new NpgsqlCommand(@"
            INSERT INTO archivo_pdf(nombre_archivo, tipo, contenido)
            VALUES (@nombre, @tipo, @contenido)", conn))
        {
            cmd.Parameters.AddWithValue("@nombre", $"factura_{idFactura}.pdf");
            cmd.Parameters.AddWithValue("@tipo", "factura");
            cmd.Parameters.AddWithValue("@contenido", pdfBytes);
            cmd.ExecuteNonQuery();
        }

        string nombreArchivo = $"factura_{idFactura}.pdf";
        string rutaTemporal = Path.Combine(Path.GetTempPath(), nombreArchivo);
        File.WriteAllBytes(rutaTemporal, pdfBytes);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
        {
            FileName = rutaTemporal,
            UseShellExecute = true
        });
    }
}




        private void LimpiarTodo()
        {
            carrito.Clear();
            RefrescarCarrito();
            txtNombreCliente.Clear();
            txtTelefono.Clear();
            txtUbicacion.Clear();
        }
    }

    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public decimal IVA { get; set; }         
        public decimal Descuento { get; set; }

        public override string ToString() => Nombre;
    }

    public class ProductoCarrito
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } // Nuevo campo
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal Descuento { get; set; }

        public decimal Subtotal =>
            (PrecioUnitario + (PrecioUnitario * IVA / 100) - (PrecioUnitario * Descuento / 100)) * Cantidad;
    }
}
