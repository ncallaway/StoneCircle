using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneCircle.Persistence
{
    public static class TypeConverter
    {
        private const int STAGE_MANAGER_ID = 1;

        public static int getTypeId(ISaveable saveable)
        {
            if (saveable.GetType() == typeof(StageManager))
            {
                return STAGE_MANAGER_ID;
            }

            throw new KeyNotFoundException("Can't find id for type: " + saveable.GetType());
        }

        public static ISaveable constructSaveableFromTypeId(int typeId, uint objectId)
        {
            switch (typeId)
            {
                case STAGE_MANAGER_ID:
                    return new StageManager(objectId);
            }

            throw new KeyNotFoundException("Can't construct saveable from id : " + typeId);
        }
    }
}
