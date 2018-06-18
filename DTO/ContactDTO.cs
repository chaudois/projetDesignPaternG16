using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ContactDTO:ICloneable
    { 

        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<FieldDTO> fields = new List<FieldDTO>();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
