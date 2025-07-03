using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using GyG.Datos;

namespace GyG.Presentacion
{
    public partial class ProveedoresForm : Form
    {
        public ProveedoresForm()
        {
            InitializeComponent();
            ConfigurarGrid();
            CargarProveedores();
            ActualizarEstadoBotones(false); 
        }

        private void ConfigurarGrid()
        {
            dgvProveedores.AutoGenerateColumns = false;
            dgvProveedores.Columns.Clear();

            // Columna ID (oculta)
            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "id";
            colId.DataPropertyName = "id";
            colId.HeaderText = "ID";
            colId.Visible = false;
            dgvProveedores.Columns.Add(colId);

            // Columna Nombre
            var colNombre = new DataGridViewTextBoxColumn();
            colNombre.Name = "nombre";
            colNombre.DataPropertyName = "nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProveedores.Columns.Add(colNombre);

            // Columna Teléfono
            var colTelefono = new DataGridViewTextBoxColumn();
            colTelefono.Name = "telefono";
            colTelefono.DataPropertyName = "telefono";
            colTelefono.HeaderText = "Teléfono";
            colTelefono.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProveedores.Columns.Add(colTelefono);

            // Columna Dirección
            var colDireccion = new DataGridViewTextBoxColumn();
            colDireccion.Name = "direccion";
            colDireccion.DataPropertyName = "direccion";
            colDireccion.HeaderText = "Dirección";
            colDireccion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProveedores.Columns.Add(colDireccion);

            dgvProveedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProveedores.MultiSelect = false;
        }

        private void CargarProveedores()
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string query = "SELECT id, nombre, telefono, direccion FROM proveedor ORDER BY nombre";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvProveedores.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string query = "INSERT INTO proveedor (nombre, telefono, direccion) VALUES (@nombre, @telefono, @direccion)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                        cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Proveedor agregado correctamente.");
                LimpiarCampos();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar proveedor: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un proveedor para modificar.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            int idProveedor = Convert.ToInt32(dgvProveedores.SelectedRows[0].Cells["id"].Value);

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string query = "UPDATE proveedor SET nombre = @nombre, telefono = @telefono, direccion = @direccion WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                        cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", idProveedor);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Proveedor modificado correctamente.");
                LimpiarCampos();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar proveedor: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un proveedor para eliminar.");
                return;
            }

            int idProveedor = Convert.ToInt32(dgvProveedores.SelectedRows[0].Cells["id"].Value);

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar este proveedor?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string query = "DELETE FROM proveedor WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idProveedor);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Proveedor eliminado correctamente.");
                LimpiarCampos();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar proveedor: " + ex.Message);
            }
        }

        private void dgvProveedores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProveedores.Rows[e.RowIndex].Cells["id"].Value != null)
            {
                DataGridViewRow fila = dgvProveedores.Rows[e.RowIndex];
                txtNombre.Text = fila.Cells["nombre"].Value?.ToString();
                txtTelefono.Text = fila.Cells["telefono"].Value?.ToString();
                txtDireccion.Text = fila.Cells["direccion"].Value?.ToString();
                
                ActualizarEstadoBotones(true); 
            }
        }


        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            ActualizarEstadoBotones(false); 
        }
        
        private void ActualizarEstadoBotones(bool editar)
        {
            btnAgregar.Enabled = !editar;
            btnModificar.Enabled = editar;
            btnEliminar.Enabled = editar;
        }

    }
}
