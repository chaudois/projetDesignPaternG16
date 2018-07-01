using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Import
    {
        private Work worker;
        private string dataToImport ;
 
        public Import(string importedData)
        {
            this.dataToImport = importedData;
            if(importedData[0]=='['|| importedData[0] == '{')
            {
                this.worker = new JSONWork();
            }
            else
            {
                this.worker = new CSVWork();
            }
        }
          

        public List<ContactDTO> ImportContact()
        {
            return worker.Import(dataToImport);
        }
    }
}
