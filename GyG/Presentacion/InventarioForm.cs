using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using GyG.Datos;

namespace GyG.Presentacion
{
    public partial class InventarioForm : Form
    {
        private int? productoSeleccionadoId = null;

        public InventarioForm()
        {
            InitializeComponent();
        }

        private void InventarioForm_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarProductos();
            btnEscanearQR = new Button
            {
                Text = "Escanear QR",
                Width = 120,
                Height = 30,
                Left = 600, // ajustá según diseño
                Top = 20
            };
            btnEscanearQR.Click += BtnEscanearQR_Click;
            this.Controls.Add(btnEscanearQR);

        }

        private void CargarCategorias()
        {
            cmbCategoria.Items.Clear();

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT nombre FROM categoria ORDER BY nombre";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            cmbCategoria.Items.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
        }

        private void BtnGestionarCategorias_Click(object sender, EventArgs e)
        {
            using (var gestionCat = new GestionCategoriasForm())
            {
                gestionCat.ShowDialog();
            }

            CargarCategorias(); // Recarga siempre después de cerrar el formulario
        }


        private void BtnEscanearQR_Click(object sender, EventArgs e)
        {
            var lector = new LectorCodigoForm(); // Debés tener este formulario ya creado
            lector.ShowDialog();

            // Opcional: si tu lector llena datos del producto, lo podrías actualizar aquí.
        }


        private void CargarProductos()
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    // Agregamos todas las columnas que luego accedes
                    string sql = @"
                SELECT id, nombre, descripcion, categoria, precio_inventario, precio_venta, stock, codigo,
                       fecha_vencimiento, iva, descuento, precio_final
                FROM producto 
                ORDER BY id DESC";

                    using (var da = new NpgsqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvProductos.DataSource = dt;

                        dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvProductos.ScrollBars = ScrollBars.Both;
                        dgvProductos.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

 
        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                productoSeleccionadoId = Convert.ToInt32(fila.Cells["id"].Value);
                txtNombre.Text = fila.Cells["nombre"].Value.ToString();
                txtDescripcion.Text = fila.Cells["descripcion"].Value.ToString();
                cmbCategoria.Text = fila.Cells["categoria"].Value == DBNull.Value ? "" : fila.Cells["categoria"].Value.ToString();
                txtPrecioInv.Text = fila.Cells["precio_inventario"].Value.ToString();
                txtPrecioVenta.Text = fila.Cells["precio_venta"].Value.ToString();
                txtStock.Text = fila.Cells["stock"].Value.ToString();
                txtCodigoBarra.Text = fila.Cells["codigo"].Value == DBNull.Value ? "" : fila.Cells["codigo"].Value.ToString();

                // Fecha de expiración: puede ser NULL, por eso chequeamos DBNull
                if (fila.Cells["fecha_vencimiento"].Value == DBNull.Value)
                {
                    dtpFechaExpiracion.Checked = false; // desactiva checkbox para fecha opcional
                    dtpFechaExpiracion.Value = DateTime.Today; // o alguna fecha por defecto
                }
                else
                {
                    dtpFechaExpiracion.Value = Convert.ToDateTime(fila.Cells["fecha_vencimiento"].Value);
                    dtpFechaExpiracion.Checked = true;
                }

                // IVA y descuento: pueden ser NULL o vacíos, chequeamos también
                txtIVA.Text = fila.Cells["iva"].Value == DBNull.Value ? "" : fila.Cells["iva"].Value.ToString();
                txtDescuento.Text = fila.Cells["descuento"].Value == DBNull.Value ? "" : fila.Cells["descuento"].Value.ToString();
            }
        }


private void btnEditar_Click(object sender, EventArgs e)
{
    if (productoSeleccionadoId == null)
    {
        MessageBox.Show("Seleccione un producto haciendo doble clic en la lista.");
        return;
    }

    if (!ValidarCampos())
        return;

    try
    {
        decimal precioInv = decimal.TryParse(txtPrecioInv.Text, out var pi) ? pi : 0;
        decimal precioVenta = decimal.TryParse(txtPrecioVenta.Text, out var pv) ? pv : 0;
        int stock = int.TryParse(txtStock.Text, out var st) ? st : 0;

        // Obtener IVA y descuento en porcentaje, convertir a decimal (ej 15 -> 0.15)
        string ivaTexto = txtIVA.Text.Replace("%", "").Trim();
        string descTexto = txtDescuento.Text.Replace("%", "").Trim();
        decimal.TryParse(ivaTexto, out decimal ivaPorcentaje);
        decimal.TryParse(descTexto, out decimal descuentoPorcentaje);

        decimal ivaDecimal = ivaPorcentaje / 100m;
        decimal descuentoDecimal = descuentoPorcentaje / 100m;

        // Calcular precio final
        decimal precioFinal = precioVenta * (1 + ivaDecimal) * (1 - descuentoDecimal);

        DateTime? fechaExpiracion = null;
        if (dtpFechaExpiracion.Checked)
            fechaExpiracion = dtpFechaExpiracion.Value.Date;

        using (var conn = Conexion.ObtenerConexion())
        {
            using (var cmd = new NpgsqlCommand(
                "SELECT sp_update_producto(@id, @n, @d, @c, @pi, @pv, @s, @cod, @fe, @iva, @desc, @pf)", conn))
            {
                cmd.Parameters.AddWithValue("id", productoSeleccionadoId);
                cmd.Parameters.AddWithValue("n", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("d", txtDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("c", string.IsNullOrWhiteSpace(cmbCategoria.Text) ? (object)DBNull.Value : cmbCategoria.Text.Trim());
                cmd.Parameters.AddWithValue("pi", precioInv);
                cmd.Parameters.AddWithValue("pv", precioVenta);
                cmd.Parameters.AddWithValue("s", stock);
                cmd.Parameters.AddWithValue("cod", string.IsNullOrWhiteSpace(txtCodigoBarra.Text) ? (object)DBNull.Value : txtCodigoBarra.Text.Trim());

                cmd.Parameters.Add(new NpgsqlParameter("fe", NpgsqlTypes.NpgsqlDbType.Date)
                {
                    Value = fechaExpiracion.HasValue ? (object)fechaExpiracion.Value.Date : DBNull.Value
                });

                cmd.Parameters.AddWithValue("iva", ivaPorcentaje);
                cmd.Parameters.AddWithValue("desc", descuentoPorcentaje);
                cmd.Parameters.AddWithValue("pf", precioFinal);

                cmd.ExecuteNonQuery();
            }
        }

        MessageBox.Show("Producto actualizado correctamente.");
        LimpiarCampos();
        CargarProductos();
        productoSeleccionadoId = null;
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al actualizar: " + ex.Message);
    }
}



        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == null)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            DialogResult confirm = MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Confirmar",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Conexion.ObtenerConexion())
                    {
                        using (var cmd = new NpgsqlCommand("SELECT sp_delete_producto(@id)", conn))
                        {
                            cmd.Parameters.AddWithValue("id", productoSeleccionadoId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Producto eliminado correctamente.");
                    LimpiarCampos();
                    CargarProductos();
                    productoSeleccionadoId = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }

  private void btnGuardar_Click(object sender, EventArgs e)
{
    // Primero validamos campos obligatorios
    if (!ValidarCampos())
    {
        MessageBox.Show("Por favor complete correctamente los campos obligatorios (marcados con *).");
        return;
    }

    // Limpiamos símbolos y parseamos IVA y descuento
    string ivaTexto = txtIVA.Text.Replace("%", "").Trim();
    string descTexto = txtDescuento.Text.Replace("%", "").Trim();

    decimal.TryParse(ivaTexto, out decimal iva);
    decimal.TryParse(descTexto, out decimal descuento);

    // Convertimos porcentaje a decimal (ej. 15% => 0.15)
    decimal ivaDecimal = iva / 100m;
    decimal descuentoDecimal = descuento / 100m;

    // Parseamos los otros valores necesarios
    decimal.TryParse(txtPrecioInv.Text, out decimal precioInv);
    decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta);
    int.TryParse(txtStock.Text, out int stock);

    // Calculamos el precio final usando IVA y descuento
    decimal precioFinal = precioVenta * (1 + ivaDecimal) * (1 - descuentoDecimal);

    string nombre = txtNombre.Text.Trim();
    string descripcion = txtDescripcion.Text.Trim();
    string categoria = string.IsNullOrWhiteSpace(cmbCategoria.Text) ? null : cmbCategoria.Text.Trim();
    string codigo = string.IsNullOrWhiteSpace(txtCodigoBarra.Text) ? null : txtCodigoBarra.Text.Trim();

    DateTime? fechaExpiracion = null;
    if (dtpFechaExpiracion.Checked)
    {
        fechaExpiracion = dtpFechaExpiracion.Value.Date;
    }

    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
            using (var cmd = new NpgsqlCommand(
                       "SELECT sp_insert_producto(@n, @d, @c, @pi, @pv, @s, @cod, @fe, @iva, @desc, @pf)", conn))
            {
                cmd.Parameters.AddWithValue("n", nombre);
                cmd.Parameters.AddWithValue("d", descripcion);
                cmd.Parameters.AddWithValue("c", string.IsNullOrWhiteSpace(categoria) ? (object)DBNull.Value : categoria);
                cmd.Parameters.AddWithValue("pi", precioInv);
                cmd.Parameters.AddWithValue("pv", precioVenta);
                cmd.Parameters.AddWithValue("s", stock);
                cmd.Parameters.AddWithValue("cod", string.IsNullOrWhiteSpace(codigo) ? (object)DBNull.Value : codigo);

                cmd.Parameters.Add(new NpgsqlParameter("fe", NpgsqlTypes.NpgsqlDbType.Date)
                {
                    Value = fechaExpiracion.HasValue ? (object)fechaExpiracion.Value.Date : DBNull.Value
                });

                cmd.Parameters.AddWithValue("iva", iva);
                cmd.Parameters.AddWithValue("desc", descuento);
                cmd.Parameters.AddWithValue("pf", precioFinal);

                cmd.ExecuteNonQuery();
            }
        }

        MessageBox.Show("Producto registrado con éxito.");
        LimpiarCampos();
        CargarProductos();
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al guardar producto: " + ex.Message);
    }
}

        private bool ValidarCampos()
        {
            bool valido = true;

            // Nombre obligatorio
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                txtNombre.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtNombre.BackColor = Color.White;

            // Precio Inventario
            if (!decimal.TryParse(txtPrecioInv.Text, out _))
            {
                txtPrecioInv.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtPrecioInv.BackColor = Color.White;

            // Precio Venta
            if (!decimal.TryParse(txtPrecioVenta.Text, out _))
            {
                txtPrecioVenta.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtPrecioVenta.BackColor = Color.White;

            // Stock
            if (!int.TryParse(txtStock.Text, out _))
            {
                txtStock.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtStock.BackColor = Color.White;

            // IVA opcional pero válido
            string ivaTexto = txtIVA.Text.Replace("%", "").Trim();
            if (!string.IsNullOrWhiteSpace(ivaTexto) && (!decimal.TryParse(ivaTexto, out decimal iva) || iva < 0))
            {
                txtIVA.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtIVA.BackColor = Color.White;

            // Descuento opcional pero válido
            string descTexto = txtDescuento.Text.Replace("%", "").Trim();
            if (!string.IsNullOrWhiteSpace(descTexto) && (!decimal.TryParse(descTexto, out decimal desc) || desc < 0))
            {
                txtDescuento.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtDescuento.BackColor = Color.White;

            return valido;
        }


        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            if (dgvProductos.DataSource is DataTable dt)
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    dt.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    dt.DefaultView.RowFilter = string.Format("Convert(nombre, 'System.String') LIKE '%{0}%' OR Convert(descripcion, 'System.String') LIKE '%{0}%'", filtro.Replace("'", "''"));
                }
            }
        }


        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            cmbCategoria.SelectedIndex = -1;
            txtPrecioInv.Clear();
            txtPrecioVenta.Clear();
            txtStock.Clear();
            txtCodigoBarra.Clear();
            productoSeleccionadoId = null;
        }

    }
}