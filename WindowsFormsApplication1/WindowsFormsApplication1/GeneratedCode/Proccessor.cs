using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;

using System.Text;
using System.Threading;


/*
 * Processor class handles connection between several C# 
 * programs for receiving input, executing SQlite statements on a metaDatabase and retrieving metaDatabase results. 
 */
namespace WindowsFormsApplication1
{
    public class Proccessor
    {
        /// <summary>
        /// recieves the CEQ query and Processes it and its results
        /// </summary>
        /// <param name="ceq"></param>
        public static void LoadAndProccessCeq(String ceq)
        {
            Dictionary<string, string> parsedCeq = QueryParser.parseInput(ceq);
            DataTable rows = CheckIfSingleOption(parsedCeq);
            
            DataTable table = TableProccessor.RetrieveTable();
            Dictionary<DataRow, double> ratings = new Dictionary< DataRow, double>( );
            Dictionary<string, Tuple<object, double, double>> elements = new Dictionary<string, Tuple<object, double, double>>();

            
            //retrieve k
            int k = 10;
            if (parsedCeq.ContainsKey("k"))
            {
                k = Convert.ToInt32(parsedCeq["k"]);
                parsedCeq.Remove("k");
            }

            if (rows != null)
            {
                parsedCeq = new Dictionary<string, string>();
                foreach(DataColumn col in rows.Columns)
                {
                    if(col.ColumnName != "id")
                        parsedCeq.Add(col.ColumnName, rows.Rows[0][col].ToString());
                }
            }

            //prepare each element in the parsed CEQ for rating calculation
            foreach (var elem in parsedCeq)
            {
                double idf = getIDF(elem.Value, elem.Key);
                double qf = getQF(elem.Value, elem.Key);
                Tuple<object, double, double> element = new Tuple<object, double, double>(elem.Value, idf, qf);
                //needs parsing of int64 and double i believe
                elements.Add(elem.Key, element);
            }
            foreach (DataRow row in table.Rows)
            {
                ratings.Add( row, GetSimilarity(elements, row));//add rating
            }


            var results = ratings.ToList();//convert to sorted list
            results.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));//TODO More efficient top k, idf and qf values can be used as well as distance for numerical and plausibly even jaccard

            List<DataRow> toReturn = new List<DataRow>();
            for (int i = 0; i < k; i++)
            {
                toReturn.Add(results[i].Key);//add top 10 rows
            }

            //create form
            Results form = new Results();
            form.Show();
            DataTable resultTable = table.Clone();
            
            foreach (DataRow row in toReturn)
            {
                resultTable.ImportRow(row);
                Thread.Sleep(10);// for animation
            }
            form.BindData(resultTable);
        }

        public static DataTable CheckIfSingleOption(            Dictionary<string, string> ParsedSeq)
        {

            Dictionary<string, string> temp = new Dictionary<string, string>(ParsedSeq);
            if (temp.ContainsKey("k"))
                temp.Remove("k");
            string SQL = "";
            foreach(var el in temp)
            {
                SQL += " " + el.Key + " = '" + el.Value + "' AND ";
            }
            SQL = SQL.Remove(SQL.Length - 4);

            DataTable table = TableProccessor.connection.QueryForDataTable("SELECT * FROM autompg WHERE" + SQL + " ;");
            if (table.Rows.Count == 1)
            {
                return table;
            }


            return null;


        }


        /// <summary>
        /// gets the similarity between the row and the query
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static double GetSimilarity(Dictionary<string, Tuple<object,double,double>> elements, DataRow row)
        {
            double ranking = 0;

            foreach (var elem in elements)
            {
                if (elem.Value.Item1 as string == row[elem.Key] as string) //if values are the same
                {
                    ranking += elem.Value.Item2 * elem.Value.Item3;
                }
                else if (TableProccessor.ColumnProperties[elem.Key].numerical.HasValue && TableProccessor.ColumnProperties[elem.Key].numerical.Value == false)
                {//when non numerical use the jaccard if available, if not just 0
                    ranking += elem.Value.Item2 * elem.Value.Item3 * getJaqcuard(elem.Value.Item1 as string, row[elem.Key] as string, elem.Key );
                }
                else
                {//else get similarity 
                    ranking += elem.Value.Item2 * elem.Value.Item3 * getSim(Convert.ToDouble(elem.Value.Item1), Convert.ToDouble(row[elem.Key]), elem.Key);
                }
            }

            return ranking;
        }


        /// <summary>
        /// gets the jaccard value of two strings
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="columname"></param>
        /// <returns></returns>
        public static double getJaqcuard(string value, string value2, string columname)
        {
            if (!MetaDbFiller.jacquard.ContainsKey(columname))
                return 0;
            var jaquardTable = MetaDbFiller.jacquard[columname] as Dictionary<string, object>;
            if (!jaquardTable.ContainsKey(value))
                return 0;
            Dictionary<string, double> column = jaquardTable[value] as Dictionary<string, double>;
            if (!column.ContainsKey(value2))
                return 0;
            return column[value2];
        }

        /// <summary>
        /// Gets numerical similarity by determing the weight of the distance between two values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="columname"></param>
        /// <returns></returns>
        public static double getSim(double value, double value2, string columname)
        {
            ColumnProperties properties = TableProccessor.ColumnProperties[columname];
            double h = properties.max - properties.min;
            double dist = Math.Abs(value2 - value);

            //TODO take the distribution into account
            return 1.0 -(dist / h);
        }

        /// <summary>
        /// Gets the QF for a value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columname"></param>
        /// <returns></returns>
        public static double getQF(string value, string columname)
        {
            ColumnProperties properties = TableProccessor.ColumnProperties[columname];
            if (!properties.numerical.HasValue || properties.numerical.Value)
            {
               // if (properties.numerical.HasValue)
                {
                    double u = Convert.ToDouble(value.Replace('.', ','));
                    double start = 0 , end;
                    if (properties.min > u || properties.max < u)
                    {
                        return 0;//TODO recalculate when outside of saved values
                    }
                    double interval = properties.GetInterval();
                    for (double d = properties.min; d <= u; d += interval)
                    {
                        start = d;
                    }
                    end = start + interval;

                    double qf1, qf2;
                    qf1 = (MetaDbFiller.qf[columname] as Dictionary<double, double>)[start];
                    qf2 = (MetaDbFiller.qf[columname] as Dictionary<double, double>)[end];
                    double div = (u - start) / interval;// interpolate between the two closest values
                    qf1 *= div; 
                    qf2 *= 1.0 - div;
                    return qf2 + qf1;
                }

            }
            Dictionary<string, double> qfs = MetaDbFiller.qf[columname] as Dictionary<string, double>;
            if (!qfs.ContainsKey(value))
                return 0; // When query frequency is nill
            return qfs[value];
        }

        /// <summary>
        /// gets the IDF for certain value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columname"></param>
        /// <returns></returns>
        public static double getIDF(string value, string columname)
        {
            //todo take into account non existing values
            ColumnProperties properties = TableProccessor.ColumnProperties[columname];
            if (!properties.numerical.HasValue || properties.numerical.Value)
            {
                //if (properties.numerical.HasValue)
                {
                    double u = Convert.ToDouble(value.Replace('.',',' ));
                    double start = 0, end;
                    if (properties.min > u)
                    {
                        return 0;//TODO recalculate
                    }
                    double interval = properties.GetInterval();
                    for (double d = properties.min; d <= u; d += interval)
                    {
                        start = d;
                    }
                    end = start + interval;

                    double idf1, idf2;
                    idf1 = (MetaDbFiller.idfs[columname] as Dictionary<double, double>)[start];
                    idf2 = (MetaDbFiller.idfs[columname] as Dictionary<double, double>)[end];
                    double div = (u - start) / interval;
                    idf1 *= div;//interpolate
                    idf2 *= 1.0 - div;
                    return idf2 + idf1;
                }
                //return 0;
            }
            Dictionary<string, double> idfs = MetaDbFiller.idfs[columname] as Dictionary<string, double>;
            if (!idfs.ContainsKey(value))
                return 0;//should not occur though
            return idfs[value];
        }


        public static void Execute(string query, string nameDatabase)
        {
            Executequery.Execute(query, nameDatabase);
        }

    }
}
