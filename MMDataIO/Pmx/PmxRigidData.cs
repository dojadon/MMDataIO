using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VecMath;

namespace MMDataIO.Pmx
{
    [Serializable]
    public class PmxRigidData : IPmxData
    {
        public string RigidName = "";
        public string RigidNameE = "";

        public int BoneId;

        public byte Group;
        public ushort GroupFlag;

        public RigidShape Shape;
        public Vector3 Size;

        public Vector3 Pos;
        public Vector3 Rot;

        public float Mass;
        public float MovingAttenuation;
        public float RotationAttenuation;
        public float Repulsive;
        public float Frictional;

        public RigidType RigidType;

        public object Clone() => new PmxRigidData
        {
            RigidName = RigidName,
            RigidNameE = RigidNameE,
            BoneId = BoneId,
            Group = Group,
            GroupFlag = GroupFlag,
            Shape = Shape,
            Size = Size,
            Pos = Pos,
            Rot = Rot,
            Mass = Mass,
            MovingAttenuation = MovingAttenuation,
            RotationAttenuation = RotationAttenuation,
            Repulsive = Repulsive,
            Frictional = Frictional,
            RigidType = RigidType,
        };

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            RigidName = reader.ReadText(header.Encoding);
            RigidNameE = reader.ReadText(header.Encoding);

            BoneId = reader.ReadPmxId(header.BoneIndexSize);

            Group = reader.ReadByte();
            GroupFlag = reader.ReadUInt16();

            Shape = (RigidShape)reader.ReadByte();
            Size = reader.ReadVector3();

            Pos = reader.ReadVector3();
            Rot = reader.ReadVector3();

            Mass = reader.ReadSingle();
            MovingAttenuation = reader.ReadSingle();
            RotationAttenuation = reader.ReadSingle();
            Repulsive = reader.ReadSingle();
            Frictional = reader.ReadSingle();

            RigidType = (RigidType)reader.ReadByte();
        }

        public void ReadPmd(BinaryReader reader, PmxHeaderData header)
        {
            throw new NotImplementedException();
        }

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WriteText(header.Encoding, RigidName);
            writer.WriteText(header.Encoding, RigidNameE);

            writer.WritePmxId(header.BoneIndexSize, BoneId);

            writer.Write(Group);
            writer.Write(GroupFlag);

            writer.Write((byte)Shape);
            writer.Write(Size);
            writer.Write(Pos);
            writer.Write(Rot);

            writer.Write(Mass);
            writer.Write(MovingAttenuation);
            writer.Write(RotationAttenuation);
            writer.Write(Repulsive);
            writer.Write(Frictional);

            writer.Write((byte)RigidType);
        }
    }

    public enum RigidShape : byte
    {
        Sphere = 0,
        Box = 1,
        Capsule = 2,
    }

    public enum RigidType : byte
    {
        Static = 0,
        Dynamic = 1,
        Hybrid = 2,
    }
}
