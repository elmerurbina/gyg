namespace GyG.Presentacion
{
    partial class PedidosForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvPedidos = new System.Windows.Forms.DataGridView();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.txtUmbral = new System.Windows.Forms.TextBox();
            this.cmbProveedores = new System.Windows.Forms.ComboBox();
            this.btnGenerarPedido = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.lblUmbral = new System.Windows.Forms.Label();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.lblProveedores = new System.Windows.Forms.Label();
            this.lblProductos = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
          
            this.SuspendLayout();

            // 
            // dgvProveedores
            // 
            this.dgvPedidos.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidos.Location = new System.Drawing.Point(20, 60);
            this.dgvPedidos.Name = "dgvProveedores";
            this.dgvPedidos.RowHeadersWidth = 51;
            this.dgvPedidos.RowTemplate.Height = 29;
            dgvProductos.MultiSelect = true;  // Permite seleccionar varias filas
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  // Selección por fila completa

            this.dgvPedidos.Size = new System.Drawing.Size(450, 180);
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPedidos.TabIndex = 0;

            // 
            // dgvProductos
            // 
            this.dgvProductos.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Location = new System.Drawing.Point(20, 300);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.RowHeadersWidth = 51;
            this.dgvProductos.RowTemplate.Height = 29;
            this.dgvProductos.Size = new System.Drawing.Size(750, 200);
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductos.TabIndex = 1;

            // 
            // numUmbral
            // 
            this.txtUmbral.Location = new System.Drawing.Point(550, 60);
            this.txtUmbral.Name = "txtUmbral";
            this.txtUmbral.Size = new System.Drawing.Size(100, 27);
            this.txtUmbral.TabIndex = 2;
            this.txtUmbral.Text = "5";

            // 
            // cmbProveedores
            // 
            this.cmbProveedores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProveedores.FormattingEnabled = true;
            this.cmbProveedores.Location = new System.Drawing.Point(570, 120);
            this.cmbProveedores.Name = "cmbProveedores";
            this.cmbProveedores.Size = new System.Drawing.Size(220, 28);
            this.cmbProveedores.TabIndex = 3;

            // 
            // btnGenerarPedido
            // 
            this.btnGenerarPedido.BackColor = System.Drawing.Color.SeaGreen;
            this.btnGenerarPedido.ForeColor = System.Drawing.Color.White;
            this.btnGenerarPedido.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerarPedido.Location = new System.Drawing.Point(550, 220);
            this.btnGenerarPedido.Name = "btnGenerarPedido";
            this.btnGenerarPedido.Size = new System.Drawing.Size(220, 40);
            this.btnGenerarPedido.TabIndex = 4;
            this.btnGenerarPedido.Text = "Generar Orden de Pedido";
            this.btnGenerarPedido.UseVisualStyleBackColor = false;
            this.btnGenerarPedido.Click += new System.EventHandler(this.btnGenerarPedido_Click);

            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFiltrar.Location = new System.Drawing.Point(670, 60);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(100, 30);
            this.btnFiltrar.TabIndex = 5;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);

            // 
            // lblUmbral
            // 
            this.lblUmbral.AutoSize = true;
            this.lblUmbral.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUmbral.Location = new System.Drawing.Point(470, 60);
            this.lblUmbral.Name = "lblUmbral";
            this.lblUmbral.Size = new System.Drawing.Size(72, 23);
            this.lblUmbral.TabIndex = 6;
            this.lblUmbral.Text = "Umbral:";

            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProveedor.Location = new System.Drawing.Point(480, 120);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(88, 23);
            this.lblProveedor.TabIndex = 7;
            this.lblProveedor.Text = "Proveedor:";

            // 
            // lblProveedores
            // 
            this.lblProveedores.AutoSize = true;
            this.lblProveedores.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProveedores.Location = new System.Drawing.Point(20, 30);
            this.lblProveedores.Name = "lblProveedores";
            this.lblProveedores.Size = new System.Drawing.Size(116, 23);
            this.lblProveedores.TabIndex = 8;
            this.lblProveedores.Text = "Historial de Pedidos:";

            // 
            // lblProductos
            // 
            this.lblProductos.AutoSize = true;
            this.lblProductos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductos.Location = new System.Drawing.Point(20, 270);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Size = new System.Drawing.Size(99, 23);
            this.lblProductos.TabIndex = 9;
            this.lblProductos.Text = "Productos:";
            
            // btnManejarProveedores
            this.btnManejarProveedores = new System.Windows.Forms.Button();
            this.btnManejarProveedores.Location = new System.Drawing.Point(550, 160);
            this.btnManejarProveedores.Name = "btnManejarProveedores";
            this.btnManejarProveedores.Size = new System.Drawing.Size(220, 40);
            this.btnManejarProveedores.TabIndex = 10;
            this.btnManejarProveedores.Text = "Manejar Proveedores";
            this.btnManejarProveedores.UseVisualStyleBackColor = true;
            this.btnManejarProveedores.Click += new System.EventHandler(this.btnManejarProveedores_Click);
            this.Controls.Add(this.btnManejarProveedores);
            
            Label lblInfoSeleccionMultiple = new Label();
            lblInfoSeleccionMultiple.Name = "lblInfoSeleccionMultiple";
            lblInfoSeleccionMultiple.Text = "⚠️ Para seleccionar varios productos, presiona Ctrl + Click";
            lblInfoSeleccionMultiple.ForeColor = Color.Blue;
            lblInfoSeleccionMultiple.Location = new System.Drawing.Point(400, 500);
            lblInfoSeleccionMultiple.AutoSize = true;
            lblInfoSeleccionMultiple.Location = new Point(dgvProductos.Location.X, dgvProductos.Location.Y + dgvProductos.Height + 5);
            this.Controls.Add(lblInfoSeleccionMultiple);



            // 
            // PedidosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(850, 550);
            this.Controls.Add(this.lblProductos);
            this.Controls.Add(this.lblProveedores);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.lblUmbral);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.btnGenerarPedido);
            this.Controls.Add(this.cmbProveedores);
            this.Controls.Add(this.txtUmbral);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.dgvPedidos);
            this.Name = "PedidosForm";
            this.Text = "Gestión de Pedidos";

            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPedidos;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.TextBox txtUmbral;
        private System.Windows.Forms.ComboBox cmbProveedores;
        private System.Windows.Forms.Button btnGenerarPedido;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Label lblUmbral;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.Label lblProveedores;
        private System.Windows.Forms.Label lblProductos;
        private System.Windows.Forms.Button btnManejarProveedores;

    }
}
