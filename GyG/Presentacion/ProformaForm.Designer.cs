namespace GyG.Presentacion
{
    partial class ProformaForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvProformas;
        private System.Windows.Forms.Label lblTitulo;

        /// <summary>
        /// Limpiar recursos.
        /// </summary>
        /// <param name="disposing">true si se deben eliminar los recursos administrados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvProformas = new System.Windows.Forms.DataGridView();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProformas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProformas
            // 
            this.dgvProformas.AllowUserToAddRows = false;
            this.dgvProformas.AllowUserToDeleteRows = false;
            this.dgvProformas.AllowUserToResizeRows = false;
            this.dgvProformas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProformas.Location = new System.Drawing.Point(20, 60);
            this.dgvProformas.MultiSelect = false;
            this.dgvProformas.Name = "dgvProformas";
            this.dgvProformas.ReadOnly = true;
            this.dgvProformas.RowHeadersVisible = false;
            this.dgvProformas.RowTemplate.Height = 25;
            this.dgvProformas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProformas.Size = new System.Drawing.Size(860, 400);
            this.dgvProformas.TabIndex = 0;
           // this.dgvProformas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProformas_CellContentClick);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(212, 25);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "📄 Gestión de Proformas";
            // 
            // ProformasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 480);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.dgvProformas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProformasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proformas";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProformas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
