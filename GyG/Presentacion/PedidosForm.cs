using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using GyG.Datos;

namespace GyG.Presentacion
{
    
    public partial class PedidosForm : Form
    {
       
        public PedidosForm()
        {
            InitializeComponent();
            CargarProveedores();
        }

        private void CargarProveedores()
{
    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT id, nombre FROM proveedor", conn))
            using (var reader = cmd.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);

                // Bind a combobox, se recomienda Resetear DataSource antes para evitar problemas
                cmbProveedores.DataSource = null;
                cmbProveedores.DataSource = dt;
                cmbProveedores.DisplayMember = "nombre";
                cmbProveedores.ValueMember = "id";
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al cargar proveedores: " + ex.Message);
    }
}

private void CargarProductos(int umbral)
{
    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = @"
                SELECT id, nombre, descripcion, stock, precio_inventario
                FROM producto
                WHERE stock <= @umbral
                AND id NOT IN (
                    SELECT id_producto
                    FROM pedido_detalle pd
                    JOIN pedido p ON pd.id_pedido = p.id
                    WHERE p.estado = 'solicitado'
                )
                ORDER BY stock ASC";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@umbral", umbral);
                DataTable dt = new DataTable();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }

                dgvProductos.DataSource = null;
                dgvProductos.DataSource = dt;
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al cargar productos: " + ex.Message);
    }
}


private void btnManejarProveedores_Click(object sender, EventArgs e)
{
    var form = new ProveedoresForm();
    form.ShowDialog();

    // Recarga proveedores después de cerrar el formulario para actualizar la lista
    CargarProveedores();
}

        
private void btnFiltrar_Click(object sender, EventArgs e)
{
    int umbral;

    if (!int.TryParse(txtUmbral.Text.Trim(), out umbral) || umbral < 0)
    {
        MessageBox.Show("Por favor ingresa un valor numérico válido para el umbral.", "Valor inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            string query = @"
            SELECT id, nombre, descripcion, stock
            FROM producto
            WHERE stock <= @umbral AND id NOT IN (
                SELECT id_producto
                FROM pedido_detalle pd
                JOIN pedido p ON pd.id_pedido = p.id
                WHERE p.estado = 'solicitado'
            )
            ORDER BY stock ASC";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@umbral", umbral);
                DataTable dt = new DataTable();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
                dgvProductos.DataSource = dt;
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al filtrar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        
        private void btnBuscarProductos_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUmbral.Text.Trim(), out int umbral))
            {
                MessageBox.Show("Por favor ingresa un número válido para el umbral.");
                return; // Sale si no es válido
            }

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT id, nombre, descripcion, stock, precio_inventario
                        FROM producto
                        WHERE stock <= @umbral
                        AND id NOT IN (
                            SELECT id_producto
                            FROM pedido_detalle pd
                            JOIN pedido p ON pd.id_pedido = p.id
                            WHERE p.estado = 'solicitado'
                        )", conn))
                    {
                        cmd.Parameters.AddWithValue("@umbral", umbral);
                        DataTable dt = new DataTable();
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        dgvProductos.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message);
            }
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            var form = new AgregarProveedorForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarProveedores();
            }
        }

        private void btnGenerarPedido_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un producto para generar el pedido.");
                return;
            }

            if (cmbProveedores.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            int idProveedor = (int)cmbProveedores.SelectedValue;
            DataTable productosSeleccionados = ((DataTable)dgvProductos.DataSource).Clone();

            foreach (DataGridViewRow row in dgvProductos.SelectedRows)
            {
                productosSeleccionados.ImportRow(((DataRowView)row.DataBoundItem).Row);
            }

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Insertar en pedido
                        int idPedido;
                        using (var cmd = new NpgsqlCommand("INSERT INTO pedido (id_proveedor, estado) VALUES (@prov, 'solicitado') RETURNING id", conn))
                        {
                            cmd.Parameters.AddWithValue("@prov", idProveedor);
                            idPedido = (int)cmd.ExecuteScalar();
                        }

                        // Insertar detalles
                        foreach (DataRow producto in productosSeleccionados.Rows)
                        {
                            using (var cmd = new NpgsqlCommand(@"
                                INSERT INTO pedido_detalle (id_pedido, id_producto, cantidad, precio_compra, precio_venta)
                                VALUES (@id_pedido, @id_producto, @cantidad, @precio_compra, @precio_venta)", conn))
                            {
                                cmd.Parameters.AddWithValue("@id_pedido", idPedido);
                                cmd.Parameters.AddWithValue("@id_producto", (int)producto["id"]);
                                cmd.Parameters.AddWithValue("@cantidad", 10); // Por ahora fijo
                                cmd.Parameters.AddWithValue("@precio_compra", (decimal)producto["precio_inventario"]);
                                cmd.Parameters.AddWithValue("@precio_venta", (decimal)producto["precio_inventario"] * 1.25m);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Pedido generado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el pedido: " + ex.Message);
            }
        }
    }
}
