using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CsMmdDataIO.Pmx
{
    public interface IPmxData : ICloneable
    {
        void Write(BinaryWriter writer, PmxHeaderData header);
        void Parse(BinaryReader reader, PmxHeaderData header);
    }
}
