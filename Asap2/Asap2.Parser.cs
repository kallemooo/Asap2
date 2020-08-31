using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Asap2
{
    internal partial class Asap2Parser
    {
        private readonly IErrorReporter errorHandler;
        public Asap2File Asap2File = new Asap2File();
        public Asap2Parser(Asap2Scanner scanner, IErrorReporter errorHandler) : base(scanner)
        {
            this.errorHandler = errorHandler;
        }

        public void yywarning(string format, params object[] args)
        {
            StringBuilder errorMsg = new StringBuilder();
            errorMsg.AppendFormat("{0} : Line: {1} : Column: {2} : {3}", this.CurrentLocationSpan.FileName, this.CurrentLocationSpan.StartLine, this.CurrentLocationSpan.StartColumn, string.Format(format, args));
            errorHandler.reportWarning(errorMsg.ToString());
        }

        private object EnumToStringOrAbort(Type type, string strIn)
        {
            try
            {
                return Enum.Parse(type, strIn);
            }
            catch (ArgumentException e)
            {
                StringBuilder values = new StringBuilder();
                string[] myArray = Enum.GetNames(type);
                foreach (var item in myArray)
                {
                    if (values.Length > 0)
                    {
                        values.Append(", ");
                    }
                    values.Append(item);
                }
                Scanner.yyerror(String.Format("Syntax error: Unknown '{0}' enum value '{1}' expecting one of '{2}'", type.ToString(), strIn, values.ToString()));
                YYAbort();
                throw;
            }
        }
    }
}
