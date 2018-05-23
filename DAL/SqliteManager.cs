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
    public class SqliteManager :IDisposable
    {
        public SQLiteConnection connection = new SQLiteConnection("Data Source=saveFile.db;Version=3;New=False;Compress=True;");
        private SQLiteCommand command;
        private SQLiteDataAdapter adapter;
        private DataSet dataSet=new DataSet();
        public SqliteManager()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("../../../DAL/Tables");
            foreach (var item in directoryInfo.GetFiles())
            {
                using (StreamReader sr = item.OpenText())
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        ExecQuery(line);
                    }
                }

            }
             
         }
        public DataSet ExecQuery(string query)
        {
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
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
            
        }
         
    }
}
