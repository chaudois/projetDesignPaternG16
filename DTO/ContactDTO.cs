using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ContactDTO : ICloneable, IEnumerator
    {

        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public object Current => getCurrent();
        public List<FieldDTO> fields = new List<FieldDTO>();

        private int cpt = 0;
        private object getCurrent()
        {
            if (cpt == 0)
            {
                return new FieldDTO
                {
                    idContact = id,
                    name = "FirstName",
                    value = firstName
                };
            }
            else if (cpt == 1)
            {
                return new FieldDTO
                {
                    idContact = id,
                    name = "LastName",
                    value = lastName
                };
            }else if (cpt - 2 < fields.Count())
            {
                return fields[cpt - 2];
            }
            return null;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public IEnumerator<FieldDTO> GetEnumerator()
        {
            List<FieldDTO> retour = new List<FieldDTO>
            {
                new FieldDTO{idContact=id,name="FirstName",value=firstName },
                new FieldDTO{idContact=id,name="LastName",value=lastName }
            };
            foreach (var item in fields)
            {
                retour.Add(item);
            }
            return retour.GetEnumerator();
        }

        public bool MoveNext()
        {
            return ++cpt < fields.Count() + 2;
        }

        public void Reset()
        {
            cpt = 0;
        }
    }
}
