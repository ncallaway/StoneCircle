using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Xna.Framework;

namespace StoneCircle.Persistence
{
    static class Saver
    {
        public static readonly uint VALUE_TYPE_ID = 0;

        public static void Save(ISaveable root, BinaryWriter writer, SaveType saveType)
        {
            /* 0 - Build Object Table */
            Dictionary<uint, ISaveable> objects = buildObjectTable(root, saveType);

            /* 1 - Persist Object Table */
            persistObjectTable(root, writer, saveType, objects);
        }

        private static void persistObjectTable(ISaveable root, BinaryWriter writer, SaveType type, Dictionary<uint, ISaveable> objectTable)
        {
            if (objectTable == null)
            {
                writer.Write(-1);
                return;
            }

            Dictionary<ISaveable, uint> reverseTable = reverseObjectTable(objectTable);

            writer.Write(objectTable.Count);
            foreach (KeyValuePair<uint, ISaveable> pair in objectTable)
            {
                writeHeader(pair.Key, pair.Value, root, writer);
                if (pair.Value != null)
                {
                    pair.Value.Save(writer, type, reverseTable);
                }
            }
        }

        private static Dictionary<ISaveable, uint> reverseObjectTable(Dictionary<uint, ISaveable> objectTable)
        {
            Dictionary<ISaveable, uint> reverseTable = new Dictionary<ISaveable, uint>();
            foreach (KeyValuePair<uint, ISaveable> pair in objectTable)
            {
                if (reverseTable.ContainsKey(pair.Value))
                {
                    throw new NotSupportedException("Cannot have multiple saveables in the object table");
                }
                reverseTable.Add(pair.Value, pair.Key);
            }

            return reverseTable;
        }

        private static void writeHeader(uint id, ISaveable saveable, ISaveable root, BinaryWriter writer)
        {
            writer.Write(id);
            writer.Write(saveable.GetType().AssemblyQualifiedName);
            writer.Write((saveable == root));
        }

        private static Dictionary<uint, ISaveable> buildObjectTable(ISaveable root, SaveType saveType)
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

        

        private static void addUnvisitedSaveablesToStack(Dictionary<uint, ISaveable> visited, List<ISaveable> saveables, Stack<ISaveable> stack)
        {
            if (saveables != null)
            {
                foreach (ISaveable saveable in saveables)
                {
                    if (saveable.GetId() == VALUE_TYPE_ID ||  visited.ContainsKey(saveable.GetId()) == false)
                    {
                        if (stack.Contains(saveable) == false)
                        {
                            stack.Push(saveable);
                        }
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

        public static void SaveSaveableList(List<ISaveable> saveableList, BinaryWriter writer, Dictionary<ISaveable, uint> objectTable)
        {
            if (saveableList == null)
            {
                writer.Write(-1);
                return;
            }

            writer.Write(saveableList.Count);

            foreach (ISaveable s in saveableList)
            {
                writer.Write(objectTable[s]);
            }
        }

        public static void SaveSaveableList<T>(List<T> saveableList, BinaryWriter writer, Dictionary<ISaveable, uint> objectTable)
            where T : ISaveable
        {
            if (saveableList == null)
            {
                writer.Write(-1);
                return;
            }

            writer.Write(saveableList.Count);

            foreach (T s in saveableList)
            {
                writer.Write(objectTable[s]);
            }
        }

        public static void SaveString(String s, BinaryWriter writer)
        {
            writer.Write(s != null);
            if (s != null)
                writer.Write(s);
        }

        public static void SaveColor(Color c, BinaryWriter writer)
        {
            writer.Write(c.R);
            writer.Write(c.G);
            writer.Write(c.B);
            writer.Write(c.A);
        }

        public static void SaveVector2(Vector2 v, BinaryWriter writer)
        {
            writer.Write(v.X);
            writer.Write(v.Y);
        }

        public static void SaveVector3(Vector3 v, BinaryWriter writer)
        {
            writer.Write(v.X);
            writer.Write(v.Y);
            writer.Write(v.Z);
        }

        public static List<ISaveable> ConstructSaveableList(List<ISaveable> reference)
        {
            if (reference != null) {
                return reference;
            }

            reference = new List<ISaveable>();
            return reference;
        }
    }
}
