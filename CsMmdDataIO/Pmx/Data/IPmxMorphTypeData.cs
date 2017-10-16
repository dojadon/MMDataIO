using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
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

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_BONE, Index);

            exporter.Write(Position);
            exporter.Write(Rotation);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeBone);

            Position = parser.ReadVector3();
            Rotation = parser.ReadQuaternion();
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

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_MORPH, Index);
            exporter.Write(Weight);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeMorph);
            Weight = parser.ReadSingle();
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

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_MATERIAL, Index);

            exporter.Write(CalcType);
            exporter.Write(Diffuse);
            exporter.Write(Specular);
            exporter.Write(Shininess);
            exporter.Write(Ambient);
            exporter.Write(Edge);
            exporter.Write(EdgeThick);
            exporter.Write(Texture);
            exporter.Write(SphereTexture);
            exporter.Write(ToonTexture);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeMaterial);

            CalcType = parser.ReadByte();
            Diffuse = parser.ReadVector4();
            Specular = parser.ReadVector3();
            Shininess = parser.ReadSingle();
            Ambient = parser.ReadVector3();
            Edge = parser.ReadVector4();
            EdgeThick = parser.ReadSingle();
            Texture = parser.ReadVector4();
            SphereTexture = parser.ReadVector4();
            ToonTexture = parser.ReadVector4();
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

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, Index);
            exporter.Write(Uv);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeVertex);
            Uv = parser.ReadVector4();
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

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, Index);
            exporter.Write(Position);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeVertex);
            Position = parser.ReadVector3();
        }
    }
}
