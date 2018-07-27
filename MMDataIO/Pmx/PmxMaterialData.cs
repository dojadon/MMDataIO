using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VecMath;

namespace MMDataIO.Pmx
{
    [Serializable]
    public class PmxMaterialData : IPmxData
    {
        public const int PMD_TEXTUREFILENAME_LEN = 20;

        public string MaterialName { get; set; } = "";
        public string MaterialNameE { get; set; } = "";

        public string Script { get; set; } = "";

        public RenderFlags Flag { get; set; }

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

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WriteText(header.Encoding, MaterialName);
            writer.WriteText(header.Encoding, MaterialNameE);

            writer.Write(Diffuse);
            writer.Write(Specular);
            writer.Write(Shininess);
            writer.Write(Ambient);

            writer.Write((byte)Flag);

            writer.Write(Edge);
            writer.Write(EdgeThick);

            writer.WritePmxId(header.TextureIndexSize, TextureId);
            writer.WritePmxId(header.TextureIndexSize, SphereId);
            writer.Write((byte)Mode);
            writer.Write((byte)SharedToon);

            if (SharedToon == 0)
            {
                writer.WritePmxId(header.TextureIndexSize, ToonId);
            }
            else
            {
                writer.Write((byte)ToonId);
            }

            writer.WriteText(header.Encoding, Script);

            writer.Write(FaceCount);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            MaterialName = reader.ReadText(header.Encoding);
            MaterialNameE = reader.ReadText(header.Encoding);

            Diffuse = reader.ReadVector4();
            Specular = reader.ReadVector3();
            Shininess = reader.ReadSingle();
            Ambient = reader.ReadVector3();

            Flag = (RenderFlags)reader.ReadByte();

            Edge = reader.ReadVector4();
            EdgeThick = reader.ReadSingle();

            TextureId = reader.ReadPmxId(header.TextureIndexSize);
            SphereId = reader.ReadPmxId(header.TextureIndexSize);
            Mode = (SphereMode)reader.ReadByte();
            SharedToon = (ToonMode)reader.ReadByte();

            if (SharedToon == 0)
            {
                ToonId = reader.ReadPmxId(header.TextureIndexSize);
            }
            else
            {
                ToonId = reader.ReadByte();
            }

            Script = reader.ReadText(header.Encoding);

            FaceCount = reader.ReadInt32();
        }

        public void ReadPmd(BinaryReader reader, PmxHeaderData header)
        {
            Diffuse = reader.ReadVector4();
            Shininess = reader.ReadSingle();
            Specular = reader.ReadVector3();
            Ambient = reader.ReadVector3();

            Edge = new Vector4(1, 1, 1, 1);
            EdgeThick = 1.0F;

            SharedToon = ToonMode.SHARED_FILE;
            ToonId = reader.ReadByte();

            if(reader.ReadBoolean())
            {
                Flag |= RenderFlags.EDGE;
            }

            FaceCount = reader.ReadInt32();

            reader.ReadBytes(PMD_TEXTUREFILENAME_LEN); // テクスチャ未実装
        }
    }

    [Flags]
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
