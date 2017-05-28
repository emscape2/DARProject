using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;

public class DatabaseConnection
{
    public SQLiteConnection m_dbConnection;

    public DatabaseConnection(string location)
    {
        m_dbConnection = new SQLiteConnection("Data Source=" + location + ";Version=3;");
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

    public void runCreationSql(string rawSql)
    {
        //TODO testen of alle statements ingevuld worden
        using (SQLiteConnection objConnection = m_dbConnection)
        {
            using (SQLiteCommand objCommand = objConnection.CreateCommand())
            {
                if(objConnection.State != ConnectionState.Open)
                {
                    objConnection.Open();
                }
                objCommand.CommandText = rawSql;
                objCommand.ExecuteNonQuery();
                objConnection.Close();
            } 
        } 
    }

    public void runCreationSqlMeta(string rawSql)
    {
        runCreationSql(rawSql); 
    }

    /// <summary>
    /// create a DataTable containing results of the query, if the DB is opened
    /// </summary>
    /// <param name="query"></param>
    public  DataTable QueryForDataTable(string query)
    {
        DataTable dataTable = new DataTable();
        SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
        SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
        dataAdapter.Fill(dataTable);
        return dataTable;
    }

    /// <summary>
    /// processes a batch of non-read queries
    /// </summary>
    /// <param name="sqls"></param>
    public  void ProcessQueries(List<string> sqls)
    {
        int rowsAffected = 0;
        foreach (var sql in sqls)
        {
            SQLiteCommand cmd = m_dbConnection.CreateCommand();
            cmd.CommandText = sql;
            rowsAffected += cmd.ExecuteNonQuery();
        }
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

