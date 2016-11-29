using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Asap2
{
    internal partial class Asap2Scanner
    {

        void GetNumber()
        {
            yylval.s = yytext;
            yylval.n = int.Parse(yytext);
        }

        void GetHexNumber()
        {
            yylval.s = yytext;
            yylval.n = int.Parse(yytext, NumberStyles.HexNumber);
        }

		public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
			Console.WriteLine(format, args);
			Console.WriteLine();
		}
    }
}
