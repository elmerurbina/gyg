using System.ComponentModel;

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

    private void InitializeComponent()
    {
        this.dgvEstadoResultados = new DataGridView();
        this.dgvBalanceGeneral = new DataGridView();
        this.dgvLibroDiario = new DataGridView();
        this.dgvLibroMayor = new DataGridView();
        this.dgvFlujoCaja = new DataGridView();

        this.lblEstadoResultados = new Label();
        this.lblBalanceGeneral = new Label();
        this.lblLibroDiario = new Label();
        this.lblLibroMayor = new Label();
        this.lblFlujoCaja = new Label();

        this.SuspendLayout();

        // Colores comunes ferretería
        var colorFondoForm = System.Drawing.Color.FromArgb(40, 40, 40);
        var colorFondoGrid = System.Drawing.Color.FromArgb(255, 140, 0); // Naranja oscuro
        var colorTextoGrid = System.Drawing.Color.Black; // Texto negro para celdas
        var colorEtiqueta = System.Drawing.Color.Orange;

        // 
        // ContabilidadForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1000, 800);  // Aumenté la altura del form a 800 px
        this.BackColor = colorFondoForm;
        this.Name = "ContabilidadForm";
        this.Text = "Reportes Contables";

        // --- Labels ---
        void SetupLabel(Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lbl.ForeColor = colorEtiqueta;
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Text = text;
        }

        SetupLabel(lblEstadoResultados, "Estado de Resultados", 20, 10);
        SetupLabel(lblBalanceGeneral, "Balance General", 530, 10);
        SetupLabel(lblLibroDiario, "Libro Diario", 20, 220);
        SetupLabel(lblLibroMayor, "Libro Mayor", 530, 220);
        SetupLabel(lblFlujoCaja, "Flujo de Caja", 20, 460); 

        // --- DataGridViews ---
        void SetupGrid(DataGridView dgv, int x, int y, int w, int h)
        {
            dgv.Location = new System.Drawing.Point(x, y);
            dgv.Size = new System.Drawing.Size(w, h);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.BackgroundColor = colorFondoGrid;
            dgv.ForeColor = colorTextoGrid;  
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 165, 0); // naranja más claro
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White; // Encabezados en blanco para contraste
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
        }

        SetupGrid(dgvEstadoResultados, 20, 35, 460, 160);
        SetupGrid(dgvBalanceGeneral, 530, 35, 440, 160);
        SetupGrid(dgvLibroDiario, 20, 245, 460, 210);
        SetupGrid(dgvLibroMayor, 530, 245, 440, 210);
        SetupGrid(dgvFlujoCaja, 20, 485, 950, 170); 
        // Añadir controles al formulario
        this.Controls.Add(lblEstadoResultados);
        this.Controls.Add(dgvEstadoResultados);
        this.Controls.Add(lblBalanceGeneral);
        this.Controls.Add(dgvBalanceGeneral);
        this.Controls.Add(lblLibroDiario);
        this.Controls.Add(dgvLibroDiario);
        this.Controls.Add(lblLibroMayor);
        this.Controls.Add(dgvLibroMayor);
        this.Controls.Add(lblFlujoCaja);
        this.Controls.Add(dgvFlujoCaja);

        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
