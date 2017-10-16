using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd.Data
{
    public class VmdMotionData : IVmdData
    {
        public VmdHeaderData Header { get; set; } = new VmdHeaderData();
        public VmdMotionFrameData[] MotionArray { get; set; } = { };
        public VmdMorphFrameData[] MorphArray { get; set; } = { };
        public VmdPropertyFrameData[] PropertyArray { get; set; } = { };

        public void Export(VmdExporter exporter)
        {
            ExportVmdData(Header, exporter);
            ExportVmdData(MotionArray, exporter);
            ExportVmdData(MorphArray, exporter);
            exporter.Write(0); //camera
            exporter.Write(0); //light
            exporter.Write(0); //self shadow
            ExportVmdData(PropertyArray, exporter);
        }

        private void ExportVmdData<T>(T[] data, VmdExporter exporter) where T : IVmdData
        {
            int len = data.Length;
            exporter.Write(len);

            for (int i = 0; i < len; i++)
            {
                data[i].Export(exporter);
            }
        }

        private void ExportVmdData<T>(T data, VmdExporter exporter) where T : IVmdData
        {
            data.Export(exporter);
        }
    }
}
