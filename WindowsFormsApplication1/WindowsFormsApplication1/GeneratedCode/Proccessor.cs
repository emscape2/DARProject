using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Proccessor
    {

        public static void LoadAndProccessCeq(String ceq)
        {
            Dictionary<string, string> parsedCeq = QueryParser.parseInput(ceq);
            DataTable table = TableProccessor.RetrieveTable();
            Dictionary<DataRow, double> ratings = new Dictionary<DataRow, double>();
            Dictionary<string, Tuple<object, double, double>> elements = new Dictionary<string, Tuple<object, double, double>>();
            
            foreach (var elem in parsedCeq)
            {
                double idf = getIDF(elem.Value, elem.Key);
                double qf = getQF(elem.Value, elem.Key);
                Tuple<object, double, double> element = new Tuple<object, double, double>(elem.Value, idf, qf);
                //needs parsing of int64 and double i believe
                elements.Add(elem.Key, element);
            }


            foreach(DataRow row in table.Rows)
            {
                ratings.Add(row, GetSimilarity(elements, row));
            }

            int i = 0;
            
        }



        public static double GetSimilarity(Dictionary<string, Tuple<object,double,double>> elements, DataRow row)
        {
            double ranking = 0;

            foreach (var elem in elements)
            {
                if (elem.Value.Item1 as string == row[elem.Key] as string)
                {
                    ranking += elem.Value.Item2 * elem.Value.Item3;
                }
                else if (TableProccessor.ColumnProperties[elem.Key].numerical.HasValue && TableProccessor.ColumnProperties[elem.Key].numerical.Value == false)
                {
                    ranking += elem.Value.Item2 * elem.Value.Item3 * getJaqcuard(elem.Value.Item1 as string, row[elem.Key] as string, elem.Key );
                }
                else
                {
                    ranking += elem.Value.Item2 * elem.Value.Item3 * getSim(Convert.ToDouble(elem.Value.Item1), Convert.ToDouble(row[elem.Key]), elem.Key);
                }
            }

            return ranking;
        }

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

        public static double getSim(double value, double value2, string columname)
        {
            ColumnProperties properties = TableProccessor.ColumnProperties[columname];
            double h = properties.max - properties.min;
            double dist = value2 - value;


            //todo numerical equivalent of jaccard
            return 1.0 - (dist / h);
        }

        public static double getQF(string value, string columname)
        {
            ColumnProperties properties = TableProccessor.ColumnProperties[columname];
            if (properties.numerical.HasValue || properties.numerical.Value)
            {
                if (properties.numerical.HasValue)
                {
                    double u = Convert.ToDouble(value);
                    double start = 0 , end;
                    if (properties.min > u)
                    {
                        return 0;//TODO recalculate
                    }
                    double interval = properties.GetInterval();
                    for (double d = properties.min; d < u; d += interval)
                    {
                        start = d;
                    }
                    end = start + interval;

                    double qf1, qf2;
                    qf1 = (MetaDbFiller.qf[columname] as Dictionary<double, double>)[start];//rekening met end
                    qf2 = (MetaDbFiller.qf[columname] as Dictionary<double, double>)[end];
                    double div = (u - start) / interval;
                    qf1 *= div;
                    qf2 *= 1.0 - div;
                    return qf2 + qf1;
                }
                    return 0;
            }
            Dictionary<string, double> qfs = MetaDbFiller.qf[columname] as Dictionary<string, double>;
            if (!qfs.ContainsKey(value))
                return 0;
            return qfs[value];
        }


        public static double getIDF(string value, string columname)
        {
            //todo take into account non existing values
            ColumnProperties properties = TableProccessor.ColumnProperties[columname];
            if (properties.numerical.HasValue || properties.numerical.Value)
            {
                if (properties.numerical.HasValue)
                {
                    double u = Convert.ToDouble(value);
                    double start = 0, end;
                    if (properties.min > u)
                    {
                        return 0;//TODO recalculate
                    }
                    double interval = properties.GetInterval();//TODO do only once 
                    for (double d = properties.min; d < u; d += interval)
                    {
                        start = d;
                    }
                    end = start + interval;

                    double idf1, idf2;
                    idf1 = (MetaDbFiller.idfs[columname] as Dictionary<double, double>)[start];
                    idf2 = (MetaDbFiller.idfs[columname] as Dictionary<double, double>)[end];
                    double div = (u - start) / interval;
                    idf1 *= div;
                    idf2 *= 1.0 - div;
                    return idf2 + idf1;
                }
                return 0;
            }
            Dictionary<string, double> idfs = MetaDbFiller.idfs[columname] as Dictionary<string, double>;
            if (!idfs.ContainsKey(value))
                return 0;
            return idfs[value];
        }


        public static void Execute(string query, string nameDatabase)
        {
            Executequery.Execute(query, nameDatabase);
        }

    }
}
