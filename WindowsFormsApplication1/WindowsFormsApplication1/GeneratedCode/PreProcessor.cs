﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

public class PreProcessor
{
    /// <summary>
    /// used for processing the create statements of the database
    /// </summary>
    /// <param name="reader"></param>
    public static void ProcessTables(StreamReader reader)
    {

        string rawSql = reader.ReadToEnd();
        TableProccessor.CreateAndFillTable(rawSql);
    }
    
    /// <summary>
    /// Loads tables and calculates IDF values and other essentials
    /// </summary>
    public static void LoadTableAndPreprocess()
    {
        TableProccessor.RetrieveTable();
        TableProccessor.CalculateColumnProperties();
        TableProccessor.Process();
    }

    /// <summary>
    /// Processes the workload supplied by the .txt file
    /// </summary>
    /// <param name="Workload"></param>
	public static void ProccessWorkload(StreamReader Workload)
	{
        string temp = Workload.ReadToEnd();
        SQLQuery[] queries = WorkloadParser.Parse(temp);
        WorkloadProcessor.Process(queries);
        
	}

}

