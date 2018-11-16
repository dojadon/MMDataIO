using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VecMath;

namespace MMDataIO.Pmx
{
    public class PmxJointData : IPmxData
    {
        public string JointName { get; set; }
        public string JointNameE { get; set; }

        public JointType JointType { get; set; }

        public int RigidBodyA { get; set; }
        public int RigidBodyB { get; set; }

        public Vector3 Pos { get; set; }
        public Vector3 Rot { get; set; }

        public Vector3 PosMin { get; set; }
        public Vector3 PosMax { get; set; }
        public Vector3 RotMin { get; set; }
        public Vector3 RotMax { get; set; }

        public Vector3 SpringConstantPos { get; set; }
        public Vector3 SpringConstantRot { get; set; }

        public object Clone() => new PmxJointData
        {
            JointName = JointName,
            JointNameE = JointNameE,
            JointType = JointType,
            RigidBodyA = RigidBodyA,
            RigidBodyB = RigidBodyB,
            Pos = Pos,
            Rot = Rot,
            PosMin = PosMin,
            PosMax = PosMax,
            RotMin = RotMin,
            RotMax = RotMax,
            SpringConstantPos = SpringConstantPos,
            SpringConstantRot = SpringConstantRot,
        };

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            JointName = reader.ReadText(header.Encoding);
            JointNameE = reader.ReadText(header.Encoding);

            JointType = (JointType)reader.ReadByte();

            RigidBodyA = reader.ReadPmxId(header.RigidIndexSize);
            RigidBodyB = reader.ReadPmxId(header.RigidIndexSize);

            Pos = reader.ReadVector3();
            Rot = reader.ReadVector3();
            PosMin = reader.ReadVector3();
            PosMax = reader.ReadVector3();
            RotMin = reader.ReadVector3();
            RotMax = reader.ReadVector3();
            SpringConstantPos = reader.ReadVector3();
            SpringConstantRot = reader.ReadVector3();
        }

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WriteText(header.Encoding, JointName);
            writer.WriteText(header.Encoding, JointNameE);

            writer.Write((byte)JointType);

            writer.WritePmxId(header.RigidIndexSize, RigidBodyA);
            writer.WritePmxId(header.RigidIndexSize, RigidBodyB);

            writer.Write(Pos);
            writer.Write(Rot);
            writer.Write(PosMin);
            writer.Write(PosMax);
            writer.Write(RotMin);
            writer.Write(RotMax);
            writer.Write(SpringConstantPos);
            writer.Write(SpringConstantRot);
        }

        public void ReadPmd(BinaryReader reader, PmxHeaderData header)
        {
            throw new NotImplementedException();
        }
    }

    public enum JointType
    {
        Generic6DofSpring = 0,
        Generic6Dof = 1,
        Point2Point = 2,
        ConeTwist = 3,
        Slider = 4,
        Hinge = 5,
    }
}
