using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ItemEditor.Structure.Model
{
    internal class cSMC
    {
        internal struct smcMesh
        {
            public string FileName;
            public List<smcObject> Object;

            public smcMesh(string FileName)
            {
                this.FileName = FileName;
                Object = new List<smcObject>();
            }
        }

        internal struct smcObject
        {
            public string Name,
                Texture;

            public smcObject(string Name, string Texture)
            {
                this.Name = Name;
                this.Texture = Texture;
            }
        }

        #region READ_SMC
        internal class SMCReader
        {
            public static List<smcMesh> ReadFile(string FileName)
            {
                string[] array = Path.GetDirectoryName(FileName).Split('\\');
                string str = "";
                bool flag = true;

                for (int i = 0; i < array.Count(); i++)
                {
                    if (array[i].ToUpper() == "DATA")
                        flag = false;
                    if (flag)
                        str = str + array[i] + "\\";
                }

                List<string> list = File.ReadAllLines(FileName).ToList();
                for (int num = list.Count() - 1; num >= 0; num--)
                {
                    list[num] = list[num].Trim();
                    list[num] = list[num].Replace("TFNM", "");
                    if (list[num].Contains("{") || list[num].Contains("}") || list[num].Contains(",") || list[num].Contains("NAME") || list[num].Contains("COLISION") || list[num].Contains("TEXTURES") || list[num].Contains("ANIM") || list[num].Contains("SKELETON") || list[num].Contains("_TAG"))
                        list.RemoveAt(num);
                }

                int num2 = -1;
                List<smcMesh> list2 = new List<smcMesh>();
                for (int i = 0; i < list.Count(); i++)
                {
                    try
                    {
                        if (list[i].Substring(0, 4) == "MESH")
                        {
                            num2++;
                            string[] array2 = list[i].Split('"');
                            list2.Add(new smcMesh(str + array2[1]));
                        }
                        else
                        {
                            string[] array2 = list[i].Split('"');
                            list2[num2].Object.Add(new smcObject(array2[1], str + array2[3]));
                        }
                    }
                    catch
                    {
                    }
                }
                return list2;
            }
        }
        #endregion
    }
}
