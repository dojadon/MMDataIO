using System;
using System.Text;
using System.IO;
using VecMath;
using CsMmdDataIO.Pmx.Data;

namespace CsMmdDataIO.Pmx
{
    public class PmxExporter : ExporterBase
    {
        public override Encoding CharEncording { get; } = Encoding.GetEncoding("utf-16");

        public const byte SIZE_VERTEX = 4;
        public const byte SIZE_TEXTURE = 2;
        public const byte SIZE_MATERIAL = 4;
        public const byte SIZE_BONE = 4;
        public const byte SIZE_MORPH = 4;
        public const byte SIZE_RIGID = 2;

        public static readonly byte[] SIZE = { 0, 0, SIZE_VERTEX, SIZE_TEXTURE, SIZE_MATERIAL, SIZE_BONE, SIZE_MORPH, SIZE_RIGID };

        public PmxExporter(Stream OutStream) : base(OutStream)
        {
        }

        public void Export(PmxModelData data)
        {
            data.Export(this);
        }

        public PmxExporter WritePmxId(byte size, int id)
        {
            switch (size)
            {
                case 1:
                    Write((byte)id);
                    break;

                case 2:
                    Write((short)id);
                    break;

                case 4:
                    Write(id);
                    break;
            }
            return this;
        }
    }
}
