namespace GyG.Presentacion
{
    partial class GestionCategoriasForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controles
        private System.Windows.Forms.DataGridView dgvCategorias;
        private System.Windows.Forms.TextBox txtCategoria;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        
        // Panel para mejor organización
        private Panel pnlMainContainer;
        private Panel pnlButtons;

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
            this.dgvCategorias = new System.Windows.Forms.DataGridView();
            this.txtCategoria = new System.Windows.Forms.TextBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.pnlMainContainer = new Panel();
            this.pnlButtons = new Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCategorias)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // ========== COLOR PALETTE (Sacuanjoche) ==========
            Color primary500 = Color.FromArgb(139, 94, 60);      // #8B5E3C
            Color secondary500 = Color.FromArgb(255, 193, 7);    // #FFC107
            Color errorColor = Color.FromArgb(211, 47, 47);      // #D32F2F
            Color textPrimary = Color.FromArgb(45, 41, 38);      // #2D2926
            Color inputBg = Color.FromArgb(249, 249, 249);       // Gris 100
            Color formBg = Color.FromArgb(255, 249, 230);        // Secundario 100
            Color panelBg = Color.FromArgb(243, 235, 225);       // Primario 100

            // ========== FORM CONFIGURATION ==========
            this.BackColor = formBg;
            this.Text = "Gestión de Categorías - Sacuanjoche";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new Size(600, 500);
            this.Size = new Size(700, 550);
            this.Font = new Font("Segoe UI", 9F);
            this.Padding = new Padding(15);

            // ========== MAIN CONTAINER ==========
            this.pnlMainContainer.Dock = DockStyle.Fill;
            this.pnlMainContainer.BackColor = formBg;
            this.pnlMainContainer.Padding = new Padding(15);
            this.pnlMainContainer.AutoScroll = true;

            // ========== DATA GRID VIEW ==========
            // Configuración del DataGridView
            this.dgvCategorias.AllowUserToAddRows = false;
            this.dgvCategorias.AllowUserToDeleteRows = false;
            this.dgvCategorias.AllowUserToResizeRows = false;
            this.dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategorias.BackgroundColor = inputBg;
            this.dgvCategorias.BorderStyle = BorderStyle.FixedSingle;
            this.dgvCategorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategorias.Dock = DockStyle.Top;
            this.dgvCategorias.Location = new Point(0, 0);
            this.dgvCategorias.Size = new Size(650, 300);
            this.dgvCategorias.MultiSelect = false;
            this.dgvCategorias.Name = "dgvCategorias";
            this.dgvCategorias.ReadOnly = true;
            this.dgvCategorias.RowHeadersVisible = false;
            this.dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategorias.GridColor = Color.FromArgb(196, 164, 132);
            this.dgvCategorias.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            this.dgvCategorias.DefaultCellStyle.ForeColor = textPrimary;
            this.dgvCategorias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvCategorias.ColumnHeadersDefaultCellStyle.BackColor = primary500;
            this.dgvCategorias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvCategorias.EnableHeadersVisualStyles = false;
            this.dgvCategorias.CellClick += new DataGridViewCellEventHandler(this.dgvCategorias_CellClick);

            // Agregar columnas al DataGridView
            var colId = new DataGridViewTextBoxColumn();
            colId.Name = "Id";
            colId.HeaderText = "ID";
            colId.Visible = false;
            this.dgvCategorias.Columns.Add(colId);

            var colNombre = new DataGridViewTextBoxColumn();
            colNombre.Name = "Nombre";
            colNombre.HeaderText = "Nombre de Categoría";
            colNombre.ReadOnly = true;
            this.dgvCategorias.Columns.Add(colNombre);

            // ========== FORM FIELDS PANEL ==========
            Panel pnlFields = new Panel();
            pnlFields.Dock = DockStyle.Top;
            pnlFields.Height = 120;
            pnlFields.BackColor = panelBg;
            pnlFields.Padding = new Padding(15);
            pnlFields.Margin = new Padding(0, 15, 0, 0);

            // Label - MOVED TO THE RIGHT
            this.lblCategoria.Text = "Nombre de Categoría:";
            this.lblCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblCategoria.ForeColor = textPrimary;
            this.lblCategoria.Location = new Point(15, 20);
            this.lblCategoria.Size = new Size(130, 35);
            this.lblCategoria.TextAlign = ContentAlignment.MiddleRight;

            // TextBox - WIDER and TALLER
            this.txtCategoria.Font = new Font("Segoe UI", 9F);
            this.txtCategoria.BackColor = inputBg;
            this.txtCategoria.ForeColor = textPrimary;
            this.txtCategoria.BorderStyle = BorderStyle.FixedSingle;
            this.txtCategoria.Location = new Point(155, 20);
            this.txtCategoria.Size = new Size(460, 35);
            this.txtCategoria.PlaceholderText = "Ingrese el nombre de la categoría";

            pnlFields.Controls.Add(this.lblCategoria);
            pnlFields.Controls.Add(this.txtCategoria);

            // ========== BUTTONS PANEL ==========
            this.pnlButtons.Dock = DockStyle.Top;
            this.pnlButtons.Height = 80;
            this.pnlButtons.BackColor = formBg;
            this.pnlButtons.Padding = new Padding(15);

            int btnWidth = 130;
            int btnHeight = 42;
            int btnSpacing = 20;
            int btnStartX = 15;

            // Botón Agregar - Color Primario 500
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Size = new Size(btnWidth, btnHeight);
            this.btnAgregar.Location = new Point(btnStartX, 15);
            this.btnAgregar.BackColor = primary500;
            this.btnAgregar.ForeColor = Color.White;
            this.btnAgregar.FlatStyle = FlatStyle.Flat;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnAgregar.Cursor = Cursors.Hand;
            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);

            // Botón Editar - Color Secundario 500
            this.btnEditar.Text = "Editar";
            this.btnEditar.Size = new Size(btnWidth, btnHeight);
            this.btnEditar.Location = new Point(btnStartX + btnWidth + btnSpacing, 15);
            this.btnEditar.BackColor = secondary500;
            this.btnEditar.ForeColor = textPrimary;
            this.btnEditar.FlatStyle = FlatStyle.Flat;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnEditar.Cursor = Cursors.Hand;
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);

            // Botón Eliminar - Color Error
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Size = new Size(btnWidth, btnHeight);
            this.btnEliminar.Location = new Point(btnStartX + (btnWidth + btnSpacing) * 2, 15);
            this.btnEliminar.BackColor = errorColor;
            this.btnEliminar.ForeColor = Color.White;
            this.btnEliminar.FlatStyle = FlatStyle.Flat;
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnEliminar.Cursor = Cursors.Hand;
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            this.pnlButtons.Controls.Add(this.btnAgregar);
            this.pnlButtons.Controls.Add(this.btnEditar);
            this.pnlButtons.Controls.Add(this.btnEliminar);

            // Agregar controles al panel principal
            this.pnlMainContainer.Controls.Add(this.dgvCategorias);
            this.pnlMainContainer.Controls.Add(pnlFields);
            this.pnlMainContainer.Controls.Add(this.pnlButtons);

            // Agregar panel principal al formulario
            this.Controls.Add(this.pnlMainContainer);

            // ========== CLEANUP RESOURCES ==========
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategorias)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}