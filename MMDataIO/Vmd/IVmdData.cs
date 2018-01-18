using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMDataIO.Vmd
{
    internal interface IVmdData
    {
        void Write(BinaryWriter writer);
    }
}
