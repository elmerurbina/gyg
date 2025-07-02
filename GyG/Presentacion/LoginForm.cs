using System;
using System.Windows.Forms;
using GyG.Datos;

namespace GyG.Presentacion
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (LoginDAO.VerificarCredenciales(usuario, clave))
            {
                this.Hide();
                MainForm main = new MainForm();
                main.Show();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}