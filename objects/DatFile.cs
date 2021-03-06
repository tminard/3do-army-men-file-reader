﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.objects
{
    public class DatFile
    {
        public List<AMObject> Objects { get; private set; }

        public Dictionary<int, Dictionary<int, List<AMObject>>> ObjectsByCatAndInstance = new Dictionary<int, Dictionary<int, List<AMObject>>>();

        public DatFile(List<AMObject> objects)
        {
            Objects = objects;
            objects.ForEach(amObject =>
            {
                if (ObjectsByCatAndInstance.ContainsKey(amObject.TypeKey))
                {
                    if (!ObjectsByCatAndInstance[amObject.TypeKey].ContainsKey(amObject.InstanceKey))
                    {
                        ObjectsByCatAndInstance[amObject.TypeKey].Add(amObject.InstanceKey, new List<AMObject>());
                    }
                    ObjectsByCatAndInstance[amObject.TypeKey][amObject.InstanceKey].Add(amObject);
                } else
                {
                    ObjectsByCatAndInstance.Add(amObject.TypeKey, new Dictionary<int, List<AMObject>>());
                    ObjectsByCatAndInstance[amObject.TypeKey].Add(amObject.InstanceKey, new List<AMObject>());

                    ObjectsByCatAndInstance[amObject.TypeKey][amObject.InstanceKey].Add(amObject);
                }
            });
        }

        public AMObject GetObject(int CategoryKey, int InstanceKey)
        {
            // TODO make this safer
            return ObjectsByCatAndInstance[CategoryKey][InstanceKey].First();
        }
    }
}
