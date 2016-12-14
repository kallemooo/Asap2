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
            var tmp = yytext;
            if (tmp.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                tmp = tmp.Substring(2);
            }
            yylval.n = int.Parse(tmp, NumberStyles.HexNumber);
        }
        
        void GetAlignment()
        {
            yylval.s = yytext;
            switch (yytext)
            {
                case "ALIGNMENT_BYTE":
                    yylval.alignment_token = ALIGNMENT.ALIGNMENT_type.ALIGNMENT_BYTE;
                break;
                case "ALIGNMENT_WORD":
                    yylval.alignment_token = ALIGNMENT.ALIGNMENT_type.ALIGNMENT_WORD;
                break;
                case "ALIGNMENT_LONG":
                    yylval.alignment_token = ALIGNMENT.ALIGNMENT_type.ALIGNMENT_LONG;
                break;
                case "ALIGNMENT_INT64":
                 yylval.alignment_token = ALIGNMENT.ALIGNMENT_type.ALIGNMENT_INT64;
                break;
                case "ALIGNMENT_FLOAT32_IEEE":
                    yylval.alignment_token = ALIGNMENT.ALIGNMENT_type.ALIGNMENT_FLOAT32_IEEE;
                break;
                case "ALIGNMENT_FLOAT64_IEEE":
                    yylval.alignment_token = ALIGNMENT.ALIGNMENT_type.ALIGNMENT_FLOAT64_IEEE;
                break;
                default:
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
