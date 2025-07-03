using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GyG.Datos;
using Npgsql;

namespace GyG.Presentacion
{
    public partial class HistorialFacturasForm : Form
    {
        private DataGridView dgvHistorialFacturas;
        private Button btnAbrirFactura;

        public HistorialFacturasForm()
        {
            InitializeComponent();
            this.Text = "Historial de Facturas - Ventas";
            this.Size = new Size(750, 450);
            this.StartPosition = FormStartPosition.CenterParent;

            dgvHistorialFacturas = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 360,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvHistorialFacturas.Columns.Add("id", "ID");
            dgvHistorialFacturas.Columns["id"].Visible = false;
            dgvHistorialFacturas.Columns.Add("nombre_archivo", "Nombre Archivo");
            dgvHistorialFacturas.Columns.Add("fecha", "Fecha");

            this.Controls.Add(dgvHistorialFacturas);

            btnAbrirFactura = new Button
            {
                Text = "Abrir Factura Seleccionada",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnAbrirFactura.Click += BtnAbrirFactura_Click;
            this.Controls.Add(btnAbrirFactura);

            CargarHistorialFacturas();
        }

        private void CargarHistorialFacturas()
        {
            dgvHistorialFacturas.Rows.Clear();

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand(@"
                SELECT id, nombre_archivo, fecha
                FROM archivo_pdf
                WHERE tipo = 'factura'
                ORDER BY fecha DESC
                LIMIT 50", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    DateTime fecha = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);

                    dgvHistorialFacturas.Rows.Add(id, nombre, fecha == DateTime.MinValue ? "" : fecha.ToString("dd/MM/yyyy HH:mm"));
                }
            }
        }

        private void BtnAbrirFactura_Click(object sender, EventArgs e)
        {
            if (dgvHistorialFacturas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una factura para abrir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvHistorialFacturas.SelectedRows[0].Cells["id"].Value);
            AbrirFacturaPorId(id);
        }

        private void AbrirFacturaPorId(int idFactura)
        {
            byte[] pdfBytes = null;

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand("SELECT contenido FROM archivo_pdf WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", idFactura);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pdfBytes = (byte[])reader["contenido"];
                    }
                }
            }

            if (pdfBytes != null)
            {
                string nombreArchivo = $"factura_{idFactura}.pdf";
                string rutaTemporal = System.IO.Path.Combine(System.IO.Path.GetTempPath(), nombreArchivo);
                System.IO.File.WriteAllBytes(rutaTemporal, pdfBytes);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = rutaTemporal,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("No se pudo encontrar el archivo PDF para esta factura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
