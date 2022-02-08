using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace Asap2
{
    internal partial class Asap2Scanner
    {
        private readonly Stack<BufferContext> buffStack = new Stack<BufferContext>();

        public override void yyerror(string format, params object[] args)
        {
            StringBuilder errorMsg = new StringBuilder();
            errorMsg.AppendFormat("{0} : Line: {1} : Column: {2} : {3}", this.buffer.FileName, yyline, yycol, string.Format(format, args));
            errorHandler.reportError(errorMsg.ToString());
        }

        private readonly IErrorReporter errorHandler;
        private readonly Stack<Stream> streamStack = new Stack<Stream>();

        public Asap2Scanner(Stream file, IErrorReporter errorHandler) : this(file)
        {
            this.errorHandler = errorHandler;
        }

        public int MakeNumber()
        {
            yylval.s = yytext;
            decimal.TryParse(yytext, NumberStyles.Float, CultureInfo.InvariantCulture, out yylval.d);
            yylloc = new Location(yyline, yycol, yyline, yycol + yyleng, this.buffer.FileName);
            return (int)Token.NUMBER;
        }

        public int MakeHexNumber()
        {
            yylval.s = yytext;
            var tmp = yytext;
            if (tmp.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                tmp = tmp.Substring(2);
            }
            yylval.d = long.Parse(tmp, NumberStyles.HexNumber);
            yylloc = new Location(yyline, yycol, yyline, yycol + yyleng, this.buffer.FileName);
            return (int)Token.NUMBER;
        }

        public int MakeAlignment()
        {
            yylloc = new Location(yyline, yycol, yyline, yycol + yyleng, this.buffer.FileName);
            yylval.s = yytext;
            try
            {
                yylval.alignment_token = (ALIGNMENT.ALIGNMENT_type)Enum.Parse(typeof(ALIGNMENT.ALIGNMENT_type), yytext);
            }
            catch (ArgumentException)
            {
                throw new ParserErrorException("{0} : Line: {1} : Row: {2} : Syntax error, Unknown ALIGNMENT type: '{3}'", this.buffer.FileName, yyline, yycol, yytext);
            }
            return (int)Token.ALIGNMENT;
        }

        public int MakeStringBuilder(Token token)
        {
            yylval.s = yylval.sb.ToString();
            yylloc = new Location(yyline, yycol, yyline, yycol + yyleng, this.buffer.FileName);
            return (int)token;
        }

        public int Make(Token token)
        {
            yylval.s = yytext;
            yylloc = new Location(yyline, yycol, yyline, yycol + yyleng, this.buffer.FileName);
            return (int)token;
        }

        private void TryInclude(string fName)
        {
            if (fName == null)
            {
                throw new ParserErrorException("{0} : Line: {1} : Row: {2} : Include error, /include, no filename", this.buffer.FileName, yyline, yycol);
            }

            try
            {
                /* Trim any leading and trailing whitespaces and " */
                fName = fName.Trim();
                char[] charsToTrim = { '\"', '\'' };
                fName = fName.Trim(charsToTrim);

                BufferContext savedCtx = MkBuffCtx();
                if (!Path.IsPathRooted(fName))
                {
                    /* Handle relative search path for the new file. */
                    fName = Path.Combine(Path.GetDirectoryName(this.buffer.FileName), fName);
                }
                var stream = new FileStream(fName, FileMode.Open);
                SetSource(stream);
                errorHandler.reportInformation(string.Format("{0} : Line: {1} : Row: {2} : Included file \"{3}\" opened", this.buffer.FileName, yyline, yycol, fName));
                buffStack.Push(savedCtx); // Don't push until file open succeeds!
                /* Push the new stream to a separate stack so it can be properly disposed. */
                streamStack.Push(stream);
            }
            catch
            {
                throw new ParserErrorException("{0} : Line: {1} : Row: {2} : Include error, /include, could not open file \"{3}\"", this.buffer.FileName, yyline, yycol, fName);
            }
        }

        protected override bool yywrap()
        {
            if (buffStack.Count == 0)
            {
                return true;
            }

            /* Dispose the stream so the same file can be opened again. */
            Stream currentStream = streamStack.Pop();
            currentStream.Dispose();
            RestoreBuffCtx(buffStack.Pop());

            return false;
        }
    }
}
