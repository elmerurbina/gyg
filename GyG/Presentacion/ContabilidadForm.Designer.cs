using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GyG.Presentacion;

partial class ContabilidadForm
{
    private System.ComponentModel.IContainer components = null;
    private DataGridView dgvEstadoResultados;
    private DataGridView dgvBalanceGeneral;
    private DataGridView dgvLibroDiario;
    private DataGridView dgvLibroMayor;
    private DataGridView dgvFlujoCaja;
    private Label lblEstadoResultados;
    private Label lblBalanceGeneral;
    private Label lblLibroDiario;
    private Label lblLibroMayor;
    private Label lblFlujoCaja;
    
    private Panel pnlMainContainer;

    private void InitializeComponent()
    {
        this.dgvEstadoResultados = new DataGridView();
        this.dgvBalanceGeneral = new DataGridView();
        this.dgvLibroDiario = new DataGridView();
        this.dgvLibroMayor = new DataGridView();
        this.dgvFlujoCaja = new DataGridView();
        this.pnlMainContainer = new Panel();

        this.lblEstadoResultados = new Label();
        this.lblBalanceGeneral = new Label();
        this.lblLibroDiario = new Label();
        this.lblLibroMayor = new Label();
        this.lblFlujoCaja = new Label();

        ((ISupportInitialize)(this.dgvEstadoResultados)).BeginInit();
        ((ISupportInitialize)(this.dgvBalanceGeneral)).BeginInit();
        ((ISupportInitialize)(this.dgvLibroDiario)).BeginInit();
        ((ISupportInitialize)(this.dgvLibroMayor)).BeginInit();
        ((ISupportInitialize)(this.dgvFlujoCaja)).BeginInit();
        this.pnlMainContainer.SuspendLayout();
        this.SuspendLayout();

        // ========== PALETA DE COLORES SACUANJOCHE ==========
        Color primary500 = Color.FromArgb(139, 94, 60);      // #8B5E3C
        Color primary300 = Color.FromArgb(196, 164, 132);    // #C4A484
        Color secondary500 = Color.FromArgb(255, 193, 7);    // #FFC107
        Color textPrimary = Color.FromArgb(45, 41, 38);      // #2D2926
        Color textLight = Color.FromArgb(255, 249, 230);     // Secundario 100
        Color inputBg = Color.FromArgb(249, 249, 249);       // Gris 100
        Color formBg = Color.FromArgb(255, 249, 230);        // Secundario 100
        Color panelBg = Color.FromArgb(243, 235, 225);       // Primario 100
        Color headerBg = primary500;
        Color headerText = Color.White;
        Color gridLine = primary300;

        // ========== FORM CONFIGURATION ==========
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(1300, 850);
        this.BackColor = formBg;
        this.Name = "ContabilidadForm";
        this.Text = "Reportes Contables - Sacuanjoche";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(1100, 700);
        this.Font = new Font("Segoe UI", 9F);

        // ========== MAIN CONTAINER ==========
        this.pnlMainContainer.Dock = DockStyle.Fill;
        this.pnlMainContainer.BackColor = formBg;
        this.pnlMainContainer.Padding = new Padding(15);
        this.pnlMainContainer.AutoScroll = true;

        // ========== LABELS ==========
        Font labelFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        
        void SetupLabel(Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Font = labelFont;
            lbl.ForeColor = primary500;
            lbl.BackColor = Color.Transparent;
            lbl.Location = new Point(x, y);
            lbl.Text = text;
        }

        SetupLabel(lblEstadoResultados, "📊 Estado de Resultados", 25, 10);
        SetupLabel(lblBalanceGeneral, "💰 Balance General", 680, 10);
        SetupLabel(lblLibroDiario, "📓 Libro Diario", 25, 260);
        SetupLabel(lblLibroMayor, "📒 Libro Mayor", 680, 260);
        SetupLabel(lblFlujoCaja, "💵 Flujo de Caja", 25, 510);

        // ========== DATA GRID VIEWS ==========
        void SetupGrid(DataGridView dgv, int x, int y, int w, int h)
        {
            dgv.Location = new Point(x, y);
            dgv.Size = new Size(w, h);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.BackgroundColor = inputBg;
            dgv.ForeColor = textPrimary;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.GridColor = gridLine;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Estilo de celdas
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.ForeColor = textPrimary;
            dgv.DefaultCellStyle.BackColor = inputBg;
            dgv.DefaultCellStyle.SelectionBackColor = secondary500;
            dgv.DefaultCellStyle.SelectionForeColor = textPrimary;
            
            // Estilo de encabezados
            dgv.ColumnHeadersDefaultCellStyle.BackColor = headerBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = headerText;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            
            // Estilo de filas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            dgv.RowTemplate.Height = 35;
        }

        // Estado de Resultados (fila 1, columna 1)
        SetupGrid(dgvEstadoResultados, 25, 35, 620, 200);
        
        // Balance General (fila 1, columna 2)
        SetupGrid(dgvBalanceGeneral, 680, 35, 580, 200);
        
        // Libro Diario (fila 2, columna 1)
        SetupGrid(dgvLibroDiario, 25, 285, 620, 200);
        
        // Libro Mayor (fila 2, columna 2)
        SetupGrid(dgvLibroMayor, 680, 285, 580, 200);
        
        // Flujo de Caja (fila 3, ancho completo)
        SetupGrid(dgvFlujoCaja, 25, 535, 1235, 180);

        // ========== AGREGAR CONTROLES AL PANEL ==========
        this.pnlMainContainer.Controls.Add(lblEstadoResultados);
        this.pnlMainContainer.Controls.Add(dgvEstadoResultados);
        this.pnlMainContainer.Controls.Add(lblBalanceGeneral);
        this.pnlMainContainer.Controls.Add(dgvBalanceGeneral);
        this.pnlMainContainer.Controls.Add(lblLibroDiario);
        this.pnlMainContainer.Controls.Add(dgvLibroDiario);
        this.pnlMainContainer.Controls.Add(lblLibroMayor);
        this.pnlMainContainer.Controls.Add(dgvLibroMayor);
        this.pnlMainContainer.Controls.Add(lblFlujoCaja);
        this.pnlMainContainer.Controls.Add(dgvFlujoCaja);

        // ========== AGREGAR PANEL AL FORMULARIO ==========
        this.Controls.Add(this.pnlMainContainer);

        // ========== CLEANUP ==========
        ((ISupportInitialize)(this.dgvEstadoResultados)).EndInit();
        ((ISupportInitialize)(this.dgvBalanceGeneral)).EndInit();
        ((ISupportInitialize)(this.dgvLibroDiario)).EndInit();
        ((ISupportInitialize)(this.dgvLibroMayor)).EndInit();
        ((ISupportInitialize)(this.dgvFlujoCaja)).EndInit();
        this.pnlMainContainer.ResumeLayout(false);
        this.pnlMainContainer.PerformLayout();
        this.ResumeLayout(false);
    }
}