using System;
using System.Windows.Forms;
using System.Drawing;

namespace GyG.Presentacion.Controles
{
    public partial class SidebarControl : UserControl
    {
        public event EventHandler<string> OpcionSeleccionada;

        public SidebarControl()
        {
            this.Width = 300;  // Aumentado de 230 a 250
            this.Dock = DockStyle.Left;
            this.BackColor = Color.FromArgb(139, 94, 60);
            
            InicializarSidebar();
        }

        private void InicializarSidebar()
        {
            string[] opciones = {
                "Inventario", 
                "Ventas", 
                "Proformas", 
                "Pedidos", 
                "Contabilidad", 
                "Graficos", 
                "Clientes"
            };

            int btnWidth = this.Width - 20;
            int btnHeight = 45;
            int btnSpacing = 8;

            // ========== PALETA DE COLORES SACUANJOCHE ==========
            Color primary500 = Color.FromArgb(139, 94, 60);
            Color primary400 = Color.FromArgb(166, 124, 82);
            Color primary300 = Color.FromArgb(196, 164, 132);
            Color textWhite = Color.White;
            Color buttonBg = Color.FromArgb(243, 235, 225);
            Color buttonHover = Color.FromArgb(255, 193, 7);
            Color buttonText = Color.FromArgb(45, 41, 38);

            // ========== TÍTULO DEL SIDEBAR - MÁS ANCHO ==========
            Panel pnlTitle = new Panel
            {
                Width = this.Width,
                Height = 90,
                BackColor = Color.FromArgb(139, 94, 60)
            };

            Label lblTitulo = new Label
            {
                Text = "SACUANJOCHE",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = textWhite,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false
            };
            pnlTitle.Controls.Add(lblTitulo);
            
            // Línea separadora debajo del título
            Panel lineSeparator = new Panel
            {
                Height = 2,
                Width = this.Width - 30,
                Left = 15,
                Top = 90,
                BackColor = primary300
            };

            this.Controls.Add(pnlTitle);
            this.Controls.Add(lineSeparator);

            // ========== BOTONES DEL SIDEBAR ==========
            int currentTop = 110;
            
            foreach (string texto in opciones)
            {
                Button btn = new Button
                {
                    Text = texto,
                    Width = btnWidth - 20,
                    Height = btnHeight,
                    Top = currentTop,
                    Left = 15,
                    BackColor = buttonBg,
                    ForeColor = buttonText,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(20, 0, 0, 0),
                    Cursor = Cursors.Hand
                };
                
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = buttonHover;
                btn.FlatAppearance.MouseDownBackColor = primary400;

                string opcionSeleccionada = texto;
                btn.Click += (s, e) => OpcionSeleccionada?.Invoke(this, opcionSeleccionada);

                this.Controls.Add(btn);
                currentTop += btnHeight + btnSpacing;
            }

            // ========== BOTÓN DE CIERRE DE SESIÓN ==========
            Button btnCerrarSesion = new Button
            {
                Text = "Cerrar Sesion",
                Width = btnWidth - 20,
                Height = btnHeight,
                Left = 15,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatAppearance.MouseOverBackColor = Color.FromArgb(211, 47, 47);
            btnCerrarSesion.Click += (s, e) => Application.Exit();
            
            // Posicionar al final
            this.Controls.Add(btnCerrarSesion);
            
            // Actualizar posición cuando se redimensione
            this.Resize += (sender, e) =>
            {
                btnCerrarSesion.Top = this.Height - 65;
            };
            
            // Posición inicial
            btnCerrarSesion.Top = this.Height - 65;
        }

        private Button btnActivo = null;
        
        public void SeleccionarOpcion(string opcion)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn.Text == opcion)
                {
                    if (btnActivo != null)
                    {
                        btnActivo.BackColor = Color.FromArgb(243, 235, 225);
                        btnActivo.ForeColor = Color.FromArgb(45, 41, 38);
                    }
                    
                    btnActivo = btn;
                    btnActivo.BackColor = Color.FromArgb(255, 193, 7);
                    btnActivo.ForeColor = Color.FromArgb(45, 41, 38);
                    break;
                }
            }
        }
    }
}