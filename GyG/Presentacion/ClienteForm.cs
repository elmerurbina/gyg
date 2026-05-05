using System;
using System.Data;
using System.Windows.Forms;
using GyG.Datos;
using Npgsql;
using NpgsqlTypes;

namespace GyG.Presentacion
{
    public partial class ClienteForm : Form
    {
        private string telefonoOriginal;
        private bool esNuevoCliente = true;

        public ClienteForm()
        {
            InitializeComponent();
            CargarResumenClientes();
            esNuevoCliente = true;
            telefonoOriginal = "";
        }

        public ClienteForm(string nombre, string telefono, string ubicacion) : this()
        {
            txtNombre.Text = nombre;
            txtTelefono.Text = telefono;
            txtUbicacion.Text = ubicacion;
            telefonoOriginal = telefono;
            esNuevoCliente = false;
        }

        private void CargarResumenClientes()
        {
            using (var conn = Conexion.ObtenerConexion())
            {
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
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        var dt = new System.Data.DataTable();
                        adapter.Fill(dt);
                        dgvClientes.DataSource = dt;

                        if (dgvClientes.Columns.Contains("id"))
                            dgvClientes.Columns["id"].Visible = false;
                        if (dgvClientes.Columns.Contains("nombre"))
                            dgvClientes.Columns["nombre"].HeaderText = "Nombre";
                        if (dgvClientes.Columns.Contains("telefono"))
                            dgvClientes.Columns["telefono"].HeaderText = "Teléfono";
                        if (dgvClientes.Columns.Contains("ubicacion"))
                            dgvClientes.Columns["ubicacion"].HeaderText = "Ubicación";
                        if (dgvClientes.Columns.Contains("numero_compras"))
                            dgvClientes.Columns["numero_compras"].HeaderText = "Número Compras";
                        if (dgvClientes.Columns.Contains("total_compras"))
                            dgvClientes.Columns["total_compras"].HeaderText = "Total Compras";
                        if (dgvClientes.Columns.Contains("contado"))
                            dgvClientes.Columns["contado"].HeaderText = "Pagos Contado";
                        if (dgvClientes.Columns.Contains("credito"))
                            dgvClientes.Columns["credito"].HeaderText = "Pagos Crédito";
                        if (dgvClientes.Columns.Contains("creditos_activos"))
                            dgvClientes.Columns["creditos_activos"].HeaderText = "Créditos Activos";
                        if (dgvClientes.Columns.Contains("fecha_credito_mas_antiguo"))
                            dgvClientes.Columns["fecha_credito_mas_antiguo"].HeaderText = "Fecha Crédito Más Antiguo";
                    }
                }
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
                    filtro = filtro.Replace("'", "''");
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

                var formEdicion = new ClienteForm(nombre, telefono, ubicacion);
                formEdicion.ShowDialog();
                CargarResumenClientes();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nuevoNombre = txtNombre.Text.Trim();
            string nuevoTelefono = txtTelefono.Text.Trim();
            string nuevaUbicacion = txtUbicacion.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoNombre) || string.IsNullOrWhiteSpace(nuevoTelefono))
            {
                MessageBox.Show("El nombre y el teléfono no pueden estar vacíos.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // NO llamar a conn.Open() porque ObtenerConexion() ya devuelve la conexión abierta
            using (var conn = Conexion.ObtenerConexion())
            {
                // NO poner conn.Open() aquí - la conexión ya está abierta
                
                if (esNuevoCliente)
                {
                    InsertarNuevoCliente(conn, nuevoNombre, nuevoTelefono, nuevaUbicacion);
                }
                else
                {
                    ActualizarClienteExistente(conn, nuevoNombre, nuevoTelefono, nuevaUbicacion);
                }
            }
        }

        private void InsertarNuevoCliente(NpgsqlConnection conn, string nombre, string telefono, string ubicacion)
        {
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT sp_insert_cliente(@p_nombre, @p_telefono, @p_ubicacion)", conn))
                {
                    var paramNombre = new NpgsqlParameter("p_nombre", NpgsqlDbType.Varchar);
                    paramNombre.Value = nombre;
                    cmd.Parameters.Add(paramNombre);
                    
                    var paramTelefono = new NpgsqlParameter("p_telefono", NpgsqlDbType.Varchar);
                    paramTelefono.Value = telefono;
                    cmd.Parameters.Add(paramTelefono);
                    
                    var paramUbicacion = new NpgsqlParameter("p_ubicacion", NpgsqlDbType.Text);
                    paramUbicacion.Value = string.IsNullOrWhiteSpace(ubicacion) ? DBNull.Value : (object)ubicacion;
                    cmd.Parameters.Add(paramUbicacion);
                    
                    cmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Cliente guardado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el cliente: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarClienteExistente(NpgsqlConnection conn, string nuevoNombre, string nuevoTelefono, string nuevaUbicacion)
        {
            try
            {
                // Validar que el nuevo teléfono no esté asignado a otro cliente distinto
                using (var validarCmd = new NpgsqlCommand("SELECT COUNT(*) FROM cliente WHERE telefono = @nuevoTelefono AND telefono != @telefonoOriginal", conn))
                {
                    var paramNuevoTelefono = new NpgsqlParameter("nuevoTelefono", NpgsqlDbType.Varchar);
                    paramNuevoTelefono.Value = nuevoTelefono;
                    validarCmd.Parameters.Add(paramNuevoTelefono);
                    
                    var paramTelefonoOriginal = new NpgsqlParameter("telefonoOriginal", NpgsqlDbType.Varchar);
                    paramTelefonoOriginal.Value = telefonoOriginal;
                    validarCmd.Parameters.Add(paramTelefonoOriginal);
                    
                    int conteo = Convert.ToInt32(validarCmd.ExecuteScalar());
                    if (conteo > 0)
                    {
                        MessageBox.Show("El nuevo número de teléfono ya está asignado a otro cliente.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Actualizar datos del cliente
                using (var cmd = new NpgsqlCommand("UPDATE cliente SET nombre = @nombre, telefono = @nuevoTelefono, ubicacion = @ubicacion WHERE telefono = @telefonoOriginal", conn))
                {
                    var paramNombre = new NpgsqlParameter("nombre", NpgsqlDbType.Varchar);
                    paramNombre.Value = nuevoNombre;
                    cmd.Parameters.Add(paramNombre);
                    
                    var paramNuevoTelefono = new NpgsqlParameter("nuevoTelefono", NpgsqlDbType.Varchar);
                    paramNuevoTelefono.Value = nuevoTelefono;
                    cmd.Parameters.Add(paramNuevoTelefono);
                    
                    var paramUbicacion = new NpgsqlParameter("ubicacion", NpgsqlDbType.Text);
                    paramUbicacion.Value = string.IsNullOrWhiteSpace(nuevaUbicacion) ? DBNull.Value : (object)nuevaUbicacion;
                    cmd.Parameters.Add(paramUbicacion);
                    
                    var paramTelefonoOriginal = new NpgsqlParameter("telefonoOriginal", NpgsqlDbType.Varchar);
                    paramTelefonoOriginal.Value = telefonoOriginal;
                    cmd.Parameters.Add(paramTelefonoOriginal);
                    
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MessageBox.Show("Cliente actualizado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente o no se pudo actualizar.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el cliente: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}