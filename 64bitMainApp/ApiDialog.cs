using System;
using System.Windows.Forms;

namespace AltInjector
{
    public enum ApiDialogResult { None, DXGI, D3D11, D3D9, OpenGL32, DInput8 };

    public partial class ApiDialog : Form
    {
        public ApiDialogResult Api = ApiDialogResult.None;

        public ApiDialog()
        {
            InitializeComponent();
            Icon = Properties.Resources.pokeball;
        }

        private void ClickedOK(object sender, EventArgs e)
        {
            if (rbDXGI.Checked)
                Api = ApiDialogResult.DXGI;
            else if (rbD3D11.Checked)
                Api = ApiDialogResult.D3D11;
            else if (rbD3D9.Checked)
                Api = ApiDialogResult.D3D9;
            else if (rbOpenGL32.Checked)
                Api = ApiDialogResult.OpenGL32;
            else if (rbDInput8.Checked)
                Api = ApiDialogResult.DInput8;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ClickedCancel(object sender, EventArgs e)
        {
            Api = ApiDialogResult.None;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ClickedLink(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
