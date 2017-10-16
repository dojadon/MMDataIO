using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd.Data
{
    public struct VmdPropertyFrameData : IVmdData, IKeyFrame
    {
        public long FrameTime { get; set; }
        public bool IsVisible { get; set; }
        public Dictionary<string, bool> IKEnabled { get; set; }

        public VmdPropertyFrameData(long frameTime, bool isVisible)
        {
            FrameTime = frameTime;
            IsVisible = isVisible;
            IKEnabled = new Dictionary<string, bool>();
        }

        public void Export(VmdExporter exporter)
        {
            exporter.Write((int)FrameTime);
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
