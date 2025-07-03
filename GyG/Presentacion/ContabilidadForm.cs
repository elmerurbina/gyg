using System;
using System.Data;
using System.Windows.Forms;
using GyG.Datos;
using Npgsql;

namespace GyG.Presentacion
{
    public partial class ContabilidadForm : Form
    {
        public ContabilidadForm()
        {
            InitializeComponent();
            CargarReportes();
        }

        private void CargarReportes()
        {
            GenerarEstadoResultados();
            GenerarBalanceGeneral();
            GenerarLibroDiario();
            GenerarLibroMayor();
            GenerarFlujoCaja();
        }

        private void GenerarEstadoResultados()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();

                var ventasNetasCmd = new NpgsqlCommand(
                    "SELECT COALESCE(SUM(total),0) FROM factura WHERE fecha >= @fechaInicio AND fecha <= @fechaFin", conn);
                ventasNetasCmd.Parameters.AddWithValue("@fechaInicio", DateTime.Now.AddMonths(-1));
                ventasNetasCmd.Parameters.AddWithValue("@fechaFin", DateTime.Now);
                decimal ventasNetas = Convert.ToDecimal(ventasNetasCmd.ExecuteScalar());

                var costoVentasCmd = new NpgsqlCommand(
                    @"SELECT COALESCE(SUM(fd.cantidad * p.precio_inventario),0)
      FROM factura_detalle fd
      JOIN producto p ON fd.id_producto = p.id
      JOIN factura f ON fd.id_factura = f.id
      WHERE f.fecha >= @fechaInicio AND f.fecha <= @fechaFin", conn);
                costoVentasCmd.Parameters.AddWithValue("@fechaInicio", DateTime.Now.AddMonths(-1));
                costoVentasCmd.Parameters.AddWithValue("@fechaFin", DateTime.Now);
                decimal costoVentas = Convert.ToDecimal(costoVentasCmd.ExecuteScalar());

                decimal gastosOperativos = 0m;

                decimal utilidadAntesImpuestos = ventasNetas - costoVentas - gastosOperativos;
                decimal impuestoRenta = utilidadAntesImpuestos * 0.30m;
                decimal utilidadNeta = utilidadAntesImpuestos - impuestoRenta;

                // Crear tabla para mostrar en DataGridView
                DataTable dt = new DataTable();
                dt.Columns.Add("Concepto");
                dt.Columns.Add("Monto");

                dt.Rows.Add("Ventas Netas", ventasNetas.ToString("C2"));
                dt.Rows.Add("Costo de Ventas", costoVentas.ToString("C2"));
                dt.Rows.Add("Gastos Operativos", gastosOperativos.ToString("C2"));
                dt.Rows.Add("Utilidad Antes de Impuestos", utilidadAntesImpuestos.ToString("C2"));
                dt.Rows.Add("Impuesto sobre la Renta (30%)", impuestoRenta.ToString("C2"));
                dt.Rows.Add("Utilidad Neta", utilidadNeta.ToString("C2"));

                dgvEstadoResultados.DataSource = dt;
                dgvEstadoResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar Estado de Resultados: " + ex.Message);
            }
        }

        private void GenerarBalanceGeneral()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();

                var activosCmd = new NpgsqlCommand(
                    @"SELECT COALESCE(SUM(stock * precio_inventario),0) FROM producto", conn);
                decimal activos = Convert.ToDecimal(activosCmd.ExecuteScalar());

                decimal pasivos = 0m;

                decimal patrimonio = activos - pasivos;

                DataTable dt = new DataTable();
                dt.Columns.Add("Cuenta");
                dt.Columns.Add("Monto");

                dt.Rows.Add("Activos", activos.ToString("C2"));
                dt.Rows.Add("Pasivos", pasivos.ToString("C2"));
                dt.Rows.Add("Patrimonio", patrimonio.ToString("C2"));

                dgvBalanceGeneral.DataSource = dt;
                dgvBalanceGeneral.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar Balance General: " + ex.Message);
            }
        }

        private void GenerarLibroDiario()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();

                var cmd = new NpgsqlCommand(@"
                    SELECT fecha, 'Venta Factura ' || id AS descripcion, total AS debito, 0 AS credito
                    FROM factura
                    ORDER BY fecha DESC", conn);

                using var reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                dgvLibroDiario.DataSource = dt;
                dgvLibroDiario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar Libro Diario: " + ex.Message);
            }
        }

        private void GenerarLibroMayor()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();

                var cmd = new NpgsqlCommand(@"
                    SELECT p.nombre AS producto, SUM(fd.cantidad * fd.precio_unitario) AS total_ventas
                    FROM factura_detalle fd
                    JOIN producto p ON fd.id_producto = p.id
                    GROUP BY p.nombre
                    ORDER BY p.nombre", conn);

                using var reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                dgvLibroMayor.DataSource = dt;
                dgvLibroMayor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar Libro Mayor: " + ex.Message);
            }
        }

        private void GenerarFlujoCaja()
        {
            try
            {
                using var conn = Conexion.ObtenerConexion();

                var cmd = new NpgsqlCommand(@"
                    SELECT fecha::date AS fecha, SUM(total) AS ingresos
                    FROM factura
                    GROUP BY fecha::date
                    ORDER BY fecha::date", conn);

                using var reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                dgvFlujoCaja.DataSource = dt;
                dgvFlujoCaja.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar Flujo de Caja: " + ex.Message);
            }
        }
    }
}
