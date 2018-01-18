using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MMDataIO.Vmd
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

        public void Write(BinaryWriter writer)
        {
            writer.Write(FrameTime);
            writer.Write(IsVisible ? (byte)1 : (byte)0);

            if(IKEnabled != null)
            {
                writer.Write(IKEnabled.Count);

                foreach (var pair in IKEnabled)
                {
                    writer.WriteTextWithFixedLength(pair.Key, 20);
                    writer.Write(pair.Value ? (byte)1 : (byte)0);
                }
            }
        }
    }
}
