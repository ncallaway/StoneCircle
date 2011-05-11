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
        public class LoadResponse
        {
            public ISaveable root;
            public List<ISaveable> objects;
        }

        public static LoadResponse Load(BinaryReader reader, SaveType saveType)
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

            /* 2 - Construct the result object */
            List<ISaveable> values = new List<ISaveable>();
            foreach (ISaveable saveable in objectTable.Values)
            {
                values.Add(saveable);
            }

            LoadResponse response = new LoadResponse();
            response.root = objectTable[rootId];
            response.objects = values;

            /* 3 - Return the response object */
            return response;
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

        public static List<uint> LoadSaveableList(BinaryReader reader) {
            int count = reader.ReadInt32();
            if (count == -1)
            {
                return null;
            }

            List<UInt32> list = new List<UInt32>();
            for (int i = 0; i < count; i++)
            {
                list.Add(reader.ReadUInt32());
            }

            return list;
        }

        public static List<ISaveable> InflateSaveableList(List<uint> saveableList, Dictionary<uint, ISaveable> objectTable)
        {
            if (saveableList == null)
            {
                return null;
            }

            List<ISaveable> list = new List<ISaveable>();

            foreach (uint objectId in saveableList)
            {
                list.Add(objectTable[objectId]);
            }

            return list;
        }
    }
}
