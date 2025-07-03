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
        private Button btnCancelarCredito;

        public HistorialFacturasForm()
        {
            InitializeComponent();
            this.Text = "Historial de Facturas - Ventas";
            this.Size = new Size(750, 500);
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
            dgvHistorialFacturas.Columns.Add("estado_pago", "Estado de Pago");

            this.Controls.Add(dgvHistorialFacturas);

            btnAbrirFactura = new Button
            {
                Text = "Abrir Factura Seleccionada",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(0, 123, 255),
            };
            btnAbrirFactura.Click += BtnAbrirFactura_Click;
            this.Controls.Add(btnAbrirFactura);

            btnCancelarCredito = new Button
            {
                Text = "Cancelar Crédito (Marcar como Pagado)",
                Dock = DockStyle.Bottom,
                Height = 40,
                Width = 50,
                BackColor = Color.FromArgb(40, 167, 69),
                Visible = false
            };
            btnCancelarCredito.Click += BtnCancelarCredito_Click;
            this.Controls.Add(btnCancelarCredito);

            dgvHistorialFacturas.SelectionChanged += DgvHistorialFacturas_SelectionChanged;

            CargarHistorialFacturas();
        }

        
        private void DgvHistorialFacturas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHistorialFacturas.SelectedRows.Count == 0)
            {
                btnCancelarCredito.Visible = false;
                return;
            }

            string estado = dgvHistorialFacturas.SelectedRows[0].Cells["estado_pago"].Value?.ToString();

            btnCancelarCredito.Visible = estado == "credito";
        }

        
        private void BtnCancelarCredito_Click(object sender, EventArgs e)
        {
            if (dgvHistorialFacturas.SelectedRows.Count == 0) return;

            DialogResult result = MessageBox.Show("¿Desea marcar esta factura como pagada (cancelar crédito)?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            string nombreArchivo = dgvHistorialFacturas.SelectedRows[0].Cells["nombre_archivo"].Value.ToString();

            int idFactura = 0;

            if (nombreArchivo.StartsWith("factura_") && nombreArchivo.EndsWith(".pdf"))
            {
                string idStr = nombreArchivo.Replace("factura_", "").Replace(".pdf", "");
                int.TryParse(idStr, out idFactura);
            }

            if (idFactura > 0)
            {
                using (var conn = Conexion.ObtenerConexion())
                using (var cmd = new NpgsqlCommand("UPDATE factura SET estado_pago = 'contado' WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idFactura);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Crédito cancelado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarHistorialFacturas();
                btnCancelarCredito.Visible = false;
            }
        }


        private void CargarHistorialFacturas()
        {
            dgvHistorialFacturas.Rows.Clear();

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand(@"
        SELECT a.id, a.nombre_archivo, a.fecha, f.estado_pago
        FROM archivo_pdf a
        JOIN factura f ON a.nombre_archivo = CONCAT('factura_', f.id, '.pdf')
        WHERE a.tipo = 'factura'
        ORDER BY a.fecha DESC
        LIMIT 50", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    DateTime fecha = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                    string estadoPago = reader.IsDBNull(3) ? "" : reader.GetString(3);

                    dgvHistorialFacturas.Rows.Add(id, nombre, fecha == DateTime.MinValue ? "" : fecha.ToString("dd/MM/yyyy HH:mm"), estadoPago);
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
