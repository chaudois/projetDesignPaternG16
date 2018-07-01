using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public interface Work
    {
        void Export(List<ContactDTO> listtoexport);
        List<ContactDTO> Import(string data);
    }
}
