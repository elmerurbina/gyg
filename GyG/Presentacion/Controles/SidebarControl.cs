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
            this.Width = 200;
            this.Dock = DockStyle.Left;
            this.BackColor = Color.Teal;

            InicializarSidebar();
        }

        private void InicializarSidebar()
        {
            string[] opciones = {
                "Inventario", "Ventas", "Proformas", "Pedidos", "Contabilidad", "Gráficos",  "Clientes" 
            };

            int top = 20;

            foreach (string texto in opciones)
            {
                Button btn = new Button
                {
                    Text = texto,
                    Width = 180,
                    Height = 40,
                    Top = top,
                    Left = 10,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                btn.Click += (s, e) => OpcionSeleccionada?.Invoke(this, texto);

                this.Controls.Add(btn);
                top += 50;
            }
        }
    }
}