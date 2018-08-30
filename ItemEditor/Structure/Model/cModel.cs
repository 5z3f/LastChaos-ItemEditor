using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEditor.Structure.Model
{
    internal class cModel
    {
        internal class Models
        {
            public string ModelFile;
            public int ModelID;
            public Textures[] Textures;
        }

        internal class Textures
        {
            public string TextureFile,
                TextureName;
        }

        internal class tFace
        {
            public short A, B, C;

            public tFace(short A, short B, short C)
            {
                this.A = A;
                this.B = B;
                this.C = C;
            }
        }

        internal struct tTextCoord
        {
            public float U, V;

            public tTextCoord(float U, float V)
            {
                this.U = U;
                this.V = V;
            }
        }
    }
}
