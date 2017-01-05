using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    /// <summary>
    /// Interface for error and warning reports.
    /// </summary>
    public interface IErrorReporter
    {
        void reportWarning(string message);
        void reportError(string message);
    }
}
