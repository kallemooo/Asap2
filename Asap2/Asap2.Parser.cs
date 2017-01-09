using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Asap2
{
    internal partial class Asap2Parser
    {
        private IErrorReporter errorHandler;
        public Asap2File Asap2File = new Asap2File();
        public Asap2Parser(Asap2Scanner scanner, IErrorReporter errorHandler) : base(scanner)
        {
            this.errorHandler = errorHandler;
        }

        public void yywarning(string format, params object[] args)
        {
            StringBuilder errorMsg = new StringBuilder();
            errorMsg.AppendFormat("{0} : Line: {1} : Row: {2} : {3}", this.CurrentLocationSpan.FileName, this.CurrentLocationSpan.StartLine, this.CurrentLocationSpan.StartColumn, string.Format(format, args));
            errorHandler.reportWarning(errorMsg.ToString());
        }

    }
}
