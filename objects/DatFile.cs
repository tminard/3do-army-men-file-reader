using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.objects
{
    public class DatFile
    {
        public List<AMObject> Objects { get; private set; }

        public Dictionary<int, Dictionary<int, AMObject>> ObjectsByCatAndInstance = new Dictionary<int, Dictionary<int, AMObject>>();

        public DatFile(List<AMObject> objects)
        {
            Objects = objects;
            objects.ForEach(amObject =>
            {
                if (ObjectsByCatAndInstance.ContainsKey(amObject.TypeKey))
                {
                    if (ObjectsByCatAndInstance[amObject.TypeKey].ContainsKey(amObject.InstanceKey))
                    {
                        ObjectsByCatAndInstance[amObject.TypeKey].Remove(amObject.InstanceKey); // dat files sometimes overwrite older versions
                    }
                    ObjectsByCatAndInstance[amObject.TypeKey].Add(amObject.InstanceKey, amObject);
                } else
                {
                    ObjectsByCatAndInstance.Add(amObject.TypeKey, new Dictionary<int, AMObject>());
                    ObjectsByCatAndInstance[amObject.TypeKey].Add(amObject.InstanceKey, amObject);
                }
            });
        }

        public AMObject GetObject(int CategoryKey, int InstanceKey)
        {
            // TODO make this safer
            return ObjectsByCatAndInstance[CategoryKey][InstanceKey];
        }
    }
}
