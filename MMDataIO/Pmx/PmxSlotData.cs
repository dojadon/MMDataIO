using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMDataIO.Pmx
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

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WriteText(header.Encoding, SlotName);
            writer.WriteText(header.Encoding, SlotNameE);

            writer.Write((byte)(NormalSlot ? 0 : 1));

            int elementCount = Indices.Length;
            writer.Write(elementCount);

            byte size = Type == SlotType.BONE ? header.BoneIndexSize : header.MorphIndexSize;

            for (int i = 0; i < elementCount; i++)
            {
                writer.Write((byte)Type);

                int id = Indices[i];
                writer.WritePmxId(size, id);
            }
        }

        public void Read(BinaryReader reader, PmxHeaderData header)
        {
            SlotName = reader.ReadText(header.Encoding);
            SlotNameE = reader.ReadText(header.Encoding);

            NormalSlot = reader.ReadByte() == 0;

            int elementCount = reader.ReadInt32();
            Indices = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                byte type = reader.ReadByte();
                byte size = type == (byte)SlotType.BONE ? header.BoneIndexSize : header.MorphIndexSize;

                Indices[i] = reader.ReadPmxId(size);
            }
        }
    }

    public enum SlotType : byte
    {
        BONE = 0,
        MORPH = 1,
    }
}
