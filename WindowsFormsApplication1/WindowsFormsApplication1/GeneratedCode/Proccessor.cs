using System;
using System.Collections.Generic;
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
        }

        public static void Execute(string query, string nameDatabase)
        {
            Executequery.Execute(query, nameDatabase);
        }

    }
}
