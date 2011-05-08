using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace StoneCircle.Persistence
{
    class Saver
    {
        public void Save(ISaveable root, BinaryWriter writer, SaveType saveType)
        {
            /* 0 - Build Object Table */
            Dictionary<uint, ISaveable> objects = new Dictionary<uint, ISaveable>();

            objects.Add(root.GetId(), root);

            Stack<ISaveable> toVisit = new Stack<ISaveable>();
            addUnvisitedSaveablesToStack(objects, root.GetSaveableRefs(saveType), toVisit);

            while (toVisit.Count != 0)
            {
                ISaveable next = toVisit.Pop();

                if (objects.ContainsKey(next.GetId())) {
                    throw new InvalidOperationException("Cannot add same object again");
                }

                objects.Add(next.GetId(), next);

                List<ISaveable> refs = next.GetSaveableRefs(saveType);
                addUnvisitedSaveablesToStack(objects, refs, toVisit);
            }
        }

        private void addUnvisitedSaveablesToStack(Dictionary<uint, ISaveable> visited, List<ISaveable> saveables, Stack<ISaveable> stack)
        {
            if (saveables != null)
            {
                foreach (ISaveable saveable in saveables)
                {
                    if (visited.ContainsKey(saveable.GetId()) == false)
                    {
                        stack.Push(saveable);
                    }
                }
            }
        }
    }
}
