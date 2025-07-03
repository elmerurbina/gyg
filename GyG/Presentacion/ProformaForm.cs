using System.Data;
using GyG.Datos;
using Npgsql;
using System.Diagnostics;
using System.Text.Json;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextDocument = iTextSharp.text.Document;
using iTextParagraph = iTextSharp.text.Paragraph;
using iTextFontFactory = iTextSharp.text.FontFactory;
using iTextPdfWriter = iTextSharp.text.pdf.PdfWriter;
using iTextPdfPTable = iTextSharp.text.pdf.PdfPTable;
using iTextPageSize = iTextSharp.text.PageSize;
using iTextElement = iTextSharp.text.Element;
using iTextRectangle = iTextSharp.text.Rectangle;
using iTextSharp.text;

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
                // Ejecutar la venta
                CompletarVentaDesdeProforma(idProforma);

                // Deshabilitar el botón
                var cell = (DataGridViewButtonCell)dgvProformas.Rows[e.RowIndex].Cells["CompletarVenta"];
                cell.Value = "Venta Completa";
                cell.ReadOnly = true;
                cell.Style.ForeColor = Color.Gray;

                // Opcional: refrescar grilla
                CargarProformas();
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
            LEFT JOIN archivo_pdf a ON a.tipo = 'proforma' AND a.id_relacionado = p.id

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


    private void CompletarVentaDesdeProforma(int idProforma)
    {
        try
        {
            // Obtener detalles de la proforma
            List<ProductoCarrito> productos = new List<ProductoCarrito>();
            int idCliente = 0;
            decimal total = 0;
            decimal descuento = 0;

            using (var conn = Conexion.ObtenerConexion())
            {
                // Cliente y total
                using (var cmd = new NpgsqlCommand(@"
                SELECT id_cliente, total, descuento
                FROM proforma
                WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idProforma);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idCliente = reader.GetInt32(0);
                            total = reader.GetDecimal(1);
                            descuento = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                        }
                    }
                }

             
                // Detalles
                using (var cmd = new NpgsqlCommand(@"
    SELECT d.id_producto, d.cantidad, d.precio_unitario,
           p.iva, p.descuento,
           p.nombre, p.descripcion
    FROM proforma_detalle d
    JOIN producto p ON p.id = d.id_producto
    WHERE d.id_proforma = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idProforma);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new ProductoCarrito
                            {
                                Id = reader.GetInt32(0),
                                Cantidad = reader.GetInt32(1),
                                PrecioUnitario = reader.GetDecimal(2),
                                IVA = reader.GetDecimal(3),
                                Descuento = reader.GetDecimal(4),
                                Nombre = reader.GetString(5),
                                Descripcion = reader.GetString(6),
                            });

                        }
                    }
                }


                // Serializar carrito en JSON
                var detallesJson = JsonSerializer.Serialize(
                    productos.Select(c => new
                    {
                        id = c.Id,
                        cantidad = c.Cantidad,
                        precioUnitario = c.PrecioUnitario,
                        IVA = c.IVA,
                        Descuento = c.Descuento
                    })
                );

                // Insertar en factura
                int idFacturaGenerada;
                using (var cmd = new NpgsqlCommand(
                           "SELECT sp_insert_factura(@id_cliente, @estado_pago, @total, @descuento, @detalles::json);",
                           conn))
                {
                    cmd.Parameters.AddWithValue("id_cliente", idCliente);
                    cmd.Parameters.AddWithValue("estado_pago", "efectivo"); // O permitir que el usuario elija
                    cmd.Parameters.AddWithValue("total", total);
                    cmd.Parameters.AddWithValue("descuento", descuento);
                    cmd.Parameters.AddWithValue("detalles", detallesJson);
                    idFacturaGenerada = Convert.ToInt32(cmd.ExecuteScalar());

                    // Actualizar stock
                    foreach (var item in productos)
                    {
                        using (var cmdStock = new NpgsqlCommand(
                                   "UPDATE producto SET stock = stock - @cantidad WHERE id = @id_producto;", conn))
                        {
                            cmdStock.Parameters.AddWithValue("cantidad", item.Cantidad);
                            cmdStock.Parameters.AddWithValue("id_producto", item.Id);
                            cmdStock.ExecuteNonQuery();
                        }
                    }
                }

                // Generar y guardar PDF
                GenerarYGuardarPDFFactura(idFacturaGenerada, conn.ConnectionString);
            }

            MessageBox.Show("Venta registrada exitosamente desde proforma.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al completar la venta: " + ex.Message);
        }
    }


    public void GenerarYGuardarPDFFactura(int idFactura, string connectionString)
    {
        byte[] pdfBytes;

        using (MemoryStream ms = new MemoryStream())
        {
            iTextDocument doc = new iTextDocument(iTextPageSize.A4, 50, 50, 50, 50);
            iTextPdfWriter writer = iTextPdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Cabecera
            var titulo = new iTextParagraph("FERRETERÍA GyG\nFACTURA",
                iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_BOLD, 20));
            titulo.Alignment = iTextElement.ALIGN_CENTER;
            doc.Add(titulo);

            doc.Add(new iTextParagraph(" ")); // Espacio

            // Obtener info del cliente y productos
            string clienteNombre = "", telefonoCliente = "", fecha = "";
            decimal total = 0;
            DataTable productos = new DataTable();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(@"
                SELECT c.nombre, c.telefono, f.total, f.fecha
                FROM factura f
                JOIN cliente c ON f.id_cliente = c.id
                WHERE f.id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idFactura);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clienteNombre = reader.GetString(0);
                            telefonoCliente = reader.GetString(1);
                            total = reader.GetDecimal(2);
                            fecha = reader.GetDateTime(3).ToString("dd/MM/yyyy HH:mm");
                        }
                    }
                }

                using (var da = new NpgsqlDataAdapter(@"
                SELECT 
                    prod.nombre AS producto,
                    prod.descripcion,
                    d.cantidad,
                    d.precio_unitario,
                    prod.iva,
                    prod.descuento,
                    d.subtotal
                FROM factura_detalle d
                JOIN producto prod ON d.id_producto = prod.id
                WHERE d.id_factura = @id", conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", idFactura);
                    da.Fill(productos);
                }
            }

            // Encabezado cliente
            doc.Add(new iTextParagraph($"N° Factura: {idFactura}"));
            doc.Add(new iTextParagraph($"Cliente: {clienteNombre}"));
            doc.Add(new iTextParagraph($"Teléfono: {telefonoCliente}"));
            doc.Add(new iTextParagraph($"Fecha: {fecha}"));
            doc.Add(new iTextParagraph(" "));

            // Tabla
            iTextPdfPTable table = new iTextPdfPTable(7);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 20, 25, 10, 10, 10, 10, 15 });

            table.AddCell("Producto");
            table.AddCell("Descripción");
            table.AddCell("Cant.");
            table.AddCell("Precio");
            table.AddCell("IVA");
            table.AddCell("Desc.");
            table.AddCell("Subtotal");

            foreach (DataRow row in productos.Rows)
            {
                decimal precio = Convert.ToDecimal(row["precio_unitario"]);
                int cantidad = Convert.ToInt32(row["cantidad"]);
                decimal iva = Convert.ToDecimal(row["iva"]);
                decimal descuento = Convert.ToDecimal(row["descuento"]);
                decimal subtotal = Convert.ToDecimal(row["subtotal"]);

                table.AddCell(row["producto"].ToString());
                table.AddCell(row["descripcion"].ToString());
                table.AddCell(cantidad.ToString());
                table.AddCell($"${precio:F2}");
                table.AddCell($"{iva}%");
                table.AddCell($"{descuento}%");
                table.AddCell($"${subtotal:F2}");
            }

            doc.Add(table);
            doc.Add(new iTextParagraph(" "));

            // Total
            doc.Add(new iTextParagraph($"TOTAL: ${total:F2}",
                iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_BOLD, 14)));
            doc.Add(new iTextParagraph(" "));

            // Dibujo de líneas de firma y texto debajo usando PdfContentByte
            PdfContentByte cb = writer.DirectContent;
            float yLinea = doc.BottomMargin + 50;
            float anchoLinea = 60;

            // Línea izquierda: Entregué Conforme
            float xEntregue = doc.LeftMargin;
            cb.MoveTo(xEntregue, yLinea);
            cb.LineTo(xEntregue + anchoLinea, yLinea);
            cb.Stroke();

            // Línea derecha: Recibí Conforme
            float xRecibi = doc.PageSize.Width - doc.RightMargin - anchoLinea;
            cb.MoveTo(xRecibi, yLinea);
            cb.LineTo(xRecibi + anchoLinea, yLinea);
            cb.Stroke();

            // Texto debajo de las líneas
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);

            cb.ShowTextAligned(Element.ALIGN_CENTER, "Entregué Conforme", xEntregue + anchoLinea / 2, yLinea - 12, 0);
            cb.ShowTextAligned(Element.ALIGN_CENTER, "Recibí Conforme", xRecibi + anchoLinea / 2, yLinea - 12, 0);

            cb.EndText();

            // Mensaje final
            doc.Add(new iTextParagraph(" "));
            doc.Add(new iTextParagraph("Gracias por su compra.",
                iTextFontFactory.GetFont(iTextFontFactory.HELVETICA_OBLIQUE, 12)));

            doc.Close();
            pdfBytes = ms.ToArray();
        }

        // Guardar en BD y abrir archivo
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(@"
            INSERT INTO archivo_pdf(nombre_archivo, tipo, contenido)
            VALUES (@nombre, @tipo, @contenido)", conn))
            {
                cmd.Parameters.AddWithValue("@nombre", $"factura_{idFactura}.pdf");
                cmd.Parameters.AddWithValue("@tipo", "factura");
                cmd.Parameters.AddWithValue("@contenido", pdfBytes);
                cmd.ExecuteNonQuery();
            }

            string nombreArchivo = $"factura_{idFactura}.pdf";
            string rutaTemporal = Path.Combine(Path.GetTempPath(), nombreArchivo);
            File.WriteAllBytes(rutaTemporal, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = rutaTemporal,
                UseShellExecute = true
            });
        }
    }


    public class ProductoCarritoProforma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal => (PrecioUnitario * Cantidad) * (1 - (Descuento / 100));
    }


    private string GuardarArchivoDesdeDB(int idProforma)
    {
        using var conn = Conexion.ObtenerConexion();
        using var cmd = new NpgsqlCommand(@"
        SELECT nombre_archivo, contenido
        FROM archivo_pdf
        WHERE tipo = 'proforma' AND id_relacionado = @id
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
