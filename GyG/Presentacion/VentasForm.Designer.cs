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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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

            ((System.ComponentModel.ISupportInitialize)(this.numIVA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();

            this.SuspendLayout();

            Color amarillo = Color.FromArgb(255, 223, 88);
            Color naranja = Color.FromArgb(255, 152, 0);
            Color azul = Color.FromArgb(0, 123, 255);
            Color gris = Color.FromArgb(240, 240, 240);

            // Labels properties and positions - separados verticalmente
            lblProducto.Text = "Producto:";
            lblProducto.Location = new Point(30, 10);
            lblProducto.AutoSize = true;

            this.cbProductos.Location = new Point(30, 30);
            this.cbProductos.Size = new Size(250, 24);
            this.cbProductos.DropDownStyle = ComboBoxStyle.DropDown;
            this.cbProductos.BackColor = gris;

            lblDescripcion.Text = "Descripción:";
            lblDescripcion.Location = new Point(30, 60);
            lblDescripcion.AutoSize = true;

            this.txtDescripcion.Location = new Point(30, 80);
            this.txtDescripcion.Size = new Size(300, 22);
            this.txtDescripcion.ReadOnly = true;

            lblPrecio.Text = "Precio:";
            lblPrecio.Location = new Point(30, 110);
            lblPrecio.AutoSize = true;

            this.txtPrecio.Location = new Point(30, 130);
            this.txtPrecio.Size = new Size(100, 22);
            this.txtPrecio.ReadOnly = true;

            lblIVA.Text = "IVA (%):";
            lblIVA.Location = new Point(150, 110);
            lblIVA.AutoSize = true;

            this.numIVA.Location = new Point(150, 130);
            this.numIVA.Maximum = 100;
            this.numIVA.Size = new Size(100, 22);
            this.numIVA.ReadOnly = true;
            this.numIVA.Enabled = false;

            lblDescuento.Text = "Descuento (%):";
            lblDescuento.Location = new Point(270, 110);
            lblDescuento.AutoSize = true;

            this.numDescuento.Location = new Point(270, 130);
            this.numDescuento.Maximum = 100;
            this.numDescuento.Size = new Size(100, 22);
            this.numDescuento.ReadOnly = true;
            this.numDescuento.Enabled = false;

            lblCantidad.Text = "Cantidad:";
            lblCantidad.Location = new Point(30, 160);
            lblCantidad.AutoSize = true;

            this.numCantidad.Location = new Point(30, 180);
            this.numCantidad.Minimum = 1;
            this.numCantidad.Value = 1;
            this.numCantidad.Size = new Size(100, 22);

            lblSubtotal.Text = "Subtotal:";
            lblSubtotal.Location = new Point(150, 160);
            lblSubtotal.AutoSize = true;

            this.txtSubtotal.Location = new Point(150, 180);
            this.txtSubtotal.ReadOnly = true;
            this.txtSubtotal.Size = new Size(150, 22);

            lblNombreCliente.Text = "Nombre Cliente:";
            lblNombreCliente.Location = new Point(400, 10);
            lblNombreCliente.AutoSize = true;

            this.txtNombreCliente.Location = new Point(400, 30);
            this.txtNombreCliente.Size = new Size(250, 22);

            lblTelefono.Text = "Teléfono:";
            lblTelefono.Location = new Point(400, 60);
            lblTelefono.AutoSize = true;

            this.txtTelefono.Location = new Point(400, 80);
            this.txtTelefono.Size = new Size(250, 22);

            lblUbicacion.Text = "Ubicación:";
            lblUbicacion.Location = new Point(400, 110);
            lblUbicacion.AutoSize = true;

            this.txtUbicacion.Location = new Point(400, 130);
            this.txtUbicacion.Size = new Size(250, 22);

            // Buttons with enough width
            this.btnAgregar.Text = "Agregar al Carrito";
            this.btnAgregar.Location = new Point(30, 220);
            this.btnAgregar.BackColor = azul;
            this.btnAgregar.Size = new Size(160, 30);
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            this.btnFinalizarVenta.Text = "Finalizar Venta (Contado)";
            this.btnFinalizarVenta.Location = new Point(400, 170);
            this.btnFinalizarVenta.BackColor = naranja;
            this.btnFinalizarVenta.Size = new Size(200, 30);
            this.btnFinalizarVenta.Click += new System.EventHandler(this.btnFinalizarVenta_Click);

            this.btnCredito.Text = "Venta a Crédito";
            this.btnCredito.Location = new Point(400, 210);
            this.btnCredito.BackColor = amarillo;
            this.btnCredito.Size = new Size(200, 30);
            this.btnCredito.Click += new System.EventHandler(this.btnCredito_Click);

            this.btnGenerarProforma.Text = "Generar Proforma";
            this.btnGenerarProforma.Location = new Point(400, 250);
            this.btnGenerarProforma.BackColor = gris;
            this.btnGenerarProforma.Size = new Size(200, 30);
            this.btnGenerarProforma.Click += new System.EventHandler(this.btnGenerarProforma_Click);

            // DataGridView dgvCarrito - setup columns para edición y eliminar
            this.dgvCarrito.Location = new Point(30, 290);
            this.dgvCarrito.Size = new Size(640, 200);
            this.dgvCarrito.BackgroundColor = Color.White;
            this.dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarrito.MultiSelect = false;
            this.dgvCarrito.AllowUserToAddRows = false;
            this.dgvCarrito.ReadOnly = false; // para permitir edición de cantidad

            this.dgvCarrito.Columns.Clear();
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Id", HeaderText = "ID", Visible = false });
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Nombre", HeaderText = "Producto", ReadOnly = true });
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descripcion", HeaderText = "Descripción", ReadOnly = true });
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Precio", HeaderText = "Precio", ReadOnly = true });
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "IVA", HeaderText = "IVA (%)", ReadOnly = true });
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descuento", HeaderText = "Descuento (%)", ReadOnly = true });
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Cantidad", HeaderText = "Cantidad" }); // editable
            this.dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Subtotal", HeaderText = "Subtotal", ReadOnly = true });

            var btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.Name = "Eliminar";
            btnEliminar.HeaderText = "Eliminar";
            btnEliminar.Text = "X";
            btnEliminar.UseColumnTextForButtonValue = true;
            this.dgvCarrito.Columns.Add(btnEliminar);

            this.dgvCarrito.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCarrito_CellValueChanged);
            this.dgvCarrito.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCarrito_CellContentClick);

            // Label lblTotal
            this.lblTotal.Location = new Point(30, 500);
            this.lblTotal.Size = new Size(300, 24);
            this.lblTotal.Font = new Font("Arial", 10, FontStyle.Bold);
            this.lblTotal.Text = "Total: $0.00";

            // Añadir controles al formulario
            this.Controls.Add(lblProducto);
            this.Controls.Add(cbProductos);
            this.Controls.Add(lblDescripcion);
            this.Controls.Add(txtDescripcion);
            this.Controls.Add(lblPrecio);
            this.Controls.Add(txtPrecio);
            this.Controls.Add(lblIVA);
            this.Controls.Add(numIVA);
            this.Controls.Add(lblDescuento);
            this.Controls.Add(numDescuento);
            this.Controls.Add(lblCantidad);
            this.Controls.Add(numCantidad);
            this.Controls.Add(lblSubtotal);
            this.Controls.Add(txtSubtotal);
            this.Controls.Add(lblNombreCliente);
            this.Controls.Add(txtNombreCliente);
            this.Controls.Add(lblTelefono);
            this.Controls.Add(txtTelefono);
            this.Controls.Add(lblUbicacion);
            this.Controls.Add(txtUbicacion);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnFinalizarVenta);
            this.Controls.Add(btnCredito);
            this.Controls.Add(btnGenerarProforma);
            this.Controls.Add(dgvCarrito);
            this.Controls.Add(lblTotal);

            // Form properties
            this.Text = "Gestión de Ventas";
            this.BackColor = Color.White;
            this.Size = new Size(720, 580);

            ((System.ComponentModel.ISupportInitialize)(this.numIVA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            // Evento para actualizar campos al cambiar selección
            this.cbProductos.SelectedIndexChanged += new System.EventHandler(this.cbProductos_SelectedIndexChanged);
        }
    }
}
