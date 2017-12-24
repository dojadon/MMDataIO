using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Pmx
{
    public interface IPmxData : ICloneable
    {
        void Export(PmxExporter exporter);
        void Parse(PmxParser parser);
    }
}
