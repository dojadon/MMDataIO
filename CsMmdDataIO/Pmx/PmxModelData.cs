using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxModelData : IPmxData
    {
        public PmxHeaderData Header { get; set; } = new PmxHeaderData();
        public PmxVertexData[] VertexArray { get; set; } = { };
        public PmxMaterialData[] MaterialArray { get; set; } = { };
        public PmxBoneData[] BoneArray { get; set; } = { };
        public PmxMorphData[] MorphArray { get; set; } = { };
        public PmxSlotData[] SlotArray { get; set; } = { };
        public int[] VertexIndices { get; set; } = { };
        public string[] TextureFiles { get; set; } = { };

        public object Clone() => new PmxModelData()
        {
            Header = CloneUtil.Clone(Header),
            VertexArray = CloneUtil.CloneArray(VertexArray),
            MaterialArray = CloneUtil.CloneArray(MaterialArray),
            BoneArray = CloneUtil.CloneArray(BoneArray),
            MorphArray = CloneUtil.CloneArray(MorphArray),
            SlotArray = CloneUtil.CloneArray(SlotArray),
            VertexIndices = CloneUtil.CloneArray(VertexIndices),
            TextureFiles = CloneUtil.CloneArray(TextureFiles),
        };

        public void Export(PmxExporter exporter)
        {
            ExportPmxData(Header, exporter);
            ExportPmxData(VertexArray, exporter);
            ExportData(VertexIndices, (i, ex) => ex.Write(i), exporter);
            ExportData(TextureFiles, (s, ex) => ex.WriteText(s), exporter);
            ExportPmxData(MaterialArray, exporter);
            ExportPmxData(BoneArray, exporter);
            ExportPmxData(MorphArray, exporter);
            ExportPmxData(SlotArray, exporter);
            exporter.Write(0);//Number of Rigid
            exporter.Write(0);//Number of Joint
            // exporter.Write(0);//Number of SoftBody
        }

        public void Parse(PmxParser parser)
        {
            ParsePmxData(Header, parser);
            VertexArray = ParsePmxData(len => new PmxVertexData[len], parser);
            VertexIndices = ParseData(len => new int[len], (p, i) => p.ReadPmxId(parser.SizeVertex), parser);
            TextureFiles = ParseData(len => new string[len], (p, i) => p.ReadText(), parser);
            MaterialArray = ParsePmxData(len => new PmxMaterialData[len], parser);
            BoneArray = ParsePmxData(len => new PmxBoneData[len], parser);
            MorphArray = ParsePmxData(len => new PmxMorphData[len], parser);
            SlotArray = ParsePmxData(len => new PmxSlotData[len], parser);
        }

        private void ExportData<T>(T[] data, Action<T, PmxExporter> action, PmxExporter exporter)
        {
            exporter.Write(data.Length);
            Array.ForEach(data, d => action.Invoke(d, exporter));
        }

        private void ExportPmxData<T>(T data, PmxExporter exporter) where T : IPmxData
        {
            data.Export(exporter);
        }

        private void ExportPmxData<T>(T[] data, PmxExporter exporter) where T : IPmxData
        {
            exporter.Write(data.Length);
            Array.ForEach(data, d => d.Export(exporter));
        }

        private T[] ParseData<T>(Func<int, T[]> func, Func<PmxParser, T, T> valueFunc, PmxParser parser)
        {
            int len = parser.ReadInt32();
            T[] array = func(len);

            for (int i = 0; i < len; i++)
            {
                array[i] = valueFunc(parser, array[i]);
            }
            return array;
        }

        private void ParsePmxData<T>(T data, PmxParser parser) where T : IPmxData
        {
            data.Parse(parser);
        }

        private T[] ParsePmxData<T>(Func<int, T[]> func, PmxParser parser) where T : IPmxData, new()
        {
            int len = parser.ReadInt32();
            T[] array = func(len);

            for (int i = 0; i < len; i++)
            {
                array[i] = new T();
                array[i].Parse(parser);
            }
            return array;
        }
    }
}
