using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoneCircle.Persistence
{
    public interface ISaveable
    {
        void Save(BinaryWriter writer, SaveType type);
        void Load(BinaryReader reader, SaveType type);

        List<ISaveable> GetSaveableRefs(SaveType type);
        uint GetId();
    }
}
