using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace BO
{
    public class Export
    {
        private Work worker;
        private List<ContactDTO> listToExport;

        public Export(Work cSVWork, List<ContactDTO> listToExport)
        {
            this.worker = cSVWork;
            this.listToExport = listToExport;
        }

        public void ExportContact()
        {
            worker.Export(listToExport);
        }
    }
}
