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
        // Parsear valores numéricos con TryParse para evitar excepciones
        decimal.TryParse(txtPrecioInv.Text, out decimal precioInv);
        decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta);
        int.TryParse(txtStock.Text, out int stock);

        // Limpiar y parsear IVA y descuento (porcentaje)
        string ivaTexto = txtIVA.Text.Replace("%", "").Trim();
        string descTexto = txtDescuento.Text.Replace("%", "").Trim();

        decimal.TryParse(ivaTexto, out decimal ivaPorcentaje);
        decimal.TryParse(descTexto, out decimal descuentoPorcentaje);

        // Convertir a decimales (ej: 15 -> 0.15)
        decimal ivaDecimal = ivaPorcentaje / 100m;
        decimal descuentoDecimal = descuentoPorcentaje / 100m;

        // Calcular precio final con fórmula
        decimal precioFinal = precioVenta * (1 + ivaDecimal) * (1 - descuentoDecimal);

        // Depuración de valores
        System.Diagnostics.Debug.WriteLine(">>> DTP Checked: " + dtpFechaExpiracion.Checked);
        System.Diagnostics.Debug.WriteLine(">>> DTP Value: " + dtpFechaExpiracion.Value.ToShortDateString());

        // Fecha expiración: solo si checkbox está marcado, sino NULL
        DateTime? fechaExpiracion = dtpFechaExpiracion.Checked ? dtpFechaExpiracion.Value.Date : (DateTime?)null;

        System.Diagnostics.Debug.WriteLine(">>> Fecha a enviar a BD: " + (fechaExpiracion.HasValue ? fechaExpiracion.Value.ToShortDateString() : "NULL"));

        using (var conn = Conexion.ObtenerConexion())
        {
            using (var cmd = new NpgsqlCommand(
                "SELECT sp_update_producto(@id, @n, @d, @c, @pi, @pv, @s, @cod, @fe, @iva, @desc, @pf)", conn))
            {
                cmd.Parameters.AddWithValue("id", productoSeleccionadoId.Value);
                cmd.Parameters.AddWithValue("n", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("d", txtDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("c", string.IsNullOrWhiteSpace(cmbCategoria.Text) ? (object)DBNull.Value : cmbCategoria.Text.Trim());
                cmd.Parameters.AddWithValue("pi", precioInv);
                cmd.Parameters.AddWithValue("pv", precioVenta);
                cmd.Parameters.AddWithValue("s", stock);
                cmd.Parameters.AddWithValue("cod", string.IsNullOrWhiteSpace(txtCodigoBarra.Text) ? (object)DBNull.Value : txtCodigoBarra.Text.Trim());

                // ✅ Aquí el parámetro clave: explícito y controlado
                var paramFecha = new NpgsqlParameter("fe", NpgsqlTypes.NpgsqlDbType.Date);
                if (fechaExpiracion.HasValue)
                {
                    paramFecha.Value = fechaExpiracion.Value;
                }
                else
                {
                    paramFecha.Value = DBNull.Value;
                }

                cmd.Parameters.Add(paramFecha);

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
        System.Diagnostics.Debug.WriteLine(">>> Error: " + ex.Message);
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
    // Validar campos obligatorios antes de continuar
    if (!ValidarCampos())
    {
        MessageBox.Show("Por favor complete correctamente los campos obligatorios (marcados con *).");
        return;
    }

    // Limpiar símbolos y parsear IVA y descuento (porcentaje)
    string ivaTexto = txtIVA.Text.Replace("%", "").Trim();
    string descTexto = txtDescuento.Text.Replace("%", "").Trim();

    decimal.TryParse(ivaTexto, out decimal ivaPorcentaje);
    decimal.TryParse(descTexto, out decimal descuentoPorcentaje);

    // Convertir porcentaje a decimal (ej: 15 -> 0.15)
    decimal ivaDecimal = ivaPorcentaje / 100m;
    decimal descuentoDecimal = descuentoPorcentaje / 100m;

    // Parsear otros valores numéricos con TryParse
    decimal.TryParse(txtPrecioInv.Text, out decimal precioInv);
    decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta);
    int.TryParse(txtStock.Text, out int stock);

    // Calcular precio final con la fórmula
    decimal precioFinal = precioVenta * (1 + ivaDecimal) * (1 - descuentoDecimal);

    // Preparar datos de texto, usando DBNull.Value si están vacíos
    string nombre = txtNombre.Text.Trim();
    string descripcion = txtDescripcion.Text.Trim();
    string categoria = string.IsNullOrWhiteSpace(cmbCategoria.Text) ? null : cmbCategoria.Text.Trim();
    string codigo = string.IsNullOrWhiteSpace(txtCodigoBarra.Text) ? null : txtCodigoBarra.Text.Trim();

    // Solo asignar fecha si checkbox está marcado, sino null
    DateTime? fechaExpiracion = dtpFechaExpiracion.Checked ? dtpFechaExpiracion.Value.Date : (DateTime?)null;

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

                // Parámetro fecha: DBNull si no hay fecha
                var paramFecha = new NpgsqlParameter("fe", NpgsqlTypes.NpgsqlDbType.Date);
                if (dtpFechaExpiracion.Checked)
                    paramFecha.Value = dtpFechaExpiracion.Value.Date;
                else
                    paramFecha.Value = DBNull.Value;

                cmd.Parameters.Add(paramFecha);
                cmd.Parameters.AddWithValue("iva", ivaPorcentaje);
                cmd.Parameters.AddWithValue("desc", descuentoPorcentaje);
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
            txtIVA.Clear();
            txtDescuento.Clear();
            productoSeleccionadoId = null;
        }

    }
}