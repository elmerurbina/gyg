namespace GyG.Presentacion
{
    partial class ClienteForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblUbicacion;
        private System.Windows.Forms.TextBox txtUbicacion;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblBuscar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblUbicacion = new System.Windows.Forms.Label();
            this.txtUbicacion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvClientes = new System.Windows.Forms.DataGridView();

            this.SuspendLayout();

            // Form Background color
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255); // AliceBlue

            // lblNombre
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(30, 20);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(54, 15);
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41); // Dark text

            // txtNombre
            this.txtNombre.Location = new System.Drawing.Point(120, 17);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(300, 23);
            this.txtNombre.BackColor = System.Drawing.Color.White;
            this.txtNombre.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // lblTelefono
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(30, 65);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(57, 15);
            this.lblTelefono.Text = "Teléfono:";
            this.lblTelefono.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // txtTelefono
            this.txtTelefono.Location = new System.Drawing.Point(120, 62);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(300, 23);
            this.txtTelefono.BackColor = System.Drawing.Color.White;
            this.txtTelefono.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // lblUbicacion
            this.lblUbicacion.AutoSize = true;
            this.lblUbicacion.Location = new System.Drawing.Point(30, 110);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(65, 15);
            this.lblUbicacion.Text = "Ubicación:";
            this.lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // txtUbicacion
            this.txtUbicacion.Location = new System.Drawing.Point(120, 107);
            this.txtUbicacion.Name = "txtUbicacion";
            this.txtUbicacion.Size = new System.Drawing.Size(300, 23);
            this.txtUbicacion.BackColor = System.Drawing.Color.White;
            this.txtUbicacion.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // btnGuardar
            this.btnGuardar.Location = new System.Drawing.Point(120, 150);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 35);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(0, 123, 255); // Bootstrap primary blue
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            // lblBuscar
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(30, 210);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(46, 15);
            this.lblBuscar.Text = "Buscar:";
            this.lblBuscar.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);

            // txtBuscar
            this.txtBuscar.Location = new System.Drawing.Point(120, 207);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(300, 23);
            this.txtBuscar.BackColor = System.Drawing.Color.White;
            this.txtBuscar.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);

            // dgvClientes
            this.dgvClientes.Location = new System.Drawing.Point(30, 245);
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.Size = new System.Drawing.Size(760, 230);
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes.MultiSelect = false;
            this.dgvClientes.AllowUserToOrderColumns = true;
            this.dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientes.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvClientes.GridColor = System.Drawing.Color.LightGray;
            this.dgvClientes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvClientes.DoubleClick += new System.EventHandler(this.dgvClientes_DoubleClick);

            // ClienteForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 500);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblTelefono);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.lblUbicacion);
            this.Controls.Add(this.txtUbicacion);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.dgvClientes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClienteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modificar Cliente";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
    }