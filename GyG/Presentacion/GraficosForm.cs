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
        private int paginaActualGraficos = 0;
        private const int graficosPorPagina = 4;

        // Array con todos los gráficos
        private Chart[] todosGraficos;
        
        private int paginaActualRentabilidad = 0;
        private int productosPorPaginaRentabilidad = 10;
        private List<(string nombre, decimal rentabilidad)> listaRentabilidadCompleta;

        public GraficosForm()
        {
            InitializeComponent();
            todosGraficos = new Chart[]
            {
                chartMasVendidos,
                chartMenosVendidos,
                chartRentabilidad,
                chartHistorialVentas,
                chartClientesMayoresCompras,
                chartClientesContado,
                chartFechasMasVentas
            };

            // Asignar eventos a botones
            btnAnteriorPagina.Click += btnAnteriorPagina_Click;
            btnSiguientePagina.Click += btnSiguientePagina_Click;

            // Mostrar la primera página
            MostrarPaginaGraficos(paginaActualGraficos);

            // Carga inicial de datos en gráficos (tu código original)
            CargarDatosGraficos();
        
            // Botón Anterior
            Button btnAnteriorRentabilidad = new Button
            {
                Text = "Anterior",
                Location = new Point(chartRentabilidad.Location.X, chartRentabilidad.Location.Y + chartRentabilidad.Height + 10)
            };
            btnAnteriorRentabilidad.Click += btnAnteriorRentabilidad_Click;
            this.Controls.Add(btnAnteriorRentabilidad);

// Botón Siguiente
            Button btnSiguienteRentabilidad = new Button
            {
                Text = "Siguiente",
                Location = new Point(btnAnteriorRentabilidad.Location.X + btnAnteriorRentabilidad.Width + 10, btnAnteriorRentabilidad.Location.Y)
            };
            btnSiguienteRentabilidad.Click += btnSiguienteRentabilidad_Click;
            this.Controls.Add(btnSiguienteRentabilidad);

            CargarDatosGraficos();
            
        }

        private void CargarDatosGraficos()
        {
            CargarProductosMasVendidos();
            CargarProductosMenosVendidos();
            CargarRentabilidadProductos();
            CargarProyeccionHistorialVentas();
            CargarClientesMayoresCompras();
            CargarClientesPagosContado();
            CargarFechasConMasVentas();
            
        }

        private void MostrarPaginaGraficos(int pagina)
        {
            int totalGraficos = todosGraficos.Length;
            int start = pagina * graficosPorPagina;
            int end = Math.Min(start + graficosPorPagina, totalGraficos);

            // Ocultar todos inicialmente
            foreach (var chart in todosGraficos)
            {
                chart.Visible = false;
            }

            // Mostrar sólo los gráficos de la página actual, y reposicionarlos
            for (int i = start; i < end; i++)
            {
                todosGraficos[i].Visible = true;

                // Posiciones para 2 columnas y 2 filas por página
                int indexPagina = i - start;
                int fila = indexPagina / 2;
                int columna = indexPagina % 2;

                int posX = 10 + columna * 470; // 470 es ancho de gráfico + margen
                int posY = 10 + fila * 270;    // 270 es alto de gráfico + margen

                todosGraficos[i].Location = new System.Drawing.Point(posX, posY);
            }

            // Opcional: activar/desactivar botones si estás en primera o última página
            btnAnteriorPagina.Enabled = pagina > 0;
            btnSiguientePagina.Enabled = end < totalGraficos;
        }

        private void btnAnteriorPagina_Click(object sender, EventArgs e)
        {
            if (paginaActualGraficos > 0)
            {
                paginaActualGraficos--;
                MostrarPaginaGraficos(paginaActualGraficos);
            }
        }

        private void btnSiguientePagina_Click(object sender, EventArgs e)
        {
            int maxPaginas = (int)Math.Ceiling((double)todosGraficos.Length / graficosPorPagina);
            if (paginaActualGraficos < maxPaginas - 1)
            {
                paginaActualGraficos++;
                MostrarPaginaGraficos(paginaActualGraficos);
            }
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
            ORDER BY rentabilidad DESC", conn);  // Sin LIMIT para traer todo

                using var reader = cmd.ExecuteReader();
                listaRentabilidadCompleta = new List<(string, decimal)>();

                while (reader.Read())
                {
                    string nombre = reader.GetString(0);
                    decimal rentabilidad = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                    listaRentabilidadCompleta.Add((nombre, rentabilidad));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando rentabilidad: " + ex.Message);
            }

            // Cargar primera página
            MostrarPaginaRentabilidad(paginaActualRentabilidad);
        }

        private void CargarClientesMayoresCompras()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
            SELECT c.nombre, SUM(f.total) AS total_compras
            FROM factura f
            JOIN cliente c ON f.id_cliente = c.id
            GROUP BY c.nombre
            ORDER BY total_compras DESC
            LIMIT 5;", conn);

                using var reader = cmd.ExecuteReader();
                var nombres = new List<string>();
                var totales = new List<decimal>();

                while (reader.Read())
                {
                    nombres.Add(reader.GetString(0));
                    totales.Add(reader.GetDecimal(1));
                }

                chartClientesMayoresCompras.Series.Clear();
                var series = new Series("Clientes")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.DarkCyan
                };
                for (int i = 0; i < nombres.Count; i++)
                {
                    series.Points.AddXY(nombres[i], totales[i]);
                }

                chartClientesMayoresCompras.Series.Add(series);
                chartClientesMayoresCompras.ChartAreas[0].AxisX.Interval = 1;
                chartClientesMayoresCompras.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando clientes con mayores compras: " + ex.Message);
            }
        }

        
        private void CargarClientesPagosContado()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
            SELECT c.nombre, COUNT(*) AS num_compras_contado
            FROM factura f
            JOIN cliente c ON f.id_cliente = c.id
            WHERE f.estado_pago = 'contado'
            GROUP BY c.nombre
            ORDER BY num_compras_contado DESC
            LIMIT 5;", conn);

                using var reader = cmd.ExecuteReader();
                var nombres = new List<string>();
                var conteos = new List<int>();

                while (reader.Read())
                {
                    nombres.Add(reader.GetString(0));
                    conteos.Add(reader.GetInt32(1));
                }

                chartClientesContado.Series.Clear();
                var series = new Series("Pagos Contado")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.OrangeRed
                };
                for (int i = 0; i < nombres.Count; i++)
                {
                    series.Points.AddXY(nombres[i], conteos[i]);
                }

                chartClientesContado.Series.Add(series);
                chartClientesContado.ChartAreas[0].AxisX.Interval = 1;
                chartClientesContado.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando clientes que pagan al contado: " + ex.Message);
            }
        }

        
        private void CargarFechasConMasVentas()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();
                var cmd = new NpgsqlCommand(@"
            SELECT fecha::date AS dia, SUM(total) AS total_ventas
            FROM factura
            GROUP BY dia
            ORDER BY total_ventas DESC
            LIMIT 10;", conn);

                using var reader = cmd.ExecuteReader();
                var fechas = new List<string>();
                var totales = new List<decimal>();

                while (reader.Read())
                {
                    fechas.Add(reader.GetDateTime(0).ToString("yyyy-MM-dd"));
                    totales.Add(reader.GetDecimal(1));
                }

                chartFechasMasVentas.Series.Clear();
                var series = new Series("Ventas por Día")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.MediumPurple
                };
                for (int i = 0; i < fechas.Count; i++)
                {
                    series.Points.AddXY(fechas[i], totales[i]);
                }

                chartFechasMasVentas.Series.Add(series);
                chartFechasMasVentas.ChartAreas[0].AxisX.Interval = 1;
                chartFechasMasVentas.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando fechas con más ventas: " + ex.Message);
            }
        }

        
        private void MostrarPaginaRentabilidad(int pagina)
        {
            if (listaRentabilidadCompleta == null || listaRentabilidadCompleta.Count == 0)
                return;

            int skip = pagina * productosPorPaginaRentabilidad;
            var paginaDatos = listaRentabilidadCompleta.Skip(skip).Take(productosPorPaginaRentabilidad).ToList();

            chartRentabilidad.Series.Clear();
            var series = new Series("Rentabilidad")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Blue
            };

            foreach (var item in paginaDatos)
            {
                series.Points.AddXY(item.nombre, item.rentabilidad);
            }

            chartRentabilidad.Series.Add(series);
            chartRentabilidad.ChartAreas[0].AxisX.Interval = 1;
            chartRentabilidad.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
        }

        private void btnAnteriorRentabilidad_Click(object sender, EventArgs e)
        {
            if (paginaActualRentabilidad > 0)
            {
                paginaActualRentabilidad--;
                MostrarPaginaRentabilidad(paginaActualRentabilidad);
            }
        }

        private void btnSiguienteRentabilidad_Click(object sender, EventArgs e)
        {
            int maxPaginas = (int)Math.Ceiling((double)listaRentabilidadCompleta.Count / productosPorPaginaRentabilidad);
            if (paginaActualRentabilidad < maxPaginas - 1)
            {
                paginaActualRentabilidad++;
                MostrarPaginaRentabilidad(paginaActualRentabilidad);
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
