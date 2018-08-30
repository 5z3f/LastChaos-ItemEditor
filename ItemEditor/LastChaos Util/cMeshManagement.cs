using System.IO;

using static ItemEditor.Structure.Model.cMesh;
using static ItemEditor.Structure.Model.cModel;
using static ItemEditor.Structure.Model.cDecoder;
using static ItemEditor.Structure.Model.cVertex;

namespace ItemEditor.LastChaosUtil
{
	internal class cMeshManagement
	{
		public static tMeshContainer pMesh;
        private enum MESH_VERSION { MESH_OLD_VERSION = 16, MESH_NEW_VERSION = 17 };

		public static bool ReadFile(string FileName)
		{
			pMesh = new tMeshContainer();
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(File.ReadAllBytes(FileName))))
            {
                pMesh.HeaderInfo = new tHeaderInfo
                {
                    Format = binaryReader.ReadBytes(4),                //WriteID_t(CChunkID(MESH_ID)) //MESH_ID - "MESH"
                    Version = binaryReader.ReadInt32(),                //(INDEX)MESH_NEW_VER
                    MeshDataSize = binaryReader.ReadInt32(),           //slInvalidSize
                    MeshCount = binaryReader.ReadUInt32(),             //EncodeSimpleMesh(ctmlod, checker)
                    VertexCount = binaryReader.ReadUInt32(),           //mlod_aVertexWeights
                    JointCount = binaryReader.ReadUInt32(),            //mlod_aWeightMaps
                    TextureMaps = binaryReader.ReadUInt32(),           //mlod_aUVMaps
                    NormalCount = binaryReader.ReadUInt32(),           //mlod_aVertices
                    ObjCount = binaryReader.ReadUInt32(),              //mlod_aSurfaces
                    UnknownCount = binaryReader.ReadUInt32()           //mlod_aMorphMaps
                };

                pMesh.FileName = binaryReader.ReadBytes(binaryReader.ReadInt32());  //mlod_fnSourceFile
                pMesh.Scale = binaryReader.ReadSingle();                            //mlod_fMaxDistance
                pMesh.Value1 = binaryReader.ReadUInt32();                           //mlod.mlod_ulFlags
                pMesh.FilePath = FileName;

                if (pMesh.HeaderInfo.Version != (int)MESH_VERSION.MESH_OLD_VERSION && pMesh.HeaderInfo.Version != (int)MESH_VERSION.MESH_NEW_VERSION)
                    return false;

                // call to read mesh function and return true if succeed
                {
                    if (pMesh.HeaderInfo.Version == (int)MESH_VERSION.MESH_OLD_VERSION && ReadMesh(binaryReader, false))
                        return true;
                    else if (pMesh.HeaderInfo.Version == (int)MESH_VERSION.MESH_NEW_VERSION && ReadMesh(binaryReader, true))
                        return true;
                }
            }
            return false;
		}

		private static bool ReadMesh(BinaryReader b, bool isNewVersion)
		{
            if (isNewVersion)
            {
                ResetKey();

                pMesh.HeaderInfo.MeshCount = DecodeMesh(pMesh.HeaderInfo.MeshCount);
                pMesh.HeaderInfo.VertexCount = DecodeMesh(pMesh.HeaderInfo.VertexCount);
                pMesh.HeaderInfo.JointCount = DecodeMesh(pMesh.HeaderInfo.JointCount);
                pMesh.HeaderInfo.TextureMaps = DecodeMesh(pMesh.HeaderInfo.TextureMaps);
                pMesh.HeaderInfo.NormalCount = DecodeMesh(pMesh.HeaderInfo.NormalCount);
                pMesh.HeaderInfo.ObjCount = DecodeMesh(pMesh.HeaderInfo.ObjCount);
                pMesh.HeaderInfo.UnknownCount = DecodeMesh(pMesh.HeaderInfo.UnknownCount);
                pMesh.Value1 = DecodeMesh(pMesh.Value1);
            }
            else
            {
                pMesh.HeaderInfo.NormalCount = pMesh.HeaderInfo.UnknownCount;
                pMesh.HeaderInfo.JointCount = pMesh.HeaderInfo.NormalCount;
                pMesh.HeaderInfo.UnknownCount = pMesh.HeaderInfo.TextureMaps;
                pMesh.HeaderInfo.TextureMaps = pMesh.HeaderInfo.JointCount;
            }

            // reading vertices
            {
                pMesh.Vertices = new tVertex3f[pMesh.HeaderInfo.VertexCount];
                for (int i = 0; i < pMesh.HeaderInfo.VertexCount; i++)
                    pMesh.Vertices[i] = new tVertex3f(b.ReadSingle(), b.ReadSingle(), b.ReadSingle());  //mlod_aVertices
            }

            // reading normals
            {
                uint uNormals = isNewVersion ? pMesh.HeaderInfo.NormalCount : pMesh.HeaderInfo.VertexCount;
                pMesh.Normals = new tVertex3f[uNormals];
                for (int j = 0; j < uNormals; j++)
                    pMesh.Normals[j] = new tVertex3f(b.ReadSingle(), b.ReadSingle(), b.ReadSingle());   //mlod_aNormals
            }

            // reading uv maps
            if (pMesh.HeaderInfo.TextureMaps != 0)
			{
				pMesh.UVMaps = new tMeshUVMap[pMesh.HeaderInfo.TextureMaps];
				for (int k = 0; k < pMesh.HeaderInfo.TextureMaps; k++)
				{
                    if (isNewVersion)
                    {
                        tMeshUVMap tMeshUVMap = new tMeshUVMap
                        {
                            Name = b.ReadBytes(b.ReadInt32()),
                            Coords = new tTextCoord[pMesh.HeaderInfo.VertexCount]
                        };

                        for (int l = 0; l < pMesh.HeaderInfo.VertexCount; l++)
                            tMeshUVMap.Coords[l] = new tTextCoord(b.ReadSingle(), b.ReadSingle());

                        pMesh.UVMaps[k] = tMeshUVMap;
                    }
                    else
                    {
                        pMesh.UVMaps[k] = new tMeshUVMap
                        {
                            Name = b.ReadBytes(b.ReadInt32()),  //mlod_aUVMaps[imuvm].muv_iID
                            Coords = new tTextCoord[pMesh.HeaderInfo.VertexCount]
                        };

                        for (int l = 0; l < pMesh.HeaderInfo.VertexCount; l++)
                            pMesh.UVMaps[k].Coords[l] = new tTextCoord(b.ReadSingle(), b.ReadSingle()); //sizeof(MeshTexCoord)*ctmvx
                    }
				}
			}

            tMeshObject tMeshObject = new tMeshObject();
            pMesh.Objects = new tMeshObject[pMesh.HeaderInfo.ObjCount];
			for (int m = 0; m < pMesh.HeaderInfo.ObjCount; m++)
			{
                if (isNewVersion)
                {
                    tMeshObject = new tMeshObject
                    {
                        FromVert = DecodeMesh(b.ReadUInt32()),
                        ToVert = DecodeMesh(b.ReadUInt32()),
                        FaceCount = DecodeMesh(b.ReadUInt32())
                    };
                }
                else
                {
                    tMeshObject = new tMeshObject
                    {
                        MaterialName = b.ReadBytes(b.ReadInt32()),  //ska_IDToString(INDEX msrf.msrf_iSurfaceID)
                        Value1 = b.ReadUInt32(),    //msrf.msrf_ulFlags
                        FromVert = b.ReadUInt32(),  //msrf.msrf_iFirstVertex
                        ToVert = b.ReadUInt32(),    //msrf.msrf_ctVertices
                        FaceCount = b.ReadUInt32()  //msrf.msrf_auwTriangles.Count()
                    };
                }

                tMeshObject.Faces = new tFace[tMeshObject.FaceCount];
                for (int n = 0; n < tMeshObject.FaceCount; n++)
                    tMeshObject.Faces[n] = new tFace(b.ReadInt16(), b.ReadInt16(), b.ReadInt16());  //mlod_aSurfaces[imsrt].msrf_auwTriangles[0]

                if (isNewVersion)
                {
                    tMeshObject.MaterialName = b.ReadBytes(b.ReadInt32());
                    tMeshObject.Value1 = DecodeMesh(b.ReadUInt32());
                }

                tMeshObject.JValue = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32();    //msrf_aubRelIndexTable.Count()
                tMeshObject.JData = new byte[tMeshObject.JValue];
                for (int num = 0; num < tMeshObject.JValue; num++)
                    tMeshObject.JData[num] = b.ReadByte();  //msrf_aubRelIndexTable[0]

				tMeshObject.ShaderFlag = DecodeMesh(b.ReadUInt32());
				if (tMeshObject.ShaderFlag != 0)
				{
                    tMeshObject.ShaderInfo = new tMeshShaderInfo
                    {
                        cParam1             = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32(),
                        cParamFloats        = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32(),
                        cTextureUnits       = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32(),
                        cParam2             = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32()
                    };

                    if (!isNewVersion)
                    {
                        tMeshObject.ShaderInfo = new tMeshShaderInfo
                        {
                            cTextureUnits = tMeshObject.ShaderInfo.cParam1,
                            cParamFloats = tMeshObject.ShaderInfo.cParamFloats,
                            cParam1 = tMeshObject.ShaderInfo.cParam2,
                            cParam2 = tMeshObject.ShaderInfo.cTextureUnits
                        };
                    }

                    tMeshObject.ShaderData = new tMeshShaderData
                    {
                        ShaderName = b.ReadBytes(b.ReadInt32()) //msrf_pShader->GetName()
                    };

                    tMeshObject.Textures = new tMeshTexture[tMeshObject.ShaderInfo.cTextureUnits];
                    if (isNewVersion)
                    {
                        for (int num2 = 0; num2 < tMeshObject.ShaderInfo.cTextureUnits; num2++)
                        {
                            tMeshObject.Textures[num2] = default(tMeshTexture);
                            tMeshObject.Textures[num2].InternalName = b.ReadBytes(b.ReadInt32());
                        }
                    }
                    else
                    {
                        // reading shader texture IDs
                        for (int num2 = 0; num2 < tMeshObject.ShaderInfo.cTextureUnits; num2++)
                            tMeshObject.Textures[num2] = new tMeshTexture(b.ReadBytes(b.ReadInt32()));  //ska_IDToString(INDEX msrf.msrf_ShadingParams.sp_aiTextureIDs[itx])
                    }

                    uint uParam = isNewVersion ? tMeshObject.ShaderInfo.cParam2 : tMeshObject.ShaderInfo.cParam1;
                    if (uParam != 0)
						tMeshObject.ShaderData.Param1 = new uint[tMeshObject.ShaderInfo.cParam1];
					
					if (tMeshObject.ShaderInfo.cParamFloats != 0)
						tMeshObject.ShaderData.ParamFloats = new float[tMeshObject.ShaderInfo.cParamFloats];

                    uint uParam1 = isNewVersion ? tMeshObject.ShaderInfo.cParam1 : tMeshObject.ShaderInfo.cParam2;
                    if (uParam1 != 0)
						tMeshObject.ShaderData.Param2 = new uint[tMeshObject.ShaderInfo.cParam2];
					
					tMeshObject.ShaderData.cParam0 = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32();    //msrf_ShadingParams.sp_aiTexCoordsIndex[itc]

                    // reading shader colors
                    for (int num3 = 0; num3 < tMeshObject.ShaderInfo.cParam2; num3++)
						tMeshObject.ShaderData.Param2[num3] = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32();   //msrf_ShadingParams.sp_acolColors[icol]

                    // reading shader floats
                    for (int num4 = 0; num4 < tMeshObject.ShaderInfo.cParamFloats; num4++)
						tMeshObject.ShaderData.ParamFloats[num4] = b.ReadSingle();  //msrf.msrf_ShadingParams.sp_afFloats[ifl]

                    // reading shader flags ; no idea why loop is there
                    for (int num5 = 0; num5 < tMeshObject.ShaderInfo.cParam1; num5++)
						tMeshObject.ShaderData.Param1[num5] = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32();   //msrf_ShadingParams.sp_ulFlags
					
				}
				pMesh.Objects[m] = tMeshObject;
			}

            // reading weightmaps
            pMesh.Weights = new tMeshJointWeights[pMesh.HeaderInfo.JointCount];
            for (int num6 = 0; num6 < pMesh.HeaderInfo.JointCount; num6++)
            {
                pMesh.Weights[num6] = new tMeshJointWeights
                {
                    JointName = b.ReadBytes(b.ReadInt32()), //ska_IDToString(MeshWeightMap mlod_aWeightMaps[imwm].mwm_iID)
                    Count = isNewVersion ? DecodeMesh(b.ReadUInt32()) : b.ReadUInt32()  //mwm_aVertexWeight.Count()
                };

                // reading vertex weights
                pMesh.Weights[num6].WeightsMap = new tMeshWeightsMap[pMesh.Weights[num6].Count];
                for (int num7 = 0; num7 < pMesh.Weights[num6].Count; num7++)
                    pMesh.Weights[num6].WeightsMap[num7] = new tMeshWeightsMap(b.ReadInt32(), b.ReadSingle());  //mwm_aVertexWeight[0]
            }

            // reading morph map
			pMesh.MorphMap = new tMeshMorphMap[pMesh.HeaderInfo.VertexCount];
			for (int num8 = 0; num8 < pMesh.HeaderInfo.VertexCount; num8++)
			    pMesh.MorphMap[num8] = new tMeshMorphMap(b.ReadBytes(4), b.ReadBytes(4));   //ska_IDToString(MeshMorphMap mlod_aMorphMaps[immm] mmm.mmp_iID)

			return b.BaseStream.Position == pMesh.HeaderInfo.MeshDataSize + 8;
        }
	}
}
