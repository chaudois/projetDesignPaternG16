using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace BO
{
    public class CSVWork : Work
    {
        public void Export(List<ContactDTO> listToExport)
        {
            string output = "firstName,lastName,";
            List<string> fields = new List<string>();
            foreach (var contact in listToExport)
            {
                foreach (var field in contact.fields)
                {
                    if (!fields.Contains(field.name))
                    {
                        fields.Add(field.name);
                        output += field.name;
                        output += ",";

                    }
                }
            }
            output = output.Remove(output.Length - 1, 1);
            output += "\n";
            foreach (var contact in listToExport)
            {

                output += contact.firstName + "," + contact.lastName + ",";
                foreach (var field in fields)
                {
                    foreach (var contactField in contact)
                    {
                        if (contactField.name == field)
                        {
                            output += contactField.value;
                            break;
                        }
                    }
                    output += ",";


                }
                output = output.Remove(output.Length - 1, 1);
                output += "\n";
            }
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path += "/exportCSV-";
            path += DateTime.Now.ToString("dd-MM-yyyy");
            path += ".txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                file.Write(output);
                Process.Start(path);
            }
            output = "";

        }

        public List<ContactDTO> Import(string data)
        {
            List<string> lines = data.Split('\n').OfType<string>().ToList();
            lines.Remove("");
            List<ContactDTO> retour = new List<ContactDTO>();
            var lineOfFields = data.Remove(data.IndexOf('\n')).Split(','); 

            List<String> fieldsName = new List<string>();
            for (int i = 2; i < lineOfFields.Length; i++)
            {
                fieldsName.Add(lineOfFields[i]);
            }

            for (int cptLine = 1; cptLine < lines.Count; cptLine++)
            {
                var champs = lines[cptLine].Split(',');
                var contactToAdd = new ContactDTO
                {
                    firstName = champs[0],
                    lastName = champs[1]
                };
                for (int cptChamp = 2; cptChamp < champs.Length; cptChamp++)
                {
                    if (champs[cptChamp].Trim() != "")
                    {
                        contactToAdd.fields.Add(new FieldDTO
                        {
                            name = lineOfFields[cptChamp],
                            value = champs[cptChamp]
                        });
                    }
                }
                retour.Add(contactToAdd);
            }
            return retour;
        }
    }
}