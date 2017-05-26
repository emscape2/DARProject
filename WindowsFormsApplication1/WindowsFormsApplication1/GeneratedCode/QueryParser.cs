using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    class QueryParser
    {

        public static Dictionary<string, string> parseInput(string ceq)
        {
            Dictionary<string, string> input = new Dictionary<string, string>();
            string[] seperator = new string[] { "," };
            string[] attributes = ceq.Split(seperator, StringSplitOptions.None);

            foreach (string attr in attributes)
            {
                string[] valueNames = attr.Split('=');
                string column = valueNames[0];
                column = column.Replace(" ", string.Empty);
                valueNames[1] = valueNames[1].Replace(" ", string.Empty);
                valueNames[1] = valueNames[1].Replace("'", string.Empty);
                valueNames[1] = valueNames[1].Replace(";", string.Empty);

                input.Add(column, valueNames[1]);
            }
            return input;
        }



        public static Query Parse(string query)
        {
            Query toReturn;
            List<QueryAttribute> attributes = new List<QueryAttribute>();
            int k = 0;

            //neem alles achter "WHERE" 
            string[] separator = new string[] { "WHERE" };
            string everything = query.Split(separator, StringSplitOptions.None)[1];
            //split op "AND"
            separator = new string[] { "AND" };
            string[] and = everything.Split(separator, StringSplitOptions.None);
            
            //switch op "IN" en "="
            List<string> ins = new List<string>();
            List<string> isses = new List<string>();
            foreach (string str in and)
            {
                if (str.Contains("IN"))
                {
                    ins.Add(str);
                }
                else
                {
                    isses.Add(str);
                }
            }
            separator = new string[] { "IN" };
            foreach (string str in ins)
            {
                string[] In = str.Split(separator, StringSplitOptions.None);
                string column = In[0];
                column = column.Replace(" ", string.Empty);


                object[] desiredValues;
                int l = str.IndexOf("(");
                string values;
                values = str.Substring(l + 1, str.Length - (2 + l));
                desiredValues = values.Split(',');

                //retrieve the properties of any column and thereby determine the data type

                ColumnProperties properties = TableProccessor.ColumnProperties[column];

                if (properties.numerical != null && properties.numerical.Value)
                {
                    for (int i = 0; i < desiredValues.Length; i++)
                    {
                        desiredValues[i] = Convert.ToDecimal(desiredValues[i]);
                    }
                    QueryAttribute attr = new QueryAttribute(In[0], desiredValues, true);
                    attributes.Add(attr);
                }
                else
                {
                    QueryAttribute attr = new QueryAttribute(In[0], desiredValues, false);
                    attributes.Add(attr);
                }
            }

            foreach (string str in isses)
            {
                string[] Is = str.Split('=');
                string column = Is[0];
                column = column.Replace(" ", string.Empty);
                Is[1] = Is[1].Replace(" ", string.Empty);
                Is[1] = Is[1].Replace("'", string.Empty);

                //when k is given, save in variable
                if (column == "k")
                {
                    k = Convert.ToInt32(Is[1]); 
                }

                object[] desiredValues = new object[1];
                desiredValues[0] = Is[1];

                //retrieve the properties of column and determine datatype
                ColumnProperties properties = TableProccessor.ColumnProperties[column];

                if (properties.numerical != null && properties.numerical.Value)
                {
                    desiredValues[0] = Convert.ToDecimal(desiredValues[0]);
                    QueryAttribute attr = new QueryAttribute(Is[0], desiredValues[0], true);
                    attributes.Add(attr);
                }
                else
                {
                    QueryAttribute attr = new QueryAttribute(Is[0], Is[1], false);
                    attributes.Add(attr);
                }
            }
            if (k != 0)
            {
                toReturn = new Query(attributes, k);
            } 
            else
            {
                toReturn = new Query(attributes);
            }

            return toReturn;
        }
    }
}
