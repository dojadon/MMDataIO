using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd
{
    public struct VmdPropertyFrameData : IVmdData, IKeyFrame
    {
        public int FrameTime { get; set; }
        public bool IsVisible { get; set; }
        public Dictionary<string, bool> IKEnabled { get; set; }

        public VmdPropertyFrameData(int frameTime, bool isVisible)
        {
            FrameTime = frameTime;
            IsVisible = isVisible;
            IKEnabled = new Dictionary<string, bool>();
        }

        public void Export(VmdExporter exporter)
        {
            exporter.Write(FrameTime);
            exporter.Write(IsVisible ? (byte)1 : (byte)0);

            if(IKEnabled != null)
            {
                exporter.Write(IKEnabled.Count);

                foreach (var pair in IKEnabled)
                {
                    exporter.WriteTextWithFixedLength(pair.Key, 20);
                    exporter.Write(pair.Value ? (byte)1 : (byte)0);
                }
            }
        }
    }
}
