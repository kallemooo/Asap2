using System;
using Asap2;

namespace Asap2Test
{
    internal class ErrorHandler : IErrorReporter
    {
        public void reportError(string message)
        {
            Console.WriteLine(message);
        }

        public void reportWarning(string message)
        {
            Console.WriteLine(message);
        }
    }
}
