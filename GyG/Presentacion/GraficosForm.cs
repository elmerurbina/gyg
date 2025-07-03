using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GyG.Datos;
using Npgsql;

namespace GyG.Presentacion
{
    public partial class GraficosForm : Form
    {
        public GraficosForm()
        {
            InitializeComponent();
            CargarDatosGraficos();
        }

        private void CargarDatosGraficos()
        {
            CargarProductosMasVendidos();
            CargarProductosMenosVendidos();
            CargarRentabilidadProductos();
            CargarProyeccionHistorialVentas();
        }

        private void CargarProductosMasVendidos()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
                    SELECT p.nombre, SUM(fd.cantidad) AS total_vendido
                    FROM factura_detalle fd
                    JOIN producto p ON fd.id_producto = p.id
                    GROUP BY p.nombre
                    ORDER BY total_vendido DESC
                    LIMIT 5;", conn);

                using var reader = cmd.ExecuteReader();
                var nombres = new System.Collections.Generic.List<string>();
                var cantidades = new System.Collections.Generic.List<int>();

                while (reader.Read())
                {
                    nombres.Add(reader.GetString(0));
                    cantidades.Add(reader.GetInt32(1));
                }

                chartMasVendidos.Series.Clear();
                var series = new Series("Más Vendidos")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.Green
                };
                for (int i = 0; i < nombres.Count; i++)
                {
                    series.Points.AddXY(nombres[i], cantidades[i]);
                }

                chartMasVendidos.Series.Add(series);
                chartMasVendidos.ChartAreas[0].AxisX.Interval = 1;
                chartMasVendidos.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando productos más vendidos: " + ex.Message);
            }
        }

        private void CargarProductosMenosVendidos()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
                    SELECT p.nombre, COALESCE(SUM(fd.cantidad), 0) AS total_vendido
                    FROM producto p
                    LEFT JOIN factura_detalle fd ON fd.id_producto = p.id
                    GROUP BY p.nombre
                    ORDER BY total_vendido ASC
                    LIMIT 5;", conn);

                using var reader = cmd.ExecuteReader();
                var nombres = new System.Collections.Generic.List<string>();
                var cantidades = new System.Collections.Generic.List<int>();

                while (reader.Read())
                {
                    nombres.Add(reader.GetString(0));
                    cantidades.Add(reader.IsDBNull(1) ? 0 : reader.GetInt32(1));
                }

                chartMenosVendidos.Series.Clear();
                var series = new Series("Menos Vendidos")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.Red
                };
                for (int i = 0; i < nombres.Count; i++)
                {
                    series.Points.AddXY(nombres[i], cantidades[i]);
                }

                chartMenosVendidos.Series.Add(series);
                chartMenosVendidos.ChartAreas[0].AxisX.Interval = 1;
                chartMenosVendidos.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando productos menos vendidos: " + ex.Message);
            }
        }

        private void CargarRentabilidadProductos()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
                    SELECT p.nombre,
                           SUM(fd.cantidad * (fd.precio_unitario - p.precio_inventario)) AS rentabilidad
                    FROM factura_detalle fd
                    JOIN producto p ON fd.id_producto = p.id
                    GROUP BY p.nombre
                    ORDER BY rentabilidad DESC
                    LIMIT 10;", conn);

                using var reader = cmd.ExecuteReader();
                var nombres = new System.Collections.Generic.List<string>();
                var rentabilidades = new System.Collections.Generic.List<decimal>();

                while (reader.Read())
                {
                    nombres.Add(reader.GetString(0));
                    rentabilidades.Add(reader.IsDBNull(1) ? 0 : reader.GetDecimal(1));
                }

                chartRentabilidad.Series.Clear();
                var series = new Series("Rentabilidad")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.Blue
                };
                for (int i = 0; i < nombres.Count; i++)
                {
                    series.Points.AddXY(nombres[i], rentabilidades[i]);
                }

                chartRentabilidad.Series.Add(series);
                chartRentabilidad.ChartAreas[0].AxisX.Interval = 1;
                chartRentabilidad.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando rentabilidad: " + ex.Message);
            }
        }

        private void CargarProyeccionHistorialVentas()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
                    SELECT to_char(f.fecha, 'YYYY-MM') AS mes, SUM(fd.cantidad * fd.precio_unitario) AS total_ventas
                    FROM factura f
                    JOIN factura_detalle fd ON f.id = fd.id_factura
                    GROUP BY mes
                    ORDER BY mes;", conn);

                using var reader = cmd.ExecuteReader();
                var meses = new System.Collections.Generic.List<string>();
                var totales = new System.Collections.Generic.List<decimal>();

                while (reader.Read())
                {
                    meses.Add(reader.GetString(0));
                    totales.Add(reader.GetDecimal(1));
                }

                chartHistorialVentas.Series.Clear();
                var series = new Series("Ventas")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.Purple,
                    BorderWidth = 3,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 7
                };

                for (int i = 0; i < meses.Count; i++)
                {
                    series.Points.AddXY(meses[i], totales[i]);
                }

                chartHistorialVentas.Series.Add(series);
                chartHistorialVentas.ChartAreas[0].AxisX.Interval = 1;
                chartHistorialVentas.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando historial y proyección de ventas: " + ex.Message);
            }
        }
    }
}
