using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GyG.Datos;
using Npgsql;
using ZXing;
using ZXing.Windows.Compatibility; // Necesario para BitmapLuminanceSource

namespace GyG.Presentacion
{
    public partial class LectorCodigoForm : Form
    {
        private FilterInfoCollection dispositivos;
        private VideoCaptureDevice fuenteVideo;
        // NUEVO: Checkbox para activar/desactivar entrada manual
        private CheckBox chkIngresoManual;
        private TextBox txtCodigoManual;
        private Button btnBuscarManual;


        private System.Windows.Forms.Timer timerEscaneo;  // Timer para escanear continuamente la cámara
        private System.Windows.Forms.Timer timeoutTimer;   // Timer para timeout de 8 segundos
        private bool codigoEncontrado = false;

        public string CodigoDetectado { get; private set; }

        public LectorCodigoForm()
        {
            InitializeComponent();

            // Inicializar timers explícitamente con System.Windows.Forms.Timer
            timerEscaneo = new System.Windows.Forms.Timer();
            timerEscaneo.Interval = 200; // cada 200 ms revisa la imagen
            timerEscaneo.Tick += TimerEscaneo_Tick;

            timeoutTimer = new System.Windows.Forms.Timer();
            timeoutTimer.Interval = 8000; // 8 segundos timeout
            timeoutTimer.Tick += TimeoutTimer_Tick;
        }

        private void LectorCodigoForm_Load(object sender, EventArgs e)
        {
            dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in dispositivos)
                cbCamaras.Items.Add(device.Name);

            if (cbCamaras.Items.Count > 0)
                cbCamaras.SelectedIndex = 0;

            btnTerminar.Click += BtnTerminar_Click;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            codigoEncontrado = false;

            fuenteVideo = new VideoCaptureDevice(dispositivos[cbCamaras.SelectedIndex].MonikerString);
            fuenteVideo.NewFrame += Capturar;
            fuenteVideo.Start();

            timerEscaneo.Start();
            timeoutTimer.Start();

            lblEstado.Text = "Escaneando código...";
        }

        private void Capturar(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pbCamara.Image = bitmap;
        }

        private void TimerEscaneo_Tick(object sender, EventArgs e)
        {
            if (pbCamara.Image == null || codigoEncontrado)
                return;

            var barcodeReader = new BarcodeReader<Bitmap>(
                bitmap => new BitmapLuminanceSource(bitmap)
            );

            var resultado = barcodeReader.Decode((Bitmap)pbCamara.Image);

            if (resultado != null)
            {
                codigoEncontrado = true;
                CodigoDetectado = resultado.Text;
                txtCodigo.Text = CodigoDetectado;

                timerEscaneo.Stop();
                timeoutTimer.Stop();
                DetenerCamara();

                lblEstado.Text = $"Código detectado: {CodigoDetectado}";

                BuscarProductoPorCodigo(CodigoDetectado);
            }
        }
        
        private void chkIngresoManual_CheckedChanged(object sender, EventArgs e)
        {
            bool manual = chkIngresoManual.Checked;

            txtCodigoManual.Enabled = manual;
            btnBuscarManual.Enabled = manual;

            if (manual)
            {
                timerEscaneo.Stop();
                timeoutTimer.Stop();
                DetenerCamara();
                lblEstado.Text = "Modo manual activado. Escriba el código y presione Buscar.";
            }
            else
            {
                txtCodigoManual.Clear();
                btnBuscarManual.Enabled = false;
                txtCodigoManual.Enabled = false;
                lblEstado.Text = "Modo escaneo activado. Inicie la cámara.";
            }
        }

        private void btnBuscarManual_Click(object sender, EventArgs e)
        {
            string codigoManual = txtCodigoManual.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigoManual))
            {
                MessageBox.Show("Ingrese un código antes de buscar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtCodigo.Text = codigoManual;
            BuscarProductoPorCodigo(codigoManual);
        }


        private void TimeoutTimer_Tick(object sender, EventArgs e)
        {
            timeoutTimer.Stop();
            timerEscaneo.Stop();
            DetenerCamara();

            if (!codigoEncontrado)
            {
                MessageBox.Show("No se pudo escanear el código en el tiempo esperado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "No se detectó código.";
            }
        }

        private void DetenerCamara()
        {
            if (fuenteVideo != null && fuenteVideo.IsRunning)
            {
                fuenteVideo.SignalToStop();
                fuenteVideo.WaitForStop();
            }
        }

        private void BuscarProductoPorCodigo(string codigo)
        {
            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT * FROM producto WHERE codigo = @codigo LIMIT 1";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("codigo", codigo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Producto encontrado, mostrar info
                                string nombre = reader["nombre"]?.ToString();
                                string descripcion = reader["descripcion"]?.ToString();
                                string categoria = reader["categoria"]?.ToString();
                                decimal precioInv = reader["precio_inventario"] != DBNull.Value ? (decimal)reader["precio_inventario"] : 0;
                                decimal precioVenta = reader["precio_venta"] != DBNull.Value ? (decimal)reader["precio_venta"] : 0;
                                int stock = reader["stock"] != DBNull.Value ? (int)reader["stock"] : 0;
                                DateTime? fechaVencimiento = reader["fecha_vencimiento"] != DBNull.Value ? (DateTime?)reader["fecha_vencimiento"] : null;
                                decimal iva = reader["iva"] != DBNull.Value ? (decimal)reader["iva"] : 0;
                                decimal descuento = reader["descuento"] != DBNull.Value ? (decimal)reader["descuento"] : 0;

                                // Mostrar en UI para edición o confirmación
                                txtNombre.Text = nombre;
                                txtDescripcion.Text = descripcion;
                                txtCategoria.Text = categoria;
                                numPrecioInv.Value = precioInv;
                                numPrecioVenta.Value = precioVenta;
                                numStock.Value = stock;
                                dtpFechaExpiracion.Value = fechaVencimiento ?? DateTime.Today;
                                dtpFechaExpiracion.Checked = fechaVencimiento.HasValue;
                                numIVA.Value = iva;
                                numDescuento.Value = descuento;

                                lblEstado.Text = "Producto encontrado. Puede editar o confirmar.";
                            }
                            else
                            {
                                // Producto no existe, habilitar ingreso nuevo
                                MessageBox.Show("Código no encontrado. Por favor, ingrese los datos para registrar el producto.", "Producto no encontrado");
                                lblEstado.Text = "Ingresar datos para nuevo producto.";

                                HabilitarIngresoProducto();
                                txtCodigo.Text = codigo; // Código detectado
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar producto: " + ex.Message);
                lblEstado.Text = "Error en búsqueda.";
            }
        }

        private void HabilitarIngresoProducto()
        {
            txtNombre.Enabled = true;
            txtDescripcion.Enabled = true;
            txtCategoria.Enabled = true;
            numPrecioInv.Enabled = true;
            numPrecioVenta.Enabled = true;
            numStock.Enabled = true;
            dtpFechaExpiracion.Enabled = true;
            numIVA.Enabled = true;
            numDescuento.Enabled = true;
            btnGuardar.Enabled = true;
        }

        private void BtnTerminar_Click(object sender, EventArgs e)
        {
            timerEscaneo.Stop();
            timeoutTimer.Stop();
            DetenerCamara();
            this.Close();
        }

        private void LectorCodigoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetenerCamara();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar datos mínimos antes de guardar
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numPrecioVenta.Value <= 0)
            {
                MessageBox.Show("El precio de venta debe ser mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numStock.Value < 0)
            {
                MessageBox.Show("El stock no puede ser negativo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calcular precio final (ejemplo simple)
            decimal precioFinal = numPrecioVenta.Value - (numPrecioVenta.Value * (numDescuento.Value / 100)) + (numPrecioVenta.Value * (numIVA.Value / 100));

            RegistrarProducto(
                txtNombre.Text.Trim(),
                txtDescripcion.Text.Trim(),
                txtCategoria.Text.Trim(),
                numPrecioInv.Value,
                numPrecioVenta.Value,
                (int)numStock.Value,
                txtCodigo.Text.Trim(),
                dtpFechaExpiracion.Checked ? dtpFechaExpiracion.Value.Date : (DateTime?)null,
                numIVA.Value,
                numDescuento.Value,
                precioFinal
            );
        }

        // Método para registrar el producto usando el procedimiento almacenado
        private void RegistrarProducto(string nombre, string descripcion, string categoria, decimal precioInv, decimal precioVenta,
    int stock, string codigo, DateTime? fechaVencimiento, decimal iva, decimal descuento, decimal precioFinal)
{
    try
    {
        using (var conn = Conexion.ObtenerConexion())
        {
          using (var cmd = new NpgsqlCommand(@"
    SELECT sp_insert_producto(
        CAST(@p_nombre AS VARCHAR),
        CAST(@p_descripcion AS TEXT),
        CAST(@p_categoria AS VARCHAR),
        CAST(@p_precio_inventario AS NUMERIC),
        CAST(@p_precio_venta AS NUMERIC),
        CAST(@p_stock AS INT),
        CAST(@p_codigo AS VARCHAR),
        CAST(@p_fecha_vencimiento AS DATE),
        CAST(@p_iva AS NUMERIC),
        CAST(@p_descuento AS NUMERIC),
        CAST(@p_precio_final AS NUMERIC)
    );", conn))

            {
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("p_nombre", nombre);
                cmd.Parameters.AddWithValue("p_descripcion", descripcion);
                cmd.Parameters.AddWithValue("p_categoria", string.IsNullOrWhiteSpace(categoria) ? (object)DBNull.Value : categoria);
                cmd.Parameters.AddWithValue("p_precio_inventario", precioInv);
                cmd.Parameters.AddWithValue("p_precio_venta", precioVenta);
                cmd.Parameters.AddWithValue("p_stock", stock);
                cmd.Parameters.AddWithValue("p_codigo", codigo);
                cmd.Parameters.AddWithValue("p_fecha_vencimiento", fechaVencimiento ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("p_iva", iva);
                cmd.Parameters.AddWithValue("p_descuento", descuento);
                cmd.Parameters.AddWithValue("p_precio_final", precioFinal);

                cmd.ExecuteNonQuery();
            }
        }

        MessageBox.Show("Producto registrado correctamente.", "Registro exitoso");
        lblEstado.Text = "Producto registrado.";
        btnGuardar.Enabled = false;
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al registrar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        lblEstado.Text = "Error en registro.";
    }
}

    }
}
