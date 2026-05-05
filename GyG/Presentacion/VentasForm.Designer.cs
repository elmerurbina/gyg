namespace GyG.Presentacion
{
    partial class VentasForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cbProductos;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.TextBox txtSubtotal;
        private System.Windows.Forms.NumericUpDown numIVA;
        private System.Windows.Forms.NumericUpDown numDescuento;
        private System.Windows.Forms.NumericUpDown numCantidad;

        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnFinalizarVenta;
        private System.Windows.Forms.Button btnCredito;
        private System.Windows.Forms.Button btnGenerarProforma;

        private System.Windows.Forms.DataGridView dgvCarrito;
        private System.Windows.Forms.Label lblTotal;

        private System.Windows.Forms.TextBox txtNombreCliente;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.TextBox txtUbicacion;

        // Panels for better organization and scrolling
        private Panel pnlMainContainer;
        private Panel pnlProductSection;
        private Panel pnlClientSection;
        private Panel pnlCartSection;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

       private void InitializeComponent()
{
    this.components = new System.ComponentModel.Container();
    
    // Create main container with AutoScroll
    this.pnlMainContainer = new Panel();
    this.pnlProductSection = new Panel();
    this.pnlClientSection = new Panel();
    this.pnlCartSection = new Panel();
    
    this.cbProductos = new System.Windows.Forms.ComboBox();
    this.txtDescripcion = new System.Windows.Forms.TextBox();
    this.txtPrecio = new System.Windows.Forms.TextBox();
    this.txtSubtotal = new System.Windows.Forms.TextBox();
    this.numIVA = new System.Windows.Forms.NumericUpDown();
    this.numDescuento = new System.Windows.Forms.NumericUpDown();
    this.numCantidad = new System.Windows.Forms.NumericUpDown();
    this.btnAgregar = new System.Windows.Forms.Button();
    this.btnFinalizarVenta = new System.Windows.Forms.Button();
    this.btnCredito = new System.Windows.Forms.Button();
    this.btnGenerarProforma = new System.Windows.Forms.Button();
    this.dgvCarrito = new System.Windows.Forms.DataGridView();
    this.lblTotal = new System.Windows.Forms.Label();
    this.txtNombreCliente = new System.Windows.Forms.TextBox();
    this.txtTelefono = new System.Windows.Forms.TextBox();
    this.txtUbicacion = new System.Windows.Forms.TextBox();

    // Labels
    Label lblProducto = new Label();
    Label lblDescripcion = new Label();
    Label lblPrecio = new Label();
    Label lblIVA = new Label();
    Label lblDescuento = new Label();
    Label lblCantidad = new Label();
    Label lblSubtotal = new Label();
    Label lblNombreCliente = new Label();
    Label lblTelefono = new Label();
    Label lblUbicacion = new Label();
    Label lblInfoEditarCantidad = new Label();

    ((System.ComponentModel.ISupportInitialize)(this.numIVA)).BeginInit();
    ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).BeginInit();
    ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
    ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
    this.pnlMainContainer.SuspendLayout();
    this.pnlProductSection.SuspendLayout();
    this.pnlClientSection.SuspendLayout();
    this.pnlCartSection.SuspendLayout();
    this.SuspendLayout();

    // ========== COLOR PALETTE (Sacuanjoche) ==========
    Color primary500 = Color.FromArgb(139, 94, 60);
    Color secondary500 = Color.FromArgb(255, 193, 7);
    Color infoColor = Color.FromArgb(2, 136, 209);
    Color textPrimary = Color.FromArgb(45, 41, 38);
    Color inputBg = Color.FromArgb(249, 249, 249);
    Color formBg = Color.FromArgb(255, 249, 230);
    Color panelBg = Color.FromArgb(243, 235, 225);

    // ========== FORM CONFIGURATION ==========
    this.Text = "Gestión de Ventas - Sacuanjoche";
    this.BackColor = formBg;
    this.Size = new Size(1900, 1100);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.MinimumSize = new Size(1200, 700);
    this.Font = new Font("Segoe UI", 9F);
    
    // ========== MAIN CONTAINER WITH SCROLLING ==========
    this.pnlMainContainer.Dock = DockStyle.Fill;
    this.pnlMainContainer.AutoScroll = true;
    this.pnlMainContainer.BackColor = formBg;
    this.pnlMainContainer.Padding = new Padding(15);

    // Font definitions - SMALLER TEXT, TALLER CONTROLS
    Font labelFont = new Font("Segoe UI", 9F, FontStyle.Bold);
    Font inputFont = new Font("Segoe UI", 9F);
    int fieldHeight = 42;
    int labelWidth = 120;
    int inputWidth = 320;
    int fieldSpacing = 50;

    // ========== PANEL 1: PRODUCT SECTION ==========
    this.pnlProductSection.Location = new Point(15, 15);
    this.pnlProductSection.Size = new Size(700, 460);
    this.pnlProductSection.BackColor = panelBg;
    this.pnlProductSection.BorderStyle = BorderStyle.FixedSingle;
    
    int currentY = 18;
    int labelLeft = 15;
    int inputLeft = 150;

    // Producto ComboBox
    lblProducto.Text = "Producto:";
    lblProducto.Location = new Point(labelLeft, currentY);
    lblProducto.Size = new Size(labelWidth, fieldHeight);
    lblProducto.Font = labelFont;
    lblProducto.ForeColor = textPrimary;
    lblProducto.TextAlign = ContentAlignment.MiddleRight;
    
    this.cbProductos.Location = new Point(inputLeft, currentY);
    this.cbProductos.Size = new Size(inputWidth, fieldHeight);
    this.cbProductos.Font = inputFont;
    this.cbProductos.BackColor = inputBg;
    this.cbProductos.DropDownStyle = ComboBoxStyle.DropDown;
    this.cbProductos.SelectedIndexChanged += new System.EventHandler(this.cbProductos_SelectedIndexChanged);
    this.pnlProductSection.Controls.Add(lblProducto);
    this.pnlProductSection.Controls.Add(this.cbProductos);
    
    currentY += fieldSpacing;

    // Descripción
    lblDescripcion.Text = "Descripción:";
    lblDescripcion.Location = new Point(labelLeft, currentY);
    lblDescripcion.Size = new Size(labelWidth, fieldHeight);
    lblDescripcion.Font = labelFont;
    lblDescripcion.ForeColor = textPrimary;
    lblDescripcion.TextAlign = ContentAlignment.MiddleRight;
    
    this.txtDescripcion.Location = new Point(inputLeft, currentY);
    this.txtDescripcion.Size = new Size(inputWidth, fieldHeight);
    this.txtDescripcion.Font = inputFont;
    this.txtDescripcion.BackColor = inputBg;
    this.txtDescripcion.ReadOnly = true;
    this.txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
    this.pnlProductSection.Controls.Add(lblDescripcion);
    this.pnlProductSection.Controls.Add(this.txtDescripcion);
    
    currentY += fieldSpacing;

    // Precio row
    lblPrecio.Text = "Precio:";
    lblPrecio.Location = new Point(labelLeft, currentY);
    lblPrecio.Size = new Size(labelWidth, fieldHeight);
    lblPrecio.Font = labelFont;
    lblPrecio.ForeColor = textPrimary;
    lblPrecio.TextAlign = ContentAlignment.MiddleRight;
    
    this.txtPrecio.Location = new Point(inputLeft, currentY);
    this.txtPrecio.Size = new Size(160, fieldHeight);
    this.txtPrecio.Font = inputFont;
    this.txtPrecio.BackColor = inputBg;
    this.txtPrecio.ReadOnly = true;
    this.txtPrecio.BorderStyle = BorderStyle.FixedSingle;
    
    // IVA
    lblIVA.Text = "IVA (%):";
    lblIVA.Location = new Point(inputLeft + 180, currentY);
    lblIVA.Size = new Size(65, fieldHeight);
    lblIVA.Font = labelFont;
    lblIVA.ForeColor = textPrimary;
    lblIVA.TextAlign = ContentAlignment.MiddleRight;
    
    this.numIVA.Location = new Point(inputLeft + 250, currentY + 2);
    this.numIVA.Size = new Size(90, fieldHeight - 4);
    this.numIVA.Font = inputFont;
    this.numIVA.Maximum = 100;
    this.numIVA.ReadOnly = true;
    this.numIVA.Enabled = false;
    
    this.pnlProductSection.Controls.Add(lblPrecio);
    this.pnlProductSection.Controls.Add(this.txtPrecio);
    this.pnlProductSection.Controls.Add(lblIVA);
    this.pnlProductSection.Controls.Add(this.numIVA);
    
    currentY += fieldSpacing;

    // Cantidad row
    lblCantidad.Text = "Cantidad:";
    lblCantidad.Location = new Point(labelLeft, currentY);
    lblCantidad.Size = new Size(labelWidth, fieldHeight);
    lblCantidad.Font = labelFont;
    lblCantidad.ForeColor = textPrimary;
    lblCantidad.TextAlign = ContentAlignment.MiddleRight;
    
    this.numCantidad.Location = new Point(inputLeft, currentY + 2);
    this.numCantidad.Size = new Size(130, fieldHeight - 4);
    this.numCantidad.Font = inputFont;
    this.numCantidad.Minimum = 1;
    this.numCantidad.Value = 1;
    
    // Descuento
    lblDescuento.Text = "Descuento (%):";
    lblDescuento.Location = new Point(inputLeft + 150, currentY);
    lblDescuento.Size = new Size(90, fieldHeight);
    lblDescuento.Font = labelFont;
    lblDescuento.ForeColor = textPrimary;
    lblDescuento.TextAlign = ContentAlignment.MiddleRight;
    
    this.numDescuento.Location = new Point(inputLeft + 245, currentY + 2);
    this.numDescuento.Size = new Size(95, fieldHeight - 4);
    this.numDescuento.Font = inputFont;
    this.numDescuento.Maximum = 100;
    this.numDescuento.ReadOnly = true;
    this.numDescuento.Enabled = false;
    
    this.pnlProductSection.Controls.Add(lblCantidad);
    this.pnlProductSection.Controls.Add(this.numCantidad);
    this.pnlProductSection.Controls.Add(lblDescuento);
    this.pnlProductSection.Controls.Add(this.numDescuento);
    
    currentY += fieldSpacing;

    // Subtotal
    lblSubtotal.Text = "Precio Final:";
    lblSubtotal.Location = new Point(labelLeft, currentY);
    lblSubtotal.Size = new Size(labelWidth, fieldHeight);
    lblSubtotal.Font = labelFont;
    lblSubtotal.ForeColor = textPrimary;
    lblSubtotal.TextAlign = ContentAlignment.MiddleRight;
    
    this.txtSubtotal.Location = new Point(inputLeft, currentY);
    this.txtSubtotal.Size = new Size(200, fieldHeight);
    this.txtSubtotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
    this.txtSubtotal.BackColor = Color.FromArgb(255, 248, 225);
    this.txtSubtotal.ReadOnly = true;
    this.txtSubtotal.BorderStyle = BorderStyle.FixedSingle;
    this.txtSubtotal.ForeColor = primary500;
    
    this.pnlProductSection.Controls.Add(lblSubtotal);
    this.pnlProductSection.Controls.Add(this.txtSubtotal);
    
    currentY += fieldSpacing + 10;

    // Agregar al Carrito button
    this.btnAgregar.Text = "AGREGAR AL CARRITO";
    this.btnAgregar.Location = new Point(inputLeft, currentY);
    this.btnAgregar.Size = new Size(250, 50);
    this.btnAgregar.BackColor = primary500;
    this.btnAgregar.ForeColor = Color.White;
    this.btnAgregar.FlatStyle = FlatStyle.Flat;
    this.btnAgregar.FlatAppearance.BorderSize = 0;
    this.btnAgregar.Font = new Font("Segoe UI", 6F, FontStyle.Bold);
    this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
    this.pnlProductSection.Controls.Add(this.btnAgregar);

    // ========== PANEL 2: CLIENT SECTION ==========
    this.pnlClientSection.Location = new Point(730, 15);
    this.pnlClientSection.Size = new Size(550, 460);
    this.pnlClientSection.BackColor = panelBg;
    this.pnlClientSection.BorderStyle = BorderStyle.FixedSingle;
    
    currentY = 18;
    labelLeft = 15;
    inputLeft = 130;

    lblNombreCliente.Text = "Cliente:";
    lblNombreCliente.Location = new Point(labelLeft, currentY);
    lblNombreCliente.Size = new Size(100, fieldHeight);
    lblNombreCliente.Font = labelFont;
    lblNombreCliente.ForeColor = textPrimary;
    lblNombreCliente.TextAlign = ContentAlignment.MiddleRight;
    
    this.txtNombreCliente.Location = new Point(inputLeft, currentY);
    this.txtNombreCliente.Size = new Size(370, fieldHeight);
    this.txtNombreCliente.Font = inputFont;
    this.txtNombreCliente.BackColor = inputBg;
    this.txtNombreCliente.BorderStyle = BorderStyle.FixedSingle;
    this.txtNombreCliente.PlaceholderText = "Ingrese el nombre del cliente";
    this.pnlClientSection.Controls.Add(lblNombreCliente);
    this.pnlClientSection.Controls.Add(this.txtNombreCliente);
    
    currentY += fieldSpacing;

    lblTelefono.Text = "Teléfono:";
    lblTelefono.Location = new Point(labelLeft, currentY);
    lblTelefono.Size = new Size(100, fieldHeight);
    lblTelefono.Font = labelFont;
    lblTelefono.ForeColor = textPrimary;
    lblTelefono.TextAlign = ContentAlignment.MiddleRight;
    
    this.txtTelefono.Location = new Point(inputLeft, currentY);
    this.txtTelefono.Size = new Size(370, fieldHeight);
    this.txtTelefono.Font = inputFont;
    this.txtTelefono.BackColor = inputBg;
    this.txtTelefono.BorderStyle = BorderStyle.FixedSingle;
    this.txtTelefono.PlaceholderText = "Teléfono de contacto";
    this.pnlClientSection.Controls.Add(lblTelefono);
    this.pnlClientSection.Controls.Add(this.txtTelefono);
    
    currentY += fieldSpacing;

    lblUbicacion.Text = "Ubicación:";
    lblUbicacion.Location = new Point(labelLeft, currentY);
    lblUbicacion.Size = new Size(100, fieldHeight);
    lblUbicacion.Font = labelFont;
    lblUbicacion.ForeColor = textPrimary;
    lblUbicacion.TextAlign = ContentAlignment.MiddleRight;
    
    this.txtUbicacion.Location = new Point(inputLeft, currentY);
    this.txtUbicacion.Size = new Size(370, fieldHeight);
    this.txtUbicacion.Font = inputFont;
    this.txtUbicacion.BackColor = inputBg;
    this.txtUbicacion.BorderStyle = BorderStyle.FixedSingle;
    this.txtUbicacion.PlaceholderText = "Dirección o ubicación del cliente";
    this.pnlClientSection.Controls.Add(lblUbicacion);
    this.pnlClientSection.Controls.Add(this.txtUbicacion);
    
    currentY += fieldSpacing + 10;

    // Botones de acción
    this.btnFinalizarVenta.Text = "FINALIZAR VENTA (CONTADO)";
    this.btnFinalizarVenta.Location = new Point(inputLeft, currentY);
    this.btnFinalizarVenta.Size = new Size(370, 45);
    this.btnFinalizarVenta.BackColor = secondary500;
    this.btnFinalizarVenta.ForeColor = textPrimary;
    this.btnFinalizarVenta.FlatStyle = FlatStyle.Flat;
    this.btnFinalizarVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    this.btnFinalizarVenta.Click += new System.EventHandler(this.btnFinalizarVenta_Click);
    this.pnlClientSection.Controls.Add(this.btnFinalizarVenta);
    
    currentY += 55;

    this.btnCredito.Text = "VENTA A CRÉDITO";
    this.btnCredito.Location = new Point(inputLeft, currentY);
    this.btnCredito.Size = new Size(370, 45);
    this.btnCredito.BackColor = infoColor;
    this.btnCredito.ForeColor = Color.White;
    this.btnCredito.FlatStyle = FlatStyle.Flat;
    this.btnCredito.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    this.btnCredito.Click += new System.EventHandler(this.btnCredito_Click);
    this.pnlClientSection.Controls.Add(this.btnCredito);
    
    currentY += 55;

    this.btnGenerarProforma.Text = "GENERAR PROFORMA";
    this.btnGenerarProforma.Location = new Point(inputLeft, currentY);
    this.btnGenerarProforma.Size = new Size(370, 45);
    this.btnGenerarProforma.BackColor = Color.FromArgb(100, 100, 100);
    this.btnGenerarProforma.ForeColor = Color.White;
    this.btnGenerarProforma.FlatStyle = FlatStyle.Flat;
    this.btnGenerarProforma.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    this.btnGenerarProforma.Click += new System.EventHandler(this.btnGenerarProforma_Click);
    this.pnlClientSection.Controls.Add(this.btnGenerarProforma);

    // ========== PANEL 3: CART SECTION ==========
    this.pnlCartSection.Location = new Point(15, 490);
    this.pnlCartSection.Size = new Size(1265, 400);
    this.pnlCartSection.BackColor = panelBg;
    this.pnlCartSection.BorderStyle = BorderStyle.FixedSingle;
    this.pnlCartSection.AutoScroll = true;

    // DataGridView
    this.dgvCarrito.Location = new Point(10, 10);
    this.dgvCarrito.Size = new Size(1245, 300);
    this.dgvCarrito.BackgroundColor = inputBg;
    this.dgvCarrito.BorderStyle = BorderStyle.FixedSingle;
    this.dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    this.dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    this.dgvCarrito.MultiSelect = false;
    this.dgvCarrito.AllowUserToAddRows = false;
    this.dgvCarrito.ReadOnly = false;
    this.dgvCarrito.RowHeadersVisible = false;
    this.dgvCarrito.GridColor = Color.FromArgb(196, 164, 132);
    this.dgvCarrito.DefaultCellStyle.Font = new Font("Segoe UI", 8F);
    this.dgvCarrito.DefaultCellStyle.ForeColor = textPrimary;
    this.dgvCarrito.DefaultCellStyle.Padding = new Padding(4);
    this.dgvCarrito.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    this.dgvCarrito.ColumnHeadersDefaultCellStyle.BackColor = primary500;
    this.dgvCarrito.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
    this.dgvCarrito.ColumnHeadersHeight = 40;
    this.dgvCarrito.RowTemplate.Height = 35;
    this.dgvCarrito.EnableHeadersVisualStyles = false;

    // Setup columns
    this.dgvCarrito.Columns.Clear();
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Id", HeaderText = "ID", Visible = false });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Nombre", HeaderText = "Producto", ReadOnly = true });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descripcion", HeaderText = "Descripción", ReadOnly = true });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Precio", HeaderText = "Precio Unit.", ReadOnly = true });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "IVA", HeaderText = "IVA%", ReadOnly = true });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descuento", HeaderText = "Desc%", ReadOnly = true });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Cantidad", HeaderText = "Cantidad" });
    this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Subtotal", HeaderText = "Subtotal", ReadOnly = true });

    var btnEliminar = new DataGridViewButtonColumn();
    btnEliminar.Name = "Eliminar";
    btnEliminar.HeaderText = "";
    btnEliminar.Text = "X";
    btnEliminar.UseColumnTextForButtonValue = true;
    btnEliminar.Width = 40;
    this.dgvCarrito.Columns.Add(btnEliminar);

    this.dgvCarrito.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCarrito_CellValueChanged);
    this.dgvCarrito.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCarrito_CellContentClick);

    // Total label - CORRECTED with C$
    this.lblTotal.Location = new Point(10, 320);
    this.lblTotal.Size = new Size(350, 40);
    this.lblTotal.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
    this.lblTotal.ForeColor = primary500;
    this.lblTotal.Text = "TOTAL: C$0.00";
    this.lblTotal.TextAlign = ContentAlignment.MiddleLeft;

    // Info label
    lblInfoEditarCantidad.Text = "Sugerencia: Edite la cantidad directamente en la tabla";
    lblInfoEditarCantidad.Location = new Point(370, 325);
    lblInfoEditarCantidad.Size = new Size(350, 30);
    lblInfoEditarCantidad.Font = new Font("Segoe UI", 7F);
    lblInfoEditarCantidad.ForeColor = Color.FromArgb(100, 100, 100);
    lblInfoEditarCantidad.TextAlign = ContentAlignment.MiddleLeft;

    // Historial Facturas button
    Button btnHistorialFacturas = new Button
    {
        Text = "HISTORIAL DE VENTAS",
        Location = new Point(900, 325),
        Width = 220,
        Height = 60,
        BackColor = primary500,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 5F, FontStyle.Bold)
    };
    btnHistorialFacturas.Click += BtnHistorialFacturas_Click;

    this.pnlCartSection.Controls.Add(this.dgvCarrito);
    this.pnlCartSection.Controls.Add(this.lblTotal);
    this.pnlCartSection.Controls.Add(lblInfoEditarCantidad);
    this.pnlCartSection.Controls.Add(btnHistorialFacturas);

    // Add all panels to main container
    this.pnlMainContainer.Controls.Add(this.pnlProductSection);
    this.pnlMainContainer.Controls.Add(this.pnlClientSection);
    this.pnlMainContainer.Controls.Add(this.pnlCartSection);

    // Add main container to form
    this.Controls.Add(this.pnlMainContainer);

    // Clean up resources
    ((System.ComponentModel.ISupportInitialize)(this.numIVA)).EndInit();
    ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).EndInit();
    ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
    ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
    this.pnlMainContainer.ResumeLayout(false);
    this.pnlProductSection.ResumeLayout(false);
    this.pnlClientSection.ResumeLayout(false);
    this.pnlCartSection.ResumeLayout(false);
    this.ResumeLayout(false);
}
    }
}