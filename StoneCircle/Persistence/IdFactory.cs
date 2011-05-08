using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneCircle.Persistence
{
    static class IdFactory
    {
        private static uint nextId = 0;

        public static uint GetNextId()
        {
            uint id = nextId;
            nextId++;
            return id;
        }

        public static void MoveNextIdPast(uint id)
        {
            if (nextId <= id)
            {
                nextId = id + 1;
            }
        }

    }
}
