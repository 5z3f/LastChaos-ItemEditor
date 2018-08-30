using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using ItemEditor.Data;
using ItemEditor.Configuration;
using ItemEditor.Structure.Effect;

namespace ItemEditor
{
    public partial class frmEffect : Form
    {

        public frmEffect()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ofdEffect.Filter = "L-Chaos Plain Effect File (*.txt)|*.txt|" +
                               "L-Chaos Formatted Effect File (*.json)|*.json";

            if (ofdEffect.ShowDialog() == DialogResult.OK)
            {
                cImport.GetEffects(ofdEffect.FileName);
                MakeList();
            }
        }

        private void frmEffect_Load(object sender, EventArgs e)
        {
            cbSearchType.SelectedIndex = 0;
        }

        private void MakeList()
        {
            List<string> list = new List<string>();
            List<cEffect> EffectData = new List<cEffect>();

            if (cbSearchType.Text == "Name")
            {
                EffectData = cImport.EffectData.FindAll(p => p.Name.ToLower().Contains(tbSearch.Text.ToLower()));
                for (int index = 0; index < Enumerable.Count(EffectData); ++index)
                    list.Add(EffectData[index].ID + " - " + EffectData[index].Name);
            }

            lbItems.Items.Clear();
            lbItems.Items.AddRange(list.ToArray());
        }


        public async void getItemsWithEffect(string effectName)
        {
            dgItems.Rows.Clear();
            string getItemsWithEffect = string.Format("SELECT * FROM t_item WHERE a_effect_name = '{0}' " +
                                            "OR a_attack_effect_name = '{0}' " +
                                            "OR a_damage_effect_name = '{0}';", effectName);

            var itemsWithEffect = await cMySQL.QueryToDataTable(getItemsWithEffect);

            foreach (DataRow dataRow in itemsWithEffect.Rows)
                dgItems.Rows.Add(await cImport.GetIcon(IconType.ITEM, Convert.ToInt32(dataRow["a_index"])), dataRow["a_name_usa"], Convert.ToInt32(dataRow["a_index"]));
        }
        public async void getTitlesWithEffect(string effectName)
        {
            dgTitles.Rows.Clear();

            string getTitlesWithEffect = string.Format("SELECT * FROM t_title WHERE a_effect_name = '{0}' " +
                                            "OR a_attack = '{0}' " +
                                            "OR a_damage = '{0}';", effectName);

            var titlesWithEffect = await cMySQL.QueryToDataTable(getTitlesWithEffect);

            foreach (DataRow dataRow in titlesWithEffect.Rows)
            {
                /*int id = */dgTitles.Rows.Add(dataRow["a_name"], Convert.ToInt32(dataRow["a_index"]), Convert.ToInt32(dataRow["a_item_index"]));
            //    var row = dgTitles.Rows[id];

            //    int bgColorFromHex = Int32.Parse(dataRow["a_bgcolor"].ToString(), NumberStyles.HexNumber);
            //    int foreColorFromHex = Int32.Parse(dataRow["a_color"].ToString(), NumberStyles.HexNumber);

            //    System.Drawing.Color bgColor = System.Drawing.Color.FromArgb(bgColorFromHex);
            //    System.Drawing.Color foreColor = System.Drawing.Color.FromArgb(foreColorFromHex);

            //    row.Cells[0].Style.BackColor = bgColor;
            //    row.Cells[0].Style.ForeColor = foreColor;
            }

            dgTitles.ClearSelection();
        }
        public async void getMonstersWithEffect(string effectName)
        {
            dgMonster.Rows.Clear();
            string getMonstersWithEffect = string.Format("SELECT * FROM t_npc WHERE a_fireEffect0 = '{0}' " +
                                            "OR a_fireEffect1 = '{0}' " +
                                            "OR a_fireEffect2 = '{0}';", effectName);

            var monstersWithEffect = await cMySQL.QueryToDataTable(getMonstersWithEffect);

            foreach (DataRow dataRow in monstersWithEffect.Rows)
                dgMonster.Rows.Add(dataRow["a_name"], Convert.ToInt32(dataRow["a_level"]), Convert.ToInt32(dataRow["a_index"]));
        }
        public async void getSkillsWithEffect(string effectName)
        {
            dgSkill.Rows.Clear();
            string getSkillsWithEffect = string.Format("SELECT * FROM t_skill WHERE a_cd_ra = '{0}' " +
                                            "OR a_cd_re = '{0}' " +
                                            "OR a_cd_sa = '{0}' " +
                                            "OR a_cd_fa = '{0}' " +
                                            "OR a_cd_fe0 = '{0}' " +
                                            "OR a_cd_fe1 = '{0}' " +
                                            "OR a_cd_fe2 = '{0}' " +
                                            "OR a_cd_fe_after = '{0}' " +
                                            "OR a_cd_fe_after2 = '{0}' " +
                                            "OR a_cd_ra2 = '{0}' " +
                                            "OR a_cd_re2 = '{0}' " +
                                            "OR a_cd_sa2 = '{0}' " +
                                            "OR a_cd_fa2 = '{0}' " +
                                            "OR a_cd_fe3 = '{0}' " +
                                            "OR a_cd_fe4 = '{0}' " +
                                            "OR a_cd_fe5 = '{0}';", effectName);

            var skillsWithEffect = await cMySQL.QueryToDataTable(getSkillsWithEffect);

            foreach (DataRow dataRow in skillsWithEffect.Rows)
            {
                var iconFromSkill = await cImport.GetIcon(IconType.SKILL, Convert.ToInt32(dataRow["a_index"]));
                dgSkill.Rows.Add(iconFromSkill, dataRow["a_name"], Convert.ToInt32(dataRow["a_index"]));
            }
        }

        private void lbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = getIDfromListBox();
            cEffect cEffect = cImport.EffectData.Find(p => p.ID.Equals(id));
            if (cEffect != null)
            {
                tbName.Text = cEffect.Name;
                tbNote.Text = cEffect.Note;

                getItemsWithEffect(cEffect.Name);
                getTitlesWithEffect(cEffect.Name);
                getMonstersWithEffect(cEffect.Name);
                getSkillsWithEffect(cEffect.Name);
            }
        }

        private int getIDfromListBox()
        {
            if (lbItems.Text.Split(' ')[0].All(char.IsDigit))
                return Convert.ToInt32(lbItems.Text.Split(' ')[0]);
            else
                return 0;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            MakeList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           cExport.ExportContent(cExport.Export.EFFECT);
        }
    }
}
