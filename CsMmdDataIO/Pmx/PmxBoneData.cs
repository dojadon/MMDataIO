using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VecMath;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxBoneData : IPmxData
    {
        public String BoneName { get; set; } = "";
        public String BoneNameE { get; set; } = "";
        /** 座標 */
        public Vector3 Pos { get; set; }
        /** ボーンID */
        public int BoneId { get; set; }
        /** 親(前)ボーンID. 無い場合は0xffff(-1). */
        public int ParentId { get; set; }
        /** 子(次)ボーンID. 末端の場合は0. */
        public int ArrowId { get; set; }
        /** flags フラグが収められてる16 bit. */
        public BoneFlags Flag { get; set; }
        /** 外部親のID. */
        public int ExtraParentId { get; set; }
        /** 変形階層 */
        public int Depth { get; set; }
        /** 矢先相対座標 */
        public Vector3 PosOffset { get; set; }
        /** 連動親ボーンID. */
        public int LinkParentId { get; set; }
        /** 連動比. 負を指定することも可能. */
        public float LinkWeight { get; set; }
        /** 軸の絶対座標 */
        public Vector3 AxisVec { get; set; }
        /** ローカルx軸 */
        public Vector3 LocalAxisVecX { get; set; }
        /** ローカルz軸 */
        public Vector3 LocalAxisVecZ { get; set; }
        /** IKボーンが接近しようとするIK接続先ボーンID */
        public int IkTargetId { get; set; }
        /** 再帰演算の深さ */
        public int IkDepth { get; set; }
        /** 制限角度強度 */
        public float AngleLimit { get; set; }

        public PmxIkData[] IkChilds { get; set; } = { };

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

                for (int i = 0; i < boneNum; i++)
                {
                    IkChilds[i].Write(writer, header);
                }
            }
        }

        public void Parse(BinaryReader reader, PmxHeaderData header)
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
                    IkChilds[i].Parse(reader, header);
                }
            }
        }
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

        public void Parse(BinaryReader reader, PmxHeaderData header)
        {
            ChildId = reader.ReadPmxId(header.BoneIndexSize);

            int limit = reader.ReadByte();

            if (limit > 0)
            {
                AngleMin = reader.ReadVector3();
                AngleMax = reader.ReadVector3();
            }
        }
    }
}
