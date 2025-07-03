using System;
using System.Windows.Forms;
using GyG.Presentacion.Controles;

namespace GyG.Presentacion
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InicializarLayout();
        }

        private void InicializarLayout()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Sistema GyG";
            this.BackColor = System.Drawing.Color.White;

            // Crear Sidebar
            var sidebar = new SidebarControl
            {
                Dock = DockStyle.Left
            };

            sidebar.OpcionSeleccionada += Sidebar_OpcionSeleccionada;

            this.Controls.Add(sidebar);
        }

        private void Sidebar_OpcionSeleccionada(object sender, string opcion)
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
                ventana.Show(); // o ventana.ShowDialog() si querés modal
            }
        }
    }
}