using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using System.IO;

namespace StoneCircle.Persistence
{
    static class Loader
    {
        public static ISaveable Load(BinaryReader reader, SaveType saveType)
        {
            /* 0 - Depersist object table. */
            uint rootId = 0;
            bool rootFound = false;
            Dictionary<uint, ISaveable> objectTable = new Dictionary<uint, ISaveable>();

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                uint objectId = reader.ReadUInt32();
                String assemblyQualifiedTypeName = reader.ReadString();
                bool isRoot = reader.ReadBoolean();
                if (isRoot && rootFound)
                {
                    throw new NotSupportedException("More than one root object defined in the save file!");
                }
                else if (isRoot)
                {
                    rootFound = true;
                    rootId = objectId;
                }

                ISaveable loadedObject = constructType(assemblyQualifiedTypeName, objectId);
                loadedObject.Load(reader, saveType);

                objectTable.Add(loadedObject.GetId(), loadedObject);
            }

            if (rootFound == false)
            {
                throw new NotSupportedException("No root object defined in the save file!");
            }

            /* 1 - Inflate all objects */
            inflateObjects(objectTable, saveType);

            /* 3 - Return the root object */
            return objectTable[rootId];
        }

        private static ISaveable constructType(String assemblyQualifiedName, uint objectId)
        {
            Type saveableType = Type.GetType(assemblyQualifiedName);
            Type[] neededConstructorParams = { typeof(UInt32) };
            ConstructorInfo availableConstructor = saveableType.GetConstructor(neededConstructorParams);

            if (availableConstructor == null)
            {
                throw new Exception("Attempted to load an ISaveable with no object id constructor. Type: " + saveableType);
            }
            Object[] b = { objectId };

            Object result = availableConstructor.Invoke(b);

            return (ISaveable)result; /* Could be an illegal conversion! */
        }

        private static void inflateObjects(Dictionary<uint, ISaveable> objectTable, SaveType type)
        {
            foreach (KeyValuePair<uint, ISaveable> pair in objectTable)
            {
                pair.Value.Inflate(objectTable);
            }
        }

        public static List<String> LoadStringList(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count == -1)
            {
                return null;
            }

            List<String> list = new List<String>();
            for (int i = 0; i < count; i++)
            {
                list.Add(reader.ReadString());
            }

            return list;
        }

        //public static void SaveStringList(List<String> stringList, BinaryWriter writer)
        //{
        //    if (stringList == null)
        //    {
        //        writer.Write(-1);
        //        return;
        //    }

        //    writer.Write(stringList.Count);

        //    foreach (String s in stringList)
        //    {
        //        writer.Write(s);
        //    }
        //}
    }
}
