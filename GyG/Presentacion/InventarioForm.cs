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

        private void CargarProductos()
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT id, nombre, descripcion, categoria, precio_inventario, precio_venta, stock, codigo FROM producto ORDER BY id DESC";
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

        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            string nuevaCategoria = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el nombre de la nueva categoría:", "Nueva Categoría");

            if (!string.IsNullOrWhiteSpace(nuevaCategoria))
            {
                try
                {
                    using (var conn = Conexion.ObtenerConexion())
                    {
                        string sql = "INSERT INTO categoria(nombre) VALUES(@nombre)";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("nombre", nuevaCategoria.Trim());
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Categoría agregada correctamente.");
                    CargarCategorias();
                    cmbCategoria.SelectedItem = nuevaCategoria.Trim();
                }
                catch (PostgresException pgEx) when (pgEx.SqlState == "23505")
                {
                    MessageBox.Show("Esa categoría ya existe.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar categoría: " + ex.Message);
                }
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
                cmbCategoria.Text = fila.Cells["categoria"].Value.ToString();
                txtPrecioInv.Text = fila.Cells["precio_inventario"].Value.ToString();
                txtPrecioVenta.Text = fila.Cells["precio_venta"].Value.ToString();
                txtStock.Text = fila.Cells["stock"].Value.ToString();
                txtCodigoBarra.Text = fila.Cells["codigo"].Value.ToString();
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
                using (var conn = Conexion.ObtenerConexion())
                {
                    using (var cmd = new NpgsqlCommand("CALL sp_update_producto(@id, @n, @d, @c, @pi, @pv, @s, @cod)", conn))
                    {
                        cmd.Parameters.AddWithValue("id", productoSeleccionadoId);
                        cmd.Parameters.AddWithValue("n", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("d", txtDescripcion.Text.Trim());
                        cmd.Parameters.AddWithValue("c", cmbCategoria.Text);
                        cmd.Parameters.AddWithValue("pi", Convert.ToDecimal(txtPrecioInv.Text));
                        cmd.Parameters.AddWithValue("pv", Convert.ToDecimal(txtPrecioVenta.Text));
                        cmd.Parameters.AddWithValue("s", Convert.ToInt32(txtStock.Text));
                        cmd.Parameters.AddWithValue("cod", txtCodigoBarra.Text.Trim());
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

            DialogResult confirm = MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Conexion.ObtenerConexion())
                    {
                        using (var cmd = new NpgsqlCommand("CALL sp_delete_producto(@id)", conn))
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
            if (!ValidarCampos())
            {
                MessageBox.Show("Por favor complete correctamente los campos obligatorios.");
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            string categoria = cmbCategoria.Text;
            decimal.TryParse(txtPrecioInv.Text, out decimal precioInv);
            decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta);
            int.TryParse(txtStock.Text, out int stock);
            string codigo = txtCodigoBarra.Text.Trim();

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    using (var cmd = new NpgsqlCommand("CALL sp_insert_producto(@n, @d, @c, @pi, @pv, @s, @cod)", conn))
                    {
                        cmd.Parameters.AddWithValue("n", nombre);
                        cmd.Parameters.AddWithValue("d", descripcion);
                        cmd.Parameters.AddWithValue("c", (object)categoria ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("pi", precioInv);
                        cmd.Parameters.AddWithValue("pv", precioVenta);
                        cmd.Parameters.AddWithValue("s", stock);
                        cmd.Parameters.AddWithValue("cod", (object)codigo ?? DBNull.Value);

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

        private bool ValidarCampos()
        {
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                txtNombre.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtNombre.BackColor = Color.White;

            if (!decimal.TryParse(txtPrecioInv.Text, out _))
            {
                txtPrecioInv.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtPrecioInv.BackColor = Color.White;

            if (!decimal.TryParse(txtPrecioVenta.Text, out _))
            {
                txtPrecioVenta.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtPrecioVenta.BackColor = Color.White;

            if (!int.TryParse(txtStock.Text, out _))
            {
                txtStock.BackColor = Color.MistyRose;
                valido = false;
            }
            else txtStock.BackColor = Color.White;

            return valido;
        }
    }
}