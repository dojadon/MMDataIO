using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsMmdDataIO.Vmd;

namespace CsMmdDataIO.Vmd
{
    public class VmdParser : ParserBase
    {
        public override Encoding CharEncording => Encoding.GetEncoding("Shift_JIS");

        public VmdParser(Stream stream) : base(stream)
        {

        }

        public void Parse(VmdMotionData data)
        {
            data.Parse(this);
        }

        public static VmdMotionData ParserMotionData(Stream stream)
        {
            var parser = new VmdParser(stream);
            var data = new VmdMotionData();
            parser.Parse(data);
            return data;
        }
    }
}
