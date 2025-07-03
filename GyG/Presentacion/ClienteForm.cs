using System;
using System.Windows.Forms;
using GyG.Datos;

namespace GyG.Presentacion
{
    public partial class ClienteForm : Form
    {
        public ClienteForm()
        {
            InitializeComponent();
        }

        public ClienteForm(string nombre, string telefono, string ubicacion) : this()
        {
            txtNombre.Text = nombre;
            txtTelefono.Text = telefono;
            txtUbicacion.Text = ubicacion;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new Npgsql.NpgsqlCommand(
                       "UPDATE cliente SET nombre = @nombre, ubicacion = @ubicacion WHERE telefono = @telefono", conn))
            {
                cmd.Parameters.AddWithValue("nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("telefono", txtTelefono.Text.Trim());
                cmd.Parameters.AddWithValue("ubicacion", string.IsNullOrWhiteSpace(txtUbicacion.Text) ? DBNull.Value : txtUbicacion.Text.Trim());

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0)
                {
                    MessageBox.Show("Cliente actualizado correctamente.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente.");
                }
            }
        }
    }
}