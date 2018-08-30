using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using SlimDX;
using SlimDX.Direct3D9;

using ItemEditor.Data;
using ItemEditor.LastChaosUtil;
using ItemEditor.Structure.Item;
using ItemEditor.Structure.Model;
using ItemEditor.Structure;
using ItemEditor.Forms.Pickers;
using ItemEditor.Configuration;

namespace ItemEditor.Forms
{
    public partial class frmMain : Form
    {
        public bool bToggleFlagSelector = true;
        public bool bToggleFlagEdit = false;
        public bool bToggleSupport;

        private Device device_1, device_2;
        private List<cMesh.tMesh> models = new List<cMesh.tMesh>();
        public static System.Text.UTF7Encoding encoding = new System.Text.UTF7Encoding();
        private Direct3D mD3d_1, mD3d_2;

        private float fZoom;
        private float fRotation;
        private float fLeftnRight;

        private float fUpnDown = -0.9f;

        cLog cLogger;

        public frmMain()
        {
            InitializeComponent();
            cLogger = new cLog(rtbLog);
        }


        public Image Base64ToImage(string commands)
        {

            byte[] photoarray = Convert.FromBase64String(commands);
            MemoryStream ms = new MemoryStream(photoarray, 0, photoarray.Length);
            ms.Write(photoarray, 0, photoarray.Length);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;

        }

        private async void button14_Click(object sender, EventArgs e)
        {
            pIcon.Image = await cImport.GetIcon(IconType.ITEM, getIDfromListBox());
            // string drop = cImport.getDropsFromItem(999);

            //    tbSearch.Text = drop;
        }

        private void MakeList(string flagValue = null, string additionalFlagValue = null)
        {
            List<string> searchData = new List<string>();
            List<cItem> cItem = new List<cItem>();

            if (cbSearchType.Text == "Name")
            {
                cItem = cImport.ItemData.FindAll(p => p.GetNameLang(cImport.CurrentLanguage).ToLower().Contains(tbSearch.Text.ToLower()) /*|| p.ID == Convert.ToInt32(tbSearch.Text)*/);
                for (int index = 0; index < Enumerable.Count(cItem); ++index)
                    searchData.Add(cItem[index].ID + " - " + cItem[index].GetNameLang(cImport.CurrentLanguage));
            }

            else if (cbSearchType.Text == "Description")
            {
                cItem = cImport.ItemData.FindAll(p => p.GetDescrLang(cImport.CurrentLanguage).ToLower().Contains(tbSearch.Text.ToLower()));
                for (int index = 0; index < Enumerable.Count(cImport.ItemData.FindAll(p => p.GetDescrLang(cImport.CurrentLanguage).ToLower().Contains(tbSearch.Text.ToLower()))); ++index)
                    searchData.Add(cItem[index].ID + " - " + cItem[index].GetNameLang(cImport.CurrentLanguage));
            }

            else if (cbSearchType.Text == "Type")
            {
                int.TryParse(flagValue ?? tbSearch.Text.Split(',')[0], out int flagValue1);
                int.TryParse(additionalFlagValue ?? tbSearch.Text.Split(',')[1], out int flagValue2);

                foreach (cItem cit in cImport.ItemData)
                {
                    if (cit.ItemType == flagValue1 && cit.ItemSubType == flagValue2)
                        searchData.Add(cit.ID + " - " + cit.GetNameLang(cImport.CurrentLanguage));
                }
            }

            else if (cbSearchType.Text == "Flag")
            {
                ulong.TryParse(flagValue ?? tbSearch.Text, out ulong flagValue1);

                foreach (cItem cit in cImport.ItemData)
                {
                    if (flagValue1 == cit.ItemFlag)
                        searchData.Add(cit.ID + " - " + cit.GetNameLang(cImport.CurrentLanguage));
                }
            }

            else if (cbSearchType.Text == "JobFlag")
            {
                ulong.TryParse(flagValue ?? tbSearch.Text, out ulong jobFlag);

                foreach (cItem cit in cImport.ItemData)
                {
                    if (cit.JobFlag == jobFlag)
                        searchData.Add(cit.ID + " - " + cit.GetNameLang(cImport.CurrentLanguage));
                }
            }

            lbItems.Items.Clear();
            lbItems.Items.AddRange(searchData.ToArray());
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var flagType        = from i in Enum.GetNames(typeof(ITYPE)) select i;
            var flagWearing     = from i in Enum.GetNames(typeof(IWEARING)) select i;
            var flagPetGrade    = from i in Enum.GetNames(typeof(GRADE)) select i;
            
            var typeRVR         = from i in Enum.GetNames(typeof(SYNDICATE_TYPE)) select i;
            var typeCastle      = from i in Enum.GetNames(typeof(CASTLEWAR)) select i;

            cbType.Items.AddRange(flagType.ToArray());
            cbWearPos.Items.AddRange(flagWearing.ToArray());
            cbSearchTypeFlag.Items.AddRange(flagType.ToArray());
            cbPetGrade.Items.AddRange(flagPetGrade.ToArray());
            cbRVRValue.Items.AddRange(typeRVR.ToArray());
            cbCastleWar.Items.AddRange(typeCastle.ToArray());

            cbSearchType.SelectedIndex = 0;
            cbLang.SelectedIndex = 8;

            cImport.GetConfiguration();

            frmSettings frmSettings = new frmSettings();
            frmSettings.cbDatabase.Items.Clear();
            for (int i = 0; i < cMySQL.configStructure.Count(); i++)
                frmSettings.cbDatabase.Items.Add(cMySQL.configStructure[i].Name);
            frmSettings.cbDatabase.SelectedIndex = 0;


            loadTooltip();
        }

        private void SaveItem()
        {
            int ID = getIDfromListBox();
            cItem cItems = cImport.ItemData.Find(p => p.ID.Equals(ID));
            if (cItems != null)
            {
                cItems.ID = Convert.ToInt32(tbID.Text);
                cItems.Name[(int)cImport.CurrentLanguage] = tbName.Text;
                cItems.Description[(int)cImport.CurrentLanguage] = tbDescription.Text;
                cItems.SMC = tbSMC.Text;
                cItems.Price = Convert.ToInt32(tbPrice.Text.Replace(".", ""));
                cItems.QuestTriggerCount = Convert.ToInt32(tbQTCount.Text);
                cItems.QuestTriggerIDs = tbQTIDs.Text;
                cItems.EffectNormal = tbENormal.Text;
                cItems.EffectAttack = tbEAttack.Text;
                cItems.EffectDamage = tbEDamage.Text;
                cItems.ItemType = cbType.SelectedIndex;
                cItems.ItemSubType = cbSubType.SelectedIndex;
                cItems.Wearing = cbWearPos.SelectedIndex - 1;
                cItems.ItemFlag = Convert.ToUInt64(tbFlag.Text);
                cItems.LevelMin = Convert.ToInt32(tbLevel1.Text);
                cItems.LevelMax = Convert.ToInt32(tbLevel2.Text);
                cItems.JobFlag = getFlagFromListBox(clbClass);
                cItems.Grade = cbPetGrade.SelectedIndex;
                cItems.CastleWar = cbCastleWar.SelectedIndex;
                cItems.Durability = Convert.ToInt32(tbDurability.Text);

               // cItems.Set = Convert.ToInt32(tbSet.Text);
                cItems.Fame = Convert.ToInt32(tbFame.Text);
                cItems.ZoneFlag = Convert.ToInt32(tbZoneFlag.Text);
                cItems.MaxUse = Convert.ToInt32(tbMaxUse.Text);
                cItems.Weight = Convert.ToInt32(tbStacks.Text);

                SYNDICATE_TYPE rvrValue = GetEnumValue<SYNDICATE_TYPE>(cbRVRValue.Text);
                cItems.RVRValue = Convert.ToInt32(rvrValue);

                if (rvrValue == SYNDICATE_TYPE.KAILUX)
                {
                    SYNDICATE_GRADE_KAILUX rvrGrade = GetEnumValue<SYNDICATE_GRADE_KAILUX>(cbRVRGrade.Text);
                    cItems.RVRGrade = Convert.ToInt32(rvrGrade);
                }
                else if (rvrValue == SYNDICATE_TYPE.DEALERMOON)
                {
                    SYNDICATE_GRADE_DEALERMOON rvrGrade = GetEnumValue<SYNDICATE_GRADE_DEALERMOON>(cbRVRGrade.Text);
                    cItems.RVRGrade = Convert.ToInt32(rvrGrade);
                }

                for (int i = 0; i <= 4; i++)
                {
                    Control[] ctlNUM = Controls.Find("tbNUM" + i.ToString(), true);
                    Control[] ctlSET = Controls.Find("tbSET" + i.ToString(), true);

                    cItems.NUM[i] = Convert.ToInt32(ctlNUM[0].Text);
                    cItems.SET[i] = Convert.ToInt32(ctlSET[0].Text);
                }

                for(int i = 1; i <= 6; i++)
                {
                    Control[] ctlORIGIN = Controls.Find("tbORIGIN" + i.ToString(), true);
                    cItems.ORIGIN[i] = Convert.ToInt32(ctlORIGIN[0].Text);
                }

                for (int i = 0; i <= 9; i++)
                {
                    Control[] ctlRAREID = Controls.Find("tbRareOpt" + i.ToString(), true);
                    Control[] ctlRAREPROB = Controls.Find("tbRareChance" + i.ToString(), true);

                    cItems.RAREID[i] = Convert.ToInt32(ctlRAREID[0].Text);
                    cItems.RAREPROB[i] = Convert.ToInt32(ctlRAREPROB[0].Text);
                }

                string str2 = cItems.ID + " - " + cItems.Name[(int)cImport.CurrentLanguage];
                if ((string)lbItems.SelectedItem != str2)
                    lbItems.Items[lbItems.SelectedIndex] = str2;

                cItems.changesWasHere = true;
            }
        }

        public T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }
                }
            }

            return val;
        }

        #region model_stuff_not_safe_optimize_needed
        private void MakeModel(string SMCFile)
        {
            int num = -1;
            int ID = getIDfromListBox();
            int num2 = cImport.ItemData.FindIndex((cItem p) => p.ID.Equals(ID));
            if (num2 != -1)
            {
                ulong jobFlag = cImport.ItemData[num2].JobFlag;
                num = cImport.ItemData[num2].Wearing;
            }
            try
            {
                List<cSMC.smcMesh> list = cSMC.SMCReader.ReadFile(SMCFile);
                for (int i = 0; i < list.Count(); i++)
                {
                    bool flag = true;
                    flag = ((num != 0 || !list[i].FileName.Contains("_hair_000")) && (num != 1 || !list[i].FileName.Contains("_bu_000")) && (num != 3 || !list[i].FileName.Contains("_bd_000")) && (num != 5 || !list[i].FileName.Contains("_hn_000")) && ((num != 6 || !list[i].FileName.Contains("_ft_000")) ? true : false));
                    if (flag && cMeshManagement.ReadFile(list[i].FileName))
                    {
                        cMesh.tMeshContainer pMesh = cMeshManagement.pMesh;
                        for (int j = 0; j < pMesh.Objects.Count(); j++)
                        {
                            int toVert = (int)pMesh.Objects[j].ToVert;
                            uint faceCount = pMesh.Objects[j].FaceCount;
                            short[] faces = pMesh.Objects[j].GetFaces();
                            cVertex.CustomVertex.PositionNormalTextured[] array = new cVertex.CustomVertex.PositionNormalTextured[toVert];
                            int num3 = (int)pMesh.Objects[j].FromVert;
                            for (int k = 0; k < pMesh.Objects[j].ToVert; k++)
                            {
                                array[k].Position = new Vector3(pMesh.Vertices[num3].X, pMesh.Vertices[num3].Y, pMesh.Vertices[num3].Z);
                                array[k].Normal = new Vector3(pMesh.Normals[num3].X, pMesh.Normals[num3].Y, pMesh.Normals[num3].Z);
                                try
                                {
                                    array[k].Texture = new Vector2(pMesh.UVMaps[0].Coords[num3].U, pMesh.UVMaps[0].Coords[num3].V);
                                }
                                catch
                                {
                                    array[k].Texture = new Vector2(0f, 0f);
                                }
                                num3++;
                            }
                            new VertexBuffer(device_1, array.Count() * 32, Usage.None, VertexFormat.Position | VertexFormat.Texture1 | VertexFormat.Normal, Pool.Default);
                            Mesh mesh = new Mesh(device_1, faces.Count() / 3, array.Count(), MeshFlags.Managed, VertexFormat.Position | VertexFormat.Texture1 | VertexFormat.Normal);
                            DataStream dataStream;
                            using (dataStream = mesh.VertexBuffer.Lock(0, array.Count() * 32, LockFlags.None))
                            {
                                dataStream.WriteRange(array);
                                mesh.VertexBuffer.Unlock();
                            }
                            using (dataStream = mesh.IndexBuffer.Lock(0, faces.Count() * 2, LockFlags.None))
                            {
                                dataStream.WriteRange(faces);
                                mesh.IndexBuffer.Unlock();
                            }
                            if (pMesh.Weights.Count() != 0)
                            {
                                string[] array2 = new string[pMesh.Weights.Count()];
                                List<int>[] array3 = new List<int>[pMesh.Weights.Count()];
                                List<float>[] array4 = new List<float>[pMesh.Weights.Count()];
                                for (int l = 0; l < pMesh.Weights.Count(); l++)
                                {
                                    array2[l] = encoding.GetString(pMesh.Weights[l].JointName);
                                    array3[l] = new List<int>();
                                    array4[l] = new List<float>();
                                    for (int m = 0; m < pMesh.Weights[l].WeightsMap.Count(); m++)
                                    {
                                        array3[l].Add(pMesh.Weights[l].WeightsMap[m].Index);
                                        array4[l].Add(pMesh.Weights[l].WeightsMap[m].Weight);
                                    }
                                }
                                mesh.SkinInfo = new SkinInfo(array.Count(), VertexFormat.Position | VertexFormat.Texture1 | VertexFormat.Normal, (int)pMesh.HeaderInfo.JointCount);
                                for (int k = 0; k < array3.Count(); k++)
                                {
                                    mesh.SkinInfo.SetBoneName(k, array2[k]);
                                    mesh.SkinInfo.SetBoneInfluence(k, array3[k].ToArray(), array4[k].ToArray());
                                }
                            }
                            mesh.GenerateAdjacency(0.5f);
                            mesh.ComputeNormals();
                            Texture texture = null;
                            string objName = encoding.GetString(pMesh.Objects[j].Textures[0].InternalName);
                            int num4 = list[i].Object.FindIndex((cSMC.smcObject x) => x.Name.Equals(objName));
                            if (num4 != -1)
                            {
                                texture = GetTextureFromFile(list[i].Object[num4].Texture, device_1);
                            }
                            models.Add(new cMesh.tMesh(mesh, texture));
                        }
                    }
                }
            }
            catch
            {
            }
            fZoom = 4f;
        }

        private void MakeModel1(string smc)
        {
            int num = -1;

            try
            {
                List<cSMC.smcMesh> list = cSMC.SMCReader.ReadFile(smc);
                for (int i = 0; i < list.Count(); i++)
                {
                    bool flag = true;
                    flag = ((num != 0 || !list[i].FileName.Contains("_hair_000")) && (num != 1 || !list[i].FileName.Contains("_bu_000")) && (num != 3 || !list[i].FileName.Contains("_bd_000")) && (num != 5 || !list[i].FileName.Contains("_hn_000")) && ((num != 6 || !list[i].FileName.Contains("_ft_000")) ? true : false));
                    if (flag && cMeshManagement.ReadFile(list[i].FileName))
                    {
                        cMesh.tMeshContainer pMesh = cMeshManagement.pMesh;
                        for (int j = 0; j < pMesh.Objects.Count(); j++)
                        {
                            int toVert = (int)pMesh.Objects[j].ToVert;
                            uint faceCount = pMesh.Objects[j].FaceCount;
                            short[] faces = pMesh.Objects[j].GetFaces();

                            cVertex.CustomVertex.PositionNormalTextured[] array = new cVertex.CustomVertex.PositionNormalTextured[toVert];

                            int num2 = (int)pMesh.Objects[j].FromVert;
                            for (int k = 0; k < pMesh.Objects[j].ToVert; k++)
                            {
                                array[k].Position = new Vector3(pMesh.Vertices[num2].X, pMesh.Vertices[num2].Y, pMesh.Vertices[num2].Z);
                                array[k].Normal = new Vector3(pMesh.Normals[num2].X, pMesh.Normals[num2].Y, pMesh.Normals[num2].Z);

                                try { array[k].Texture = new Vector2(pMesh.UVMaps[0].Coords[num2].U, pMesh.UVMaps[0].Coords[num2].V); }
                                catch { array[k].Texture = new Vector2(0f, 0f); }

                                num2++;
                            }

                            new VertexBuffer(device_2, array.Count() * 32, Usage.None, VertexFormat.Position | VertexFormat.Texture1 | VertexFormat.Normal, Pool.Default);
                            Mesh mesh = new Mesh(device_2, faces.Count() / 3, array.Count(), MeshFlags.Managed, VertexFormat.Position | VertexFormat.Texture1 | VertexFormat.Normal);
                            DataStream dataStream;

                            using (dataStream = mesh.VertexBuffer.Lock(0, array.Count() * 32, LockFlags.None))
                            {
                                dataStream.WriteRange(array);
                                mesh.VertexBuffer.Unlock();
                            }

                            using (dataStream = mesh.IndexBuffer.Lock(0, faces.Count() * 2, LockFlags.None))
                            {
                                dataStream.WriteRange(faces);
                                mesh.IndexBuffer.Unlock();
                            }

                            if (pMesh.Weights.Count() != 0)
                            {
                                string[] array2 = new string[pMesh.Weights.Count()];
                                List<int>[] array3 = new List<int>[pMesh.Weights.Count()];
                                List<float>[] array4 = new List<float>[pMesh.Weights.Count()];

                                for (int l = 0; l < pMesh.Weights.Count(); l++)
                                {
                                    array2[l] = encoding.GetString(pMesh.Weights[l].JointName);
                                    array3[l] = new List<int>();
                                    array4[l] = new List<float>();

                                    for (int m = 0; m < pMesh.Weights[l].WeightsMap.Count(); m++)
                                    {
                                        array3[l].Add(pMesh.Weights[l].WeightsMap[m].Index);
                                        array4[l].Add(pMesh.Weights[l].WeightsMap[m].Weight);
                                    }
                                }

                                mesh.SkinInfo = new SkinInfo(array.Count(), VertexFormat.Position | VertexFormat.Texture1 | VertexFormat.Normal, (int)pMesh.HeaderInfo.JointCount);
                                for (int k = 0; k < array3.Count(); k++)
                                {
                                    mesh.SkinInfo.SetBoneName(k, array2[k]);
                                    mesh.SkinInfo.SetBoneInfluence(k, array3[k].ToArray(), array4[k].ToArray());
                                }
                            }

                            mesh.GenerateAdjacency(0.5f);
                            mesh.ComputeNormals();

                            Texture texture = null;

                            string objName = encoding.GetString(pMesh.Objects[j].Textures[0].InternalName);
                            int num3 = list[i].Object.FindIndex((cSMC.smcObject x) => x.Name.Equals(objName));
                            if (num3 != -1)
                                texture = GetTextureFromFile(list[i].Object[num3].Texture, device_2);

                            models.Add(new cMesh.tMesh(mesh, texture));
                        }
                    }
                }
            }
            catch { }
            fZoom = 10f;
        }

        private Texture GetTextureFromFile(string FileName, Device device)
        {
            Texture result = null;
            if (File.Exists(FileName))
            {
                cTextureManagement cTextureManagement = new cTextureManagement();
                cTextureManagement.ReadFile(FileName);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(cTextureManagement.MakeBitmap());
                    bmp.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    memoryStream.Position = 0L;

                    result = Texture.FromStream(device, memoryStream, (int)cTextureManagement.Width, (int)cTextureManagement.Height, 0, Usage.SoftwareProcessing, Format.A8B8G8R8, Pool.Default, Filter.None, Filter.None, 0);
                }
            }
            return result;
        }

        private void CameraPositioning()
        {
            device_1.SetTransform(TransformState.Projection, Matrix.PerspectiveFovLH(100f, 1f, 1f, 450f));
            device_1.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f)));
            device_1.SetTransform(TransformState.World, Matrix.RotationYawPitchRoll(0f, 0f, 0f));
        }

        private void CameraPositioning1()
        {
            device_2.SetTransform(TransformState.Projection, Matrix.PerspectiveFovLH(100f, 1f, 1f, 450f));
            device_2.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f)));
            device_2.SetTransform(TransformState.World, Matrix.RotationYawPitchRoll(0f, 0f, 0f));
        }

        private void Render()
        {
            Viewport viewport = new Viewport(0, 0, pBody.Width, pBody.Height);
            device_1.Viewport = viewport;
            device_1.Clear(ClearFlags.ZBuffer | ClearFlags.Target, new Color4(Color.FromKnownColor(KnownColor.Control)), 1f, 0);
            device_1.BeginScene();
            device_1.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0f, 0f, fZoom), new Vector3(fLeftnRight, fUpnDown, 0f), new Vector3(0f, 1f, 0f)));
            device_1.SetTransform(TransformState.World, Matrix.RotationYawPitchRoll(fRotation, 3.1f, 0f));

            if (models != null && models.Count() != 0)
            {
                for (int i = 0; i < models.Count(); i++)
                {
                    if (models[i].TexData != null)
                        device_1.SetTexture(0, models[i].TexData);
                    for (int j = 0; j < 1000; j++)
                        models[i].MeshData.DrawSubset(j);
                }
            }

            device_1.EndScene();

            // ?? lmao
            try { device_1.Present(); }
            catch { try { initDevice(); } catch { } }

            if (cbRotate.Checked)
                fRotation -= 0.03f;
        }

        private void Render1()
        {
            Viewport viewport = new Viewport(0, 0, pItem.Width, pItem.Height);
            device_2.Viewport = viewport;
            device_2.Clear(ClearFlags.ZBuffer | ClearFlags.Target, new Color4(Color.FromKnownColor(KnownColor.Control)), 1f, 0);
            device_2.BeginScene();
            device_2.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0f, 0f, fZoom), new Vector3(fLeftnRight, fUpnDown, 0f), new Vector3(0f, 1f, 0f)));
            device_2.SetTransform(TransformState.World, Matrix.RotationYawPitchRoll(fRotation, 3.1f, 0f));

            if (models != null && models.Count() != 0)
            {
                for (int i = 0; i < models.Count(); i++)
                {
                    if (models[i].TexData != null)
                        device_2.SetTexture(0, models[i].TexData);
                    for (int j = 0; j < 1000; j++)
                        models[i].MeshData.DrawSubset(j);
                }
            }

            device_2.EndScene();

            // ?? lmao
            try { device_2.Present(); }
            catch { try { initDevice1(); } catch { } }

            if (cbRotate.Checked)
                fRotation -= 0.03f;
        }

        private void initDevice()
        {
            mD3d_1 = new Direct3D();
            PresentParameters presentParameters = new PresentParameters();
            presentParameters.SwapEffect = SwapEffect.Discard;
            presentParameters.DeviceWindowHandle = pBody.Handle;
            presentParameters.Windowed = true;
            presentParameters.BackBufferWidth = pBody.Width;
            presentParameters.BackBufferHeight = pBody.Height;
            presentParameters.BackBufferFormat = Format.A8R8G8B8;
            PresentParameters presentParameters2 = presentParameters;
            device_1 = new Device(mD3d_1, 0, DeviceType.Hardware, base.Handle, CreateFlags.SoftwareVertexProcessing, presentParameters2);
            device_1.SetRenderState(RenderState.CullMode, Cull.None);
            device_1.SetRenderState(RenderState.FillMode, FillMode.Solid);
            device_1.SetRenderState(RenderState.Lighting, value: false);
            CameraPositioning();
            cbRotate.Checked = true;
        }

        private void initDevice1()
        {
            mD3d_2 = new Direct3D();
            PresentParameters presentParameters = new PresentParameters();
            presentParameters.SwapEffect = SwapEffect.Discard;
            presentParameters.DeviceWindowHandle = pItem.Handle;
            presentParameters.Windowed = true;
            presentParameters.BackBufferWidth = pItem.Width;
            presentParameters.BackBufferHeight = pItem.Height;
            presentParameters.BackBufferFormat = Format.A8R8G8B8;
            PresentParameters presentParameters2 = presentParameters;
            device_2 = new Device(mD3d_2, 0, DeviceType.Hardware, base.Handle, CreateFlags.SoftwareVertexProcessing, presentParameters2);
            device_2.SetRenderState(RenderState.CullMode, Cull.None);
            device_2.SetRenderState(RenderState.FillMode, FillMode.Solid);
            device_2.SetRenderState(RenderState.Lighting, value: false);
            CameraPositioning1();
            cbRotate.Checked = true;
        }
        #endregion

        public void loadTooltip()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;

            #region tooltip_strings

            toolTip.SetToolTip(btnAdd,          "Add new item to Memory and Database");
            toolTip.SetToolTip(btnDelete,       "Delete item from Memory and Database");
            toolTip.SetToolTip(btnImport,       "Import all data from database to memory");
            toolTip.SetToolTip(btnExport,       "Export all edited data to database from memory");

            toolTip.SetToolTip(tbSearch,        "Supported schema (based on SearchType combo box):\n" +
                                                "---\n" +
                                                "Name: (string)itemName\n" +
                                                "Description: (string)itemDesc\n" +
                                                "Flag: (ulong)itemFlag\n" +
                                                "Type: (int)itemType,(int)itemSubType\n" +
                                                "JobFlag: (ulong)itemJobFlag");

            toolTip.SetToolTip(btnscam,      "Donate");

            toolTip.SetToolTip(btnTitan,        "Titan");
            toolTip.SetToolTip(btnKnight,       "Knight");
            toolTip.SetToolTip(btnHealer,       "Healer");
            toolTip.SetToolTip(btnMage,         "Mage");
            toolTip.SetToolTip(btnRogue,        "Rogue");
            toolTip.SetToolTip(btnSorcerer,     "Sorcerer");
            toolTip.SetToolTip(btnNightShadow,  "NightShadow");
            toolTip.SetToolTip(btnExRogue,      "EX-Rogue");
            toolTip.SetToolTip(btnExMage,       "EX-Mage (Archmage)");
            toolTip.SetToolTip(btnP1,           "Pet Type 1");
            toolTip.SetToolTip(btnP2,           "Pet Type 2 (APet)");

            #endregion
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            if (!bgwLoadData.IsBusy)//Check if the worker is already in progress
            {
                btnImport.Enabled = false;//Disable the Start button
                bgwLoadData.RunWorkerAsync();//Call the background worker
            }

            tslbStatus.Text = "Gathering Item Data...";
        }

        private void bgwLoadData_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            MakeList();
            tslbStatus.Text = string.Format("Succesufully loaded {0} Items!", lbItems.Items.Count);
            cLogger.MessageLog(string.Format("Loaded {0} Items from Database", lbItems.Items.Count), MESSAGE_TYPE.SUCCESS);

            {
                btnImport.Enabled = true;
                btnExport.Enabled = true;
                tbContent.Enabled = true;
                pOptions.Enabled = true;
                pOptions1.Enabled = true;
            }
            
        }

        private int getIDfromListBox()
        {
            if (lbItems.Text.Split(' ')[0].All(char.IsDigit))
                return Convert.ToInt32(lbItems.Text.Split(' ')[0]);
            else
                return 0;
        }

        private async void lbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            initDevice();
            initDevice1();
            int id = 0;
            try { id = getIDfromListBox(); } catch { }

            cItem cItem = cImport.ItemData.Find(p => p.ID.Equals(id));

            if (cItem != null)
            {
                tbName.Text = cItem.GetNameLang(cImport.CurrentLanguage);
                tbDescription.Text = cItem.GetDescrLang(cImport.CurrentLanguage);
                tbID.Text = cItem.ID.ToString();
                tbSMC.Text = cItem.SMC;
                tbENormal.Text = cItem.EffectNormal;
                tbEAttack.Text = cItem.EffectAttack;
                tbEDamage.Text = cItem.EffectDamage;

                tbPrice.Text = cItem.Price.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
                tbQTCount.Text = cItem.QuestTriggerCount.ToString();
                tbQTIDs.Text = cItem.QuestTriggerIDs.ToString();
                tbLevel1.Text = cItem.LevelMin.ToString();
                tbLevel2.Text = cItem.LevelMax.ToString();
                cbPetGrade.SelectedIndex = cItem.Grade;
                cbCastleWar.SelectedIndex = cItem.CastleWar;
                tbDurability.Text = cItem.Durability.ToString();
                //tbSet.Text = cItem.Set.ToString();
                tbFame.Text = cItem.Fame.ToString();
                cbRVRValue.SelectedIndex = cItem.RVRValue;
                tbZoneFlag.Text = cItem.ZoneFlag.ToString();
                tbFlag.Text = cItem.ItemFlag.ToString();
                gBFlagBuilder.Text = "Flag Builder - " + cItem.ItemFlag.ToString();
                tbMaxUse.Text = cItem.MaxUse.ToString();
                cbEnabled.Checked = cItem.Enable == 1;
                checkFlagInListBox(clbClass, cItem.JobFlag);
                cbType.SelectedIndex = cItem.ItemType;
                checkFlagInListBox(clbFlagTest, cItem.ItemFlag);
                cbSubType.SelectedIndex = cItem.ItemSubType;

                cbWearPos.SelectedIndex = cItem.Wearing == -2 ? cItem.Wearing + 2 : cItem.Wearing + 1;

                tbStacks.Text = cItem.Weight.ToString();

                cTempVariables cTempVariables = new cTempVariables();
                cTempVariables.lastTexID = cItem.TexID;


                if (cItem.RVRValue > 0)
                    cbRVRGrade.SelectedIndex = cItem.RVRGrade - 1;

                for (int i = 0; i <= 4; i++)
                {
                    Control[] ctlNUM = Controls.Find("tbNUM" + i.ToString(), true);
                    Control[] ctlSET = Controls.Find("tbSET" + i.ToString(), true);

                    ctlNUM[0].Text = cItem.NUM[i].ToString();
                    ctlSET[0].Text = cItem.SET[i].ToString();
                }

                for (int i = 1; i <= 6; i++)
                {
                    Control[] ctlORIGIN = Controls.Find("tbORIGIN" + i.ToString(), true);
                    ctlORIGIN[0].Text = cItem.ORIGIN[i].ToString();
                }

                for (int i = 0; i <= 9; i++)
                {
                    Control[] ctlRAREID = Controls.Find("tbRareID" + i.ToString(), true);
                    Control[] ctlRAREPROB = Controls.Find("tbRareProb" + i.ToString(), true);
                    Control[] ctlRARENAME = Controls.Find("tbRareName" + i.ToString(), true);

                    ctlRAREID[0].Text = cItem.RAREID[i].ToString();
                    ctlRAREPROB[0].Text = cItem.RAREPROB[i].ToString();

                    var optionAddons = await cImport.getRarePrefix(cItem.RAREID[i]);
                    ctlRARENAME[0].Text = optionAddons.ToString();
                            
                }

                pIcon.Image = await cImport.GetIcon(IconType.ITEM, getIDfromListBox());

                #region LC_MODEL
                string str = @"X:\LastChaos\Servers\LCGenericName03\";
                if (File.Exists(str + cItem.SMC))
                {
                    models = new List<cMesh.tMesh>();
                    switch (cItem.JobFlag)
                    {
                        case 16:
                            MakeModel(str + "Data\\Character\\Rogue\\ro.smc");
                            break;
                        case 128:
                            MakeModel(str + "Data\\Character\\Rogue\\ro.smc");
                            break;
                        case 144:
                            MakeModel(str + "Data\\Character\\Rogue\\ro.smc");
                            break;
                        case 32:
                            MakeModel(str + "Data\\Character\\Sorcerer\\so.smc");
                            break;
                        case 64:
                            MakeModel(str + "Data\\Character\\NightShadow\\ns.smc");
                            break;
                        case 1:
                            MakeModel(str + "Data\\Character\\Titan\\ti.smc");
                            break;
                        case 2:
                            MakeModel(str + "Data\\Character\\Knight\\ni.smc");
                            break;
                        case 4:
                            MakeModel(str + "Data\\Character\\Healer\\hw.smc");
                            break;
                        case 8:
                            MakeModel(str + "Data\\Character\\Mage\\ma.smc");
                            break;
                        case 256:
                            MakeModel(str + "Data\\Character\\Mage\\ma.smc");
                            break;
                        case 264:
                            MakeModel(str + "Data\\Character\\Mage\\ma.smc");
                            break;
                    }
                    MakeModel(str + cItem.SMC);
                    Render();
                }
                MakeModel1(str + cItem.SMC);
                Render1();
                #endregion
            }
        }

        private ulong getFlagFromListBox(CheckedListBox clbFlagControl)
        {
            ulong flag = 0;
            for (int i = 0; i < clbFlagControl.Items.Count; i++)
                if (clbFlagControl.GetItemChecked(i))
                    flag += ((ulong)1 << i);

            return flag;
        }

        private void checkFlagInListBox(CheckedListBox clbFlagControl, ulong flag)
        {
            for (int index = 0; index < clbFlagControl.Items.Count; ++index)
                clbFlagControl.SetItemChecked(index, Convert.ToBoolean(((ulong)1 << index) & flag));
        }

        public enum SUBTYPE { ITEM, RVR };

        private void SetSubType(SUBTYPE subtype, int type, ComboBox control)
        {
            if (subtype == SUBTYPE.ITEM)
            {
                switch (type)
                {
                    case 0:
                        var iFlagWeapon = from i in Enum.GetNames(typeof(IWEAPON)) select i;
                        control.Items.AddRange(iFlagWeapon.ToArray());
                        break;
                    case 1:
                        var iFlagWear = from i in Enum.GetNames(typeof(IWEAR)) select i;
                        control.Items.AddRange(iFlagWear.ToArray());
                        break;
                    case 2:
                        var iFlagOnce = from i in Enum.GetNames(typeof(IONCE)) select i;
                        control.Items.AddRange(iFlagOnce.ToArray());
                        break;
                    case 3:
                        var iFlagShot = from i in Enum.GetNames(typeof(ISHOT)) select i;
                        control.Items.AddRange(iFlagShot.ToArray());
                        break;
                    case 4:
                        var iFlagETC = from i in Enum.GetNames(typeof(IETC)) select i;
                        control.Items.AddRange(iFlagETC.ToArray());
                        break;
                    case 5:
                        var iFlagAccessory = from i in Enum.GetNames(typeof(IACCESSORY)) select i;
                        control.Items.AddRange(iFlagAccessory.ToArray());
                        break;
                    case 6:
                        var iFlagPotion = from i in Enum.GetNames(typeof(IPOTION)) select i;
                        control.Items.AddRange(iFlagPotion.ToArray());
                        break;
                }
            }
            else if (subtype == SUBTYPE.RVR)
            {
                switch (type)
                {
                    case 1:
                        var flagKailux = from i in Enum.GetNames(typeof(SYNDICATE_GRADE_KAILUX)) select i;
                        control.Items.AddRange(flagKailux.ToArray());
                        break;
                    case 2:
                        var flagDealermoon = from i in Enum.GetNames(typeof(SYNDICATE_GRADE_DEALERMOON)) select i;
                        control.Items.AddRange(flagDealermoon.ToArray());
                        break;
                }
            }
        }

        private void clbFlagTest_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                tbFlag.Text = getFlagFromListBox(clbFlagTest).ToString();
            }));
        }

        private void clbSearchFlag_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                tbSearchFlagValue.Text = getFlagFromListBox(clbSearchFlag).ToString();
            }));
        }


        private void clbClass_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                ulong flag = getFlagFromListBox(clbClass);
            }));
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if(cbSearchType.SelectedIndex != 1)
                MakeList();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSubType.Items.Clear();
            SetSubType(SUBTYPE.ITEM, cbType.SelectedIndex, cbSubType);
        }

        private void cbWearPos_SelectedIndexChanged(object sender, EventArgs e)
        {
     //       if (c != this.cbWearPos.SelectedIndex - 1)
      //          tbPosition.Text = (cbWearPos.SelectedIndex - 1).ToString();
        }

        private void btnEditFlag_Click(object sender, EventArgs e)
        {
            #region design
            bToggleFlagEdit = bToggleFlagEdit ? false : true;

            gBFlagBuilder.Size = bToggleFlagEdit ? new Size(259, 449) : new Size(259, 162);
            gBFlagBuilder.Location = bToggleFlagEdit ? new Point(571, 40) : new Point(571, 327);
            
            gbRVR.Size = bToggleFlagEdit ? new Size(56, 105) : new Size(205, 105);
            gbRVR.Text = bToggleFlagEdit ? "RVR" : "RVR Settings";
            lbValue.Visible = bToggleFlagEdit ? false : true;
            lbGrade.Visible = bToggleFlagEdit ? false : true;

            pFlagBuilder.Size = bToggleFlagEdit ? new Size(243, 417) : new Size(243, 129);
            pFlagBuilder.Location = bToggleFlagEdit ? new Point(9, 19) : new Point(8, 23);

            clbFlagTest.Size = bToggleFlagEdit ? new Size(235, 345) : new Size(235, 90);
            clbFlagTest.Location = bToggleFlagEdit ? new Point(3, 5) : new Point(3, 5);

            clbFlagTest.Enabled = bToggleFlagEdit;

            tbFlag.Size = bToggleFlagEdit ? new Size(243, 30) : new Size();
            tbFlag.Location = bToggleFlagEdit ? new Point(-1, 356) : new Point();
            tbFlag.Visible = bToggleFlagEdit;

            btnEditFlag.Size = bToggleFlagEdit ? new Size(241, 29) : new Size(241, 29);
            btnEditFlag.Location = bToggleFlagEdit ? new Point(0, 386) : new Point(0, 98);
            btnEditFlag.Text = bToggleFlagEdit ? "SAVE" : "CHANGE";
            #endregion

            if (!bToggleFlagEdit)
            {
                gBFlagBuilder.Text = "Flag Builder - " + tbFlag.Text;

                int ID = getIDfromListBox();
                int num = cImport.ItemData.FindIndex((cItem p) => p.ID.Equals(ID));

                if (num != -1)
                {
                    cImport.ItemData[num].ItemFlag = Convert.ToUInt64(tbFlag.Text);
                    cImport.ItemData[num].changesWasHere = true;
                }

                tslbStatus.Text = "Flag has been succesufully changed";
            }
        }

        private void btnClass_Click(object sender, EventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "btnTitan":
                    MakeList("1");
                    break;
                case "btnKnight":
                    MakeList("2");
                    break;
                case "btnHealer":
                    MakeList("4");
                    break;
                case "btnMage":
                    MakeList("8");
                    break;
                case "btnRogue":
                    MakeList("16");
                    break;
                case "btnSorcerer":
                    MakeList("32");
                    break;
                case "btnNightShadow":
                    MakeList("64");
                    break;
                case "btnExRogue":
                    MakeList("128");
                    break;
                case "btnExMage":
                    MakeList("256");
                    break;
                case "btnP1":
                    MakeList("1024");
                    break;
                case "btnP2":
                    MakeList("2048");
                    break;
                default:
                    MakeList();
                    break;
            }

        }

        private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbItems.Location = cbSearchType.Text == "Type" 
                ? new Point(12, 212) : cbSearchType.Text == "Flag" 
                ? new Point(12, 190) : cbSearchType.Text == "JobFlag" 
                ? new Point(12, 186) : new Point(12, 134);

            lbItems.Size = cbSearchType.Text == "Type"
                ? new Size(350, 381) : cbSearchType.Text == "Flag"
                ? new Size(350, 407) : cbSearchType.Text == "JobFlag"
                ? new Size(350, 407) : new Size(350, 459);


            tbSearch.Size = cbSearchType.Text == "Description" 
                ? new Size(257, 20) : new Size(349, 20);


            bool isJobFlag = cbSearchType.Text == "JobFlag",
                 isType = cbSearchType.Text == "Type",
                 isFlag = cbSearchType.Text == "Flag",
                 isDescription = cbSearchType.Text == "Description";

            btnFind.Visible = isDescription ? true : false;
            btnFind.Location = isDescription ? new Point(275, 77) : new Point(1229, 218);

            gBType.Visible = isType ? true : false;
            gBType.Location = isType ? new Point(11, 131) : new Point(1229, 91);

            gBClassType.Visible = isJobFlag ? true : false;
            gBClassType.Location = isJobFlag ? new Point(12, 127) : new Point(1229, 159);

            gBFlag.Visible = isFlag ? true : false;
            gBFlag.Location = isFlag ? new Point(12, 129) : new Point(1229, 33);
        }

        private void cbSearchTypeFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSearchSubTypeFlag.Items.Clear();
            SetSubType(SUBTYPE.ITEM, cbSearchTypeFlag.SelectedIndex, cbSearchSubTypeFlag);
           // lbSearchTypeFlagValue.Text = cbSearchTypeFlag.SelectedIndex.ToString();
        }

        private void cbSearchSubTypeFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeList(cbSearchTypeFlag.SelectedIndex.ToString(), cbSearchSubTypeFlag.SelectedIndex.ToString());
            //lbSearchSubTypeFlagValue.Text = cbSearchSubTypeFlag.SelectedIndex.ToString();
        }

        private void tbSearchFlagValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                checkFlagInListBox(clbSearchFlag, Convert.ToUInt64(tbSearchFlagValue.Text));
                MakeList(tbSearchFlagValue.Text);
            }
            catch { } //
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            MakeList();
        }

        private void btnToggleFlagSelector_Click(object sender, EventArgs e)
        {
            bToggleFlagSelector = bToggleFlagSelector ? false : true;

            pFlagSelector.Size = bToggleFlagSelector ? new Size(338, 31) : new Size(338, 219);
            gBFlag.Size = bToggleFlagSelector ? new Size(350, 52) : new Size(350, 240);
            lbItems.Size = bToggleFlagSelector ? new Size(350, 407) : new Size(350, 225);
            lbItems.Location = bToggleFlagSelector ? new Point(12, 190) : new Point(12, 372);
            btnToggleFlagSelector.Image = bToggleFlagSelector ? Properties.Resources.icon_expand : Properties.Resources.icon_collapse;
        }

        private void btnSupport_Click(object sender, EventArgs e)
        {
            bToggleSupport = bToggleSupport ? false : true; 
            pSupport.Size = bToggleSupport ? new Size(664, 38) : new Size(43, 38);
            pSupport.Location = bToggleSupport ? new Point(552, 33) : new Point(1173, 33);
            pMenu.Size = bToggleSupport ? new Size(497, 38) : new Size(1111, 38);
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "btnMonero":
                    Clipboard.SetText("42NRbCTQfZc7jMgJMwoKnQg26j8mzbHBZPecWJu4zpmJ4t5mHHWdLLa8qBqWhJ3BBUC2VLZjX39ENafMss1TnvDvNsQVUPM");
                    tslbStatus.Text = "Monero (XMR) address has been copied to clipboard []";
                    break;
                case "btnBitcoin":
                    Clipboard.SetText("//");
                    tslbStatus.Text = "Bitcoin (BTC) address has been copied to clipboard []";
                    break;
                case "btnEthereum":
                    Clipboard.SetText("//");
                    tslbStatus.Text = "Ethereum (ETH) address has been copied to clipboard []";
                    break;
            }
        }

        private void effectHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEffect frmEffect = new frmEffect();
            frmEffect.Show();
        }

        private void clientLodFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings frmSettings = new frmSettings();
            frmSettings.Show();
        }

        private void bgwLoadData_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            cImport.GetItems();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            for (int index = 0; index < cImport.ItemData.Count(); ++index)
            {
                if (cImport.ItemData[index].changesWasHere)
                {
                    if (cExport.ItemUpdate(cImport.ItemData[index]))
                        cImport.ItemData[index].changesWasHere = false;

                    cnt++;
                }
            }

            tslbStatus.Text = string.Format("Succesufully updated {0} items to database!", cnt);
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbPrice_Click(object sender, EventArgs e)
        {
            tbPrice.Text = tbPrice.Text.Replace(".", "");
            tbPrice.SelectionStart = tbPrice.Text.Length;
        }

        private void tbPrice_Leave(object sender, EventArgs e)
        {
            try
            {
                tbPrice.Text = Convert.ToInt64(tbPrice.Text).ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
            }
            catch { } //
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
          //  object sCount = await cMySQL.QueryToObject("SELECT COUNT(*) FROM t_item");
           // tslbStatus.Text = sCount.ToString();
           // cImport.GetItems();
          //  MakeList();
        }

        private void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Filter = "JSON File (*.json)|*.json";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                cExport.ExportContent(cExport.Export.ITEM, sfd.FileName);

                tslbStatus.Text = "Items exported succesufully | Format: JSON";
                cLogger.MessageLog(string.Format("{0} Items have been exported to JSON format", lbItems.Items.Count), MESSAGE_TYPE.SUCCESS);
            }
        }

        private void btnOpenSMC_Click(object sender, EventArgs e)
        {
            frmSMC frmSMC = new frmSMC();
            frmSMC.SMCFile = tbSMC.Text;
            frmSMC.Show();
        }

        private void cbRVRValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbRVRGrade.Items.Clear();
            SetSubType(SUBTYPE.RVR, cbRVRValue.SelectedIndex, cbRVRGrade);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Render();
            Render1();
        }

        private void checkBox_turn_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRotate.Checked)
                timer1.Enabled = true;
            else
                timer1.Enabled = false;
        }

        private void trackBar_Zoom_Scroll(object sender, EventArgs e)
        {
            fZoom = trackBar_Zoom.Value / 100f;
            if (!cbRotate.Checked)
                Render();
        }

        private void slideZoom_Scroll(object sender, EventArgs e)
        {
            fRotation = trackBar_turn.Value / 500f;
            if (!cbRotate.Checked)
                Render();
        }

        private void trackBar_leftright_Scroll(object sender, EventArgs e)
        {
            fLeftnRight = trackBar_leftright.Value / 5500f;
            if (!cbRotate.Checked)
                Render();
        }

        private void trackBar_updown_Scroll(object sender, EventArgs e)
        {
            fUpnDown = trackBar_updown.Value / 5500f;
            if (!cbRotate.Checked)
                Render();
        }

        private async void pIcon_Click(object sender, EventArgs e)
        {
            frmIconPicker frmIconPicker = new frmIconPicker();
            if (frmIconPicker.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int ID = getIDfromListBox();
                    int num = cImport.ItemData.FindIndex((cItem p) => p.ID.Equals(ID));

                    tslbStatus.Text = "Icon has been succesufully changed";
                 //   cLogger.MessageLog(string.Format("(ID {0}) Icon Changed \t [{1}]:[{2}]:[{3}] -> [{4}]:[{5}]:[{6}]", getIDfromListBox(), 
                 //       cImport.ItemData[num].TexID, cImport.ItemData[num].TexCOL, cImport.ItemData[num].TexROW,
                 //       frmIconPicker.iIcon[0], frmIconPicker.iIcon[1], frmIconPicker.iIcon[2]), MESSAGE_TYPE.SUCCESS);

                    if (num != -1)
                    {
                        cImport.ItemData[num].TexID = Convert.ToInt32(frmIconPicker.iIcon[0]);
                        cImport.ItemData[num].TexCOL = Convert.ToInt32(frmIconPicker.iIcon[1]);
                        cImport.ItemData[num].TexROW = Convert.ToInt32(frmIconPicker.iIcon[2]);
                        cImport.ItemData[num].changesWasHere = true;
                    }

                    pIcon.Image = await cImport.GetIcon(IconType.ITEM, getIDfromListBox());

                }
                catch(Exception ex)
                {
                 //   tslbStatus.Text = "There was an error, check Log tab for more informations";
                 //   cLogger.MessageLog("Exception on pIcon_Click function, Message: " + ex.Message, MESSAGE_TYPE.EXCEPTION);
                }
            }
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnabled.Checked)
                cbEnabled.BackColor = Color.DarkSeaGreen;
            else
                cbEnabled.BackColor = Color.IndianRed;

            int id = getIDfromListBox();
            int index = cImport.ItemData.FindIndex(p => p.ID == id);

            if (index != -1)
                cImport.ItemData[index].Enable = !cbEnabled.Checked ? 0 : 1;
        }

        public void lbItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush brush = Brushes.Black;

            try
            {
                int.TryParse(lbItems.Items[e.Index].ToString().Split(' ')[0], out int id);
                int index = cImport.ItemData.FindIndex(p => p.ID == id);

                if (index != -1)
                {
                    if (cImport.ItemData[index].changesWasHere)
                        brush = Brushes.Orange;
                }
            }
            catch { }

            if (e.Index != -1)
                e.Graphics.DrawString(lbItems.Items[e.Index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        public enum Maps
        {
            OldJuno = 0,
            Juno = 0,
            Dratan = 4,
            Merac = 7,
            Egeha = 15,
            Strayana = 23,
            Mondshine = 32,
            Tarian = 39,
            Nariat = 40,
            Alber = 42
        }

        public float[] MapWidth = { 1536.0f, 480.0f, 768.0f, 504.0f, 3072.0f, 720.0f, 768.0f, 1536.0f, 100.0f, 504.0f, 768.0f, 516.0f, 1152.0f, 504.0f, 120.0f, 1536.0f, 768.0f, 600.0f, 600.0f, 516.0f, 516.0f, 1536.0f, 2400.0f, 1536.0f, 1536.0f, 1536.0f, 1024.0f, 1024.0f, 1024.0f, 720.0f, 768.0f, 1024.0f, 2560.0f, 1920.0f, 960.0f, 1536.0f, 1024.0f, 1024.0f, 256.0f, 2048.0f, 2048.0f, 2048.0f, 1536.0f };

        public async void printSpawnToMap(Bitmap bmp, int zone = -1)
        {
            if (zone == -1)
            {
                cbMap.Items.Clear();
                cbMap.Items.Add("All");
                cbMap.SelectedIndex = 0;
            }

            DataTable dt = await cImport.getDropsFromItem(getIDfromListBox());

            lvMonsters.Items.Clear();
            lvMonsters.FullRowSelect = true;

            float posx = 0.0f, posy = 0.0f;
            int zoneid = 0, npcid = 0;

            Graphics graphics = Graphics.FromImage(bmp);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                npcid = (int)row["a_index"];

                Color color = Color.FromKnownColor((KnownColor)(npcid % 174));

                ListViewItem item = new ListViewItem(npcid.ToString());
                item.SubItems.Add(row["a_name"].ToString());
                item.BackColor = color;
                item.SubItems.Add(row["a_level"].ToString());
                lvMonsters.Items.Add(item);


                DataTable dtSpawn = await cImport.getSpawnData(npcid);

                for (int j = 0; j < dtSpawn.Rows.Count; j++)
                {
                    DataRow drow = dtSpawn.Rows[j];

                    zoneid = (int)drow["a_zone_num"];
                    posx = (float)drow["a_pos_x"];
                    posy = (float)drow["a_pos_z"];
                    // var mapData = Enum.GetNames(typeof(Maps));

                    Maps eMaps = (Maps)zoneid;
                    if (zone == -1 && !cbMap.Items.Contains(zoneid + " " + eMaps) && Enum.IsDefined(typeof(Maps), zoneid))
                    {
                        if(zoneid == 0)
                            cbMap.Items.Add("0 OldJuno");
                        cbMap.Items.Add(zoneid + " " + eMaps);
                    }

                    float mw = (zoneid < 0 ? pbMap.Width : bmp.Width) / MapWidth[zoneid];
                    if (zoneid == zone || zone < 0)
                        graphics.FillRectangle(new SolidBrush(color), new RectangleF(posx * mw, posy * mw, 5, 5));
                }
            }

            pbMap.Image = bmp;
        }


        Brush RandomBrush()
        {
            cToolConfig cToolConfig = new cToolConfig();
            cTextureManagement cTextureManagement = new cTextureManagement();

            System.Reflection.PropertyInfo[] brushInfo = typeof(Brushes).GetProperties();
            Brush[] brushList = new Brush[brushInfo.Length];
            for (int i = 0; i < brushInfo.Length; i++)
            {
                brushList[i] = (Brush)brushInfo[i].GetValue(null, null);
            }
            Random randomNumber = new Random(DateTime.Now.Second);
            return brushList[randomNumber.Next(1, brushList.Length)];
        }

        private void mapchange(string zone)
        {
            Bitmap bmp = null;
            if (zone != "All" && zone != "-1")
            {
                if (cMySQL.configStructureTool[0].UseTextureFilesFromClient)
                {
                    cTextureManagement cTextureManagement = new cTextureManagement();
                    cTextureManagement.ReadFile(cMySQL.configStructureTool[0].ClientDirectory + @"\Data\Interface\Map_World" + zone + "0.tex");
                    bmp = new Bitmap(cTextureManagement.MakeBitmap());
                }
                else
                    bmp = new Bitmap(@"Images\Map_World" + zone + "0.png");
            }
            else
                bmp = new Bitmap(1024, 1024);

            printSpawnToMap(bmp, (zone == "All" ? -2 : zone == "" ? 0 : Convert.ToInt32(zone)));

        }

        private async void btnLoadDrop_Click(object sender, EventArgs e)
        {
            mapchange("-1");
        }

        private void cbMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            mapchange(cbMap.SelectedItem.ToString() == "0 OldJuno" ? "" : cbMap.SelectedItem.ToString().Split(' ')[0]);
        }

        private void btnRareSearch0_Click(object sender, EventArgs e)
        {
            frmRarePicker frmRarePicker = new frmRarePicker();
            frmRarePicker.Show();
        }

        private async void btnPickRare_Click(object sender, EventArgs e)
        {
            frmRarePicker frmRarePicker = new frmRarePicker();
            if (frmRarePicker.ShowDialog() == DialogResult.OK)
            {
                var btn = (Button)sender;
                    var rareIdControl = btn.Name.Split(new[] { "btnPickRare" }, StringSplitOptions.None)[1];
                    var rareName = await cImport.getRarePrefix(frmRarePicker.RAREID);

                TextBox tbID = (TextBox)Controls.Find("tbRareID" + rareIdControl, true)[0];
                TextBox tbNAME = (TextBox)Controls.Find("tbRareName" + rareIdControl, true)[0];

                tbNAME.Text = rareName.ToString();
                tbID.Text = frmRarePicker.RAREID.ToString();
            }
        }

        private void btnModelInfo_Click(object sender, EventArgs e)
        {
            frmModelInfo frmModelInfo = new frmModelInfo();
            frmModelInfo.Show();
        }

        private void btnDeleteSelectedNPC_Click(object sender, EventArgs e)
        {
            if (lvMonsters.SelectedItems.Count != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure? This action cannot be undone as it goes straight to Database", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
               //     tslbStatus.Text = lvMonsters.SelectedItems[0].Text;
              //      Configuration.cMySQL.ExecuteQuery(string.Format("itemid delete row query here = '{0}';", lvMonsters.SelectedItems[0].Text));
                }                
            }
        }

        private void btnDeleteFromAllNPC_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure? This action cannot be undone as it goes straight to Database", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                //     tslbStatus.Text = lvMonsters.SelectedItems[0].Text;
                //      Configuration.cMySQL.ExecuteQuery(string.Format("itemid delete row query here = '{0}';", lvMonsters.SelectedItems[0].Text));
            }
        }

        private void cbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            cImport.CurrentLanguage = (Language)cbLang.SelectedIndex;

            if (cImport.ItemData == null)
                return;

            MakeList();
        }

        private void tbFlag_TextChanged(object sender, EventArgs e)
        {
            try { checkFlagInListBox(clbFlagTest, Convert.ToUInt64(tbFlag.Text)); } catch { };
        }

        private void tsDatabaseImport_Click(object sender, EventArgs e)
        {

        }

        private void pHPToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void UpdateItemToDB()
        {
            int ID = getIDfromListBox();
            cItem cItem = cImport.ItemData.Find(p => p.ID.Equals(ID));
            if (cItem != null)
                cExport.ItemUpdate(cItem);    
        }

        private void btnUpdateRecord_Click(object sender, EventArgs e)
        {
            SaveItem();
            UpdateItemToDB();
        }

        //
    }
}

