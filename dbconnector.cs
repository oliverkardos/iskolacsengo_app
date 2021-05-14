using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite; // from nuget pkg
using System.Data.SqlClient;

namespace iskolacsengo
{
    class dbconnector
    {
        // db connector for sqlite
        /*
      * This class is used to connect to the local sqlite db
      * The file name is fetched from the user
      * 
      * 
      *////// FORKED FROM UrbanApp @ oliverkardos
        public string db_filepath;
        private SQLiteConnection con;
        public dbconnector(string file_path)
        {
            db_filepath = file_path;
            con = new SQLiteConnection("data source=" + db_filepath);
        }

        public SQLiteConnection GetConnection()
        {
            return con;
        }
        public void openConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void closeConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }


    }
}
