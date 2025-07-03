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

        private void InitializeComponent()
        {
            this.panelGraficosScrollable = new Panel();
            this.chartMasVendidos = new Chart();
            this.chartMenosVendidos = new Chart();
            this.chartRentabilidad = new Chart();
            this.chartHistorialVentas = new Chart();
            this.chartClientesMayoresCompras = new Chart();
            this.chartClientesContado = new Chart();
            this.chartFechasMasVentas = new Chart();

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

            // 
            // panelGraficosScrollable
            // 
            this.panelGraficosScrollable.AutoScroll = true;
            this.panelGraficosScrollable.Location = new System.Drawing.Point(10, 10);
            this.panelGraficosScrollable.Name = "panelGraficosScrollable";
            this.panelGraficosScrollable.Size = new System.Drawing.Size(940, 560);
            this.panelGraficosScrollable.TabIndex = 0;

            // Helper para configurar los charts
            void ConfigurarChart(Chart chart, string titulo)
            {
                chart.Size = new System.Drawing.Size(450, 250);
                chart.ChartAreas.Clear();
                chart.Legends.Clear();

                var area = new ChartArea("ChartArea1");
                chart.ChartAreas.Add(area);

                var legend = new Legend("Legend1");
                chart.Legends.Add(legend);

                chart.Name = titulo.Replace(" ", "");
                chart.Text = titulo;
            }

            // Configurar los gráficos
            ConfigurarChart(chartMasVendidos, "Productos Más Vendidos");
            ConfigurarChart(chartMenosVendidos, "Productos Menos Vendidos");
            ConfigurarChart(chartRentabilidad, "Rentabilidad por Producto");
            ConfigurarChart(chartHistorialVentas, "Historial y Proyección de Ventas");
            ConfigurarChart(chartClientesMayoresCompras, "Clientes con Mayores Compras");
            ConfigurarChart(chartClientesContado, "Clientes que Pagan al Contado");
            ConfigurarChart(chartFechasMasVentas, "Fechas con Más Ventas");

            // Añadimos todos los charts al panel (posiciones iniciales fijas)
            // La visibilidad se controlará desde el código
            this.chartMasVendidos.Location = new System.Drawing.Point(10, 10);
            this.chartMenosVendidos.Location = new System.Drawing.Point(480, 10);
            this.chartRentabilidad.Location = new System.Drawing.Point(10, 270);
            this.chartHistorialVentas.Location = new System.Drawing.Point(480, 270);
            this.chartClientesMayoresCompras.Location = new System.Drawing.Point(10, 10);  // mismo lugar que chartMasVendidos
            this.chartClientesContado.Location = new System.Drawing.Point(480, 10);        // mismo lugar que chartMenosVendidos
            this.chartFechasMasVentas.Location = new System.Drawing.Point(10, 270);       // mismo lugar que chartRentabilidad

            // Agregar charts al panel
            this.panelGraficosScrollable.Controls.Add(this.chartMasVendidos);
            this.panelGraficosScrollable.Controls.Add(this.chartMenosVendidos);
            this.panelGraficosScrollable.Controls.Add(this.chartRentabilidad);
            this.panelGraficosScrollable.Controls.Add(this.chartHistorialVentas);
            this.panelGraficosScrollable.Controls.Add(this.chartClientesMayoresCompras);
            this.panelGraficosScrollable.Controls.Add(this.chartClientesContado);
            this.panelGraficosScrollable.Controls.Add(this.chartFechasMasVentas);

            // 
            // btnAnteriorPagina
            // 
            this.btnAnteriorPagina.Location = new System.Drawing.Point(300, 580);
            this.btnAnteriorPagina.Name = "btnAnteriorPagina";
            this.btnAnteriorPagina.Size = new System.Drawing.Size(120, 30);
            this.btnAnteriorPagina.Text = "Anterior";
            this.btnAnteriorPagina.UseVisualStyleBackColor = true;
            // El evento lo asignarás en el form.cs

            // 
            // btnSiguientePagina
            // 
            this.btnSiguientePagina.Location = new System.Drawing.Point(520, 580);
            this.btnSiguientePagina.Name = "btnSiguientePagina";
            this.btnSiguientePagina.Size = new System.Drawing.Size(120, 30);
            this.btnSiguientePagina.Text = "Siguiente";
            this.btnSiguientePagina.UseVisualStyleBackColor = true;
            // El evento lo asignarás en el form.cs

            // 
            // GraficosForm
            // 
            this.ClientSize = new System.Drawing.Size(960, 630);
            this.Controls.Add(this.panelGraficosScrollable);
            this.Controls.Add(this.btnAnteriorPagina);
            this.Controls.Add(this.btnSiguientePagina);
            this.Name = "GraficosForm";
            this.Text = "Reportes Gráficos de Ventas";

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
