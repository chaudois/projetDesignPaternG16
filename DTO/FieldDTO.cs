using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FieldDTO
    {
        public string name { get; set ; }
        public string value { get; set; }
        public int idContact { get; set; }

        internal string jsonify()
        {
            string retour = "{\"name\":\"" + name + "\",";
            retour += "\"value\":\"" + value + "\",";
            retour += "\"idContact\":\"" + idContact + "\"}";
            return retour;
        }
    }
}
