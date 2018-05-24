using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Data
    {
        public static Dictionary<string, string> SQL_CREATE = new Dictionary<string, string>
        {
            {"create_contact","CREATE TABLE IF NOT EXISTS 'contact' (id INTEGER PRIMARY KEY AUTOINCREMENT, firstname TEXT, lastname TEXT, adresse TEXT,mail TEXT,phonenum TEXTE);" }
        };
    }
}
