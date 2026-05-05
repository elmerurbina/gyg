namespace GyG.Presentacion
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Controles.HeaderControl header;
    

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.header = new Controles.HeaderControl();
            

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(10000, 7000);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.Text = "Peleteria Sacuanjoche";
            
            // 
            // Header
            // 
            this.header.Dock = DockStyle.Top;
            this.header.Height = 60;

            // 
            // Sidebar
            // 
           

            // 
            // Add Controls
            // 
            this.Controls.Add(this.header);
            
        }
    }
}