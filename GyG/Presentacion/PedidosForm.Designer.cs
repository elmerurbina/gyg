namespace GyG.Presentacion
{
    partial class PedidosForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.btnManejarProveedores = new System.Windows.Forms.Button();
            this.lblInfoSeleccionMultiple = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();

            // 
            // lblUmbral
            // 
            this.lblUmbral.AutoSize = true;
            this.lblUmbral.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUmbral.Location = new System.Drawing.Point(20, 15);
            this.lblUmbral.Name = "lblUmbral";
            this.lblUmbral.Size = new System.Drawing.Size(61, 23);
            this.lblUmbral.TabIndex = 0;
            this.lblUmbral.Text = "Umbral:";

            // 
            // txtUmbral
            // 
            this.txtUmbral.Location = new System.Drawing.Point(85, 14);
            this.txtUmbral.Name = "txtUmbral";
            this.txtUmbral.Size = new System.Drawing.Size(60, 27);
            this.txtUmbral.TabIndex = 1;
            this.txtUmbral.Text = "5";

            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFiltrar.Location = new System.Drawing.Point(155, 12);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 30);
            this.btnFiltrar.TabIndex = 2;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);

            // 
            // lblProveedores
            // 
            this.lblProveedores.AutoSize = true;
            this.lblProveedores.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProveedores.Location = new System.Drawing.Point(560, 15);
            this.lblProveedores.Name = "lblProveedores";
            this.lblProveedores.Size = new System.Drawing.Size(98, 23);
            this.lblProveedores.TabIndex = 3;
            this.lblProveedores.Text = "Proveedores";

            // 
            // cmbProveedores
            // 
            this.cmbProveedores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProveedores.FormattingEnabled = true;
            this.cmbProveedores.Location = new System.Drawing.Point(235, 15);
            this.cmbProveedores.Name = "cmbProveedores";
            this.cmbProveedores.Size = new System.Drawing.Size(220, 28);
            this.cmbProveedores.TabIndex = 4;

            // 
            // btnManejarProveedores
            // 
            this.btnManejarProveedores.Location = new System.Drawing.Point(560, 11);
            this.btnManejarProveedores.Name = "btnManejarProveedores";
            this.btnManejarProveedores.Size = new System.Drawing.Size(150, 35);
            this.btnManejarProveedores.TabIndex = 5;
            this.btnManejarProveedores.Text = "Manejar Proveedores";
            this.btnManejarProveedores.UseVisualStyleBackColor = true;
            this.btnManejarProveedores.Click += new System.EventHandler(this.btnManejarProveedores_Click);

            // 
            // btnGenerarPedido
            // 
            this.btnGenerarPedido.BackColor = System.Drawing.Color.SeaGreen;
            this.btnGenerarPedido.ForeColor = System.Drawing.Color.White;
            this.btnGenerarPedido.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerarPedido.Location = new System.Drawing.Point(450, 280);
            this.btnGenerarPedido.Name = "btnGenerarPedido";
            this.btnGenerarPedido.Size = new System.Drawing.Size(220, 35);
            this.btnGenerarPedido.TabIndex = 6;
            this.btnGenerarPedido.Text = "Generar Orden de Pedido";
            this.btnGenerarPedido.UseVisualStyleBackColor = false;
            this.btnGenerarPedido.Click += new System.EventHandler(this.btnGenerarPedido_Click);

            // 
            // lblProveedores (Historial Label)
            // 
            this.lblProveedores.AutoSize = true;
            this.lblProveedores.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProveedores.Location = new System.Drawing.Point(20, 50);
            this.lblProveedores.Name = "lblHistorialPedidos";
            this.lblProveedores.Size = new System.Drawing.Size(150, 23);
            this.lblProveedores.TabIndex = 7;
            this.lblProveedores.Text = "Historial de Pedidos";

            // 
            // dgvPedidos
            // 
            this.dgvPedidos.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidos.Location = new System.Drawing.Point(20, 75);
            this.dgvPedidos.Name = "dgvPedidos";
            this.dgvPedidos.RowHeadersWidth = 51;
            this.dgvPedidos.RowTemplate.Height = 29;
            this.dgvPedidos.Size = new System.Drawing.Size(760, 200);
            this.dgvPedidos.TabIndex = 8;
            this.dgvPedidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // 
            // lblProductos
            // 
            this.lblProductos.AutoSize = true;
            this.lblProductos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductos.Location = new System.Drawing.Point(20, 290);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Size = new System.Drawing.Size(86, 23);
            this.lblProductos.TabIndex = 9;
            this.lblProductos.Text = "Productos";

            // 
            // dgvProductos
            // 
            this.dgvProductos.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Location = new System.Drawing.Point(20, 320);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.RowHeadersWidth = 51;
            this.dgvProductos.RowTemplate.Height = 29;
            this.dgvProductos.Size = new System.Drawing.Size(760, 210);
            this.dgvProductos.TabIndex = 10;
            this.dgvProductos.MultiSelect = true;
            this.dgvProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // 
            // lblInfoSeleccionMultiple
            // 
            this.lblInfoSeleccionMultiple.AutoSize = true;
            this.lblInfoSeleccionMultiple.ForeColor = System.Drawing.Color.Blue;
            this.lblInfoSeleccionMultiple.Location = new System.Drawing.Point(20, 540);
            this.lblInfoSeleccionMultiple.Name = "lblInfoSeleccionMultiple";
            this.lblInfoSeleccionMultiple.Size = new System.Drawing.Size(370, 23);
            this.lblInfoSeleccionMultiple.TabIndex = 11;
            this.lblInfoSeleccionMultiple.Text = "⚠️ Para seleccionar varios productos, presiona Ctrl + Click";

            // 
            // lblProveedor (Label for combobox)
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProveedor.Location = new System.Drawing.Point(560, 20);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(0, 23); // Optional, can be removed

            // 
            // PedidosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(800, 580);
            this.Controls.Add(this.lblInfoSeleccionMultiple);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.lblProductos);
            this.Controls.Add(this.dgvPedidos);
            this.Controls.Add(this.lblProveedores);
            this.Controls.Add(this.btnGenerarPedido);
            this.Controls.Add(this.btnManejarProveedores);
            this.Controls.Add(this.cmbProveedores);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.txtUmbral);
            this.Controls.Add(this.lblUmbral);
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
        private System.Windows.Forms.Label lblInfoSeleccionMultiple;
    }
}
