using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
{
    [Serializable]
    public class PmxMaterialData : IPmxData
    {
        public string MaterialName { get; set; } = "";
        public string MaterialNameE { get; set; } = "";

        public string Script { get; set; } = "";

        public byte Flag { get; set; }

        public Vector4 Edge { get; set; } = new Vector4();
        public float EdgeThick { get; set; }

        public Vector4 Diffuse { get; set; } = new Vector4();
        public Vector3 Specular { get; set; } = new Vector3();
        public Vector3 Ambient { get; set; } = new Vector3();
        public float Shininess { get; set; }

        public int TextureId { get; set; }
        public int SphereId { get; set; }

        public SphereMode Mode { get; set; }

        public ToonMode SharedToon { get; set; }
        public int ToonId { get; set; }
        public int FaceCount { get; set; }

        public object Clone() => new PmxMaterialData()
        {
            MaterialName = MaterialName,
            MaterialNameE = MaterialNameE,
            Script = Script,
            Flag = Flag,
            Edge = Edge,
            EdgeThick = EdgeThick,
            Diffuse = Diffuse,
            Specular = Specular,
            Ambient = Ambient,
            Shininess = Shininess,
            TextureId = TextureId,
            SphereId = SphereId,
            Mode = Mode,
            SharedToon = SharedToon,
            ToonId = ToonId,
            FaceCount = FaceCount,
        };

        public void Export(PmxExporter exporter)
        {
            exporter.WriteText(MaterialName);
            exporter.WriteText(MaterialNameE);

            exporter.Write(Diffuse);
            exporter.Write(Specular);
            exporter.Write(Shininess);
            exporter.Write(Ambient);

            exporter.Write(Flag);

            exporter.Write(Edge);
            exporter.Write(EdgeThick);

            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, TextureId);
            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, SphereId);
            exporter.Write((byte)Mode);
            exporter.Write((byte)SharedToon);

            if (SharedToon == 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, ToonId);
            }
            else
            {
                exporter.Write((byte)ToonId);
            }

            exporter.WriteText(Script);

            exporter.Write(FaceCount);
        }

        public void Parse(PmxParser parser)
        {
            MaterialName = parser.ReadText();
            MaterialNameE = parser.ReadText();

            Diffuse = parser.ReadVector4();
            Specular = parser.ReadVector3();
            Shininess = parser.ReadSingle();
            Ambient = parser.ReadVector3();

            Flag = parser.ReadByte();

            Edge = parser.ReadVector4();
            EdgeThick = parser.ReadSingle();

            TextureId = parser.ReadPmxId(parser.SizeTexture);
            SphereId = parser.ReadPmxId(parser.SizeTexture);
            Mode = (SphereMode)parser.ReadByte();
            SharedToon = (ToonMode)parser.ReadByte();

            if (SharedToon == 0)
            {
                ToonId = parser.ReadPmxId(parser.SizeTexture);
            }
            else
            {
                ToonId = parser.ReadByte();
            }

            Script = parser.ReadText();

            FaceCount = parser.ReadInt32();
        }
    }

    public enum RenderFlags : byte
    {
        DOUBLE_SIDED = 0x01,
        GROUND_SHADOW = 0x02,
        TO_SHADOW_MAP = 0x04,
        SLEF_SHADOW = 0x08,
        EDGE = 0x10,
    }

    public enum SphereMode : byte
    {
        DISBLE = 0,
        MULT = 1,
        ADD = 2,
        SUB_TEXTURE = 3
    }

    public enum ToonMode : byte
    {
        TEXTURE_FILE = 0,
        SHARED_FILE = 1,
    }
}
