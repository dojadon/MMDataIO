using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
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
        public short Flag { get; set; }
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

        public void Export(PmxExporter exporter)
        {
            exporter.WriteText(BoneName);
            exporter.WriteText(BoneNameE);

            exporter.Write(Pos);
            exporter.WritePmxId(PmxExporter.SIZE_BONE, ParentId);

            exporter.Write(Depth);
            exporter.Write(Flag);

            if ((BoneFlags.OFFSET & Flag) > 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, ArrowId);
            }
            else
            {
                exporter.Write(PosOffset);
            }

            if ((BoneFlags.ROTATE_LINK & Flag) > 0 || (BoneFlags.MOVE_LINK & Flag) > 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, LinkParentId);
                exporter.Write(LinkWeight);
            }

            if ((BoneFlags.AXIS_ROTATE & Flag) > 0)
            {
                exporter.Write(AxisVec);
            }

            if ((BoneFlags.LOCAL_AXIS & Flag) > 0)
            {
                exporter.Write(LocalAxisVecX);
                exporter.Write(LocalAxisVecZ);
            }

            if ((BoneFlags.EXTRA & Flag) > 0)
            {
                exporter.Write(ExtraParentId);
            }

            if ((BoneFlags.IK & Flag) > 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, IkTargetId);

                exporter.Write(IkDepth);
                exporter.Write(AngleLimit);

                int boneNum = IkChilds.Length;

                for (int i = 0; i < boneNum; i++)
                {
                    IkChilds[i].Export(exporter);
                }
            }
        }

        public void Parse(PmxParser parser)
        {
            BoneName = parser.ReadText();
            BoneNameE = parser.ReadText();

            Pos = parser.ReadVector3();
            ParentId = parser.ReadPmxId(parser.SizeBone);

            Depth = parser.ReadInt32();
            Flag = parser.ReadInt16();

            if ((BoneFlags.OFFSET & Flag) > 0)
            {
                ArrowId = parser.ReadPmxId(parser.SizeBone);
            }
            else
            {
                PosOffset = parser.ReadVector3();
            }

            if ((BoneFlags.ROTATE_LINK & Flag) > 0 || (BoneFlags.MOVE_LINK & Flag) > 0)
            {
                LinkParentId = parser.ReadPmxId(parser.SizeBone);
                LinkWeight = parser.ReadSingle();
            }

            if ((BoneFlags.AXIS_ROTATE & Flag) > 0)
            {
                AxisVec = parser.ReadVector3();
            }

            if ((BoneFlags.LOCAL_AXIS & Flag) > 0)
            {
                LocalAxisVecX = parser.ReadVector3();
                LocalAxisVecZ = parser.ReadVector3();
            }

            if ((BoneFlags.EXTRA & Flag) > 0)
            {
                ExtraParentId = parser.ReadInt32();
            }

            if ((BoneFlags.IK & Flag) > 0)
            {
                IkTargetId = parser.ReadPmxId(parser.SizeBone);

                IkDepth = parser.ReadInt32();
                AngleLimit = parser.ReadSingle();

                int boneNum = parser.ReadInt32();
                IkChilds = new PmxIkData[boneNum];

                for (int i = 0; i < boneNum; i++)
                {
                    IkChilds[i] = new PmxIkData();
                    IkChilds[i].Parse(parser);
                }
            }
        }
    }

    public class BoneFlags
    {
        /** オフセット. (0:のときオフセット. 1:のときボーン.) */
        public const short OFFSET = 0x0001;
        /** 回転. */
        public const short ROTATE = 0x0002;
        /** 移動. */
        public const short MOVE = 0x0004;
        /** 表示. */
        public const short VISIBLE = 0x0008;
        /** 操作. */
        public const short OP = 0x0010;
        /** IK. */
        public const short IK = 0x0020;
        /** ローカル付与フラグ. */
        public const short LOCAL_LINK = 0x0080;
        /** 回転付与. */
        public const short ROTATE_LINK = 0x0100;
        /** 移動付与. */
        public const short MOVE_LINK = 0x0200;
        /** 回転軸固定. */
        public const short AXIS_ROTATE = 0x0400;
        /** ローカル座標軸. */
        public const short LOCAL_AXIS = 0x0800;
        /** 物理後変形 */
        public const short PHYSICAL = 0x1000;
        /** 外部親変形 */
        public const short EXTRA = 0x2000;
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

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_BONE, ChildId);

            int limit = AngleMin == Vector3.Zero && AngleMax == Vector3.Zero ? 0 : 1;
            exporter.Write((byte)limit);

            if (limit > 0)
            {
                exporter.Write(AngleMin);
                exporter.Write(AngleMax);
            }
        }

        public void Parse(PmxParser parser)
        {
            ChildId = parser.ReadPmxId(parser.SizeBone);

            int limit = parser.ReadByte();

            if (limit > 0)
            {
                AngleMin = parser.ReadVector3();
                AngleMax = parser.ReadVector3();
            }
        }
    }
}
