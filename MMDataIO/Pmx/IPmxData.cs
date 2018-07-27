using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MMDataIO.Pmx
{
    public interface IPmxData : ICloneable
    {
        void Write(BinaryWriter writer, PmxHeaderData header);
        void Read(BinaryReader reader, PmxHeaderData header);
        void ReadPmd(BinaryReader reader, PmxHeaderData header);
    }
}
