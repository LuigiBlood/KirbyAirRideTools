using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml.Linq;

namespace KirbyAirRideTools
{
    class Export
    {
        public static long OBJvtx_i;

        public static void initOBJexport(TextWriter fileOut)
        {
            OBJvtx_i = 0;

            fileOut.WriteLine("#\n# Converted with KirbyAirRideTools\n#");
        }

        public static int exportCollisionOBJ(TextWriter fileOut, BinaryReader fileIn)
        {
            long offset = ParseDAT.getOffsetFromNameRegex(fileIn, new Regex("^grData"));
            if (offset == -1)
            {
                //Error
                return -1;
            }

            UInt32 hdrsize = 0x20;
            fileIn.BaseStream.Seek(offset + 0x18, SeekOrigin.Begin);

            long CollisionOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            fileIn.BaseStream.Seek(CollisionOffset, SeekOrigin.Begin);

            long VtxOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 VtxNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
            long TriOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 TriNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
            long MdlOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 MdlNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);

            //Vertex List
            List<float> VtxXList = new List<float>();
            List<float> VtxYList = new List<float>();
            List<float> VtxZList = new List<float>();

            fileIn.BaseStream.Seek(VtxOffset, SeekOrigin.Begin);
            for (int i = 0; i < VtxNum; i++)
            {
                VtxXList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
                VtxYList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
                VtxZList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
            }

            //Triangle List
            List<UInt32> Tri1List = new List<UInt32>();
            List<UInt32> Tri2List = new List<UInt32>();
            List<UInt32> Tri3List = new List<UInt32>();

            fileIn.BaseStream.Seek(TriOffset, SeekOrigin.Begin);
            for (int i = 0; i < TriNum; i++)
            {
                Tri1List.Add(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0));
                Tri2List.Add(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0));
                Tri3List.Add(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0));
                fileIn.ReadUInt32(); //Color
                fileIn.ReadUInt32(); //Unk
            }

            //Make OBJ
            //fileOut.WriteLine("o CollisionData");
            //fileOut.WriteLine("\n# Model Vertex Data\n");

            int vtx_i = 0;
            for (; vtx_i < VtxNum; vtx_i++)
            {
                //fileOut.WriteLine("v " + VtxXList[vtx_i] + " " + VtxYList[vtx_i] + " " + VtxZList[vtx_i]);
            }

            fileIn.BaseStream.Seek(MdlOffset, SeekOrigin.Begin);
            for (int i = 0; i < MdlNum; i++)
            {
                int Unk0 = BitConverter.ToInt32(fileIn.ReadBytes(4).Reverse(), 0); //?
                int VtxIDMin = BitConverter.ToInt32(fileIn.ReadBytes(4).Reverse(), 0);
                int VtxIDSize = BitConverter.ToInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 TriIDMin = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 TriIDSize = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 Unk1 = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 Unk2 = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);

                //Managing Models
                fileOut.WriteLine("\n# Model Data " + i + " / Bone " + Unk0 + " / Vtx " + VtxIDMin + "-" + VtxIDSize + " / Tri " + TriIDMin + "-" + TriIDSize + " / Unk1 " + Unk1 + " / Unk2 " + Unk2);
                fileOut.WriteLine("o Model" + i);
                for (int j = VtxIDMin; j < (VtxIDMin + VtxIDSize); j++)
                {
                    //fileOut.WriteLine("v " + (VtxXList[j] + VtxXList[Unk0]).ToString("F6") + " " + (VtxYList[j] + VtxYList[Unk0]).ToString("F6") + " " + (VtxZList[j] + VtxZList[Unk0]).ToString("F6"));
                    fileOut.WriteLine("v " + VtxXList[j].ToString("F6") + " " + VtxYList[j].ToString("F6") + " " + VtxZList[j].ToString("F6"));
                }

                for (UInt32 j = TriIDMin; j < (TriIDMin + TriIDSize); j++)
                {
                    //fileOut.WriteLine("f " + (Tri1List[(int)j] + OBJvtx_i + 1) + "// " + (Tri2List[(int)j] + OBJvtx_i + 1) + "// " + (Tri3List[(int)j] + OBJvtx_i + 1) + "//");
                    fileOut.WriteLine("f " + (Tri1List[(int)j] - VtxIDMin - VtxIDSize) + "// " + (Tri2List[(int)j] - VtxIDMin - VtxIDSize) + "// " + (Tri3List[(int)j] - VtxIDMin - VtxIDSize) + "//");
                }
            }

            OBJvtx_i += VtxNum;

            return 0;
        }

        public static int exportPathOBJ(TextWriter fileOut, BinaryReader fileIn)
        {
            long offset = ParseDAT.getOffsetFromNameRegex(fileIn, new Regex("^grData"));
            if (offset == -1)
            {
                //Error
                return -1;
            }

            UInt32 hdrsize = 0x20;
            fileIn.BaseStream.Seek(offset + 0x18, SeekOrigin.Begin);

            //Path Data
            fileIn.BaseStream.Seek(offset + 0x1C, SeekOrigin.Begin);
            long PathOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            fileIn.BaseStream.Seek(PathOffset, SeekOrigin.Begin);

            long MainPathOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            long AIPathOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            fileIn.ReadUInt32();
            fileIn.ReadUInt32();
            fileIn.ReadUInt32();
            fileIn.ReadUInt32();
            long RailPathOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;

            //Main Path
            fileIn.BaseStream.Seek(MainPathOffset, SeekOrigin.Begin);
            fileIn.BaseStream.Seek(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize, SeekOrigin.Begin);

            MainPathOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 MainPathNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;

            fileIn.BaseStream.Seek(MainPathOffset, SeekOrigin.Begin);
            long MainPathOffset2 = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            fileIn.BaseStream.Seek(MainPathOffset2, SeekOrigin.Begin);

            UInt32 VtxNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
            fileIn.ReadUInt32();
            fileIn.BaseStream.Seek(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize, SeekOrigin.Begin);
            List<float> VtxXList = new List<float>();
            List<float> VtxYList = new List<float>();
            List<float> VtxZList = new List<float>();
            for (int i = 0; i < VtxNum; i++)
            {
                VtxXList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
                VtxYList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
                VtxZList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
            }

            fileOut.WriteLine("o MainPathData");

            for (int i = 0; i < VtxNum; i++)
            {
                fileOut.WriteLine("v " + VtxXList[i] + " " + VtxYList[i] + " " + VtxZList[i]);
            }

            for (int i = 0; i < (VtxNum - 1); i++)
            {
                fileOut.WriteLine("l " + (OBJvtx_i + i + 1) + "/ " + (OBJvtx_i + i + 2) + "/");
            }
            OBJvtx_i += (int)VtxNum;

            return 0;
        }

        public static int exportPartitionOBJ(TextWriter fileOut, BinaryReader fileIn)
        {
            long offset = ParseDAT.getOffsetFromNameRegex(fileIn, new Regex("^grData"));
            if (offset == -1)
            {
                //Error
                return -1;
            }

            UInt32 hdrsize = 0x20;
            fileIn.BaseStream.Seek(offset + 0x48, SeekOrigin.Begin);
            fileIn.BaseStream.Seek((BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize), SeekOrigin.Begin);
            fileIn.BaseStream.Seek((BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize), SeekOrigin.Begin);

            //Partition Data?
            long PartitionOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            long PartitionNum = BitConverter.ToUInt16(fileIn.ReadBytes(2).Reverse(), 0);
            //fileOut.WriteLine("\no PartitionData");
            for (int i = 0; i < PartitionNum; i++)
            {
                fileIn.BaseStream.Seek(PartitionOffset + (i * 4), SeekOrigin.Begin);
                fileIn.BaseStream.Seek((BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize), SeekOrigin.Begin);

                fileOut.WriteLine("o Partition" + i.ToString("D4"));

                float MinX = BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0);
                float MinY = BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0);
                float MinZ = BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0);
                float MaxX = BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0);
                float MaxY = BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0);
                float MaxZ = BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0);

                fileOut.WriteLine("v " + MinX.ToString("F6") + " " + MinY.ToString("F6") + " " + MinZ.ToString("F6")); //0,0,0
                fileOut.WriteLine("v " + MaxX.ToString("F6") + " " + MinY.ToString("F6") + " " + MinZ.ToString("F6")); //1,0,0
                fileOut.WriteLine("v " + MinX.ToString("F6") + " " + MaxY.ToString("F6") + " " + MinZ.ToString("F6")); //0,1,0
                fileOut.WriteLine("v " + MaxX.ToString("F6") + " " + MaxY.ToString("F6") + " " + MinZ.ToString("F6")); //1,1,0
                fileOut.WriteLine("v " + MinX.ToString("F6") + " " + MinY.ToString("F6") + " " + MaxZ.ToString("F6")); //0,0,1
                fileOut.WriteLine("v " + MaxX.ToString("F6") + " " + MinY.ToString("F6") + " " + MaxZ.ToString("F6")); //1,0,1
                fileOut.WriteLine("v " + MinX.ToString("F6") + " " + MaxY.ToString("F6") + " " + MaxZ.ToString("F6")); //0,1,1
                fileOut.WriteLine("v " + MaxX.ToString("F6") + " " + MaxY.ToString("F6") + " " + MaxZ.ToString("F6")); //1,1,1


                fileOut.WriteLine("l -8/ -7/");
                fileOut.WriteLine("l -8/ -6/");
                fileOut.WriteLine("l -7/ -5/");
                fileOut.WriteLine("l -6/ -5/");

                fileOut.WriteLine("l -4/ -3/");
                fileOut.WriteLine("l -4/ -2/");
                fileOut.WriteLine("l -3/ -1/");
                fileOut.WriteLine("l -2/ -1/");

                fileOut.WriteLine("l -8/ -4/");
                fileOut.WriteLine("l -7/ -3/");
                fileOut.WriteLine("l -6/ -2/");
                fileOut.WriteLine("l -5/ -1/");
            }


            OBJvtx_i += PartitionNum * 8;

            return 0;
        }

        //DAE
        public static void initDAEexport(XmlWriter xmlOut)
        {
            xmlOut.WriteStartDocument();

            xmlOut.WriteStartElement("COLLADA", @"http://www.collada.org/2005/11/COLLADASchema");
            xmlOut.WriteAttributeString("version", "1.4.1");

            //asset
            xmlOut.WriteStartElement("asset");

            xmlOut.WriteStartElement("contributor");
            xmlOut.WriteStartElement("author");
            xmlOut.WriteString("HAL Laboratory");
            xmlOut.WriteEndElement();
            xmlOut.WriteStartElement("authoring_tool");
            xmlOut.WriteString("KirbyAirRideTools");
            xmlOut.WriteEndElement();
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("created");
            xmlOut.WriteString(DateTime.Now.ToString("O"));
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("modified");
            xmlOut.WriteString(DateTime.Now.ToString("O"));
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("unit");
            xmlOut.WriteAttributeString("name", "meter");
            xmlOut.WriteAttributeString("meter", "1");
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("up_axis");
            xmlOut.WriteString("Y_UP");
            xmlOut.WriteEndElement();

            xmlOut.WriteEndElement();
        }

        public static void endDAEexport(XmlWriter xmlOut)
        {
            xmlOut.WriteEndElement();
            xmlOut.WriteEndDocument();
            xmlOut.Close();
        }

        public static int DAEexport(XmlWriter xmlOut, BinaryReader fileIn)
        {
            long grData_offset = ParseDAT.getOffsetFromNameRegex(fileIn, new Regex("^grData"));
            if (grData_offset == -1)
            {
                //Error
                return -1;
            }

            XNamespace XmlColladaNS = @"http://www.collada.org/2005/11/COLLADASchema";
            XElement colladaNode = new XElement(XmlColladaNS + "COLLADA");
            colladaNode.Add(new XAttribute("version", "1.4.1"));

            XElement asset_node = new XElement("asset");
            XElement contributor_node = new XElement("contributor",
                new XElement("author",
                    new XText("HAL Laboratory")),
                new XElement("authoring_tool",
                    new XText("KirbyAirRideTools"))
                );
            asset_node.Add(contributor_node,
                new XElement("created", new XText(DateTime.Now.ToString("O"))),
                new XElement("modified", new XText(DateTime.Now.ToString("O"))),
                new XElement("unit", new XAttribute("name", "meter"), new XAttribute("meter", "1")),
                new XElement("up_axis", new XText("Y_UP"))
                );

            XElement library_cameras_node = new XElement("library_cameras");
            XElement library_lights_node = new XElement("library_lights");
            XElement library_images_node = new XElement("library_images");
            XElement library_effects_node = new XElement("library_effects");
            XElement library_materials_node = new XElement("library_materials");
            XElement library_geometries_node = new XElement("library_geometries");
            XElement library_visual_scenes_node = new XElement("library_visual_scenes");

            XElement visual_scene = new XElement("visual_scene", new XAttribute("id", "Scene"), new XAttribute("name", "Scene"));
            XElement scene_node = new XElement("scene", new XElement("instance_visual_scene", new XAttribute("url", "#Scene")));

            UInt32 hdrsize = 0x20;
            //Collision Node
            fileIn.BaseStream.Seek(grData_offset + 0x18, SeekOrigin.Begin);

            long CollisionOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            fileIn.BaseStream.Seek(CollisionOffset, SeekOrigin.Begin);

            long VtxOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 VtxNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
            long TriOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 TriNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
            long MdlOffset = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0) + hdrsize;
            UInt32 MdlNum = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);

            //Vertex List
            List<float> VtxXList = new List<float>();
            List<float> VtxYList = new List<float>();
            List<float> VtxZList = new List<float>();

            fileIn.BaseStream.Seek(VtxOffset, SeekOrigin.Begin);
            for (int i = 0; i < VtxNum; i++)
            {
                VtxXList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
                VtxYList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
                VtxZList.Add(BitConverter.ToSingle(fileIn.ReadBytes(4).Reverse(), 0));
            }

            //Triangle List
            List<UInt32> Tri1List = new List<UInt32>();
            List<UInt32> Tri2List = new List<UInt32>();
            List<UInt32> Tri3List = new List<UInt32>();

            fileIn.BaseStream.Seek(TriOffset, SeekOrigin.Begin);
            for (int i = 0; i < TriNum; i++)
            {
                Tri1List.Add(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0));
                Tri2List.Add(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0));
                Tri3List.Add(BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0));
                fileIn.ReadUInt32(); //Color
                fileIn.ReadUInt32(); //Unk
            }

            fileIn.BaseStream.Seek(MdlOffset, SeekOrigin.Begin);
            XElement collision_scene_node = new XElement("node", new XAttribute("id", "Collision"), new XAttribute("name", "Collision Data"), new XAttribute("type", "NODE"));
            for (int i = 0; i < MdlNum; i++)
            {
                int JointID = BitConverter.ToInt32(fileIn.ReadBytes(4).Reverse(), 0);
                int VtxIDMin = BitConverter.ToInt32(fileIn.ReadBytes(4).Reverse(), 0);
                int VtxIDSize = BitConverter.ToInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 TriIDMin = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 TriIDSize = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 Unk1 = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);
                UInt32 Unk2 = BitConverter.ToUInt32(fileIn.ReadBytes(4).Reverse(), 0);

                //--Geometry Data
                XElement geometry_node = new XElement("geometry",
                    new XAttribute("id", "CollMesh" + i.ToString()),
                    new XAttribute("name", "CollisionMesh" + i.ToString()));
                XElement mesh_node = new XElement("mesh");

                XElement source_node = new XElement("source",
                    new XAttribute("id", "CollMesh" + i.ToString() + "-positions"));

                XElement float_array_node = new XElement("float_array",
                    new XAttribute("id", "CollMesh" + i.ToString() + "-positions-array"),
                    new XAttribute("count", (VtxIDSize * 3).ToString()));

                string float_array_data = "";
                for (int j = VtxIDMin; j < (VtxIDMin + VtxIDSize); j++)
                {
                    float_array_data += (VtxXList[j].ToString("F6", CultureInfo.InvariantCulture) + " " + VtxYList[j].ToString("F6", CultureInfo.InvariantCulture) + " " + VtxZList[j].ToString("F6", CultureInfo.InvariantCulture) + " ");
                }
                float_array_node.Add(new XText((float_array_data)));

                XElement technique_common_node = new XElement("technique_common",
                    new XElement("accessor",
                    new XAttribute("source", "#CollMesh" + i.ToString() + "-positions-array"),
                    new XAttribute("count", VtxIDSize.ToString()),
                    new XAttribute("stride", "3"),
                            new XElement("param",
                                new XAttribute("name", "X"),
                                new XAttribute("type", "float")
                            ),
                            new XElement("param",
                                new XAttribute("name", "Y"),
                                new XAttribute("type", "float")
                            ),
                            new XElement("param",
                                new XAttribute("name", "Z"),
                                new XAttribute("type", "float")
                            )
                    )
                );

                source_node.Add(float_array_node, technique_common_node);

                XElement vertices_node = new XElement("vertices",
                    new XAttribute("id", "CollMesh" + i.ToString() + "-vertices"),
                        new XElement("input",
                            new XAttribute("semantic", "POSITION"),
                            new XAttribute("source", "#CollMesh" + i.ToString() + "-positions")
                        )
                );

                XElement polylist_node = new XElement("polylist",
                    new XAttribute("count", TriIDSize.ToString()),
                        new XElement("input",
                            new XAttribute("semantic", "VERTEX"),
                            new XAttribute("source", "#CollMesh" + i.ToString() + "-vertices"),
                            new XAttribute("offset", "0")
                        )
                );

                XElement vcount_node = new XElement("vcount");
                string vcount = "";
                for (UInt32 j = TriIDMin; j < (TriIDMin + TriIDSize); j++)
                {
                    vcount += "3 ";
                }
                vcount_node.Add(new XText(vcount));

                XElement p_node = new XElement("p");
                string p = "";
                for (UInt32 j = TriIDMin; j < (TriIDMin + TriIDSize); j++)
                {
                    p += ((Tri1List[(int)j] - VtxIDMin) + " " + (Tri2List[(int)j] - VtxIDMin) + " " + (Tri3List[(int)j] - VtxIDMin) + " ");
                }
                p_node.Add(new XText(p));

                polylist_node.Add(vcount_node, p_node);
                mesh_node.Add(source_node, vertices_node, polylist_node);
                geometry_node.Add(mesh_node);

                //Add Geometry Data
                library_geometries_node.Add(geometry_node);

                //--Scene Data
                XElement scene_model_node = new XElement("node",
                    new XAttribute("id", "CollModel" + i.ToString()),
                    new XAttribute("name", "CollisionModel" + i.ToString()),
                    new XAttribute("type", "NODE"),
                        new XElement("matrix", 
                        new XAttribute("sid", "transform"),
                        new XText("1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1")
                        ),
                        new XElement("instance_geometry",
                        new XAttribute("url", "#CollMesh" + i.ToString()),
                        new XAttribute("name", "CollisionMesh" + i.ToString())
                        )
                );

                collision_scene_node.Add(scene_model_node);
            }

            visual_scene.Add(collision_scene_node);

            library_visual_scenes_node.Add(visual_scene);

            colladaNode.Add(asset_node, library_geometries_node, library_visual_scenes_node, scene_node);

            colladaNode.WriteTo(xmlOut);
            xmlOut.Close();
            xmlOut.Dispose();
            return 0;
        }
    }
}
