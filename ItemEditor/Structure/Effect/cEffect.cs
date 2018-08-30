using System.IO;
using System.Linq;

namespace ItemEditor.Structure.Effect
{
    internal class cEffect
    {
        public int ID;
        public string Name, Note;
    }

    #region binary_effect_reader

    internal class effectInfo
    {
        //particlegroup
        public uint verptgr;
        public string chunkptgr;
        public string nameptgr;
        public string texture;
        public uint rendertype, blendtype, mexwidth, mexheight, icol, irow, dwcount;
    }

    class binaryEffect
    {
        private static effectInfo effectInfo;
        public static void ReadEffectData()
        {
            //header
            int size;
            byte ver;
            string start, particle, efc;


            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(File.ReadAllBytes(@"X:\LastChaos\Servers\LastChaos0xFF\Data\Effect\Effect.dat"))))
            {
                start = System.Text.Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
                particle = System.Text.Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
                efc = System.Text.Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
                ver = binaryReader.ReadByte();
                size = binaryReader.ReadInt32();

                // richTextBox1.Text = string.Format("{0},{1},{2},{3},{4}\n\n\n", start, particle, efc, ver, size);

                effectInfo = new effectInfo();
              // for (int i = 0; i < size; i++)
              //  {
                    effectInfo.chunkptgr = System.Text.Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
                    effectInfo.verptgr = binaryReader.ReadByte();

                    byte[] name = ReadLine(binaryReader);
                    effectInfo.nameptgr = System.Text.Encoding.ASCII.GetString(name);

                    byte[] tex = ReadTex(binaryReader);
                    effectInfo.texture = System.Text.Encoding.ASCII.GetString(tex);

                    effectInfo.rendertype = binaryReader.ReadUInt32();
                    effectInfo.blendtype = binaryReader.ReadUInt32();
                    effectInfo.mexwidth = binaryReader.ReadUInt32();
                    effectInfo.mexheight = binaryReader.ReadUInt32();
                    effectInfo.icol = binaryReader.ReadUInt32();
                    effectInfo.irow = binaryReader.ReadUInt32();
                    effectInfo.dwcount = binaryReader.ReadUInt32();

                    uint temp = 0, version = 0;
                    string ppds = "";
                    for (int i = 0; i < effectInfo.dwcount; ++i)
                    {
                        temp = binaryReader.ReadUInt32();
                        ppds = System.Text.Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
                        version = binaryReader.ReadUInt32();
                    }
               // }

                /*
                richTextBox1.Text += string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}",
                effectInfo.chunkptgr,
                effectInfo.verptgr,
                effectInfo.nameptgr,
                effectInfo.texture.Replace("\0", ""),
                effectInfo.rendertype,
                effectInfo.blendtype,
                (int)effectInfo.mexwidth,
                (int)effectInfo.mexheight,
                effectInfo.icol,
                effectInfo.irow,
                effectInfo.dwcount,
                temp,
                ppds,
                version
                );
                */
            }
        }

        public static byte[] ReadLine(BinaryReader binaryReader)
        {
            byte readByte = 0;
            byte[] buffer = new byte[1024];
            int count = 0;

            while ((readByte = binaryReader.ReadByte()) != 0xA)
            {
                buffer[count] = System.Convert.ToByte(readByte);
                count++;
            }

            return buffer.TakeWhile((v, index) => buffer.Skip(index).Any(w => w != 0x00)).ToArray();
        }

        public static byte[] ReadTex(BinaryReader binaryReader)
        {
            byte readByte;
            byte[] buffer = new byte[1024];
            int count = 0;

            while ((readByte = binaryReader.ReadByte()) != '.')
            {
                buffer[count] = System.Convert.ToByte(readByte);
                count++;
            }

            buffer[count + 1] = 0x2E;
            buffer[count + 2] = 0x74;
            buffer[count + 3] = 0x65;
            buffer[count + 4] = 0x78;

            binaryReader.ReadBytes(3);

            return buffer.TakeWhile((v, index) => buffer.Skip(index).Any(w => w != 0x00)).ToArray();
        }
    }
    #endregion
}
