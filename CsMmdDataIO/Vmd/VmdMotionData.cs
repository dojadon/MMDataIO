using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using VecMath;

namespace CsMmdDataIO.Vmd
{
    public class VmdMotionData
    {
        public static readonly Encoding Encoding = Encoding.GetEncoding("Shift_JIS");

        public const int BONE_NAME_LENGTH = 15;
        public const int MORPH_NAME_LENGTH = 15;
        public const int MODEL_NAME_LENGTH = 20;
        public const int HEADER_LENGTH = 30;

        public VmdHeaderData Header { get; set; } = new VmdHeaderData();
        public VmdMotionFrameData[] MotionFrameArray { get; set; } = { };
        public VmdMorphFrameData[] MorphFrameArray { get; set; } = { };
        public VmdPropertyFrameData[] PropertyFrameArray { get; set; } = { };

        public void Write(BinaryWriter writer)
        {
            WriteVmdData(Header, writer);
            WriteVmdData(MotionFrameArray, writer);
            WriteVmdData(MorphFrameArray, writer);
            writer.Write(0); //camera
            writer.Write(0); //light
            writer.Write(0); //self shadow
            WriteVmdData(PropertyFrameArray, writer);
        }

        private void WriteVmdData<T>(T[] data, BinaryWriter writer) where T : IVmdData
        {
            int len = data.Length;
            writer.Write(len);

            for (int i = 0; i < len; i++)
            {
                data[i].Write(writer);
            }
        }

        private void WriteVmdData<T>(T data, BinaryWriter writer) where T : IVmdData
        {
            data.Write(writer);
        }
    }

    public static class VmdBinaryIOExtensions
    {
        public static void WriteTextWithFixedLength(this BinaryWriter writer, string text, int fixedLength, string filled = "\0")
        {
            byte[] bytes = VmdMotionData.Encoding.GetBytes(text);

            if (bytes.Length > fixedLength)
            {
                byte[] fixedBytes = new byte[fixedLength];
                Array.Copy(bytes, fixedBytes, fixedLength);
                bytes = fixedBytes;
            }

            writer.Write(bytes);

            int remain = fixedLength - bytes.Length;
            if (remain > 0)
            {
                writer.WriteFiller(VmdMotionData.Encoding.GetBytes(filled), remain);
            }
        }

        private static void WriteFiller(this BinaryWriter writer, byte[] filler, int fillerLength)
        {
            if (filler.Length <= 0 || fillerLength <= 0) return;

            byte lastData = filler[filler.Length - 1];

            int fillerIdx = 0;
            for (int remain = fillerLength; remain > 0; remain--)
            {
                byte bVal = fillerIdx < filler.Length ? filler[fillerIdx++] : lastData;
                writer.Write(bVal);
            }
        }

        public static void Write(this BinaryWriter writer, Vector2 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
        }

        public static void Write(this BinaryWriter writer, Vector3 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
            writer.Write(vec.z);
        }

        public static void Write(this BinaryWriter writer, Vector4 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
            writer.Write(vec.z);
            writer.Write(vec.w);
        }

        public static void Write(this BinaryWriter writer, Quaternion vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
            writer.Write(vec.z);
            writer.Write(vec.w);
        }
    }
}
