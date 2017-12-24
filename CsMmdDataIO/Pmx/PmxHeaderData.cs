using System;
using System.Text;
using System.IO;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxHeaderData : IPmxData
    {
        private static readonly byte[] MAGIC_BYTES = { 0x50, 0x4d, 0x58, 0x20 };// PMX
        protected const string CR = "\r"; // 0x0d
        protected const string LF = "\n"; // 0x0a
        protected const string CRLF = CR + LF; // 0x0d, 0x0a

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public byte NumberOfExtraUv { get; set; }
        public byte VertexIndexSize { get; set; }
        public byte TextureIndexSize { get; set; }
        public byte MaterialIndexSize { get; set; }
        public byte BoneIndexSize { get; set; }
        public byte MorphIndexSize { get; set; }
        public byte RigidIndexSize { get; set; }

        public float Version { get; set; }
        public string ModelName { get; set; } = "";
        public string ModelNameE { get; set; } = "";
        public string Description { get; set; } = "";
        public string DescriptionE { get; set; } = "";

        public object Clone() => new PmxHeaderData()
        {
            Version = Version,
            ModelName = ModelName,
            ModelNameE = ModelNameE,
            Description = Description,
            DescriptionE = DescriptionE,
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.Write(MAGIC_BYTES);
            writer.Write(Version);

            writer.Write((byte)8);//length of byte array
            writer.Write((byte)(Encoding == Encoding.UTF8 ? 1 : 0));
            writer.Write(NumberOfExtraUv);
            writer.Write(VertexIndexSize);
            writer.Write(TextureIndexSize);
            writer.Write(MaterialIndexSize);
            writer.Write(BoneIndexSize);
            writer.Write(MorphIndexSize);
            writer.Write(RigidIndexSize);

            writer.WriteText(Encoding, ModelName);
            writer.WriteText(Encoding, ModelNameE);

            writer.WriteText(Encoding, Description.Replace(LF, CRLF));
            writer.WriteText(Encoding, DescriptionE.Replace(LF, CRLF));
        }

        public void Parse(BinaryReader reader, PmxHeaderData header)
        {
            reader.ReadBytes(MAGIC_BYTES.Length);
            Version = reader.ReadSingle();

            reader.ReadByte(); //length of byte array
            Encoding = reader.ReadByte() == 1 ? Encoding.UTF8 : Encoding.GetEncoding("utf-16");
            NumberOfExtraUv = reader.ReadByte();
            VertexIndexSize = reader.ReadByte();
            TextureIndexSize = reader.ReadByte();
            MaterialIndexSize = reader.ReadByte();
            BoneIndexSize = reader.ReadByte();
            MorphIndexSize = reader.ReadByte();
            RigidIndexSize = reader.ReadByte();

            ModelName = reader.ReadText(Encoding);
            ModelNameE = reader.ReadText(Encoding);

            Description = reader.ReadText(Encoding);
            DescriptionE = reader.ReadText(Encoding);
        }
    }
}
