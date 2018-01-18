using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VecMath;

namespace MMDataIO.Pmx
{
    public interface IPmxMorphTypeData : IPmxData
    {
        int Index { get; set; }
    }

    [Serializable]
    public struct PmxMorphBoneData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public object Clone() => new PmxMorphBoneData()
        {
            Index = Index,
            Position = Position,
            Rotation = Rotation,
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WritePmxId(header.BoneIndexSize, Index);

            writer.Write(Position);
            writer.Write(Rotation);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            Index = reader.ReadPmxId(header.BoneIndexSize);

            Position = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();
        }
    }

    [Serializable]
    public struct PmxMorphGroupData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public float Weight { get; set; }

        public object Clone() => new PmxMorphGroupData()
        {
            Index = Index,
            Weight = Weight,
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WritePmxId(header.MorphIndexSize, Index);
            writer.Write(Weight);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            Index = reader.ReadPmxId(header.MorphIndexSize);
            Weight = reader.ReadSingle();
        }
    }

    [Serializable]
    public struct PmxMorphMaterialData : IPmxMorphTypeData
    {
        public int Index { get; set; }

        public byte CalcType { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float Shininess { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector4 Edge { get; set; }
        public float EdgeThick { get; set; }
        public Vector4 Texture { get; set; }
        public Vector4 SphereTexture { get; set; }
        public Vector4 ToonTexture { get; set; }

        public object Clone() => new PmxMorphMaterialData()
        {
            Index = Index,
            CalcType = CalcType,
            Diffuse = Diffuse,
            Specular = Specular,
            Shininess = Shininess,
            Ambient = Ambient,
            Edge = Edge,
            EdgeThick = EdgeThick,
            Texture = Texture,
            SphereTexture = SphereTexture,
            ToonTexture = ToonTexture,
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WritePmxId(header.MaterialIndexSize, Index);

            writer.Write(CalcType);
            writer.Write(Diffuse);
            writer.Write(Specular);
            writer.Write(Shininess);
            writer.Write(Ambient);
            writer.Write(Edge);
            writer.Write(EdgeThick);
            writer.Write(Texture);
            writer.Write(SphereTexture);
            writer.Write(ToonTexture);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            Index = reader.ReadPmxId(header.MaterialIndexSize);

            CalcType = reader.ReadByte();
            Diffuse = reader.ReadVector4();
            Specular = reader.ReadVector3();
            Shininess = reader.ReadSingle();
            Ambient = reader.ReadVector3();
            Edge = reader.ReadVector4();
            EdgeThick = reader.ReadSingle();
            Texture = reader.ReadVector4();
            SphereTexture = reader.ReadVector4();
            ToonTexture = reader.ReadVector4();
        }
    }

    [Serializable]
    public struct PmxMorphUVData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector4 Uv { get; set; }

        public object Clone() => new PmxMorphUVData()
        {
            Index = Index,
            Uv = Uv,
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WritePmxId(header.VertexIndexSize, Index);
            writer.Write(Uv);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            Index = reader.ReadPmxId(header.VertexIndexSize);
            Uv = reader.ReadVector4();
        }
    }

    [Serializable]
    public struct PmxMorphVertexData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }

        public object Clone() => new PmxMorphVertexData()
        {
            Index = Index,
            Position = Position,
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WritePmxId(header.VertexIndexSize, Index);
            writer.Write(Position);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            Index = reader.ReadPmxId(header.VertexIndexSize);
            Position = reader.ReadVector3();
        }
    }
}
