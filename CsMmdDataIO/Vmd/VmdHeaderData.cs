using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd
{
    public class VmdHeaderData : IVmdData
    {
        public const string HEADER = "Vocaloid Motion Data 0002";
        public string ModelName { get; set; }

        public void Export(VmdExporter exporter)
        {
            exporter.WriteTextWithFixedLength(HEADER, VmdExporter.HEADER_LENGTH);
            exporter.WriteTextWithFixedLength(ModelName, VmdExporter.MODEL_NAME_LENGTH);
        }
    }
}
