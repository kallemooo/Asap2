using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace Asap2
{
    internal partial class Asap2Scanner
    {
        public override void yyerror(string format, params object[] args)
        {
            StringBuilder errorMsg = new StringBuilder();
            errorMsg.AppendFormat("{0} : Line: {1} : Row: {2} : {3}", this.buffer.FileName, yyline, yycol, string.Format(format, args));
            errorHandler.reportError(errorMsg.ToString());
        }

        private IErrorReporter errorHandler;
        private Stack<Stream> StreamStack = new Stack<Stream>();

        public Asap2Scanner(Stream file, IErrorReporter errorHandler) : this(file)
        {
            this.errorHandler = errorHandler;
        }
    }
}
