using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsMmdDataIO.Vmd
{
    public class VmdHeaderData : IVmdData
    {
        public const string HEADER = "Vocaloid Motion Data 0002";
        public string ModelName { get; set; }

        public void Write(BinaryWriter writer)
        {
            writer.WriteTextWithFixedLength(HEADER, VmdMotionData.HEADER_LENGTH);
            writer.WriteTextWithFixedLength(ModelName, VmdMotionData.MODEL_NAME_LENGTH);
        }
    }
}
