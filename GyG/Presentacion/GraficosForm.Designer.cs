using System.ComponentModel;

namespace GyG.Presentacion;

partial class GraficosForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataVisualization.Charting.Chart chartMasVendidos;
    private System.Windows.Forms.DataVisualization.Charting.Chart chartMenosVendidos;
    private System.Windows.Forms.DataVisualization.Charting.Chart chartRentabilidad;
    private System.Windows.Forms.DataVisualization.Charting.Chart chartHistorialVentas;

    private void InitializeComponent()
    {
        this.chartMasVendidos = new System.Windows.Forms.DataVisualization.Charting.Chart();
        this.chartMenosVendidos = new System.Windows.Forms.DataVisualization.Charting.Chart();
        this.chartRentabilidad = new System.Windows.Forms.DataVisualization.Charting.Chart();
        this.chartHistorialVentas = new System.Windows.Forms.DataVisualization.Charting.Chart();

        ((System.ComponentModel.ISupportInitialize)(this.chartMasVendidos)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chartMenosVendidos)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chartRentabilidad)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chartHistorialVentas)).BeginInit();

        this.SuspendLayout();

        // 
        // chartMasVendidos
        // 
        this.chartMasVendidos.Location = new System.Drawing.Point(12, 12);
        this.chartMasVendidos.Size = new System.Drawing.Size(450, 250);
        var areaMasVendidos = new System.Windows.Forms.DataVisualization.Charting.ChartArea("ChartArea1");
        this.chartMasVendidos.ChartAreas.Add(areaMasVendidos);
        var legendMasVendidos = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        this.chartMasVendidos.Legends.Add(legendMasVendidos);
        this.chartMasVendidos.Name = "chartMasVendidos";
        this.chartMasVendidos.Text = "Productos Más Vendidos";

        // 
        // chartMenosVendidos
        // 
        this.chartMenosVendidos.Location = new System.Drawing.Point(480, 12);
        this.chartMenosVendidos.Size = new System.Drawing.Size(450, 250);
        var areaMenosVendidos = new System.Windows.Forms.DataVisualization.Charting.ChartArea("ChartArea1");
        this.chartMenosVendidos.ChartAreas.Add(areaMenosVendidos);
        var legendMenosVendidos = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        this.chartMenosVendidos.Legends.Add(legendMenosVendidos);
        this.chartMenosVendidos.Name = "chartMenosVendidos";
        this.chartMenosVendidos.Text = "Productos Menos Vendidos";

        // 
        // chartRentabilidad
        // 
        this.chartRentabilidad.Location = new System.Drawing.Point(12, 280);
        this.chartRentabilidad.Size = new System.Drawing.Size(450, 250);
        var areaRentabilidad = new System.Windows.Forms.DataVisualization.Charting.ChartArea("ChartArea1");
        this.chartRentabilidad.ChartAreas.Add(areaRentabilidad);
        var legendRentabilidad = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        this.chartRentabilidad.Legends.Add(legendRentabilidad);
        this.chartRentabilidad.Name = "chartRentabilidad";
        this.chartRentabilidad.Text = "Rentabilidad por Producto";

        // 
        // chartHistorialVentas
        // 
        this.chartHistorialVentas.Location = new System.Drawing.Point(480, 280);
        this.chartHistorialVentas.Size = new System.Drawing.Size(450, 250);
        var areaHistorialVentas = new System.Windows.Forms.DataVisualization.Charting.ChartArea("ChartArea1");
        this.chartHistorialVentas.ChartAreas.Add(areaHistorialVentas);
        var legendHistorialVentas = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
        this.chartHistorialVentas.Legends.Add(legendHistorialVentas);
        this.chartHistorialVentas.Name = "chartHistorialVentas";
        this.chartHistorialVentas.Text = "Historial y Proyección de Ventas";

        // 
        // GraficosForm
        // 
        this.ClientSize = new System.Drawing.Size(950, 650);
        this.Controls.Add(this.chartMasVendidos);
        this.Controls.Add(this.chartMenosVendidos);
        this.Controls.Add(this.chartRentabilidad);
        this.Controls.Add(this.chartHistorialVentas);
        this.Name = "GraficosForm";
        this.Text = "Reportes Gráficos de Ventas";

        ((System.ComponentModel.ISupportInitialize)(this.chartMasVendidos)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chartMenosVendidos)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chartRentabilidad)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chartHistorialVentas)).EndInit();

        this.ResumeLayout(false);
    }
}
