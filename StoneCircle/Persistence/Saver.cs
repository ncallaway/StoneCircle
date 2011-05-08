﻿using System;
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

            Stack<ISaveable> toVisit = new Stack<ISaveable>(root.GetSaveableRefs(saveType));
            while (toVisit.Count != 0)
            {
                ISaveable next = toVisit.Pop();

                if (objects.ContainsKey(next.GetId())) {
                    throw new InvalidOperationException("Cannot add same object again");
                }

                objects.Add(next.GetId(), next);

                List<ISaveable> refs = next.GetSaveableRefs(saveType);
                if (refs != null)
                {
                    foreach (ISaveable saveable in refs)
                    {
                        if (objects.ContainsKey(saveable.GetId()) == false)
                        {
                            toVisit.Push(saveable);
                        }
                    }
                }
            }
        }
    }
}