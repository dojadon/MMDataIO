using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using VecMath;

namespace CsMmdDataIO
{
    public abstract class ExporterBase : BinaryWriter
    {
        public abstract Encoding CharEncording { get; }

        public ExporterBase(Stream outStream) : base(outStream)
        {

        }

        public void WriteText(string text)
        {
            if (text == null) text = "";

            byte[] bytes = CharEncording.GetBytes(text.ToCharArray());

            Write(bytes.Length);
            Write(bytes);
        }

        public void WriteTextWithFixedLength(string text, int fixedLength, string filled = "\0")
        {
            byte[] bytes = CharEncording.GetBytes(text);

            if(bytes.Length > fixedLength)
            {
                byte[] fixedBytes = new byte[fixedLength];
                Array.Copy(bytes, fixedBytes, fixedLength);
                bytes = fixedBytes;
             }

            Write(bytes);

            int remain = fixedLength - bytes.Length;
            if (remain > 0)
            {
                WriteFiller(CharEncording.GetBytes(filled), remain);
            }
        }

        private void WriteFiller(byte[] filler, int fillerLength)
        {
            if (filler.Length <= 0 || fillerLength <= 0) return;

            byte lastData = filler[filler.Length - 1];

            int fillerIdx = 0;
            for (int remain = fillerLength; remain > 0; remain--)
            {
                byte bVal = fillerIdx < filler.Length ? filler[fillerIdx++] : lastData;
                Write(bVal);
            }
        }

        public void Write(Vector2 vec)
        {
            Write(vec.x);
            Write(vec.y);
        }

        public void Write(Vector3 vec)
        {
            Write(vec.x);
            Write(vec.y);
            Write(vec.z);
        }

        public void Write(Vector4 vec)
        {
            Write(vec.x);
            Write(vec.y);
            Write(vec.z);
            Write(vec.w);
        }

        public void Write(Quaternion vec)
        {
            Write(vec.x);
            Write(vec.y);
            Write(vec.z);
            Write(vec.w);
        }
    }
}
