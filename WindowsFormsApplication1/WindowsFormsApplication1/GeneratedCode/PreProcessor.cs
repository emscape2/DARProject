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

public class PreProcessor
{
    public static void ProcessTables(StreamReader Tables)
    {
        string all = Tables.ReadToEnd();
        all.Replace("\n", "");
        string[] lines = all.Split(';');
        TableCreator.CreateTable(lines[0]);
        TableCreator.FillTable((List<string>)lines.Reverse().Take(lines.Length - 1));
    }

	public static void ProcessQuerys(StreamReader Workload)
	{
        string temp = Workload.ReadToEnd();
        Query[] queries = WorkloadParser.Parse(temp);
        WorkloadProcessor.Process(queries);
	}

}

