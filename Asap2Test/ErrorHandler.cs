using System;
using Asap2;

namespace Asap2Test
{
    internal class ErrorHandler : IErrorReporter
    {
        public ErrorHandler()
        {
            informations = 0;
            warnings = 0;
            errors = 0;
        }

        public uint informations { private set; get; }
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

        public void reportInformation(string message)
        {
            informations++;
            Console.WriteLine(message);
        }
    }
}
