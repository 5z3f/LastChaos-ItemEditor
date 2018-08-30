using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using ItemEditor.Structure;
using ItemEditor.Configuration;
using System.Linq;
using ItemEditor.Data;
using System.Collections.Generic;
using System.IO;

namespace ItemEditor.Forms
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void MakeList()
        {
            lbList.Items.Clear();

            for (int i = 0; i < cMySQL.configStructure.Count(); i++)
                lbList.Items.Add(i + " - " + cMySQL.configStructure[i].Name);
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            cImport.GetConfiguration();

            cbDatabase.Items.Clear();
            for (int i = 0; i < cMySQL.configStructure.Count(); i++)
                cbDatabase.Items.Add(cMySQL.configStructure[i].Name);
            cbDatabase.SelectedIndex = 0;

            MakeList();

            tbDirectory.Text = cMySQL.configStructureTool[0].ClientDirectory;

            if (cMySQL.configStructureTool[0].ShowItemIconInList == ShowItemIconInList.ONLY_CURRENT)
                rbSelected.Checked = true;
            else
                rbNo.Checked = true;

            if (cMySQL.configStructureTool[0].UseTextureFilesFromClient)
            {
                pUseIcons.BackColor = System.Drawing.Color.DarkSeaGreen;
                cbUseIcons.Checked = true;
            }
            else
            {
                pUseIcons.BackColor = System.Drawing.Color.IndianRed;
            }
        }

        private void lbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(lbList.Text.Split(' ')[0], out int result))
            {
                tbName.Text = cMySQL.configStructure[result].Name;
                tbNote.Text = cMySQL.configStructure[result].Note;
                tbIP.Text = cMySQL.configStructure[result].Host;
                tbPort.Text = cMySQL.configStructure[result].Port.ToString();
                tbUsername.Text = cMySQL.configStructure[result].Username;
                tbPassword.Text = cMySQL.configStructure[result].Password;
                tbDatabase.Text = cMySQL.configStructure[result].Database;

                //add encrypt dialog showing
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int num = cMySQL.configStructure.FindIndex((cDatabaseConfig p) => p.Name.Equals(tbName.Text));

            if (num == -1)
            {
                int.TryParse(tbPort.Text, out int port);

                cDatabaseConfig dbc = new cDatabaseConfig
                {
                    Name = tbName.Text,
                    Note = tbNote.Text,
                    Host = tbIP.Text,
                    Port = port,
                    Username = tbUsername.Text,
                    Password = tbPassword.Text,
                    Database = tbDatabase.Text
                };

                cMySQL.configStructure.Add(dbc);
                MakeList();
            }
            else
                MessageBox.Show("A config with this name already exists");
        }

        private void btnSelectClient_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    tbDirectory.Text = fbd.SelectedPath;
                    cMySQL.configStructureTool[0].ClientDirectory = fbd.SelectedPath;
                }
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
        //    int.TryParse(tbPort.Text, out int port);

            if (cMySQL.TestConnection(new cDatabaseConfig()
            {
                Name = tbName.Text,
                Host = tbIP.Text,
                Port = Convert.ToInt32(tbPort.Text),
                Username = tbUsername.Text,
                Password = tbPassword.Text,
                Database = tbDatabase.Text
            }))
            {
                tslbStatus.ForeColor = System.Drawing.Color.DarkSeaGreen;
                tslbStatus.Text = "Succesufully connected to Database!";
            }
            else
            {
                tslbStatus.ForeColor = System.Drawing.Color.IndianRed;
                tslbStatus.Text = "Something went wrong, failed to connect with Database";
            }
        }

        private void rbYes_CheckedChanged(object sender, EventArgs e)
        {
            cMySQL.configStructureTool[0].ShowItemIconInList = ShowItemIconInList.ALL;
        }

        private void rbSelected_CheckedChanged(object sender, EventArgs e)
        {
            cMySQL.configStructureTool[0].ShowItemIconInList = ShowItemIconInList.ONLY_CURRENT;
        }

        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            cMySQL.configStructureTool[0].ShowItemIconInList = ShowItemIconInList.NO;
        }

        private void cbUseIcons_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseIcons.Checked)
            {
                cMySQL.configStructureTool[0].UseTextureFilesFromClient = true;
                pUseIcons.BackColor = System.Drawing.Color.DarkSeaGreen;
            }
            else
            {
                cMySQL.configStructureTool[0].UseTextureFilesFromClient = false;
                pUseIcons.BackColor = System.Drawing.Color.IndianRed;
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (int.TryParse(lbList.Text.Split(' ')[0], out int result))
            {
                Predicate<cDatabaseConfig> match = (cDatabaseConfig p) => p.Name.Equals(tbName.Text);
                int num = cMySQL.configStructure.FindIndex(match);
                if (num == -1 || num == result)
                {
                    int.TryParse(tbPort.Text, out int port);

                    cMySQL.configStructure[result].Name = tbName.Text;
                    cMySQL.configStructure[result].Note = tbNote.Text;
                    cMySQL.configStructure[result].Host = tbIP.Text;
                    cMySQL.configStructure[result].Port = port;
                    cMySQL.configStructure[result].Username = tbUsername.Text;
                    cMySQL.configStructure[result].Password = tbPassword.Text;
                    cMySQL.configStructure[result].Database = tbDatabase.Text;

                    var anon = new { CONNECTION = cMySQL.configStructure, SETTINGS = cMySQL.configStructureTool };
                    string json = JsonConvert.SerializeObject(anon, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
                    File.WriteAllText("Configuration/conf.json", json);
                    tslbStatus.Text = "Succesufully saved configuration to Configuration/conf.json";

                    MakeList();
                }
                else
                {
                    MessageBox.Show("A config with this name already exists\nPlease pick another name");
                }
            }
            else
            {
                MessageBox.Show("Could not find the id");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(lbList.Text.Split(' ')[0], out int result) && MessageBox.Show("Are u sure u want to delete the selected database config?", "Are u sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cMySQL.configStructure.RemoveAt(result);
                MakeList();
            }
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDatabase.Items.Count == 0)
                return;

            if (!cMySQL.SetConnection(cbDatabase.Text))
            {
                MessageBox.Show("Connection could not be set");
            }
        }

        /*
        private void btnAgree_Click(object sender, EventArgs e)
        {
            bool idMatches = cMySQL.configStructure.Any((cDatabaseConfig p) => p.Name.Equals(tbName.Text));
            int.TryParse(tbPort.Text, out int port);

            if (!idMatches)
                return;

            int num = cMySQL.configStructure.FindIndex((cDatabaseConfig p) => p.Name.Equals(tbName.Text));

            cDatabaseConfig currentid = cMySQL.configStructure[num];

            currentid.Name = tbName.Text;
            currentid.Note = tbNote.Text;
            currentid.Host = tbIP.Text;
            currentid.Port = port;
            currentid.Username = tbUsername.Text;
            currentid.Password = BCrypt.HashPassword(tbPassword.Text);
            currentid.Database = tbDatabase.Text;

            cMySQL.configStructure[num] = currentid;
        }
        */
    }
}
