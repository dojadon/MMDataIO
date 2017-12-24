using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd
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

        public void Export(VmdExporter exporter)
        {
            exporter.WriteTextWithFixedLength(Name, VmdExporter.MORPH_NAME_LENGTH);
            exporter.Write(FrameTime);
            exporter.Write(Weigth);
        }
    }
}
