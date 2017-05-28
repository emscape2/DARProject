using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;

/*
 * Executequery class executes queries for Database 
 */

namespace WindowsFormsApplication1
{
    class Executequery
    {
        //Execute given query on given Database
        public static void Execute(string query, string nameDatabase)
        {
            DatabaseConnection database = new DatabaseConnection(nameDatabase);
            DataTable result = database.QueryForDataTable(query);
        }

    }
}
