using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemEditor.Forms.Pickers
{
    public partial class frmModelInfo : Form
    {
        public frmModelInfo()
        {
            InitializeComponent();
        }

        private void frmModelInfo_Load(object sender, EventArgs e)
        { }

        /*
        public void LoadTexture(Bitmap bmp, int version, int format, int width, int height, ulong flag)
        {
            pbTexture.Image = bmp;

            lbFormat.Text   = "Format: " + format.ToString();
            lbVersion.Text  = "Version: " + version.ToString();
            lbWidth.Text    = "Width: " + width.ToString();
            lbHeight.Text   = "Height: " + height.ToString();

            checkFlagInListBox(clbTextureFlag, flag);
        }

        private void checkFlagInListBox(CheckedListBox clbFlagControl, ulong flag)
        {
            for (int index = 0; index < clbFlagControl.Items.Count; ++index)
                clbFlagControl.SetItemChecked(index, Convert.ToBoolean(((ulong)1 << index) & flag));
        }
        */
    }
}
