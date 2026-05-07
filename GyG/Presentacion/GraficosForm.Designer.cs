using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GyG.Presentacion
{
    partial class GraficosForm
    {
        private IContainer components = null;

        private Panel panelGraficosScrollable;
        private Chart chartMasVendidos;
        private Chart chartMenosVendidos;
        private Chart chartRentabilidad;
        private Chart chartHistorialVentas;
        private Chart chartClientesMayoresCompras;
        private Chart chartClientesContado;
        private Chart chartFechasMasVentas;

        private Button btnAnteriorPagina;
        private Button btnSiguientePagina;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            
            // Crear los charts explícitamente
            this.chartMasVendidos = new Chart();
            this.chartMenosVendidos = new Chart();
            this.chartRentabilidad = new Chart();
            this.chartHistorialVentas = new Chart();
            this.chartClientesMayoresCompras = new Chart();
            this.chartClientesContado = new Chart();
            this.chartFechasMasVentas = new Chart();
            
            this.panelGraficosScrollable = new Panel();
            this.btnAnteriorPagina = new Button();
            this.btnSiguientePagina = new Button();

            ((ISupportInitialize)(this.chartMasVendidos)).BeginInit();
            ((ISupportInitialize)(this.chartMenosVendidos)).BeginInit();
            ((ISupportInitialize)(this.chartRentabilidad)).BeginInit();
            ((ISupportInitialize)(this.chartHistorialVentas)).BeginInit();
            ((ISupportInitialize)(this.chartClientesMayoresCompras)).BeginInit();
            ((ISupportInitialize)(this.chartClientesContado)).BeginInit();
            ((ISupportInitialize)(this.chartFechasMasVentas)).BeginInit();

            this.panelGraficosScrollable.SuspendLayout();
            this.SuspendLayout();

            // ========== PALETA SACUANJOCHE ==========
            this.BackColor = Color.FromArgb(255, 249, 230);
            
            // 
            // panelGraficosScrollable
            // 
            this.panelGraficosScrollable.AutoScroll = true;
            this.panelGraficosScrollable.BackColor = Color.FromArgb(255, 249, 230);
            this.panelGraficosScrollable.Dock = DockStyle.Fill;
            this.panelGraficosScrollable.Location = new Point(10, 10);
            this.panelGraficosScrollable.Size = new Size(940, 560);
            
            // Helper para configurar los charts
            void ConfigurarChart(Chart chart, string titulo)
            {
                chart.Size = new Size(450, 250);
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                
                chart.Anchor = AnchorStyles.None;
                chart.BackColor = Color.FromArgb(243, 235, 225);
                chart.BorderlineColor = Color.FromArgb(196, 164, 132);
                chart.BorderlineWidth = 1;
                
                var area = new ChartArea("ChartArea1");
                area.BackColor = Color.FromArgb(255, 249, 230);
                area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
                area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
                chart.ChartAreas.Add(area);

                var legend = new Legend("Legend1");
                legend.BackColor = Color.Transparent;
                chart.Legends.Add(legend);

                chart.Name = titulo.Replace(" ", "");
                chart.Text = titulo;
            }

            // Configurar los gráficos
            ConfigurarChart(chartMasVendidos, "Productos Más Vendidos");
            ConfigurarChart(chartMenosVendidos, "Productos Menos Vendidos");
            ConfigurarChart(chartRentabilidad, "Rentabilidad por Producto");
            ConfigurarChart(chartHistorialVentas, "Historial de Ventas");
            ConfigurarChart(chartClientesMayoresCompras, "Clientes con Mayores Compras");
            ConfigurarChart(chartClientesContado, "Clientes que Pagan al Contado");
            ConfigurarChart(chartFechasMasVentas, "Fechas con Más Ventas");

            // Posiciones
            this.chartMasVendidos.Location = new Point(10, 10);
            this.chartMenosVendidos.Location = new Point(480, 10);
            this.chartRentabilidad.Location = new Point(10, 270);
            this.chartHistorialVentas.Location = new Point(480, 270);
            this.chartClientesMayoresCompras.Location = new Point(10, 530);
            this.chartClientesContado.Location = new Point(480, 530);
            this.chartFechasMasVentas.Location = new Point(10, 790);

            // Agregar charts al panel
            this.panelGraficosScrollable.Controls.Add(this.chartMasVendidos);
            this.panelGraficosScrollable.Controls.Add(this.chartMenosVendidos);
            this.panelGraficosScrollable.Controls.Add(this.chartRentabilidad);
            this.panelGraficosScrollable.Controls.Add(this.chartHistorialVentas);
            this.panelGraficosScrollable.Controls.Add(this.chartClientesMayoresCompras);
            this.panelGraficosScrollable.Controls.Add(this.chartClientesContado);
            this.panelGraficosScrollable.Controls.Add(this.chartFechasMasVentas);

            // Botones de navegación (opcionales, puedes quitarlos)
            this.btnAnteriorPagina.Visible = false;
            this.btnSiguientePagina.Visible = false;

            // 
            // GraficosForm
            // 
            this.ClientSize = new Size(960, 630);
            this.Controls.Add(this.panelGraficosScrollable);
            this.Name = "GraficosForm";
            this.Text = "Reportes Gráficos - Sacuanjoche";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;

            ((ISupportInitialize)(this.chartMasVendidos)).EndInit();
            ((ISupportInitialize)(this.chartMenosVendidos)).EndInit();
            ((ISupportInitialize)(this.chartRentabilidad)).EndInit();
            ((ISupportInitialize)(this.chartHistorialVentas)).EndInit();
            ((ISupportInitialize)(this.chartClientesMayoresCompras)).EndInit();
            ((ISupportInitialize)(this.chartClientesContado)).EndInit();
            ((ISupportInitialize)(this.chartFechasMasVentas)).EndInit();

            this.panelGraficosScrollable.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}