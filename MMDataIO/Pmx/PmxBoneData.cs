using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VecMath;

namespace MMDataIO.Pmx
{
    [Serializable]
    public class PmxBoneData : IPmxData
    {
        public const int PMD_BONENAME_LEN = 20;

        public String BoneName = "";
        public String BoneNameE = "";

        public Vector3 Pos;

        public int BoneId;

        public int ParentId;

        public int ArrowId;

        public BoneFlags Flag;

        public int ExtraParentId;

        public int Depth;

        public Vector3 PosOffset;

        public int LinkParentId;
        public float LinkWeight;

        public Vector3 AxisVec;

        public Vector3 LocalAxisVecX;
        public Vector3 LocalAxisVecZ;

        public int IkTargetId;
        public int IkDepth;
        public float AngleLimit;

        public PmxIkData[] IkChilds = { };

        public object Clone()
        {
            return new PmxBoneData()
            {
                BoneName = BoneName,
                BoneNameE = BoneNameE,
                Pos = Pos,
                BoneId = BoneId,
                ParentId = ParentId,
                ArrowId = ArrowId,
                Flag = Flag,
                ExtraParentId = ExtraParentId,
                Depth = Depth,
                PosOffset = PosOffset,
                LinkParentId = LinkParentId,
                LinkWeight = LinkWeight,
                AxisVec = AxisVec,
                LocalAxisVecX = LocalAxisVecX,
                LocalAxisVecZ = LocalAxisVecZ,
                IkTargetId = IkTargetId,
                IkDepth = IkDepth,
                AngleLimit = AngleLimit,
                IkChilds = CloneUtil.CloneArray(IkChilds),
            };
        }

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WriteText(header.Encoding, BoneName);
            writer.WriteText(header.Encoding, BoneNameE);

            writer.Write(Pos);
            writer.WritePmxId(header.BoneIndexSize, ParentId);

            writer.Write(Depth);
            writer.Write((short)Flag);

            if (Flag.HasFlag(BoneFlags.OFFSET))
            {
                writer.WritePmxId(header.BoneIndexSize, ArrowId);
            }
            else
            {
                writer.Write(PosOffset);
            }

            if (Flag.HasFlag(BoneFlags.ROTATE_LINK) || Flag.HasFlag(BoneFlags.MOVE_LINK))
            {
                writer.WritePmxId(header.BoneIndexSize, LinkParentId);
                writer.Write(LinkWeight);
            }

            if (Flag.HasFlag(BoneFlags.AXIS_ROTATE))
            {
                writer.Write(AxisVec);
            }

            if (Flag.HasFlag(BoneFlags.LOCAL_AXIS))
            {
                writer.Write(LocalAxisVecX);
                writer.Write(LocalAxisVecZ);
            }

            if (Flag.HasFlag(BoneFlags.EXTRA))
            {
                writer.Write(ExtraParentId);
            }

            if (Flag.HasFlag(BoneFlags.IK))
            {
                writer.WritePmxId(header.BoneIndexSize, IkTargetId);

                writer.Write(IkDepth);
                writer.Write(AngleLimit);

                int boneNum = IkChilds.Length;

                writer.Write(boneNum);
                for (int i = 0; i < boneNum; i++)
                {
                    IkChilds[i].Write(writer, header);
                }
            }
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            BoneName = reader.ReadText(header.Encoding);
            BoneNameE = reader.ReadText(header.Encoding);

            Pos = reader.ReadVector3();
            ParentId = reader.ReadPmxId(header.BoneIndexSize);

            Depth = reader.ReadInt32();
            Flag = (BoneFlags)reader.ReadInt16();

            if (Flag.HasFlag(BoneFlags.OFFSET))
            {
                ArrowId = reader.ReadPmxId(header.BoneIndexSize);
            }
            else
            {
                PosOffset = reader.ReadVector3();
            }

            if (Flag.HasFlag(BoneFlags.ROTATE_LINK) || Flag.HasFlag(BoneFlags.MOVE_LINK))
            {
                LinkParentId = reader.ReadPmxId(header.BoneIndexSize);
                LinkWeight = reader.ReadSingle();
            }

            if (Flag.HasFlag(BoneFlags.AXIS_ROTATE))
            {
                AxisVec = reader.ReadVector3();
            }

            if (Flag.HasFlag(BoneFlags.LOCAL_AXIS))
            {
                LocalAxisVecX = reader.ReadVector3();
                LocalAxisVecZ = reader.ReadVector3();
            }

            if (Flag.HasFlag(BoneFlags.EXTRA))
            {
                ExtraParentId = reader.ReadInt32();
            }

            if (Flag.HasFlag(BoneFlags.IK))
            {
                IkTargetId = reader.ReadPmxId(header.BoneIndexSize);

                IkDepth = reader.ReadInt32();
                AngleLimit = reader.ReadSingle();

                int boneNum = reader.ReadInt32();
                IkChilds = new PmxIkData[boneNum];

                for (int i = 0; i < boneNum; i++)
                {
                    IkChilds[i] = new PmxIkData();
                    IkChilds[i].Read(reader, header);
                }
            }
        }

        public void ReadPmd(BinaryReader reader, PmxHeaderData header)
        {
            BoneName = reader.ReadText(header.Encoding, PMD_BONENAME_LEN);

            ParentId = reader.ReadUInt16();
            ArrowId = reader.ReadUInt16();

            byte type = reader.ReadByte();

            int ikId = reader.ReadUInt16();

            Pos = reader.ReadVector3();
        }
    }

    public enum PmdBoneType : byte
    {
        ROTATE = 0,
        ROTATE_MOVE = 1,
        IK = 2,
        UNKNOWN = 3,

        IK_TARGET = 6,
        INVISIBLE = 7,
        TWIST = 8,
        LINK_ROTATE = 9,
    }


    [Flags]
    public enum BoneFlags : short
    {
        OFFSET = 0x0001,
        ROTATE = 0x0002,
        MOVE = 0x0004,
        VISIBLE = 0x0008,
        OP = 0x0010,
        IK = 0x0020,
        LOCAL_LINK = 0x0080,
        ROTATE_LINK = 0x0100,
        MOVE_LINK = 0x0200,
        AXIS_ROTATE = 0x0400,
        LOCAL_AXIS = 0x0800,
        PHYSICAL = 0x1000,
        EXTRA = 0x2000,
    }

    public struct PmxIkData : IPmxData
    {
        public int ChildId { get; set; }
        public Vector3 AngleMin { get; set; }
        public Vector3 AngleMax { get; set; }

        public object Clone() => new PmxIkData()
        {
            ChildId = ChildId,
            AngleMin = AngleMin,
            AngleMax = AngleMax
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WritePmxId(header.BoneIndexSize, ChildId);

            int limit = AngleMin == Vector3.Zero && AngleMax == Vector3.Zero ? 0 : 1;
            writer.Write((byte)limit);

            if (limit > 0)
            {
                writer.Write(AngleMin);
                writer.Write(AngleMax);
            }
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            ChildId = reader.ReadPmxId(header.BoneIndexSize);

            int limit = reader.ReadByte();

            if (limit > 0)
            {
                AngleMin = reader.ReadVector3();
                AngleMax = reader.ReadVector3();
            }
        }

        public void ReadPmd(BinaryReader reader, PmxHeaderData header)
        {
        }
    }
}
