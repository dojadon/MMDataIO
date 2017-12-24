using System;
using System.IO;

namespace CsMmdDataIO.Pmx
{
    [Serializable]
    public class PmxMorphData : IPmxData
    {
        public string MorphName { get; set; } = "";
        public string MorphNameE { get; set; } = "";
        public MorphSlotType SlotType { get; set; }
        public MorphType MorphType { get; set; }

        public IPmxMorphTypeData[] MorphArray { get; set; }

        public object Clone() => new PmxMorphData()
        {
            MorphName = MorphName,
            MorphNameE = MorphNameE,
            SlotType = SlotType,
            MorphArray = CloneUtil.CloneArray(MorphArray),
        };

        public void Write(BinaryWriter writer, PmxHeaderData header)
        {
            writer.WriteText(header.Encoding, MorphName);
            writer.WriteText(header.Encoding, MorphNameE);

            writer.Write((byte)SlotType);

            writer.Write((byte)MorphType);

            writer.Write(MorphArray.Length);

            for (int i = 0; i < MorphArray.Length; i++)
            {
                MorphArray[i].Write(writer, header);
            }
        }

        public void Parse(BinaryReader reader, PmxHeaderData header)
        {
            MorphName = reader.ReadText(header.Encoding);
            MorphNameE = reader.ReadText(header.Encoding);

            SlotType = (MorphSlotType)reader.ReadByte();

            MorphType = (MorphType)reader.ReadByte();

            int elementCount = reader.ReadInt32();
            MorphArray = new IPmxMorphTypeData[elementCount];

            Func<IPmxMorphTypeData> factory = () => null;

            switch (MorphType)
            {
                case MorphType.GROUP:
                    factory = () => new PmxMorphGroupData();
                    break;

                case MorphType.VERTEX:
                    factory = () => new PmxMorphVertexData();
                    break;

                case MorphType.BONE:
                    factory = () => new PmxMorphBoneData();
                    break;

                case MorphType.UV:
                case MorphType.EXUV1:
                case MorphType.EXUV2:
                case MorphType.EXUV3:
                case MorphType.EXUV4:
                    factory = () => new PmxMorphUVData();
                    break;

                case MorphType.MATERIAL:
                    factory = () => new PmxMorphMaterialData();
                    break;
            }

            for (int i = 0; i < MorphArray.Length; i++)
            {
                MorphArray[i] = factory();
                MorphArray[i].Parse(reader, header);
            }
        }
    }

    public enum MorphSlotType : byte
    {
        SYSTEM = 0,
        EYEBROW = 1,
        EYE = 2,
        MOUTH = 3,
        RIP = 4,
    }

    public enum MorphType : byte
    {
        GROUP = 0,
        VERTEX = 1,
        BONE = 2,
        UV = 3,
        EXUV1 = 4,
        EXUV2 = 5,
        EXUV3 = 6,
        EXUV4 = 7,
        MATERIAL = 8
    }
}
