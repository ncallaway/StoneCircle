using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace StoneCircle.Persistence
{
    class Saver
    {
        public static readonly uint VALUE_TYPE_ID = 0;

        public void Save(ISaveable root, BinaryWriter writer, SaveType saveType)
        {
            /* 0 - Build Object Table */
            Dictionary<uint, ISaveable> objects = buildObjectTable(root, saveType);

            /* 1 - Persist Object Table */
            persistObjectTable(root, writer, saveType, objects);
        }

        private void persistObjectTable(ISaveable root, BinaryWriter writer, SaveType type, Dictionary<uint, ISaveable> objectTable)
        {
            foreach (KeyValuePair<uint, ISaveable> pair in objectTable)
            {
                writeHeader(pair.Key, pair.Value, root, writer);
                pair.Value.Save(writer, type);
            }
        }

        private void writeHeader(uint id, ISaveable saveable, ISaveable root, BinaryWriter writer)
        {
            writer.Write(id);
            writer.Write(TypeConverter.getTypeId(saveable));
            writer.Write((saveable == root));
        }

        private Dictionary<uint, ISaveable> buildObjectTable(ISaveable root, SaveType saveType)
        {
            Dictionary<uint, ISaveable> objects = new Dictionary<uint, ISaveable>();

            objects.Add(root.GetId(), root);

            Stack<ISaveable> toVisit = new Stack<ISaveable>();
            addUnvisitedSaveablesToStack(objects, root.GetSaveableRefs(saveType), toVisit);

            while (toVisit.Count != 0)
            {
                ISaveable next = toVisit.Pop();

                if (objects.ContainsKey(next.GetId()))
                {
                    throw new InvalidOperationException("Cannot add same object again");
                }

                if (next.GetId() != VALUE_TYPE_ID)
                {
                    objects.Add(next.GetId(), next);
                }

                List<ISaveable> refs = next.GetSaveableRefs(saveType);
                addUnvisitedSaveablesToStack(objects, refs, toVisit);
            }

            return objects;
        }

        

        private void addUnvisitedSaveablesToStack(Dictionary<uint, ISaveable> visited, List<ISaveable> saveables, Stack<ISaveable> stack)
        {
            if (saveables != null)
            {
                foreach (ISaveable saveable in saveables)
                {
                    if (saveable.GetId() == VALUE_TYPE_ID ||visited.ContainsKey(saveable.GetId()) == false)
                    {
                        stack.Push(saveable);
                    }
                }
            }
        }

        public static void SaveStringList(List<String> stringList, BinaryWriter writer)
        {
            if (stringList == null)
            {
                writer.Write(-1);
                return;
            }

            writer.Write(stringList.Count);

            foreach (String s in stringList)
            {
                writer.Write(s);
            }
        }
    }
}
