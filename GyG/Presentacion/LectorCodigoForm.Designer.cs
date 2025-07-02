namespace GyG.Presentacion
{
    partial class LectorCodigoForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cbCamaras;
        private System.Windows.Forms.PictureBox pbCamara;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnTerminar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label lblEstado;

        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.TextBox txtCategoria;

        private System.Windows.Forms.NumericUpDown numPrecioInv;
        private System.Windows.Forms.NumericUpDown numPrecioVenta;
        private System.Windows.Forms.NumericUpDown numStock;
        private System.Windows.Forms.NumericUpDown numIVA;
        private System.Windows.Forms.NumericUpDown numDescuento;

        private System.Windows.Forms.DateTimePicker dtpFechaExpiracion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.cbCamaras = new System.Windows.Forms.ComboBox();
            this.pbCamara = new System.Windows.Forms.PictureBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnTerminar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();

            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtCategoria = new System.Windows.Forms.TextBox();

            this.numPrecioInv = new System.Windows.Forms.NumericUpDown();
            this.numPrecioVenta = new System.Windows.Forms.NumericUpDown();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            this.numIVA = new System.Windows.Forms.NumericUpDown();
            this.numDescuento = new System.Windows.Forms.NumericUpDown();

            this.dtpFechaExpiracion = new System.Windows.Forms.DateTimePicker();

            // Labels para cada input
            Label lblCodigo = new Label();
            Label lblNombre = new Label();
            Label lblDescripcion = new Label();
            Label lblCategoria = new Label();
            Label lblPrecioInv = new Label();
            Label lblPrecioVenta = new Label();
            Label lblStock = new Label();
            Label lblIVA = new Label();
            Label lblDescuento = new Label();
            Label lblFechaExpiracion = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.pbCamara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioInv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioVenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIVA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).BeginInit();

            this.SuspendLayout();

            // cbCamaras
            this.cbCamaras.Location = new System.Drawing.Point(20, 20);
            this.cbCamaras.Size = new System.Drawing.Size(300, 24);

            // pbCamara
            this.pbCamara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCamara.Location = new System.Drawing.Point(20, 60);
            this.pbCamara.Size = new System.Drawing.Size(320, 240);

            // btnIniciar
            this.btnIniciar.Text = "Iniciar Escaneo";
            this.btnIniciar.Location = new System.Drawing.Point(360, 60);
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);

            // btnTerminar
            this.btnTerminar.Text = "Terminar";
            this.btnTerminar.Location = new System.Drawing.Point(360, 100);

            // btnGuardar
            this.btnGuardar.Text = "Guardar Producto";
            this.btnGuardar.Location = new System.Drawing.Point(360, 140);
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.btnGuardar.Enabled = false;

            // lblEstado
            this.lblEstado.AutoSize = true;
            this.lblEstado.Text = "Estado:";
            this.lblEstado.Location = new System.Drawing.Point(20, 310);
            this.lblEstado.Size = new System.Drawing.Size(300, 20);

            // --- Configuración Labels y controles ---

            // Label y TextBox Código
            lblCodigo.Text = "Código:";
            lblCodigo.Location = new System.Drawing.Point(20, 340);
            lblCodigo.AutoSize = true;

            this.txtCodigo.Location = new System.Drawing.Point(100, 337);
            this.txtCodigo.Size = new System.Drawing.Size(200, 22);
            this.txtCodigo.ReadOnly = true;
            this.txtCodigo.PlaceholderText = "Código Escaneado";

            // Label y TextBox Nombre
            lblNombre.Text = "Nombre:";
            lblNombre.Location = new System.Drawing.Point(20, 370);
            lblNombre.AutoSize = true;

            this.txtNombre.Location = new System.Drawing.Point(100, 367);
            this.txtNombre.Size = new System.Drawing.Size(200, 22);
            this.txtNombre.PlaceholderText = "Nombre";
            this.txtNombre.Enabled = false;

            // Label y TextBox Descripción
            lblDescripcion.Text = "Descripción:";
            lblDescripcion.Location = new System.Drawing.Point(20, 400);
            lblDescripcion.AutoSize = true;

            this.txtDescripcion.Location = new System.Drawing.Point(100, 397);
            this.txtDescripcion.Size = new System.Drawing.Size(300, 22);
            this.txtDescripcion.PlaceholderText = "Descripción";
            this.txtDescripcion.Enabled = false;

            // Label y TextBox Categoría
            lblCategoria.Text = "Categoría:";
            lblCategoria.Location = new System.Drawing.Point(20, 430);
            lblCategoria.AutoSize = true;

            this.txtCategoria.Location = new System.Drawing.Point(100, 427);
            this.txtCategoria.Size = new System.Drawing.Size(200, 22);
            this.txtCategoria.PlaceholderText = "Categoría";
            this.txtCategoria.Enabled = false;

            // Label y NumericUpDown Precio Inventario
            lblPrecioInv.Text = "Precio Inventario:";
            lblPrecioInv.Location = new System.Drawing.Point(20, 460);
            lblPrecioInv.AutoSize = true;

            this.numPrecioInv.Location = new System.Drawing.Point(140, 457);
            this.numPrecioInv.Maximum = 1000000;
            this.numPrecioInv.DecimalPlaces = 2;
            this.numPrecioInv.Enabled = false;

            // Label y NumericUpDown Precio Venta
            lblPrecioVenta.Text = "Precio Venta:";
            lblPrecioVenta.Location = new System.Drawing.Point(20, 490);
            lblPrecioVenta.AutoSize = true;

            this.numPrecioVenta.Location = new System.Drawing.Point(140, 487);
            this.numPrecioVenta.Maximum = 1000000;
            this.numPrecioVenta.DecimalPlaces = 2;
            this.numPrecioVenta.Enabled = false;

            // Label y NumericUpDown Stock
            lblStock.Text = "Stock:";
            lblStock.Location = new System.Drawing.Point(20, 520);
            lblStock.AutoSize = true;

            this.numStock.Location = new System.Drawing.Point(140, 517);
            this.numStock.Maximum = 10000;
            this.numStock.Enabled = false;

            // Label y NumericUpDown IVA
            lblIVA.Text = "IVA (%):";
            lblIVA.Location = new System.Drawing.Point(20, 550);
            lblIVA.AutoSize = true;

            this.numIVA.Location = new System.Drawing.Point(140, 547);
            this.numIVA.Maximum = 100;
            this.numIVA.DecimalPlaces = 2;
            this.numIVA.Enabled = false;

            // Label y NumericUpDown Descuento
            lblDescuento.Text = "Descuento (%):";
            lblDescuento.Location = new System.Drawing.Point(20, 580);
            lblDescuento.AutoSize = true;

            this.numDescuento.Location = new System.Drawing.Point(140, 577);
            this.numDescuento.Maximum = 100;
            this.numDescuento.DecimalPlaces = 2;
            this.numDescuento.Enabled = false;

            // Label y DateTimePicker Fecha Expiración
            lblFechaExpiracion.Text = "Fecha Expiración:";
            lblFechaExpiracion.Location = new System.Drawing.Point(20, 610);
            lblFechaExpiracion.AutoSize = true;
            this.dtpFechaExpiracion.Checked = false;

            this.dtpFechaExpiracion.Location = new System.Drawing.Point(140, 607);
            this.dtpFechaExpiracion.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaExpiracion.ShowCheckBox = true;
            this.dtpFechaExpiracion.Enabled = false;

            // --- Otros controles ---

            this.chkIngresoManual = new System.Windows.Forms.CheckBox();
            this.chkIngresoManual.Text = "Ingresar código manualmente";
            this.chkIngresoManual.Location = new System.Drawing.Point(450, 200);
            this.chkIngresoManual.CheckedChanged += new System.EventHandler(this.chkIngresoManual_CheckedChanged);

            this.txtCodigoManual = new System.Windows.Forms.TextBox();
            this.txtCodigoManual.Location = new System.Drawing.Point(550, 330);
            this.txtCodigoManual.Size = new System.Drawing.Size(200, 22);
            this.txtCodigoManual.Enabled = false;

            this.btnBuscarManual = new System.Windows.Forms.Button();
            this.btnBuscarManual.Text = "Buscar";
            this.btnBuscarManual.Location = new System.Drawing.Point(470, 328);
            this.btnBuscarManual.Size = new System.Drawing.Size(75, 25);
            this.btnBuscarManual.Click += new System.EventHandler(this.btnBuscarManual_Click);
            this.btnBuscarManual.Enabled = false;

            // Agregar controles y labels al formulario
            this.Controls.Add(this.cbCamaras);
            this.Controls.Add(this.pbCamara);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnTerminar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.lblEstado);

            this.Controls.Add(this.chkIngresoManual);
            this.Controls.Add(this.txtCodigoManual);
            this.Controls.Add(this.btnBuscarManual);

            this.Controls.Add(lblCodigo);
            this.Controls.Add(this.txtCodigo);

            this.Controls.Add(lblNombre);
            this.Controls.Add(this.txtNombre);

            this.Controls.Add(lblDescripcion);
            this.Controls.Add(this.txtDescripcion);

            this.Controls.Add(lblCategoria);
            this.Controls.Add(this.txtCategoria);

            this.Controls.Add(lblPrecioInv);
            this.Controls.Add(this.numPrecioInv);

            this.Controls.Add(lblPrecioVenta);
            this.Controls.Add(this.numPrecioVenta);

            this.Controls.Add(lblStock);
            this.Controls.Add(this.numStock);

            this.Controls.Add(lblIVA);
            this.Controls.Add(this.numIVA);

            this.Controls.Add(lblDescuento);
            this.Controls.Add(this.numDescuento);

            this.Controls.Add(lblFechaExpiracion);
            this.Controls.Add(this.dtpFechaExpiracion);

            this.Name = "LectorCodigoForm";
            this.Text = "Lector de Código de Barras";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LectorCodigoForm_FormClosing);
            this.Load += new System.EventHandler(this.LectorCodigoForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.pbCamara)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioInv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioVenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIVA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).EndInit();

            
            this.ClientSize = new System.Drawing.Size(850, 680);


            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}