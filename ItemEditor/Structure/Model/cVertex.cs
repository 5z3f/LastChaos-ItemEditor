using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEditor.Structure.Model
{
    internal class cVertex
    {
        internal class CustomVertex
        {
            public struct PositionNormalTextured
            {
                public Vector3 Position,
                    Normal;

                public Vector2 Texture;
            }
        }

        internal struct tVertex3f
        {
            public float X, Y, Z;

            public tVertex3f(float X, float Y, float Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
        }
    }
}
