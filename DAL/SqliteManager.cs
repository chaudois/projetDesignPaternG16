using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SqliteManager : IDisposable
    {
        private Dictionary<string, string> SQL_CREATE = new Dictionary<string, string>{
                    {"create_contact","CREATE TABLE IF NOT EXISTS 'contact' (id INTEGER PRIMARY KEY AUTOINCREMENT, firstname TEXT, lastname TEXT );" },
                    {"create_field","CREATE TABLE IF NOT EXISTS 'field' (id INTEGER PRIMARY KEY AUTOINCREMENT, idcontact INTEGER,name TEXT,value TEXT);" }
                 };

        private SQLiteConnection connection = new SQLiteConnection("Data Source=saveFile.db;Version=3;New=False;Compress=True;");
        private SQLiteCommand command;
        private SQLiteDataAdapter adapter;
        private DataSet dataSet = new DataSet();
        public SqliteManager()
        {
            foreach (var key in SQL_CREATE.Keys)
            {
                ExecQuery(SQL_CREATE[key]);
            }

        }


        public DataSet ExecQuery(string query)
        {
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            adapter = new SQLiteDataAdapter(query, connection);
            dataSet.Reset();
            adapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }


        public void Dispose()
        {
            connection.Close();
            command.Dispose();
            dataSet = null;
        }

    }
}
