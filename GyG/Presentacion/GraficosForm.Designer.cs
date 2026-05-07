using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

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
        private Panel pnlBotones;

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
            this.pnlBotones = new Panel();
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
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();

            // ========== PALETA DE COLORES SACUANJOCHE ==========
            Color primary500 = Color.FromArgb(139, 94, 60);      // #8B5E3C
            Color primary300 = Color.FromArgb(196, 164, 132);    // #C4A484
            Color secondary500 = Color.FromArgb(255, 193, 7);    // #FFC107
            Color textPrimary = Color.FromArgb(45, 41, 38);      // #2D2926
            Color textWhite = Color.White;
            Color formBg = Color.FromArgb(255, 249, 230);        // Secundario 100
            Color panelBg = Color.FromArgb(243, 235, 225);       // Primario 100
            Color inputBg = Color.FromArgb(249, 249, 249);       // Gris 100
            
            // ========== FORM CONFIGURATION ==========
            this.BackColor = formBg;
            this.Text = "Reportes Gráficos - Sacuanjoche";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(1200, 800);
            this.MinimumSize = new Size(1000, 650);
            this.Font = new Font("Segoe UI", 9F);
            
            // ========== PANEL BOTONES (PARTE SUPERIOR) ==========
            this.pnlBotones.BackColor = panelBg;
            this.pnlBotones.Dock = DockStyle.Top;
            this.pnlBotones.Height = 70;
            this.pnlBotones.Padding = new Padding(15);
            
            // Botón Anterior
            this.btnAnteriorPagina.Text = "◀ ANTERIOR";
            this.btnAnteriorPagina.Size = new Size(150, 45);
            this.btnAnteriorPagina.Location = new Point(15, 12);
            this.btnAnteriorPagina.BackColor = primary500;
            this.btnAnteriorPagina.ForeColor = textWhite;
            this.btnAnteriorPagina.FlatStyle = FlatStyle.Flat;
            this.btnAnteriorPagina.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAnteriorPagina.Cursor = Cursors.Hand;
            this.btnAnteriorPagina.FlatAppearance.BorderSize = 0;
            this.btnAnteriorPagina.FlatAppearance.MouseOverBackColor = primary300;
            
            // Botón Siguiente
            this.btnSiguientePagina.Text = "SIGUIENTE ▶";
            this.btnSiguientePagina.Size = new Size(150, 45);
            this.btnSiguientePagina.Location = new Point(175, 12);
            this.btnSiguientePagina.BackColor = secondary500;
            this.btnSiguientePagina.ForeColor = textPrimary;
            this.btnSiguientePagina.FlatStyle = FlatStyle.Flat;
            this.btnSiguientePagina.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSiguientePagina.Cursor = Cursors.Hand;
            this.btnSiguientePagina.FlatAppearance.BorderSize = 0;
            this.btnSiguientePagina.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 220, 100);
            
            this.pnlBotones.Controls.Add(this.btnAnteriorPagina);
            this.pnlBotones.Controls.Add(this.btnSiguientePagina);
            
            // ========== PANEL SCROLLABLE PARA GRÁFICOS ==========
            this.panelGraficosScrollable.AutoScroll = true;
            this.panelGraficosScrollable.BackColor = formBg;
            this.panelGraficosScrollable.Dock = DockStyle.Fill;
            this.panelGraficosScrollable.Padding = new Padding(20);
            
            // Helper para configurar los charts
            void ConfigurarChart(Chart chart, string titulo)
            {
                chart.Size = new Size(500, 280);
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                
                chart.BackColor = panelBg;
                chart.BorderlineColor = primary300;
                chart.BorderlineWidth = 1;
                chart.BorderSkin.BackColor = panelBg;
                
                var area = new ChartArea("ChartArea1");
                area.BackColor = formBg;
                area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
                area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
                area.AxisX.MajorGrid.LineColor = primary300;
                area.AxisY.MajorGrid.LineColor = primary300;
                chart.ChartAreas.Add(area);

                var legend = new Legend("Legend1");
                legend.BackColor = Color.Transparent;
                legend.ForeColor = textPrimary;
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

            // ========== CENTRAR LOS GRÁFICOS ==========
            int chartWidth = 520;
            int chartHeight = 280;
            int spacing = 30;
            
            // Calcular centro del panel
            int panelCenter = (this.panelGraficosScrollable.Width / 2) - (chartWidth / 2);
            
            // Usar AnchorStyles para centrar automáticamente
            foreach (Chart chart in new Chart[] { chartMasVendidos, chartMenosVendidos, chartRentabilidad, 
                                                  chartHistorialVentas, chartClientesMayoresCompras, 
                                                  chartClientesContado, chartFechasMasVentas })
            {
                chart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }
            
            // Posiciones específicas
            this.chartMasVendidos.Location = new Point(panelCenter, 20);
            this.chartMenosVendidos.Location = new Point(panelCenter + chartWidth + spacing, 20);
            
            int row2Y = 20 + chartHeight + 30;
            this.chartRentabilidad.Location = new Point(panelCenter, row2Y);
            this.chartHistorialVentas.Location = new Point(panelCenter + chartWidth + spacing, row2Y);
            
            int row3Y = row2Y + chartHeight + 30;
            this.chartClientesMayoresCompras.Location = new Point(panelCenter, row3Y);
            this.chartClientesContado.Location = new Point(panelCenter + chartWidth + spacing, row3Y);
            
            int row4Y = row3Y + chartHeight + 30;
            this.chartFechasMasVentas.Location = new Point(panelCenter, row4Y);
            this.chartFechasMasVentas.Size = new Size((chartWidth * 2) + spacing, chartHeight);

            // Agregar charts al panel
            this.panelGraficosScrollable.Controls.Add(this.chartMasVendidos);
            this.panelGraficosScrollable.Controls.Add(this.chartMenosVendidos);
            this.panelGraficosScrollable.Controls.Add(this.chartRentabilidad);
            this.panelGraficosScrollable.Controls.Add(this.chartHistorialVentas);
            this.panelGraficosScrollable.Controls.Add(this.chartClientesMayoresCompras);
            this.panelGraficosScrollable.Controls.Add(this.chartClientesContado);
            this.panelGraficosScrollable.Controls.Add(this.chartFechasMasVentas);

            // Agregar controles al formulario
            this.Controls.Add(this.panelGraficosScrollable);
            this.Controls.Add(this.pnlBotones);

            // Ajustar posición al redimensionar
            this.Resize += (s, e) => CentrarGraficos();
            
            ((ISupportInitialize)(this.chartMasVendidos)).EndInit();
            ((ISupportInitialize)(this.chartMenosVendidos)).EndInit();
            ((ISupportInitialize)(this.chartRentabilidad)).EndInit();
            ((ISupportInitialize)(this.chartHistorialVentas)).EndInit();
            ((ISupportInitialize)(this.chartClientesMayoresCompras)).EndInit();
            ((ISupportInitialize)(this.chartClientesContado)).EndInit();
            ((ISupportInitialize)(this.chartFechasMasVentas)).EndInit();

            this.panelGraficosScrollable.ResumeLayout(false);
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        
        private void CentrarGraficos()
        {
            // Calcular centro para reposicionar gráficos
            int chartWidth = 520;
            int chartHeight = 280;
            int spacing = 30;
            
            int panelCenter = (this.panelGraficosScrollable.Width / 2) - (chartWidth / 2);
            if (panelCenter < 10) panelCenter = 10;
            
            this.chartMasVendidos.Location = new Point(panelCenter, 20);
            this.chartMenosVendidos.Location = new Point(panelCenter + chartWidth + spacing, 20);
            
            int row2Y = 20 + chartHeight + 30;
            this.chartRentabilidad.Location = new Point(panelCenter, row2Y);
            this.chartHistorialVentas.Location = new Point(panelCenter + chartWidth + spacing, row2Y);
            
            int row3Y = row2Y + chartHeight + 30;
            this.chartClientesMayoresCompras.Location = new Point(panelCenter, row3Y);
            this.chartClientesContado.Location = new Point(panelCenter + chartWidth + spacing, row3Y);
            
            int row4Y = row3Y + chartHeight + 30;
            this.chartFechasMasVentas.Location = new Point(panelCenter, row4Y);
            this.chartFechasMasVentas.Size = new Size((chartWidth * 2) + spacing, chartHeight);
        }
    }
}