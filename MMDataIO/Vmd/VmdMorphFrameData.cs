using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMDataIO.Vmd
{
    public struct VmdMorphFrameData : IVmdData, IElementKeyFrame
    {
        public string Name { get; set; }
        public int FrameTime { get; set; }
        public float Weigth { get; set; }

        public VmdMorphFrameData(string morphName, int frameTime, float weight)
        {
            Name = morphName;
            FrameTime = frameTime;
            Weigth = weight;
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteTextWithFixedLength(Name, VmdMotionData.MORPH_NAME_LENGTH);
            writer.Write(FrameTime);
            writer.Write(Weigth);
        }
    }
}
