using Timer = System.Windows.Forms.Timer;

namespace GyG.Presentacion
{
    partial class LectorCodigoForm
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pbCamara;
        private ComboBox cbCamaras;
        private Button btnIniciar;
        private TextBox txtCodigo;
        private Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pbCamara = new PictureBox();
            this.cbCamaras = new ComboBox();
            this.btnIniciar = new Button();
            this.txtCodigo = new TextBox();
            this.timer1 = new Timer();

            // 
            // LectorCodigoForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Text = "Lector de Código de Barras";
            this.FormClosing += new FormClosingEventHandler(this.LectorCodigoForm_FormClosing);
            this.Load += new EventHandler(this.LectorCodigoForm_Load);

            // 
            // pbCamara
            // 
            this.pbCamara.Location = new System.Drawing.Point(20, 20);
            this.pbCamara.Size = new System.Drawing.Size(400, 300);
            this.pbCamara.BorderStyle = BorderStyle.Fixed3D;

            // 
            // cbCamaras
            // 
            this.cbCamaras.Location = new System.Drawing.Point(20, 340);
            this.cbCamaras.Width = 250;

            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(290, 340);
            this.btnIniciar.Text = "Iniciar Cámara";
            this.btnIniciar.Click += new EventHandler(this.btnIniciar_Click);

            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(20, 380);
            this.txtCodigo.Width = 400;
            this.txtCodigo.ReadOnly = true;

            // 
            // timer1
            // 
            this.timer1.Interval = 100; // Milisegundos
            this.timer1.Tick += new EventHandler(this.timer1_Tick);

            // 
            // Add Controls
            // 
            this.Controls.Add(this.pbCamara);
            this.Controls.Add(this.cbCamaras);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.txtCodigo);
        }
    }
}
