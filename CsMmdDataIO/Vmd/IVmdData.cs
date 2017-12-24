using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd
{
    internal interface IVmdData
    {
        void Export(VmdExporter exporter);
    }
}
