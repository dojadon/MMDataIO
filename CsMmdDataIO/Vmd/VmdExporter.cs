using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VecMath;
using CsMmdDataIO.Vmd;

namespace CsMmdDataIO.Vmd
{
    public class VmdExporter : ExporterBase
    {
        public override Encoding CharEncording { get; } =  Encoding.GetEncoding("Shift_JIS");

        public const int BONE_NAME_LENGTH = 15;
        public const int MORPH_NAME_LENGTH = 15;
        public const int MODEL_NAME_LENGTH = 20;
        public const int HEADER_LENGTH = 30;

        public VmdExporter(Stream outStream) : base(outStream)
        {

        }

        public void Export(VmdMotionData data)
        {
            data.Export(this);
        }

        public static void Export(VmdMotionData data, Stream stream)
        {
            new VmdExporter(stream).Export(data);
        }
    }
}
