﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KirbyAirRideTools
{
    class ParseDAT
    {
        public static long getOffsetFromNameRegex(BinaryReader dat, Regex regex)
        {
            //Keep Offset
            long saveOffset = dat.BaseStream.Position;

            //Load Header data
            dat.BaseStream.Seek(4, SeekOrigin.Begin);
            UInt32 hdrsize = 0x20;
            UInt32 bodysize = BitConverter.ToUInt32(dat.ReadBytes(4).Reverse(), 0);
            UInt32 reloc_num = BitConverter.ToUInt32(dat.ReadBytes(4).Reverse(), 0);
            UInt32 rootnode_num = BitConverter.ToUInt32(dat.ReadBytes(4).Reverse(), 0);
            UInt32 xrefnode_num = BitConverter.ToUInt32(dat.ReadBytes(4).Reverse(), 0);

            //Read data
            dat.BaseStream.Seek(hdrsize + bodysize + (reloc_num * 4), SeekOrigin.Begin);
            
            for (int i = 0; i < (rootnode_num + xrefnode_num); i++)
            {
                long tempOffset = dat.BaseStream.Position;
                UInt32 offset = BitConverter.ToUInt32(dat.ReadBytes(4).Reverse(), 0);
                UInt32 stringoff = BitConverter.ToUInt32(dat.ReadBytes(4).Reverse(), 0);
                dat.BaseStream.Seek(hdrsize + bodysize + (reloc_num * 4) + ((rootnode_num + xrefnode_num) * 8) + stringoff, SeekOrigin.Begin);
                int j = 0;
                for (; dat.ReadByte() != 0; j++);

                dat.BaseStream.Seek(hdrsize + bodysize + (reloc_num * 4) + ((rootnode_num + xrefnode_num) * 8) + stringoff, SeekOrigin.Begin);
                string name = Encoding.ASCII.GetString(dat.ReadBytes(j));

                if (regex.IsMatch(name))
                {
                    dat.BaseStream.Seek(saveOffset, SeekOrigin.Begin);
                    return (offset + hdrsize);
                }

                dat.BaseStream.Seek(tempOffset + 8, SeekOrigin.Begin);
            }

            return -1;
        }
    }
}
