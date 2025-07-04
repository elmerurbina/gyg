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
        private TextBox txtBuscar;
        private Button btnBuscar;


        public HistorialFacturasForm()
        {
            InitializeComponent();
             this.Text = "Historial de Facturas - Ventas";
    this.Size = new Size(800, 500);
    this.StartPosition = FormStartPosition.CenterParent;

    // Crear un contenedor principal
    var mainLayout = new TableLayoutPanel
    {
        Dock = DockStyle.Fill,
        RowCount = 3,
        ColumnCount = 1,
    };
    mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));   // Buscador
    mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // DGV ocupa lo que sobra
    mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));   // Botones

    this.Controls.Add(mainLayout);

    // Panel de búsqueda
    var panelBusqueda = new Panel
    {
        Dock = DockStyle.Fill,
        Padding = new Padding(10),
    };

    txtBuscar = new TextBox
    {
        PlaceholderText = "Buscar por nombre o ID de factura...",
        Width = 300,
        Anchor = AnchorStyles.Left,
    };
    btnBuscar = new Button
    {
        Text = "Buscar",
        Width = 80,
        BackColor = Color.FromArgb(0, 153, 51),
        ForeColor = Color.White,
        Anchor = AnchorStyles.Left,
        Left = txtBuscar.Right + 10
    };
    btnBuscar.Click += BtnBuscar_Click;

    panelBusqueda.Controls.Add(txtBuscar);
    panelBusqueda.Controls.Add(btnBuscar);

    // Alinear el botón junto al textbox
    txtBuscar.Location = new Point(10, 40);
    btnBuscar.Location = new Point(txtBuscar.Right + 12, 40);

    mainLayout.Controls.Add(panelBusqueda, 0, 0);

    // DGV
    dgvHistorialFacturas = new DataGridView
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    dgvHistorialFacturas.Columns.Add("id", "ID");
    dgvHistorialFacturas.Columns["id"].Visible = false;
    dgvHistorialFacturas.Columns.Add("nombre_archivo", "Nombre Archivo");
    dgvHistorialFacturas.Columns.Add("fecha", "Fecha");
    dgvHistorialFacturas.Columns.Add("estado_pago", "Estado de Pago");

    dgvHistorialFacturas.SelectionChanged += DgvHistorialFacturas_SelectionChanged;

    mainLayout.Controls.Add(dgvHistorialFacturas, 0, 1);

    // Panel de botones
    var panelBotones = new Panel
    {
        Dock = DockStyle.Fill,
        Padding = new Padding(10),
    };

    btnAbrirFactura = new Button
    {
        Text = "Abrir Factura Seleccionada",
        Width = 200,
        Height = 35,
        BackColor = Color.FromArgb(0, 123, 255),
        ForeColor = Color.White,
        Anchor = AnchorStyles.Right
    };
    btnAbrirFactura.Click += BtnAbrirFactura_Click;

    btnCancelarCredito = new Button
    {
        Text = "Cancelar Crédito (Marcar como Pagado)",
        Width = 250,
        Height = 35,
        BackColor = Color.FromArgb(40, 167, 69),
        ForeColor = Color.White,
        Visible = false,
        Anchor = AnchorStyles.Right
    };
    btnCancelarCredito.Click += BtnCancelarCredito_Click;

    panelBotones.Controls.Add(btnCancelarCredito);
    panelBotones.Controls.Add(btnAbrirFactura);

    // Posicionar botones a la derecha
    panelBotones.Resize += (s, e) =>
    {
        btnCancelarCredito.Location = new Point(panelBotones.Width - btnCancelarCredito.Width - 10, 10);
        btnAbrirFactura.Location = new Point(panelBotones.Width - btnCancelarCredito.Width - btnAbrirFactura.Width - 20, 10);
    };

    mainLayout.Controls.Add(panelBotones, 0, 2);

    // Cargar datos
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


        private void CargarHistorialFacturas(string filtro = "")
        {
            dgvHistorialFacturas.Rows.Clear();

            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand(@"
        SELECT a.id, a.nombre_archivo, a.fecha, f.estado_pago
        FROM archivo_pdf a
        JOIN factura f ON a.nombre_archivo = CONCAT('factura_', f.id, '.pdf')
        WHERE a.tipo = 'factura'
        " + (string.IsNullOrWhiteSpace(filtro) ? "" : "AND (LOWER(a.nombre_archivo) LIKE @filtro OR f.id::TEXT LIKE @filtro)") + @"
        ORDER BY 
            CASE f.estado_pago WHEN 'credito' THEN 0 ELSE 1 END,
            a.fecha DESC
        LIMIT 50;
    ", conn))
            {
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro.ToLower() + "%");
                }

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
        }

        
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            CargarHistorialFacturas(filtro);
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