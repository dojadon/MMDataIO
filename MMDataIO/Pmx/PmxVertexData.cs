using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VecMath;

namespace MMDataIO.Pmx
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

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.Write(Pos);
            writer.Write(Normal);
            writer.Write(Uv);

            // for (byte i = 0; i < header.; i++)
            // {
            // writer.dumpLeFloat(uvEX.x).dumpLeFloat(uvEX.y).dumpLeFloat(uvEX.z).dumpLeFloat(uvEX.w);
            // }

            writer.Write((byte)WeightType);

            for (byte i = 0; i < BoneId.Length; i++)
            {
                writer.WritePmxId(header.BoneIndexSize, BoneId[i]);
            }

            switch (WeightType)
            {
                case WeightType.BDEF1:
                    break;

                case WeightType.BDEF2:
                case WeightType.SDEF:
                    writer.Write(Weight[0]);
                    break;

                case WeightType.BDEF4:
                    for (byte i = 0; i < 4; i++)
                    {
                        writer.Write(Weight[i]);
                    }
                    break;
            }

            if (WeightType == WeightType.SDEF)
            {
                writer.Write(Sdef_c);
                writer.Write(Sdef_r0);
                writer.Write(Sdef_r1);
            }
            writer.Write(Edge);
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            Pos = reader.ReadVector3();
            Normal = reader.ReadVector3();
            Uv = reader.ReadVector2();

            WeightType = (WeightType)reader.ReadByte();

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
                BoneId[i] = reader.ReadPmxId(header.BoneIndexSize);
            }

            switch (WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    Weight = new float[] { 1 };
                    break;

                case WeightType.BDEF2:
                case WeightType.SDEF:
                    float we = reader.ReadSingle();
                    Weight = new float[] { we, 1 - we };
                    break;

                case WeightType.BDEF4:
                    Weight = new float[4];
                    for (byte i = 0; i < 4; i++)
                    {
                        Weight[i] = reader.ReadSingle();
                    }
                    break;
            }

            if (WeightType == WeightType.SDEF)
            {
                Sdef_c = reader.ReadVector3();
                Sdef_r0 = reader.ReadVector3();
                Sdef_r1 = reader.ReadVector3();
            }
            Edge = reader.ReadSingle();
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
