namespace GyG.Presentacion.Controles
{
    partial class HeaderControl
    {
        private Label lblTitulo;

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();

            // 
            // lblTitulo
            // 
            this.lblTitulo.Text = "Sistema Integral de Punto de Venta, Inventario y Análisis de Datos";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);

            // 
            // HeaderControl
            // 
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.Height = 60;
            this.Dock = DockStyle.Top;
            this.Controls.Add(this.lblTitulo);
        }
    }
}