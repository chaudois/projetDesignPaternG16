using DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class JSONWork:Work
    {
        public void Export(List<ContactDTO> listToExport)
        {
            string output = "[";
            foreach (ContactDTO contact in listToExport)
            {

                output += "{\"firstName\":\"" + contact.firstName + "\",";
                output += "\"lastName\":\"" + contact.lastName + "\",\"fields\":[";
                foreach (var item in contact.fields)
                {

                    output += "{\"name\":\"" + item.name + "\",";
                    output += "\"value\":\"" + item.value + "\",";
                    output += "\"idContact\":\"" + item.idContact + "\"}";
                    output += ",";
                }
                if (output[output.Length - 1] == ',')
                {
                    output = output.Remove(output.Length - 1, 1);
                }

                output += "]}";
                 output += ",";
            }
            output = output.Remove(output.Length - 1, 1);
            output += "]";

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path += "/exportJSON-";
            path += DateTime.Now.ToString("dd-MM-yyyy");
            path += ".txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                file.Write(output);
                Process.Start(path);
            }
        }
        public List<ContactDTO> Import(string data)
        {
            return JsonConvert.DeserializeObject<List<ContactDTO>>(data);
             
        }
    }
}
