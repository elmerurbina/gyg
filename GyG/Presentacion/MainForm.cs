using System;
using System.Windows.Forms;
using GyG.Datos;
using GyG.Presentacion.Controles;
using Npgsql;

namespace GyG.Presentacion
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InicializarLayout();
            MostrarNotificacionesDeInventario();
        }

        private void InicializarLayout()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Peleteria Sacuanjoche";
            this.BackColor = Color.White;

            // Crear Sidebar
            var sidebar = new SidebarControl
            {
                Dock = DockStyle.Left
            };
            sidebar.OpcionSeleccionada += Sidebar_OpcionSeleccionada;
            this.Controls.Add(sidebar);

            // Panel de alertas
            // Panel de alertas centrado
            var panelAlertas = new Panel
            {
                Name = "panelAlertas",
                Width = 500,
                Height = 130,
                BackColor = Color.LightYellow,
                Top = 100, // Espacio desde arriba (ajustable si hay header)
                Left = (this.ClientSize.Width - 500) / 2, // Centrado horizontal
                Anchor = AnchorStyles.Top, // Solo arriba, no Dock
                Padding = new Padding(10)
            };
            this.Controls.Add(panelAlertas);
            this.Resize += (s, e) =>
            {
                // Mantener centrado cuando se cambia tamaño de ventana
                panelAlertas.Left = (this.ClientSize.Width - panelAlertas.Width) / 2;
            };

// Labels dentro del panel
            var lblBajoStock = new Label
            {
                Name = "lblBajoStock",
                AutoSize = false,
                Width = 480,
                Height = 60,
                Top = 5,
                Left = 10,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkGoldenrod,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblVencimiento = new Label
            {
                Name = "lblVencimiento",
                AutoSize = false,
                Width = 480,
                Height = 60,
                Top = 65,
                Left = 10,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkGoldenrod,
                TextAlign = ContentAlignment.MiddleCenter
            };

           

            panelAlertas.Controls.Add(lblVencimiento);
            panelAlertas.Controls.Add(lblBajoStock);
            this.Controls.Add(panelAlertas);
        }

        
        
      private void MostrarNotificacionesDeInventario()
{
    var lblBajoStock = this.Controls.Find("lblBajoStock", true).FirstOrDefault() as Label;
    var lblVencimiento = this.Controls.Find("lblVencimiento", true).FirstOrDefault() as Label;

    if (lblBajoStock == null || lblVencimiento == null) return;

    lblBajoStock.Text = "";
    lblVencimiento.Text = "";

    using (var conn = Conexion.ObtenerConexion())
    {
        // Leer bajo stock
        string mensajeStock = "";
        using (var cmd = new NpgsqlCommand(@"
            SELECT nombre, stock
            FROM producto
            WHERE stock <= 10
            ORDER BY stock ASC
            LIMIT 5", conn))
        using (var reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                mensajeStock = "⚠️ Productos con bajo stock (≤ 10):\n";
                while (reader.Read())
                {
                    string nombre = reader.GetString(0);
                    int stock = reader.GetInt32(1);
                    mensajeStock += $"- {nombre} (Stock: {stock})\n";
                }
            }
        }
        lblBajoStock.Text = mensajeStock.Trim();

        // Leer vencimiento (nueva conexión para evitar conflicto)
        using (var conn2 = Conexion.ObtenerConexion())
        using (var cmd2 = new NpgsqlCommand(@"
            SELECT nombre, fecha_vencimiento
            FROM producto
            WHERE fecha_vencimiento IS NOT NULL
              AND fecha_vencimiento <= CURRENT_DATE + INTERVAL '30 days'
            ORDER BY fecha_vencimiento ASC
            LIMIT 5", conn2))
        using (var reader2 = cmd2.ExecuteReader())
        {
            if (reader2.HasRows)
            {
                string mensaje = "📅 Productos por vencerse (30 días):\n";
                while (reader2.Read())
                {
                    string nombre = reader2.GetString(0);
                    DateTime fecha = reader2.GetDateTime(1);
                    mensaje += $"- {nombre} (Vence: {fecha:dd/MM/yyyy})\n";
                }
                lblVencimiento.Text = mensaje.Trim();
            }
        }
    }
}



       private void Sidebar_OpcionSeleccionada(object sender, string opcion)
{
    try
    {
        Form ventana = opcion switch
        {
            "Inventario" => new InventarioForm(),
            "Ventas" => new VentasForm(),
            "Proformas" => new ProformaForm(),
            "Pedidos" => new PedidosForm(),
            "Contabilidad" => new ContabilidadForm(),
            "Gráficos" => new GraficosForm(),
            "Clientes" => new ClienteForm(),
            _ => null
        };

        if (ventana != null)
        {
            ventana.StartPosition = FormStartPosition.CenterScreen;
            ventana.WindowState = FormWindowState.Normal;
            ventana.ShowDialog(); // Usar ShowDialog para que sea modal
            
            // O si prefieres que no sea modal:
            // ventana.Show();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al abrir {opcion}: {ex.Message}", "Error", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
    }
}