using System;
using System.Windows.Forms;
using GyG.Datos;
using Npgsql;

namespace GyG.Presentacion
{
    public partial class ClienteForm : Form
    {
        private string telefonoOriginal;

        public ClienteForm()
        {
            InitializeComponent();
            CargarResumenClientes();
        }

        public ClienteForm(string nombre, string telefono, string ubicacion) : this()
        {
            txtNombre.Text = nombre;
            txtTelefono.Text = telefono;
            txtUbicacion.Text = ubicacion;
            telefonoOriginal = telefono; // Guardamos el teléfono original para la consulta WHERE
        }

        private void CargarResumenClientes()
        {
            using (var conn = Conexion.ObtenerConexion())
            using (var cmd = new NpgsqlCommand(@"
        SELECT
            c.id,
            c.nombre,
            c.telefono,
            c.ubicacion,
            COUNT(f.id) AS numero_compras,
            COALESCE(SUM(f.total), 0) AS total_compras,
            COUNT(*) FILTER (WHERE f.estado_pago = 'contado') AS contado,
            COUNT(*) FILTER (WHERE f.estado_pago = 'credito') AS credito,
            COUNT(*) FILTER (WHERE f.estado_pago = 'credito' AND f.pagado = false) AS creditos_activos,
            MIN(f.fecha) FILTER (WHERE f.estado_pago = 'credito' AND f.pagado = false) AS fecha_credito_mas_antiguo
        FROM cliente c
        LEFT JOIN factura f ON f.id_cliente = c.id
        GROUP BY c.id, c.nombre, c.telefono, c.ubicacion
        ORDER BY c.nombre;
    ", conn))
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                var dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dgvClientes.DataSource = dt;

                // Opcional: ajustar columnas para mejor presentación
                dgvClientes.Columns["id"].Visible = false;
                dgvClientes.Columns["nombre"].HeaderText = "Nombre";
                dgvClientes.Columns["telefono"].HeaderText = "Teléfono";
                dgvClientes.Columns["ubicacion"].HeaderText = "Ubicación";
                dgvClientes.Columns["numero_compras"].HeaderText = "Número Compras";
                dgvClientes.Columns["total_compras"].HeaderText = "Total Compras";
                dgvClientes.Columns["contado"].HeaderText = "Pagos Contado";
                dgvClientes.Columns["credito"].HeaderText = "Pagos Crédito";
                dgvClientes.Columns["creditos_activos"].HeaderText = "Créditos Activos";
                dgvClientes.Columns["fecha_credito_mas_antiguo"].HeaderText = "Fecha Crédito Más Antiguo";
            }
        }

        
        
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            if (dgvClientes.DataSource is System.Data.DataTable dt)
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    dt.DefaultView.RowFilter = "";
                }
                else
                {
                    // Filtramos por nombre, teléfono o ubicación (ajustar según columnas disponibles)
                    dt.DefaultView.RowFilter = $"nombre LIKE '%{filtro}%' OR telefono LIKE '%{filtro}%' OR ubicacion LIKE '%{filtro}%'";
                }
            }
        }

        
        private void dgvClientes_DoubleClick(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow != null)
            {
                var fila = dgvClientes.CurrentRow;
                string nombre = fila.Cells["nombre"].Value.ToString();
                string telefono = fila.Cells["telefono"].Value.ToString();
                string ubicacion = fila.Cells["ubicacion"].Value == DBNull.Value ? "" : fila.Cells["ubicacion"].Value.ToString();

                // Abrir el formulario para editar
                
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nuevoNombre = txtNombre.Text.Trim();
            string nuevoTelefono = txtTelefono.Text.Trim();
            string nuevaUbicacion = txtUbicacion.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoNombre) || string.IsNullOrWhiteSpace(nuevoTelefono))
            {
                MessageBox.Show("El nombre y el teléfono no pueden estar vacíos.", "Validación");
                return;
            }

            using (var conn = Conexion.ObtenerConexion())
            {
                // Validar que el nuevo teléfono no esté asignado a otro cliente distinto
                using (var validarCmd = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM cliente WHERE telefono = @nuevoTelefono AND telefono != @telefonoOriginal", conn))
                {
                    validarCmd.Parameters.AddWithValue("nuevoTelefono", nuevoTelefono);
                    validarCmd.Parameters.AddWithValue("telefonoOriginal", telefonoOriginal);

                    int conteo = Convert.ToInt32(validarCmd.ExecuteScalar());
                    if (conteo > 0)
                    {
                        MessageBox.Show("El nuevo número de teléfono ya está asignado a otro cliente.", "Error");
                        return;
                    }
                }

                // Actualizar datos del cliente
                using (var cmd = new NpgsqlCommand(
                    "UPDATE cliente SET nombre = @nombre, telefono = @nuevoTelefono, ubicacion = @ubicacion WHERE telefono = @telefonoOriginal", conn))
                {
                    cmd.Parameters.AddWithValue("nombre", nuevoNombre);
                    cmd.Parameters.AddWithValue("nuevoTelefono", nuevoTelefono);
                    cmd.Parameters.AddWithValue("ubicacion", string.IsNullOrWhiteSpace(nuevaUbicacion) ? DBNull.Value : (object)nuevaUbicacion);
                    cmd.Parameters.AddWithValue("telefonoOriginal", telefonoOriginal);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MessageBox.Show("Cliente actualizado correctamente.");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente o no se pudo actualizar.");
                    }
                }
            }
        }
    }
}
