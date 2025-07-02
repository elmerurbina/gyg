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
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "GyG - Inicio de Sesión";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // 
            // lblTitulo
            // 
            this.lblTitulo.Text = "Sistema GyG";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(120, 20);
            this.lblTitulo.AutoSize = true;

            // 
            // lblUsuario
            // 
            this.lblUsuario.Text = "Usuario:";
            this.lblUsuario.Location = new System.Drawing.Point(50, 80);
            this.lblUsuario.AutoSize = true;

            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(150, 80);
            this.txtUsuario.Width = 180;

            // 
            // lblClave
            // 
            this.lblClave.Text = "Contraseña:";
            this.lblClave.Location = new System.Drawing.Point(50, 130);
            this.lblClave.AutoSize = true;

            // 
            // txtClave
            // 
            this.txtClave.Location = new System.Drawing.Point(150, 130);
            this.txtClave.Width = 180;
            this.txtClave.PasswordChar = '*';

            // 
            // btnIniciarSesion
            // 
            this.btnIniciarSesion.Text = "Iniciar Sesión";
            this.btnIniciarSesion.Location = new System.Drawing.Point(150, 180);
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
