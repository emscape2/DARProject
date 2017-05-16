using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

public class DatabaseConnection
{
    public SQLiteConnection m_dbConnection;

    public DatabaseConnection(string location)
    {
        m_dbConnection =
new SQLiteConnection("Data Source=" + location + ";Version=3;");
        m_dbConnection.Open();


    }

    public static DatabaseConnection CreateEmptyDb(string location)
    {
        SQLiteConnection.CreateFile(location);
        return new DatabaseConnection(location);

    }

    public int getKey(string table, string id)
    {
        object[] temp = QueryDatabase("SELECT MAX(" + id + ") FROM " + table + ";", true);
        if (temp == null || temp.Length == 0 || temp[0].ToString() == "")
        {
            return 0;
        }
        return Convert.ToInt32(temp[0]) + 1;
    }
    public int getKey(string table)
    {
        return getKey(table, "id");
    }



    public object[] QueryDatabase(string query, bool isReadQuery)
    {
        var dbcmd = m_dbConnection.CreateCommand();
        dbcmd.CommandText = query;
        var reader = dbcmd.ExecuteReader();
        object[] output = null;
        if (isReadQuery)
        {
            var values = new List<object>();

            int current = 0;
            while (reader.Read())
            {


                if (reader.HasRows)
                {
                    current = 0;
                }
                values.Add(reader.GetValue(current++));

            }
            output = values.ToArray();
        }
        return output;
    }
}

