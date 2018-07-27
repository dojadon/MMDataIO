using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VecMath;

namespace MMDataIO.Vmd
{
    public struct VmdMotionFrameData : IVmdData, IElementKeyFrame
    {
        public string Name { get; set; }
        public int FrameTime { get; set; }
        public Vector3 Pos { get; set; }
        public Quaternion Rot { get; set; }

        public byte[] InterpolatePointX { get; set; }
        public byte[] InterpolatePointY { get; set; }
        public byte[] InterpolatePointZ { get; set; }
        public byte[] InterpolatePointR { get; set; }

        public Vector2 InterpolationPointX1
        {
            get => new Vector2(InterpolatePointX[0] / 127.0F, InterpolatePointX[1] / 127.0F);
            set
            {
                InterpolatePointX[0] = (byte)(value.x * 127);
                InterpolatePointX[1] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointX2
        {
            get => new Vector2(InterpolatePointX[2] / 127.0F, InterpolatePointX[3] / 127.0F);
            set
            {
                InterpolatePointX[2] = (byte)(value.x * 127);
                InterpolatePointX[3] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointY1
        {
            get => new Vector2(InterpolatePointY[0] / 127.0F, InterpolatePointY[1] / 127.0F);
            set
            {
                InterpolatePointY[0] = (byte)(value.x * 127);
                InterpolatePointY[1] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointY2
        {
            get => new Vector2(InterpolatePointY[2] / 127.0F, InterpolatePointY[3] / 127.0F);
            set
            {
                InterpolatePointY[2] = (byte)(value.x * 127);
                InterpolatePointY[3] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointZ1
        {
            get => new Vector2(InterpolatePointZ[0] / 127.0F, InterpolatePointZ[1] / 127.0F);
            set
            {
                InterpolatePointZ[0] = (byte)(value.x * 127);
                InterpolatePointZ[1] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointZ2
        {
            get => new Vector2(InterpolatePointZ[2] / 127.0F, InterpolatePointZ[3] / 127.0F);
            set
            {
                InterpolatePointZ[2] = (byte)(value.x * 127);
                InterpolatePointZ[3] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointR1
        {
            get => new Vector2(InterpolatePointR[0] / 127.0F, InterpolatePointR[1] / 127.0F);
            set
            {
                InterpolatePointR[0] = (byte)(value.x * 127);
                InterpolatePointR[1] = (byte)(value.y * 127);
            }
        }

        public Vector2 InterpolationPointR2
        {
            get => new Vector2(InterpolatePointR[2] / 127.0F, InterpolatePointR[3] / 127.0F);
            set
            {
                InterpolatePointR[2] = (byte)(value.x * 127);
                InterpolatePointR[3] = (byte)(value.y * 127);
            }
        }

        public VmdMotionFrameData(string boneName, int frameTime, Vector3 pos, Quaternion rot)
        {
            Name = boneName;
            FrameTime = frameTime;
            Pos = pos;
            Rot = rot;
            InterpolatePointX = new byte[] { 64, 64, 64, 64};
            InterpolatePointY = new byte[] { 64, 64, 64, 64 };
            InterpolatePointZ = new byte[] { 64, 64, 64, 64 };
            InterpolatePointR = new byte[] { 64, 64, 64, 64 };
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteTextWithFixedLength(Name, VmdMotionData.BONE_NAME_LENGTH);
            writer.Write(FrameTime);
            writer.Write(Pos);
            writer.Write(Rot);
            WriteInterpolateData(writer);
        }

        private void WriteInterpolateData(BinaryWriter writer)
        {
            byte[][] interpolatePoint = new byte[][] { InterpolatePointX, InterpolatePointY, InterpolatePointZ, InterpolatePointR };

            byte[] distPart = new byte[16];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    distPart[i * 4 + j] = interpolatePoint[j][i];
                }
            }

            byte[] dist = new byte[64];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 16 - i; j++)
                {
                    dist[i * 16 + j] = distPart[j];
                }
            }

            dist[31] = dist[46] = dist[61] = 1;
            writer.Write(dist);
        }

        public static byte[] ConvertToBytes(Vector2 pos1, Vector2 pos2)
        {
            return new byte[] { (byte)(pos1.x * 127), (byte)(pos1.y * 127), (byte)(pos2.x * 127), (byte)(pos2.y * 127) };
        }
    }
}
