using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ItemEditor.Structure;
using ItemEditor.LastChaosUtil;
using ItemEditor.Configuration;

namespace ItemEditor.Forms.Pickers
{
    public partial class frmIconPicker : Form
    {
        public static int[] iIcon = { 0, 0, 0 },
            iRectangle = { 1, 2, 29, 29 };

        public frmIconPicker()
        {
            InitializeComponent();

            var files = Enumerable.Empty<string>();

            if(cMySQL.configStructureTool[0].UseTextureFilesFromClient)
                files = Directory.EnumerateFiles(cMySQL.configStructureTool[0].ClientDirectory + @"\Data\Interface").Where(d => Path.GetFileNameWithoutExtension(d).Contains("ItemBtn"));
            else
                files = Directory.EnumerateFiles("Images").Where(d => Path.GetFileNameWithoutExtension(d).Contains("ItemBtn"));

            for (int i = 0; i < files.Count(); i++)
            {
                Control[] FILE = Controls.Find("FILE" + i.ToString(), true);
                FILE[0].Visible = true;
            }

            if (!cMySQL.configStructureTool[0].UseTextureFilesFromClient)
            {
                gbInformations.Text = "Selected Icon";

                lbWidth.Visible = false;
                lbHeight.Visible = false;
                lbFormat.Visible = false;
                lbVersion.Visible = false;
                tbFlag.Visible = false;
                gbFlag.Visible = false;

                SelectedIcon.Location = new Point(102, 65);

                btnGrab.Location = new Point(16, 127);
            }

            {
                cTempVariables cTempVariables = new cTempVariables();
                cTextureManagement cTextureManagement = new cTextureManagement();

                if (cMySQL.configStructureTool[0].UseTextureFilesFromClient)
                    cTextureManagement.ReadFile(cMySQL.configStructureTool[0].ClientDirectory + "\\Data\\Interface\\ItemBtn" + cTempVariables.lastTexID + ".tex");

                pbIcon.Image = cMySQL.configStructureTool[0].UseTextureFilesFromClient ? cTextureManagement.MakeBitmap() : Image.FromFile("Images\\ItemBtn" + cTempVariables.lastTexID + ".png");
            }

            lbUsingMode.Text = cMySQL.configStructureTool[0].UseTextureFilesFromClient ? "TEXTURE MODE" : "IMAGE MODE";
        }

        private void frmIconPicker_Load(object sender, EventArgs e)
        {

        }

        private void checkFlagInListBox(CheckedListBox clbFlagControl, ulong flag)
        {
            for (int index = 0; index < clbFlagControl.Items.Count; ++index)
                clbFlagControl.SetItemChecked(index, Convert.ToBoolean(((ulong)1 << index) & flag));
        }

        private void FILE_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            cTextureManagement cTextureManagement = new cTextureManagement();

            if (cMySQL.configStructureTool[0].UseTextureFilesFromClient)
            {
                cTextureManagement.ReadFile(cMySQL.configStructureTool[0].ClientDirectory + "\\Data\\Interface\\ItemBtn" + btn.Text + ".tex");
                lbWidth.Text    = "Width: " + cTextureManagement.cTextureHeader.Width.ToString();
                lbHeight.Text   = "Height: " + cTextureManagement.cTextureHeader.Height.ToString();
                lbFormat.Text   = "Format: " + cTextureManagement.textureFormat.ToString();
                lbVersion.Text  = "Version: " + cTextureManagement.cTextureHeader.Version.ToString();
                tbFlag.Text     = cTextureManagement.cTextureHeader.Bits.ToString();

                checkFlagInListBox(clbTextureFlag, cTextureManagement.cTextureHeader.Bits);
            }

            pbIcon.Image = cMySQL.configStructureTool[0].UseTextureFilesFromClient ? cTextureManagement.MakeBitmap() : Image.FromFile("Images\\ItemBtn" + btn.Text + ".png");

            iIcon[0] = Convert.ToInt32(btn.Text);
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void pbIcon_MouseClick(object sender, MouseEventArgs e)
        {
            iIcon[1] = (int)Math.Floor(e.X / 32f);
            iIcon[2] = (int)Math.Floor(e.Y / 32f);

            cTextureManagement cTextureManagement = new cTextureManagement();

            Bitmap bmpMain;
            if (cMySQL.configStructureTool[0].UseTextureFilesFromClient)
                bmpMain = new Bitmap(cTextureManagement.MakeBitmap());
            else
                bmpMain = new Bitmap("Images\\ItemBtn" + iIcon[0] + ".png");

            try { SelectedIcon.Image = bmpMain.Clone(new Rectangle(iIcon[1] * 32 + iRectangle[0], iIcon[2] * 32 + iRectangle[1], iRectangle[2], iRectangle[3]), bmpMain.PixelFormat); }
            catch { }
        }
    }
}
