using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Asap2
{
    internal partial class Asap2Scanner
    {
        public override void yyerror(string format, params object[] args)
        {
            StringBuilder errorMsg = new StringBuilder();
            errorMsg.AppendFormat("Line: {0} : Row: {1} : {2}", yyline, yycol, string.Format(format, args));

            Console.WriteLine(errorMsg);
            Console.WriteLine();
        }

        public Stack<string> filenames = new Stack<string>();
        public string GetCurrentFilename()
        {
            if (filenames.Count > 0)
            {
                return filenames.Peek();
            }
            return "";
        }
    }
}
