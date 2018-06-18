using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class SqlSingleton
    {
        private static SqliteManager sqlInstance = new SqliteManager();
        public static SqliteManager  getInstance()
        {
            return sqlInstance;
        }
    }
}
