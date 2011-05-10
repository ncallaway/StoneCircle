using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneCircle.Persistence
{
    public static class TypeConverter
    {
        private const int STAGE_MANAGER_ID = 1;
        private const int STAGE_ID = 2;
        private const int STAGE_LIST_ID = 3;

        public static int GetTypeId(ISaveable saveable)
        {
            if (saveable.GetType() == typeof(StageManager))
            {
                return STAGE_MANAGER_ID;
            }
            else if (saveable.GetType() == typeof(Stage))
            {
                return STAGE_ID;
            }
            else if (saveable.GetType() == typeof(SaveableList<Stage>))
            {
                return STAGE_LIST_ID;
            }

            throw new KeyNotFoundException("Can't find id for type: " + saveable.GetType());
        }

        public static ISaveable ConstructSaveableFromTypeId(int typeId, uint objectId)
        {
            switch (typeId)
            {
                case STAGE_MANAGER_ID:
                    return new StageManager(objectId);
                case STAGE_ID:
                    return new Stage(objectId);
                case STAGE_LIST_ID:
                    return new SaveableList<Stage>(objectId);
            }

            throw new KeyNotFoundException("Can't construct saveable from id : " + typeId);
        }
    }
}
