using ItemEditor.Configuration;
using ItemEditor.Structure.Item;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ItemEditor.Data
{
    internal class cExport : cImport
    {
        public enum Export { ITEM, EFFECT, DASD }

        public static bool ItemUpdate(cItem cItems)
        {
            using (mysqlConnection = new MySqlConnection(strProvider))
            {
                mysqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("UPDATE t_item SET a_rare_prob_9 = @RareProb9, a_rare_prob_8 = @RareProb8, a_rare_prob_7 = @RareProb7, a_rare_prob_6 = @RareProb6, a_rare_prob_5 = @RareProb5, a_rare_prob_4 = @RareProb4, a_rare_prob_3 = @RareProb3, a_rare_prob_2 = @RareProb2, a_rare_prob_1 = @RareProb1, a_rare_prob_0 = @RareProb0, a_rare_index_9 = @RareID9, a_rare_index_8 = @RareID8, a_rare_index_7 = @RareID7, a_rare_index_6 = @RareID6, a_rare_index_5 = @RareID5, a_rare_index_4 = @RareID4, a_rare_index_3 = @RareID3, a_rare_index_2 = @RareID2, a_rare_index_1 = @RareID1, a_rare_index_0 = @RareID0, a_weight = @Stacks, a_texture_row = @texROW, a_texture_col = @texCOL, a_texture_id = @texID, a_enable = @Enable, a_max_use = @MaxUse, a_zone_flag = @ZoneFlag, a_origin_variation6 = @ORIGIN6, a_origin_variation5 = @ORIGIN5, a_origin_variation4 = @ORIGIN4, a_origin_variation3 = @ORIGIN3, a_origin_variation2 = @ORIGIN2, a_origin_variation2 = @ORIGIN2, a_origin_variation1 = @ORIGIN1, a_set_4 = @SET4, a_set_3 = @SET3, a_set_2 = @SET2, a_set_1 = @SET1, a_set_0 = @SET0, a_num_4 = @NUM4, a_num_3 = @NUM3, a_num_2 = @NUM2, a_num_1 = @NUM1, a_num_0 = @NUM0, a_rvr_value = @RVRValue, a_rvr_grade = @RVRGrade, a_fame = @Fame, a_durability = @Durablity, a_grade = @Grade, a_job_flag = @JobFlag, a_level2 = @Level2, a_level = @Level1, a_flag = @Flag, a_wearing = @Position, a_subtype_idx = @SubType, a_type_idx = @Type, a_damage_effect_name = @EDamage, a_attack_effect_name = @EAttack, a_effect_name = @ENormal, a_file_smc = @smc, a_quest_trigger_ids = @QTIDs, a_quest_trigger_count = @QTCount, a_price = @price,  a_name_thai = @name_thai,          a_name_thai_eng = @name_thai_eng, a_name_jpn = @name_jpn,            a_name_mal = @name_mal, a_name_mal_eng = @name_mal_eng,    a_name_usa = @name_usa, a_name_brz = @name_brz,            a_name_hk = @name_hk, a_name_hk_eng = @name_hk_eng,      a_name_ger = @name_ger, a_name_spn = @name_spn,            a_name_frc = @name_frc, a_name_pld = @name_pld,            a_name_rus = @name_rus, a_name_tur = @name_tur,            a_name_spn2 = @name_spn2, a_name_frc2 = @name_frc2,          a_name_ita = @name_ita, a_name_mex = @name_mex,            a_name_nld = @name_nld,           a_name = @name,      a_name_twn = @name_twn,       a_name_chn = @name_chn,   a_descr_thai = @descr_thai,          a_descr_thai_eng = @descr_thai_eng, a_descr_jpn = @descr_jpn,            a_descr_mal = @descr_mal, a_descr_mal_eng = @descr_mal_eng,    a_descr_usa = @descr_usa, a_descr_brz = @descr_brz,            a_descr_hk = @descr_hk, a_descr_hk_eng = @descr_hk_eng,      a_descr_ger = @descr_ger, a_descr_spn = @descr_spn,            a_descr_frc = @descr_frc, a_descr_pld = @descr_pld,            a_descr_rus = @descr_rus, a_descr_tur = @descr_tur,            a_descr_spn2 = @descr_spn2, a_descr_frc2 = @descr_frc2,          a_descr_ita = @descr_ita, a_descr_mex = @descr_mex,            a_descr_nld = @descr_nld,           a_descr = @descr,      a_descr_twn = @descr_twn,       a_descr_chn = @descr_chn   WHERE a_index = @index", mysqlConnection);
                mySqlCommand.Prepare();

                mySqlCommand.Parameters.AddWithValue("@index", cItems.ID);
                mySqlCommand.Parameters.AddWithValue("@price", cItems.Price);
                mySqlCommand.Parameters.AddWithValue("@QTCount", cItems.QuestTriggerCount);
                mySqlCommand.Parameters.AddWithValue("@QTIDs", cItems.QuestTriggerIDs);
                mySqlCommand.Parameters.AddWithValue("@ENormal", cItems.EffectNormal);
                mySqlCommand.Parameters.AddWithValue("@EAttack", cItems.EffectAttack);
                mySqlCommand.Parameters.AddWithValue("@EDamage", cItems.EffectDamage);
                mySqlCommand.Parameters.AddWithValue("@Type", cItems.ItemType);
                mySqlCommand.Parameters.AddWithValue("@SubType", cItems.ItemSubType);
                mySqlCommand.Parameters.AddWithValue("@Position", cItems.Wearing);
                mySqlCommand.Parameters.AddWithValue("@Flag", cItems.ItemFlag);
                mySqlCommand.Parameters.AddWithValue("@Level1", cItems.LevelMin);
                mySqlCommand.Parameters.AddWithValue("@Level2", cItems.LevelMax);
                mySqlCommand.Parameters.AddWithValue("@JobFlag", cItems.JobFlag);
                mySqlCommand.Parameters.AddWithValue("@Grade", cItems.Grade);
                mySqlCommand.Parameters.AddWithValue("@Durablity", cItems.Durability);
             //   mySqlCommand.Parameters.AddWithValue("@Set", cItems.Set);
                mySqlCommand.Parameters.AddWithValue("@Fame", cItems.Fame);
                mySqlCommand.Parameters.AddWithValue("@RVRGrade", cItems.RVRGrade);
                mySqlCommand.Parameters.AddWithValue("@RVRValue", cItems.RVRValue);
                mySqlCommand.Parameters.AddWithValue("@ZoneFlag", cItems.ZoneFlag);
                mySqlCommand.Parameters.AddWithValue("@MaxUse", cItems.MaxUse);
                mySqlCommand.Parameters.AddWithValue("@Enable", cItems.Enable);
                mySqlCommand.Parameters.AddWithValue("@texID", cItems.TexID);
                mySqlCommand.Parameters.AddWithValue("@texROW", cItems.TexROW);
                mySqlCommand.Parameters.AddWithValue("@texCOL", cItems.TexCOL);
                mySqlCommand.Parameters.AddWithValue("@Stacks", cItems.Weight);
                mySqlCommand.Parameters.AddWithValue("@smc", cItems.SMC);

                for (int i = 1; i <= 6; i++)
                    mySqlCommand.Parameters.AddWithValue("@ORIGIN" + i, cItems.ORIGIN[i]);

                for (int i = 0; i <= 4; i++)
                {
                    mySqlCommand.Parameters.AddWithValue("@NUM" + i, cItems.NUM[i]);
                    mySqlCommand.Parameters.AddWithValue("@SET" + i, cItems.SET[i]);
                }

                for (int i = 0; i <= 9; i++)
                {
                    mySqlCommand.Parameters.AddWithValue("@RareID" + i, cItems.RAREID[i]);
                    mySqlCommand.Parameters.AddWithValue("@RareProb" + i, cItems.RAREPROB[i]);
                }

                for (int i = 0; i <= 23; i++)
                {
                    mySqlCommand.Parameters.AddWithValue((i == 0) ? "@name" : "@name_" + Enum.GetName(typeof(Language), i).ToLower(), cItems.Name[i]);
                    mySqlCommand.Parameters.AddWithValue((i == 0) ? "@descr" : "@descr_" + Enum.GetName(typeof(Language), i).ToLower(), cItems.Description[i]);
                }
                

                return mySqlCommand.ExecuteNonQuery() == 1;
            }
        }

        public static void ExportContent(Export ex, string fileName = null)
        {
            string serializedData = JsonConvert.SerializeObject(ex == Export.ITEM ? (object)ItemData : (object)EffectData);

            File.WriteAllText(fileName ?? "Configuration/effect-data.json", serializedData);
        }
    }
}
