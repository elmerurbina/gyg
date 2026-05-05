namespace GyG.Presentacion.Controles
{
    partial class HeaderControl
    {
        private Label lblTitulo;
        private Label lblSubTitulo;
        private PictureBox picLogo;

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.lblSubTitulo = new Label();
            this.picLogo = new PictureBox();

            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();

            // ========== COLOR PALETTE (Sacuanjoche) ==========
            Color primary500 = Color.FromArgb(139, 94, 60);
            Color textWhite = Color.White;
            Color textLight = Color.FromArgb(255, 249, 230);

            // ========== HEADER CONTROL CONFIGURATION ==========
            this.BackColor = primary500;
            this.Height = 85;
            this.Dock = DockStyle.Top;
            this.MinimumSize = new Size(600, 85);

            // ========== LOGO ==========
            this.picLogo.Size = new Size(55, 55);
            this.picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picLogo.BackColor = Color.Transparent;
            this.picLogo.Location = new Point(20, 15);
            
            // Cargar logo desde archivo
            string logoPath = Path.Combine(Application.StartupPath, "Resources", "logo.png");
            if (File.Exists(logoPath))
            {
                try
                {
                    this.picLogo.Image = Image.FromFile(logoPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error cargando logo: {ex.Message}");
                }
            }

            // ========== TITULO LABEL (Arriba) ==========
            this.lblTitulo.Text = "Sistema Integral de Punto de Venta, Inventario y Análisis de Datos";
            this.lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitulo.ForeColor = textWhite;
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new Point(90, 18);
            this.lblTitulo.MaximumSize = new Size(this.Width - 120, 0);

            // ========== SUBTITULO LABEL (Debajo del título) ==========
            this.lblSubTitulo.Text = "Peletería Sacuanjoche - Calidad y Estilo";
            this.lblSubTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblSubTitulo.ForeColor = textLight;
            this.lblSubTitulo.AutoSize = true;
            this.lblSubTitulo.Location = new Point(90, 52);
            this.lblSubTitulo.MaximumSize = new Size(this.Width - 120, 0);

            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblSubTitulo);

            // Evento para ajustar texto responsivamente
            this.Resize += (s, e) => AjustarTextoResponsivo();

            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AjustarTextoResponsivo()
        {
            int logoWidth = 55;
            int leftMargin = 90;
            
            if (this.Width < 700)
            {
                lblTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                lblSubTitulo.Font = new Font("Segoe UI", 7F, FontStyle.Italic);
                lblTitulo.MaximumSize = new Size(this.Width - leftMargin - 20, 0);
                lblSubTitulo.MaximumSize = new Size(this.Width - leftMargin - 20, 0);
                this.Height = 75;
                picLogo.Size = new Size(45, 45);
                picLogo.Location = new Point(15, 12);
                lblTitulo.Location = new Point(75, 15);
                lblSubTitulo.Location = new Point(75, 45);
            }
            else if (this.Width < 1000)
            {
                lblTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblSubTitulo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
                lblTitulo.MaximumSize = new Size(this.Width - leftMargin - 20, 0);
                lblSubTitulo.MaximumSize = new Size(this.Width - leftMargin - 20, 0);
                this.Height = 80;
                picLogo.Size = new Size(50, 50);
                picLogo.Location = new Point(18, 13);
                lblTitulo.Location = new Point(85, 16);
                lblSubTitulo.Location = new Point(85, 48);
            }
            else
            {
                lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                lblSubTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
                lblTitulo.MaximumSize = new Size(this.Width - leftMargin - 20, 0);
                lblSubTitulo.MaximumSize = new Size(this.Width - leftMargin - 20, 0);
                this.Height = 85;
                picLogo.Size = new Size(55, 55);
                picLogo.Location = new Point(20, 15);
                lblTitulo.Location = new Point(90, 18);
                lblSubTitulo.Location = new Point(90, 52);
            }
        }
    }
}