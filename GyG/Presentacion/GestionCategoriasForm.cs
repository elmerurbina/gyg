using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using GyG.Datos;

namespace GyG.Presentacion
{
    public partial class GestionCategoriasForm : Form
    {
        public GestionCategoriasForm()
        {
            InitializeComponent();
            CargarCategorias();
        }

        private void CargarCategorias()
        {
            dgvCategorias.Rows.Clear();

            using (var conn = Conexion.ObtenerConexion())
            {
                string sql = "SELECT id, nombre FROM categoria ORDER BY nombre";
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvCategorias.Rows.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtCategoria.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese un nombre válido.");
                return;
            }

            using (var conn = Conexion.ObtenerConexion())
            {
                string sql = "INSERT INTO categoria(nombre) VALUES(@nombre)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Categoría agregada.");
                        CargarCategorias();
                        txtCategoria.Clear();
                    }
                    catch (PostgresException ex) when (ex.SqlState == "23505")
                    {
                        MessageBox.Show("La categoría ya existe.");
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una categoría para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvCategorias.SelectedRows[0].Cells[0].Value);
            string nuevoNombre = txtCategoria.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("Ingrese un nombre válido.");
                return;
            }

            using (var conn = Conexion.ObtenerConexion())
            {
                string sql = "UPDATE categoria SET nombre = @nombre WHERE id = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("nombre", nuevoNombre);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Categoría actualizada.");
                    CargarCategorias();
                    txtCategoria.Clear();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una categoría para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvCategorias.SelectedRows[0].Cells[0].Value);

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar esta categoría?", "Confirmar", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string sql = "DELETE FROM categoria WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Categoría eliminada.");
                        CargarCategorias();
                        txtCategoria.Clear();
                    }
                }
            }
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCategoria.Text = dgvCategorias.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }
    }
}
