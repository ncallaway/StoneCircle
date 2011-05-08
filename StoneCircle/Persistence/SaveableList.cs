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

        public void Save(BinaryWriter writer, SaveType type)
        {
            throw new NotImplementedException();
        }

        public void Load(BinaryReader reader, SaveType type)
        {
            throw new NotImplementedException();
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
            return Saver.VALUE_TYPE_ID;
        }


        public void Inflate()
        {
            throw new NotImplementedException();
        }
    }
}
