using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoneCircle.Persistence
{
    
    public interface ISaveable
    {
        void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable);
        void Load(BinaryReader reader, SaveType type);
        void Inflate(Dictionary<uint, ISaveable> objectTable);
        void FinishLoad();

        List<ISaveable> GetSaveableRefs(SaveType type);
        uint GetId();
    }
}
