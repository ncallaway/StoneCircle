using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace StoneCircle.Persistence
{
    class SaveableList<T> : List<T>, ISaveable
        where T : ISaveable
    {

        private uint objectId;
        public SaveableList(uint id)
        {
            objectId = id;
            IdFactory.MoveNextIdPast(objectId);
        }


        public SaveableList()
        {
            objectId = IdFactory.GetNextId();
        }

        public void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            writer.Write(this.Count);
            for (int i = 0; i < this.Count; i++)
            {
                writer.Write(objectTable[this[i]]);
            }
        }

        private SaveableListInflatables inflatables;

        public void Load(BinaryReader reader, SaveType type)
        {
            this.Clear();

            int count = reader.ReadInt32();
            inflatables = new SaveableListInflatables();
            inflatables.objects = new List<uint>(count);

            for (int i=0; i<count; i++) {
                inflatables.objects.Add(reader.ReadUInt32());
            }

        }

        public List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> refs = new List<ISaveable>();

            foreach (ISaveable reference in this) {
                refs.Add(reference);
            }

            return refs;
        }

        public uint GetId()
        {
            return objectId;
        }

        public void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            foreach (uint id in inflatables.objects)
            {
                ISaveable reference = objectTable[id];
                this.Add((T)reference);
            }
        }

        internal class SaveableListInflatables {
            internal List<uint> objects;
        }

        public void FinishLoad()
        {
        }
    }
}
