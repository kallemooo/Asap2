using System.Collections.Generic;

namespace Asap2.Tests
{
    public class UnitTestErrorReporter : IErrorReporter
    {
        private readonly List<string> warnings = new List<string>();
        private readonly List<string> errors = new List<string>();
        private readonly List<string> information = new List<string>();

        public IReadOnlyList<string> Warnings 
        {
            get { return warnings; }
        }

        public IReadOnlyList<string> Errors
        {
            get { return errors; }
        }

        public IReadOnlyList<string> Information
        {
            get { return information; }
        }

        public void reportWarning(string message)
        {
            warnings.Add(message);
        }

        public void reportError(string message)
        {
            errors.Add(message);
        }

        public void reportInformation(string message)
        {
            information.Add(message);
        }
    }
}