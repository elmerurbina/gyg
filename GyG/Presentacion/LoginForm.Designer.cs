namespace GyG.Presentacion
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblClave;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private Button btnIniciarSesion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.lblUsuario = new Label();
            this.lblClave = new Label();
            this.txtUsuario = new TextBox();
            this.txtClave = new TextBox();
            this.btnIniciarSesion = new Button();

            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Peleteria Sacuanjoche - Inicio de Sesión";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // 
            // lblTitulo
            // 
            this.lblTitulo.Text = "Peleteria Sacuanjoche: ¡Gestionamos operaciones, generamos mas ganancias!";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(80, 20);
            this.lblTitulo.AutoSize = true;

            // 
            // lblUsuario
            // 
            this.lblUsuario.Text = "Usuario:";
            this.lblUsuario.Location = new System.Drawing.Point(50, 120);
            this.lblUsuario.AutoSize = true;

            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(250, 120);
            this.txtUsuario.Width = 380;

            // 
            // lblClave
            // 
            this.lblClave.Text = "Contraseña:";
            this.lblClave.Location = new System.Drawing.Point(50, 190);
            this.lblClave.AutoSize = true;

            // 
            // txtClave
            // 
            this.txtClave.Location = new System.Drawing.Point(250, 190);
            this.txtClave.Width = 380;
            this.txtClave.PasswordChar = '*';

            // 
            // btnIniciarSesion
            // 
            this.btnIniciarSesion.Text = "Iniciar Sesión";
            this.btnIniciarSesion.Location = new System.Drawing.Point(190, 270);
            this.btnIniciarSesion.BackColor = ColorTranslator.FromHtml("#8B5E3C");
            this.btnIniciarSesion.Width = 300;
            this.btnIniciarSesion.Height = 60;
            this.btnIniciarSesion.Click += new EventHandler(this.btnIniciarSesion_Click);

            // 
            // Add controls
            // 
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblClave);
            this.Controls.Add(this.txtClave);
            this.Controls.Add(this.btnIniciarSesion);
        }
    }
}
