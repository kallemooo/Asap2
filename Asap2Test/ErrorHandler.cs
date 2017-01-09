using System;
using Asap2;

namespace Asap2Test
{
    internal class ErrorHandler : IErrorReporter
    {
        public ErrorHandler()
        {
            warnings = 0;
            errors = 0;
        }

        public uint warnings { private set; get; }
        public uint errors { private set; get; }
        public void reportError(string message)
        {
            errors++;
            Console.WriteLine(message);
        }

        public void reportWarning(string message)
        {
            warnings++;
            Console.WriteLine(message);
        }
    }
}
