using System;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxHeaderData : IPmxData
    {
        private static readonly byte[] MAGIC_BYTES = { 0x50, 0x4d, 0x58, 0x20 };// PMX
        private static readonly byte[] SIZE_BYTES = { 0, 0, 4, 2, 4, 4, 2, 2 };
        protected const string CR = "\r"; // 0x0d
        protected const string LF = "\n"; // 0x0a
        protected const string CRLF = CR + LF; // 0x0d, 0x0a

        public byte[] Size { get; set; }

        public float Version { get; set; }
        public string ModelName { get; set; } = "";
        public string ModelNameE { get; set; } = "";
        public string Description { get; set; } = "";
        public string DescriptionE { get; set; } = "";
        public int Uv { get; set; }

        public int Encode { get; set; }

        public object Clone() => new PmxHeaderData()
        {
            Size = Size,
            Version = Version,
            ModelName = ModelName,
            ModelNameE = ModelNameE,
            Description = Description,
            DescriptionE = DescriptionE,
            Uv = Uv,
            Encode = Encode,
        };

        public void Export(PmxExporter exporter)
        {
            exporter.Write(MAGIC_BYTES);

            exporter.Write(Version);

            exporter.Write((byte)PmxExporter.SIZE.Length);
            exporter.Write(PmxExporter.SIZE);

            exporter.WriteText(ModelName);
            exporter.WriteText(ModelNameE);

            exporter.WriteText(Description.Replace(LF, CRLF));
            exporter.WriteText(DescriptionE.Replace(LF, CRLF));
        }

        public void Parse(PmxParser parser)
        {
            byte[] magic = parser.ReadBytes(MAGIC_BYTES.Length);

            Version = parser.ReadSingle();
            byte sizeLen = parser.ReadByte();
            Size = parser.ReadBytes(sizeLen);
            parser.Size = Size;

            ModelName = parser.ReadText();
            ModelNameE = parser.ReadText();

            Description = parser.ReadText();
            DescriptionE = parser.ReadText();
        }
    }
}
