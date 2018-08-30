using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemEditor.Structure;
using ItemEditor.Configuration;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

using ItemEditor.Structure.Item;
using ItemEditor.Structure.Effect;

namespace ItemEditor.Data
{
    public enum IconType { ITEM, SKILL };

    internal class cImport : cMySQL
    {
        public static List<cItem> ItemData;
        public static List<cEffect> EffectData;

        public static Language CurrentLanguage = Language.KOR;

        public static async void GetItems()
        {
            ItemData = new List<cItem>();
            var dbData = await QueryToDataTable("SELECT * FROM t_item ORDER BY a_index;");
            Parallel.ForEach(dbData.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 1 }, dataRow =>
            {
                cItem cItem = new cItem
                {
                    ID = Convert.ToInt32(dataRow["a_index"]),
                    SMC = Convert.ToString(dataRow["a_file_smc"]),
                    Price = Convert.ToInt32(dataRow["a_price"]),
                    QuestTriggerCount = Convert.ToInt32(dataRow["a_quest_trigger_count"]),
                    QuestTriggerIDs = Convert.ToString(dataRow["a_quest_trigger_ids"]),
                    EffectNormal = Convert.ToString(dataRow["a_effect_name"]),
                    EffectAttack = Convert.ToString(dataRow["a_attack_effect_name"]),
                    EffectDamage = Convert.ToString(dataRow["a_damage_effect_name"]),
                    ItemType = Convert.ToInt32(dataRow["a_type_idx"]),
                    ItemSubType = Convert.ToInt32(dataRow["a_subtype_idx"]),
                    Wearing = Convert.ToInt32(dataRow["a_wearing"]),
                    ItemFlag = Convert.ToUInt64(dataRow["a_flag"]),
                    LevelMin = Convert.ToInt32(dataRow["a_level"]),
                    LevelMax = Convert.ToInt32(dataRow["a_level2"]),
                    JobFlag = Convert.ToUInt64(dataRow["a_job_flag"]),
                    Grade = Convert.ToInt32(dataRow["a_grade"]),
                    Durability = Convert.ToInt32(dataRow["a_durability"]),
                    Set = Convert.ToInt32(dataRow["a_set"]), // Looks like it's not used
                    Fame = Convert.ToInt32(dataRow["a_fame"]),
                    RVRGrade = Convert.ToInt32(dataRow["a_rvr_grade"]),
                    RVRValue = Convert.ToInt32(dataRow["a_rvr_value"]),
                    ZoneFlag = Convert.ToInt32(dataRow["a_zone_flag"]),
                    MaxUse = Convert.ToInt32(dataRow["a_max_use"]),
                    Enable = Convert.ToInt32(dataRow["a_enable"]),
                    TexID = Convert.ToInt32(dataRow["a_texture_id"]),
                    TexCOL = Convert.ToInt32(dataRow["a_texture_col"]),
                    TexROW = Convert.ToInt32(dataRow["a_texture_row"]),
                    Weight = Convert.ToInt32(dataRow["a_weight"]),
                    CastleWar = Convert.ToInt32(dataRow["a_castle_war"]),

                    NUM = new int[5],
                    SET = new int[5],
                    ORIGIN = new int[10],
                    RAREID = new int[10],
                    RAREPROB = new int[10],
                    Name = new string[24],
                    Description = new string[24]
            };

                for (int i = 0; i <= 4; i++)
                {
                    cItem.NUM[i] = Convert.ToInt32(dataRow["a_num_" + i]);
                    cItem.SET[i] = Convert.ToInt32(dataRow["a_set_" + i]);
                }

                for (int i = 1; i <= 6; i++)
                    cItem.ORIGIN[i] = Convert.ToInt32(dataRow["a_origin_variation" + i]);

                for (int i = 0; i <= 9; i++)
                {
                    cItem.RAREID[i] = Convert.ToInt32(dataRow["a_rare_index_" + i]);
                    cItem.RAREPROB[i] = Convert.ToInt32(dataRow["a_rare_prob_" + i]);
                }

                for (int i = 0; i <= 23; i++)
                {
                    cItem.Name[i] = (i == 0) ? (string)dataRow["a_name"] : (string)dataRow["a_name_" + Enum.GetName(typeof(Language), i).ToLower()];
                    cItem.Description[i] = (i == 0) ? (string)dataRow["a_descr"] : (string)dataRow["a_descr_" + Enum.GetName(typeof(Language), i).ToLower()];
                }

                ItemData.Add(cItem);
            });
        }

        public static void GetEffects(string fileDir = null)
        {
            string effectConfigName = "effect-data.json";

            if (string.IsNullOrEmpty(fileDir) && Path.GetExtension(fileDir) == ".json")
            {
                try
                {
                    dynamic effectJsonFile = JsonConvert.DeserializeObject(File.ReadAllText(effectConfigName));

                    cEffect cEffect = new cEffect
                    {
                        ID = effectJsonFile.ID,
                        Name = effectJsonFile.effectName,
                        Note = effectJsonFile.effectNote
                    };

                    // cEffectStructure.UsedByItemID = effectJsonFile.effectUsedByItem;
                    // cEffectStructure.UsedByTitleID = effectJsonFile.effectUsedByTitle;
                }
                catch
                {
                    //
                }
            }
            else
            {
                if (Path.GetExtension(fileDir) == ".txt") //read effect names with \n on end of the line
                {
                    EffectData = new List<cEffect>();

                    int index = 1;
                    Parallel.ForEach(File.ReadLines(fileDir), new ParallelOptions { MaxDegreeOfParallelism = 1 }, line =>
                    {
                        if (line.Contains("_"))
                        {
                            string[] lineSplitted = (line.Contains("\t") ? line : line + "\t").Split('\t');

                            cEffect cEffect = new cEffect
                            {
                                ID = index,
                                Name = lineSplitted[0],
                                Note = lineSplitted[1]
                            };

                            EffectData.Add(cEffect);
                        }
                        index++;
                    });
                }
                else if (Path.GetExtension(fileDir) == ".json")
                {
                    try
                    {
                        dynamic effectJsonFile = JsonConvert.DeserializeObject(File.ReadAllText(fileDir));

                        cEffect cEffect = new cEffect
                        {
                            Name = effectJsonFile.effectName,
                            Note = effectJsonFile.effectNote
                        };
                    }
                    catch
                    {
                        //
                    }
                }
            }
        }

        public static void GetConfiguration()
        {
            configStructure = new List<cDatabaseConfig>();
            configStructureTool = new List<cToolConfig>();

            var jDeserialized = (cSettings)JsonConvert.DeserializeObject(File.ReadAllText("Configuration/conf.json"), typeof(cSettings), new Newtonsoft.Json.Converters.StringEnumConverter());

            foreach (var jConnectionRow in jDeserialized.CONNECTION)
            {
                cDatabaseConfig cDatabaseConfig = new cDatabaseConfig
                {
                    Name = jConnectionRow.Name,
                    Note = jConnectionRow.Note,
                    Host = jConnectionRow.Host,
                    Port = jConnectionRow.Port,
                    Username = jConnectionRow.Username,
                    Password = jConnectionRow.Password,
                    Database = jConnectionRow.Database
                };

                configStructure.Add(cDatabaseConfig);
            }

            cToolConfig cToolConfig = new cToolConfig
            {
                ClientDirectory = jDeserialized.SETTINGS[0].ClientDirectory,
                UseTextureFilesFromClient = jDeserialized.SETTINGS[0].UseTextureFilesFromClient,
                LogToFile = jDeserialized.SETTINGS[0].LogToFile,
                ShowItemIconInList = jDeserialized.SETTINGS[0].ShowItemIconInList
            };

            configStructureTool.Add(cToolConfig);
        }

        public async static Task<Bitmap> GetIcon(IconType IconType, int id)
        {
            int[] iTexture      = { 0, 0, 0 },  // ROW, COL, ID
                  iRectangle    = { 1, 2, 29, 29 };

            Bitmap bmpMain = new Bitmap(29, 29);

            try
            {
                if (IconType == IconType.ITEM)
                {
                    int itemIndex = ItemData.FindIndex(p => p.ID.Equals(id));

                    if (itemIndex != -1)
                    {
                        iTexture[0] = ItemData[itemIndex].TexROW;
                        iTexture[1] = ItemData[itemIndex].TexCOL;
                        iTexture[2] = ItemData[itemIndex].TexID;
                    }
                }
                else if(IconType == IconType.SKILL)
                {
                    var skillData = await QueryToDataTable("SELECT * FROM t_skill WHERE a_index = " + id);

                    foreach (DataRow dataRow in skillData.Rows)
                    {
                        iTexture[0] = Convert.ToInt32(dataRow["a_client_icon_row"]);
                        iTexture[1] = Convert.ToInt32(dataRow["a_client_icon_col"]);
                        iTexture[2] = Convert.ToInt32(dataRow["a_client_icon_texid"]);
                    }
                }

                bmpMain = new Bitmap("Images/" + Convert.ToString((IconType == IconType.ITEM) ? "ItemBtn" : "SkillBtn") + iTexture[2].ToString() + ".png");
                return bmpMain.Clone(new Rectangle(iTexture[1] * 32 + iRectangle[0], iTexture[0] * 32 + iRectangle[1], iRectangle[2], iRectangle[3]), bmpMain.PixelFormat);
            }
            catch (Exception dbgException)
            {
                bmpMain.Dispose();
                return null;
            }
        }

        public async static Task<DataTable> getDropsFromItem(int ItemID)
        {
            string dropQuery;

            dropQuery = string.Format("SELECT DISTINCT(a_index), a_name, a_level FROM t_npc WHERE a_item_0 = '{0}' ", ItemID);
            for (int columnNumber = 1; columnNumber <= 19; columnNumber++)
                dropQuery += string.Format(" OR a_item_{0} = {1}", columnNumber, ItemID);
            dropQuery += " ORDER BY a_index";

            return await QueryToDataTable(dropQuery);
        }

        public async static Task<string> getRarePrefix(int id)
        {
            string getRareNameByID = string.Format("SELECT * FROM t_rareoption WHERE a_index = '{0}';", id);
            var grnbid = await QueryToDataTable(getRareNameByID);
            DataRow dataRow;

            string[] rareName;
            rareName = new string[24];

            if (grnbid.Rows.Count != 0)
            {
                dataRow = grnbid.Rows[0];

                for (int i = 0; i <= 23; i++)
                    rareName[i] = (i == 0 || i == 2 || i == 23) ? (string)dataRow["a_prefix"] : (string)dataRow["a_prefix_" + Enum.GetName(typeof(Language), i).ToLower()];
            }

            return (grnbid.Rows.Count == 0) ? "NONE" : rareName[(int)CurrentLanguage];
        }

        public async static Task<DataTable> getOptionAddons(int id)
        {
            string getOptionAddonsWithID = string.Format("SELECT * FROM t_option WHERE a_index = '{0}';", id);
            return await QueryToDataTable(getOptionAddonsWithID);
        }

        public async static Task<DataTable> getSpawnData(int MonsterID)
        {
            string spawnQuery = string.Format("SELECT DISTINCT(a_index), a_zone_num, a_pos_x, a_pos_z FROM t_npc_regen WHERE a_npc_idx = '{0}';", MonsterID);
            return await QueryToDataTable(spawnQuery);
        }
    }
}
