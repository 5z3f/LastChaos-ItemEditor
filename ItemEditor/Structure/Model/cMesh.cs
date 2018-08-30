using SlimDX.Direct3D9;
using System.Collections.Generic;
using System.Linq;

namespace ItemEditor.Structure.Model
{
    internal class cMesh
    {
        internal class tHeaderInfo
        {
            public byte[] Format;
            public int MeshDataSize, Version;

            public uint JointCount,
                MeshCount,
                NormalCount,
                ObjCount,
                TextureMaps,
                UnknownCount,
                VertexCount;
        }

        internal struct tMesh
        {
            public Mesh MeshData;
            public SlimDX.Direct3D9.Texture TexData;

            public tMesh(Mesh mesh, SlimDX.Direct3D9.Texture texture)
            {
                MeshData = mesh;
                TexData = texture;
            }
        }

        internal class tMeshContainer
        {
            public byte[] FileName;
            public string FilePath;
            public float Scale;
            public uint Value1;

            public tHeaderInfo HeaderInfo;
            public tMeshMorphMap[] MorphMap;
            public cVertex.tVertex3f[] Normals;
            public tMeshObject[] Objects;
            public tMeshUVMap[] UVMaps;
            public cVertex.tVertex3f[] Vertices;
            public tMeshJointWeights[] Weights;
        }

        internal class tMeshObject
        {
            public cModel.tFace[] Faces;
            public tMeshShaderData ShaderData;
            public tMeshShaderInfo ShaderInfo;
            public tMeshTexture[] Textures;

            public byte[] JData,
                MaterialName;

            public uint FaceCount,
                FromVert,
                JValue,
                ShaderFlag,
                ToVert,
                Value1;

            public short[] GetFaces()
            {
                List<short> list = new List<short>();
                for (int i = 0; i < Faces.Count(); i++)
                {
                    list.Add(Faces[i].A);
                    list.Add(Faces[i].B);
                    list.Add(Faces[i].C);
                }
                return list.ToArray();
            }
        }

        internal class tMeshJointWeights
        {
            public uint Count;
            public byte[] JointName;
            public tMeshWeightsMap[] WeightsMap;
        }

        internal struct tMeshMorphMap
        {
            public byte[] JIndex,
                Influence;

            public tMeshMorphMap(byte[] JIndex, byte[] Influence)
            {
                this.JIndex = JIndex;
                this.Influence = Influence;
            }
        }

        internal class tMeshShaderData
        {
            public uint[] Param1,
                Param2;

            public uint cParam0;
            public float[] ParamFloats;
            public byte[] ShaderName;
        }

        internal class tMeshShaderInfo
        {
            public uint cParam1,
                cParam2,
                cParamFloats,
                cTextureUnits;
        }

        internal struct tMeshTexture
        {
            public byte[] InternalName;
            public int Reserverd;

            public tMeshTexture(byte[] Name)
            {
                InternalName = Name;
                Reserverd = 0;
            }
        }

        internal class tMeshUVMap
        {
            public cModel.tTextCoord[] Coords;
            public byte[] Name;
        }

        internal struct tMeshWeightsMap
        {
            public int Index;
            public float Weight;

            public tMeshWeightsMap(int Index, float Weight)
            {
                this.Index = Index;
                this.Weight = Weight;
            }
        }
    }
}
