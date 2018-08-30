using System;
using System.IO;
using System.Windows.Forms;

namespace ItemEditor.Forms
{
    public partial class frmSMC : Form
    {
        public static string SMCFile;
        public frmSMC()
        {
            InitializeComponent();

            codeEditor1.Load();
            codeEditor1.ScrollBarsEnabled = false;
        }

        private void frmSMC_Load(object sender, EventArgs e)
        {
            codeEditor1.Text = File.ReadAllText(Configuration.cMySQL.configStructureTool[0].ClientDirectory  + @"\" + SMCFile);
        }

        private void btnSaveSMC_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to save?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
                File.WriteAllText(Configuration.cMySQL.configStructureTool[0].ClientDirectory + @"\" + SMCFile, codeEditor1.Text);
        }
    }
}
