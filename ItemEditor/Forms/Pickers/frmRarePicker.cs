using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using ItemEditor.Configuration;
using ItemEditor.Data;
using ItemEditor.Structure.Item;

namespace ItemEditor.Forms.Pickers
{
    public partial class frmRarePicker : Form
    {
        public static int RAREID;
        private static List<cRare> RareData;

        public frmRarePicker()
        {
            InitializeComponent();
        }

        private void frmRarePicker_Load(object sender, EventArgs e)
        {
            GetRare();
            MakeList();
        }

        private async void GetRare()
        {
            RareData = new List<cRare>();
            var rareData = await cMySQL.QueryToDataTable("SELECT * FROM t_rareoption ORDER BY a_index");

            Parallel.ForEach(rareData.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 1 }, dataRow =>
            {
                cRare cRare = new cRare
                {
                    ID      = Convert.ToInt32(dataRow["a_index"]),
                    Grade   = Convert.ToByte(dataRow["a_grade"]),
                    Type    = Convert.ToByte(dataRow["a_type"]),
                    Attack  = Convert.ToInt32(dataRow["a_attack"]),
                    Defense = Convert.ToInt32(dataRow["a_defense"]),
                    Magic   = Convert.ToInt32(dataRow["a_magic"]),
                    Resist  = Convert.ToInt32(dataRow["a_resist"]),

                    OPTIONID = new int[10],
                    OPTIONLVL = new int[10],
                    OPTIONPROB = new int[10],

                    Name = new string[24],
                    Prefix = new string[24],
                };

                for (int i = 0; i <= 9; i++)
                {
                    cRare.OPTIONID[i]   = Convert.ToInt32(dataRow["a_option_index" + i]);
                    cRare.OPTIONPROB[i] = Convert.ToInt32(dataRow["a_option_prob"  + i]);
                    cRare.OPTIONLVL[i]  = Convert.ToInt32(dataRow["a_option_level" + i]);
                }

                for (int i = 0; i <= 23; i++)
                {
                    cRare.Name[i] = (i == 0 || i == 2 || i == 23) ? dataRow["a_name"].ToString() : dataRow["a_name_" + Enum.GetName(typeof(Language), i).ToLower()].ToString();
                    cRare.Prefix[i] = (i == 0 || i == 2 || i == 23) ? dataRow["a_prefix"].ToString() : dataRow["a_prefix_" + Enum.GetName(typeof(Language), i).ToLower()].ToString();
                }

                RareData.Add(cRare);
            });
        }

        private int getIDfromListBox()
        {
            if (lbRares.Text.Split(' ')[0].All(char.IsDigit))
                return Convert.ToInt32(lbRares.Text.Split(' ')[0]);
            else
                return 0;
        }

        private async void lbRares_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = getIDfromListBox();
            cRare cRare = RareData.Find(p => p.ID.Equals(id));
            if (cRare != null)
            {
                lbInfo1.Text = string.Format("Attack: {0} | Defense: {1} | Magic: {2} | Resist: {3}", cRare.Attack, cRare.Defense, cRare.Magic, cRare.Resist);
                lbInfo2.Text = string.Format("Grade: {0} | Type: {1}", (RARE_GRADE)cRare.Grade, (RARE_TYPE)cRare.Type);

                dgOptions.Rows.Clear();
                for (int i = 0; i <= 9; i++)
                {
                    var optionAddons = await cImport.getOptionAddons(cRare.OPTIONID[i]);
                    foreach (DataRow dataRow in optionAddons.Rows)
                    {
                        string[] optionName;
                        optionName = new string[24];

                        for (int j = 0; j <= 23; j++)
                            optionName[j] = (j == 0) ? (string)dataRow["a_name"] : (string)dataRow["a_name_" + Enum.GetName(typeof(Language), j).ToLower()];

                        dgOptions.Rows.Add(/*cRare.OPTIONID[i],*/ optionName[(int)cImport.CurrentLanguage], cRare.OPTIONLVL[i], dataRow["a_level"].ToString().Split(' ')[cRare.OPTIONLVL[i]], cRare.OPTIONPROB[i]);
                    }
                }
            }
        }

        private void MakeList()
        {
            List<string> searchData = new List<string>();
            List<cRare> cRare = new List<cRare>();

            RareData = RareData.FindAll(p => p.GetNameLang(cImport.CurrentLanguage).ToLower().Contains(""));
            for (int index = 0; index < Enumerable.Count(RareData); ++index)
                searchData.Add(RareData[index].ID + " - " + RareData[index].GetNameLang(cImport.CurrentLanguage));

            lbRares.Items.Clear();
            lbRares.Items.AddRange(searchData.ToArray());
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            RAREID = getIDfromListBox();

            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public enum RARE_TYPE
    {
        WEAPON,     // RARE_OPTION_TYPE_WEAPON
        ARMOR,      // RARE_OPTION_TYPE_ARMOR
        ACCESSORY   // RARE_OPTION_TYPE_ACCESSORY
    }

    public enum RARE_GRADE
    {
        BLUE,       // RARE_OPTION_GRADE_A (0x1C54CCFF)
        GREEN,      // RARE_OPTION_GRADE_B (0x20A51CFF)
        YELLOW,     // RARE_OPTION_GRADE_C (0xE5E230FF)
        PALE_GREEN, // RARE_OPTION_GRADE_D (0xB2FDB7FF)
        AQUA        // RARE_OPTION_GRADE_E (0x01FDEEFF)
    }

    internal class cRare
    {
        public int ID, 
            Attack, 
            Defense, 
            Magic, 
            Resist;

        public int[] OPTIONID, 
            OPTIONLVL,
            OPTIONPROB;

        public byte Grade, // const COLOR Color[Item_Rare_Grade_MAX] = {0x1C54CCFF, 0x20A51CFF, 0xE5E230FF, 0xB2FDB7FF, 0x01FDEEFF	};
            Type;

        public string[] Name, 
            Prefix;

        public string GetNameLang(Language lang)
        {
            return Name[(int)lang];
        }

        public string GetPrefixLang(Language lang)
        {
            return Prefix[(int)lang];
        }

    }
}
