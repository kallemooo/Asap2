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
            errorMsg.AppendFormat("{0} : Line: {1} : Row: {2} : {3}", GetCurrentFilename(), yyline, yycol, string.Format(format, args));
            errorHandler.reportError(errorMsg.ToString());
        }

        private IErrorReporter errorHandler;
        private Stack<string> filenames = new Stack<string>();
        public string GetCurrentFilename()
        {
            if (filenames.Count > 0)
            {
                return filenames.Peek();
            }
            return "";
        }

        public Asap2Scanner(Stream file, IErrorReporter errorHandler, string fileName) : this(file)
        {
            this.filenames.Push(fileName);
            this.errorHandler = errorHandler;
        }
    }
}
