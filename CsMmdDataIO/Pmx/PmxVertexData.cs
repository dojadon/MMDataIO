using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxVertexData : IPmxData
    {
        public const byte WEIGHT_TYPE_BDEF1 = 0;
        public const byte WEIGHT_TYPE_BDEF2 = 1;
        public const byte WEIGHT_TYPE_BDEF4 = 2;
        public const byte WEIGHT_TYPE_SDEF = 3;

        public int VertexId { get; set; }

        public Vector3 Pos { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 Uv { get; set; }
        public float Edge { get; set; }

        public WeightType WeightType { get; set; }
        public int[] BoneId { get; set; } = { };
        public float[] Weight { get; set; } = { };
        public Vector3 Sdef_c;
        public Vector3 Sdef_r0;
        public Vector3 Sdef_r1;

        public object Clone() => new PmxVertexData()
        {
            VertexId = VertexId,
            Pos = Pos,
            Normal = Normal,
            Uv = Uv,
            Edge = Edge,
            WeightType = WeightType,
            BoneId = CloneUtil.CloneArray(BoneId),
            Weight = CloneUtil.CloneArray(Weight),
            Sdef_c = Sdef_c,
            Sdef_r0 = Sdef_r0,
            Sdef_r1 = Sdef_r1,
        };

        public void Export(PmxExporter exporter)
        {
            exporter.Write(Pos);
            exporter.Write(Normal);
            exporter.Write(Uv);

            // for (byte i = 0; i < num_uv; i++)
            // {
            // exporter.dumpLeFloat(uvEX.x).dumpLeFloat(uvEX.y).dumpLeFloat(uvEX.z).dumpLeFloat(uvEX.w);
            // }

            exporter.Write((byte)WeightType);

            for (byte i = 0; i < BoneId.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, BoneId[i]);
            }

            switch (WeightType)
            {
                case WeightType.BDEF1:
                    break;

                case WeightType.BDEF2:
                case WeightType.SDEF:
                    exporter.Write(Weight[0]);
                    break;

                case WeightType.BDEF4:
                    for (byte i = 0; i < 4; i++)
                    {
                        exporter.Write(Weight[i]);
                    }
                    break;
            }

            if (WeightType == WeightType.SDEF)
            {
                exporter.Write(Sdef_c);
                exporter.Write(Sdef_r0);
                exporter.Write(Sdef_r1);
            }
            exporter.Write(Edge);
        }

        public void Parse(PmxParser parser)
        {
            Pos = parser.ReadVector3();
            Normal = parser.ReadVector3();
            Uv = parser.ReadVector2();

            WeightType = (WeightType)parser.ReadByte();

            switch (WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    BoneId = new int[1];
                    break;

                case WeightType.BDEF2:
                case WeightType.SDEF:
                    BoneId = new int[2];
                    break;

                case WeightType.BDEF4:
                    BoneId = new int[4];
                    break;
            }

            for (int i = 0; i < BoneId.Length; i++)
            {
                BoneId[i] = parser.ReadPmxId(parser.SizeBone);
            }

            switch (WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    Weight = new float[] { 1 };
                    break;

                case WeightType.BDEF2:
                case WeightType.SDEF:
                    float we = parser.ReadSingle();
                    Weight = new float[] { we, 1 - we };
                    break;

                case WeightType.BDEF4:
                    Weight = new float[4];
                    for (byte i = 0; i < 4; i++)
                    {
                        Weight[i] = parser.ReadSingle();
                    }
                    break;
            }

            if (WeightType == WeightType.SDEF)
            {
                Sdef_c = parser.ReadVector3();
                Sdef_r0 = parser.ReadVector3();
                Sdef_r1 = parser.ReadVector3();
            }
            Edge = parser.ReadSingle();
        }
    }

    public enum WeightType : byte
    {
        BDEF1 = 0,
        BDEF2 = 1,
        BDEF4 = 2,
        SDEF = 3,
    }
}
