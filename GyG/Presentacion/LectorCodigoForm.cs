using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;



namespace GyG.Presentacion
{
    public partial class LectorCodigoForm : Form
    {
        private FilterInfoCollection dispositivos;
        private VideoCaptureDevice fuenteVideo;

        public string CodigoDetectado { get; private set; }

        public LectorCodigoForm()
        {
            InitializeComponent();
        }

        private void LectorCodigoForm_Load(object sender, EventArgs e)
        {
            dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in dispositivos)
                cbCamaras.Items.Add(device.Name);

            if (cbCamaras.Items.Count > 0)
                cbCamaras.SelectedIndex = 0;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            fuenteVideo = new VideoCaptureDevice(dispositivos[cbCamaras.SelectedIndex].MonikerString);
            fuenteVideo.NewFrame += new NewFrameEventHandler(Capturar);
            fuenteVideo.Start();
            timer1.Start();
        }

        private void Capturar(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pbCamara.Image = bitmap;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pbCamara.Image != null)
            {
                Bitmap bitmap = new Bitmap(pbCamara.Image);

                BarcodeReader lector = new BarcodeReader();
                Result resultado = lector.Decode(bitmap);

                if (resultado != null)
                {
                    CodigoDetectado = resultado.Text;
                    txtCodigo.Text = CodigoDetectado;

                    timer1.Stop();
                    fuenteVideo.SignalToStop();
                    fuenteVideo.WaitForStop();
                }
            }
        }


        private void LectorCodigoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fuenteVideo != null && fuenteVideo.IsRunning)
            {
                fuenteVideo.SignalToStop();
                fuenteVideo.WaitForStop();
            }
        }
    }
}
