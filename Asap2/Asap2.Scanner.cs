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
            yylval.n = long.Parse(yytext);
        }

        void GetHexNumber()
        {
            yylval.s = yytext;
            var tmp = yytext;
            if (tmp.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                tmp = tmp.Substring(2);
            }
            yylval.n = long.Parse(tmp, NumberStyles.HexNumber);
        }

        void GetDouble()
        {
            yylval.s = yytext;
            yylval.d = long.Parse(yytext);
        }

        void GetAlignment()
        {
            yylval.s = yytext;
            try
            {
                yylval.alignment_token = (ALIGNMENT.ALIGNMENT_type)Enum.Parse(typeof(ALIGNMENT.ALIGNMENT_type), yytext);        
            }
            catch (ArgumentException)
            {
                throw new Exception("Unknown ALIGNMENT type: " + yytext);
            }
        }

        public override void yyerror(string format, params object[] args)
        {
            base.yyerror(format, args);
            Console.WriteLine(format, args);
            Console.WriteLine();
        }
    }
}
