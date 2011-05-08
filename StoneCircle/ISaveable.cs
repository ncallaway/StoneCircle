using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoneCircle
{
    public interface ISaveable
    {
        void FullSave(BinaryWriter writer);
        void IncrementalSave(BinaryWriter writer);

        void Reset(BinaryReader fullSave, BinaryReader incrementalSave);
    }

    public static class SaveHelper
    {
        public static void Save(List<String> data, BinaryWriter writer)
        {
            if (data == null)
            {
                writer.Write(-1);
                return;
            }

            writer.Write(data.Count);

            foreach (String s in data)
            {
                writer.Write(s);
            }
        }

        public static List<String> LoadStringList(BinaryReader reader)
        {
            List<String> list = null;

            int count = reader.ReadInt32();

            if (count == -1)
            {
                return list;
            }

            list = new List<String>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(reader.ReadString());
            }

            return list;
        }
    }
}
