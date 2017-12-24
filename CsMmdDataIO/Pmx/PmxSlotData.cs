using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxSlotData : IPmxData
    {
        public string SlotName { get; set; } = "";
        public string SlotNameE { get; set; } = "";

        public bool NormalSlot { get; set; } = true;
        public SlotType Type { get; set; }
        public int[] Indices { get; set; }

        public object Clone() => new PmxSlotData()
        {
            SlotName = SlotName,
            SlotNameE = SlotNameE,
            NormalSlot = NormalSlot,
            Type = Type,
            Indices = CloneUtil.CloneArray(Indices),
        };

        public void Export(PmxExporter exporter)
        {
            exporter.WriteText(SlotName);
            exporter.WriteText(SlotNameE);

            exporter.Write((byte)(NormalSlot ? 0 : 1));

            int elementCount = Indices.Length;
            exporter.Write(elementCount);

            byte size = Type == SlotType.BONE ? PmxExporter.SIZE_BONE : PmxExporter.SIZE_MORPH;

            for (int i = 0; i < elementCount; i++)
            {
                exporter.Write((byte)Type);

                int id = Indices[i];
                exporter.WritePmxId(size, id);
            }
        }

        public void Parse(PmxParser parser)
        {
            SlotName = parser.ReadText();
            SlotNameE = parser.ReadText();

            NormalSlot = parser.ReadByte() == 0;

            int elementCount = parser.ReadInt32();
            Indices = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                byte type = parser.ReadByte();
                byte size = type == (byte)SlotType.BONE ? parser.SizeBone : parser.SizeMorph;

                Indices[i] = parser.ReadPmxId(size);
            }
        }
    }

    public enum SlotType : byte
    {
        BONE = 0,
        MORPH = 1,
    }
}
