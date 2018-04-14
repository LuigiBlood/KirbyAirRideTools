using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            fileOut.WriteLine("o CollisionData");
            fileOut.WriteLine("\n# Model Vertex Data\n");

            int vtx_i = 0;
            for (; vtx_i < VtxNum; vtx_i++)
            {
                fileOut.WriteLine("v " + VtxXList[vtx_i] + " " + VtxYList[vtx_i] + " " + VtxZList[vtx_i]);
            }

            fileOut.WriteLine("\n# Model Triangle Data\n");

            for (int i = 0; i < TriNum; i++)
            {
                fileOut.WriteLine("f " + (Tri1List[i] + 1) + "// " + (Tri2List[i] + 1) + "// " + (Tri3List[i] + 1) + "//");
            }

            OBJvtx_i += vtx_i;

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
    }
}
