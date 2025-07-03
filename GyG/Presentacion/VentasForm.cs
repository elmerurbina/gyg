using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using GyG.Datos;
using Npgsql;

namespace GyG.Presentacion
{
    public partial class VentasForm : Form
    {
        private List<ProductoCarrito> carrito = new List<ProductoCarrito>();

        public VentasForm()
        {
            InitializeComponent();
            CargarProductos();
            CargarClientes();
            numCantidad.ValueChanged += (s, e) => CalcularPrecioFinal();
            numIVA.ValueChanged += (s, e) => CalcularPrecioFinal();
            numDescuento.ValueChanged += (s, e) => CalcularPrecioFinal();

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
                        Stock = reader.GetInt32(5) // stock
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
                AutoCompleteStringCollection telefonos = new AutoCompleteStringCollection();

                while (reader.Read())
                {
                    telefonos.Add(reader.GetString(2));
                }

                txtTelefono.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtTelefono.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtTelefono.AutoCompleteCustomSource = telefonos;
            }
        }

        private void cbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProductos.SelectedItem is Producto producto)
            {
                txtDescripcion.Text = producto.Descripcion;
                txtPrecio.Text = producto.PrecioVenta.ToString("F2");

                // Ejemplo valores por defecto o puedes traer desde la BD si los tienes
                numIVA.Value = 15; // 15% IVA
                numDescuento.Value = 5; // 5% descuento por defecto

                CalcularPrecioFinal();
            }
            else
            {
                // Limpiar campos
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
                var producto = carrito.FirstOrDefault(c => c.Nombre == nombre);
                if (producto != null)
                {
                    carrito.Remove(producto);
                    RefrescarCarrito();
                }
            }
        }


        private void RefrescarCarrito()
        {
            dgvCarrito.CellValueChanged -= DgvCarrito_CellValueChanged;
            dgvCarrito.CellContentClick -= DgvCarrito_CellContentClick;

            // 🔴 Solución: limpiar columnas para evitar duplicados
            dgvCarrito.Columns.Clear();

            // Asignar datos
            dgvCarrito.DataSource = carrito.Select(c => new
            {
                c.Id,
                c.Nombre,
                Descripcion = c.Descripcion ?? "",
                Precio = c.PrecioUnitario.ToString("C2"),
                IVA = c.IVA,
                Descuento = c.Descuento,
                Cantidad = c.Cantidad,
                Subtotal = c.Subtotal.ToString("C2")
            }).ToList();

            // Ocultar ID si está presente
            if (dgvCarrito.Columns.Contains("Id"))
                dgvCarrito.Columns["Id"].Visible = false;

            // Actualizar total
            lblTotal.Text = "Total: " + carrito.Sum(c => c.Subtotal).ToString("C2");

            dgvCarrito.CellValueChanged += DgvCarrito_CellValueChanged;
            dgvCarrito.CellContentClick += DgvCarrito_CellContentClick;
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

            int idCliente = ObtenerORegistrarCliente(txtNombreCliente.Text.Trim(), txtTelefono.Text.Trim(),
                txtUbicacion.Text.Trim());
            decimal total = carrito.Sum(c => c.Subtotal);
            decimal descuento = carrito.Sum(c => c.PrecioUnitario * (c.Descuento / 100));
            var detalles = JsonSerializer.Serialize(carrito);

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand(
                       "SELECT sp_insert_factura(@id_cliente, @estado_pago, @total, @descuento, @detalles);", conn))
            {
                cmd.Parameters.AddWithValue("id_cliente", idCliente);
                cmd.Parameters.AddWithValue("estado_pago", estadoPago);
                cmd.Parameters.AddWithValue("total", total);
                cmd.Parameters.AddWithValue("descuento", descuento);
                cmd.Parameters.AddWithValue("detalles", detalles);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Venta registrada exitosamente. No se puede modificar.", "Venta registrada");
            LimpiarTodo();
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

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT sp_insert_proforma(@id_cliente, @total, @descuento, @detalles);",
                       conn))
            {
                cmd.Parameters.AddWithValue("id_cliente", idCliente);
                cmd.Parameters.AddWithValue("total", total);
                cmd.Parameters.AddWithValue("descuento", descuento);
                cmd.Parameters.AddWithValue("detalles", detalles);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Proforma generada correctamente.", "Proforma");
        }

        private int ObtenerORegistrarCliente(string nombre, string telefono, string ubicacion)
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT id FROM cliente WHERE telefono = @telefono", conn))
            {
                cmd.Parameters.AddWithValue("telefono", telefono);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return (int)result;
                }
                else
                {
                    using (var insertCmd =
                           new NpgsqlCommand("SELECT sp_insert_cliente(@nombre, @telefono, @ubicacion);", conn))
                    {
                        insertCmd.Parameters.AddWithValue("nombre", nombre);
                        insertCmd.Parameters.AddWithValue("telefono", telefono);
                        insertCmd.Parameters.AddWithValue("ubicacion",
                            string.IsNullOrWhiteSpace(ubicacion) ? DBNull.Value : ubicacion);
                        insertCmd.ExecuteNonQuery();
                    }

                    return ObtenerORegistrarCliente(nombre, telefono, ubicacion); // Recursive fetch
                }
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
