using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if (ObjectsByCatAndInstance.ContainsKey(CategoryKey) && ObjectsByCatAndInstance[CategoryKey].ContainsKey(InstanceKey))
            {
                return ObjectsByCatAndInstance[CategoryKey][InstanceKey].First();
            } else { return null; }
        }

        public List<AMObject> GetPlaceableObjects()
        {
            return Objects
                .Where(obj => obj.Placeable == true)
                .Where(obj => obj.InstanceSequence == 0)
                .ToList();
        }
    }
}
