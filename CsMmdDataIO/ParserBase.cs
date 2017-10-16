using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VecMath;

namespace CsMmdDataIO
{
    public abstract class ParserBase : BinaryReader
    {
        public abstract Encoding CharEncording { get; }

        public ParserBase(Stream inStream) : base(inStream) { }

        public string ReadText()
        {
            int len = ReadInt32();
            byte[] bytes = ReadBytes(len);

            string str = CharEncording.GetString(bytes);

            return str;
        }

        public Vector2 ReadVector2() => new Vector2(ReadSingle(), ReadSingle());

        public Vector3 ReadVector3() => new Vector3(ReadSingle(), ReadSingle(), ReadSingle());

        public Vector4 ReadVector4() => new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

        public Quaternion ReadQuaternion() => new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
    }
}
