using GyG.Datos;
using Npgsql;
using System.Diagnostics;

namespace GyG.Presentacion;

public partial class ProformaForm : Form
{
    public ProformaForm()
    {
        InitializeComponent();
        ConfigurarGrid();
        CargarProformas();
    }

    private void ConfigurarGrid()
    {
        dgvProformas.Columns.Clear();
        dgvProformas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProformas.AllowUserToAddRows = false;
        dgvProformas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvProformas.Columns.Add("Id", "ID");
        dgvProformas.Columns["Id"].Visible = false;

        dgvProformas.Columns.Add("Cliente", "Cliente");
        dgvProformas.Columns.Add("Fecha", "Fecha");
        dgvProformas.Columns.Add("Total", "Total");

        // Botón para ver el archivo PDF
        var btnArchivo = new DataGridViewButtonColumn
        {
            Name = "ArchivoPDF",
            HeaderText = "Archivo",
            Text = "Ver PDF",
            UseColumnTextForButtonValue = true
        };
        dgvProformas.Columns.Add(btnArchivo);

        // Botón para completar venta
        var btnCompletar = new DataGridViewButtonColumn
        {
            Name = "CompletarVenta",
            HeaderText = "Acción",
            Text = "Completar Venta",
            UseColumnTextForButtonValue = true
        };
        dgvProformas.Columns.Add(btnCompletar);

        dgvProformas.CellClick += dgvProformas_CellContentClicked;
    }

    private void dgvProformas_CellContentClicked(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        string columnName = dgvProformas.Columns[e.ColumnIndex].Name;
        int idProforma = (int)dgvProformas.Rows[e.RowIndex].Cells["Id"].Value;

        if (columnName == "ArchivoPDF")
        {
            string archivo = GuardarArchivoDesdeDB(idProforma);

            if (!string.IsNullOrEmpty(archivo) && File.Exists(archivo))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = archivo,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir el archivo PDF: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Archivo no encontrado o no disponible.");
            }
        }

        if (columnName == "CompletarVenta")
        {
            string textoBoton = dgvProformas.Rows[e.RowIndex].Cells["CompletarVenta"].Value?.ToString();
            if (textoBoton == "Venta Completa") return;

            var confirm = MessageBox.Show("¿Desea completar la venta para esta proforma?", "Confirmar",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                // TODO: Agregar lógica para registrar venta en la tabla factura
                MessageBox.Show("Venta completada (simulado)");
                CargarProformas(); // Recargar datos
            }
        }
    }

    private void CargarProformas()
    {
        dgvProformas.Rows.Clear();

        using (var conn = Conexion.ObtenerConexion())
        using (var cmd = new NpgsqlCommand(@"
            SELECT p.id, c.nombre, p.fecha, p.total, a.nombre_archivo,
                   CASE WHEN f.id IS NULL THEN 'Pendiente' ELSE 'Venta completada' END as estado
            FROM proforma p
            JOIN cliente c ON p.id_cliente = c.id
            LEFT JOIN factura f ON f.id = p.id
            LEFT JOIN archivo_pdf a ON a.tipo = 'proforma' AND a.id = p.id
            ORDER BY p.fecha DESC;
        ", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                int idProforma = reader.GetInt32(0);
                string cliente = reader.GetString(1);
                string fecha = reader.GetDateTime(2).ToString("dd/MM/yyyy HH:mm");
                decimal total = reader.GetDecimal(3);
                string archivoNombre = reader.IsDBNull(4) ? null : reader.GetString(4);
                string estado = reader.GetString(5);

                int rowIndex = dgvProformas.Rows.Add(idProforma, cliente, fecha, total.ToString("C2"));

                if (archivoNombre != null)
                {
                    dgvProformas.Rows[rowIndex].Cells["ArchivoPDF"].Tag = true; // Indica que sí hay PDF
                }
                else
                {
                    dgvProformas.Rows[rowIndex].Cells["ArchivoPDF"].Value = "Sin archivo";
                    dgvProformas.Rows[rowIndex].Cells["ArchivoPDF"].ReadOnly = true;
                }

                var cellCompletar = (DataGridViewButtonCell)dgvProformas.Rows[rowIndex].Cells["CompletarVenta"];
                if (estado == "Venta completada")
                {
                    cellCompletar.Value = "Venta Completa";
                    cellCompletar.Style.ForeColor = Color.Gray;
                    cellCompletar.ReadOnly = true;
                }
            }
        }
    }

    private string GuardarArchivoDesdeDB(int idProforma)
    {
        using var conn = Conexion.ObtenerConexion();
        using var cmd = new NpgsqlCommand(@"
            SELECT nombre_archivo, contenido
            FROM archivo_pdf
            WHERE tipo = 'proforma' AND id = @id
            LIMIT 1;
        ", conn);

        cmd.Parameters.AddWithValue("@id", idProforma);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            string nombreArchivo = reader.GetString(0);
            byte[] contenido = (byte[])reader["contenido"];

            string ruta = Path.Combine(Path.GetTempPath(), nombreArchivo);
            File.WriteAllBytes(ruta, contenido);
            return ruta;
        }

        return null;
    }
}
