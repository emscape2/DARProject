using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace WindowsFormsApplication1.GeneratedCode
{
    class Executequery
    {
        public static void Execute(string query, string nameDatabase)
        {
            DatabaseConnection database = new DatabaseConnection(nameDatabase);
            DataTable result = database.QueryForDataTable(query);
        }

    }
}
