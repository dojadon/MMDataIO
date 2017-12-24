using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CsMmdDataIO.Pmx;
using VecMath;

namespace CsMmdDataIO.Pmx
{
    public class PmxParser : ParserBase
    {
        public byte SizeVertex => Size[2];
        public byte SizeTexture => Size[3];
        public byte SizeMaterial => Size[4];
        public byte SizeBone => Size[5];
        public byte SizeMorph => Size[6];
        public byte SizeRigid => Size[7];

        public override Encoding CharEncording => Size[0] == 0 ? Encoding.Unicode : Encoding.UTF8;

        public byte[] Size { get; set; }

        public PmxParser(Stream outStream) : base(outStream)
        {

        }

        public void Parse(PmxModelData data)
        {
            data.Parse(this);
        }

        public int ReadPmxId(byte size)
        {
            int id = 0;

            switch (size)
            {
                case 1:
                    id = ReadByte();
                    break;

                case 2:
                    id = ReadInt16();
                    break;

                case 4:
                    id = ReadInt32();
                    break;
            }
            return id;
        }
    }
}
