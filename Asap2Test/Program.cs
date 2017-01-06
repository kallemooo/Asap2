using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Asap2Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var errorHandler = new ErrorHandler();
            var parser = new Asap2.Parser("../../../testFile.a2l", errorHandler);
            Asap2.FileComment comment = new Asap2.FileComment(Environment.NewLine + "A2l file for testing ASAP2 parser." + Environment.NewLine, true);
            Asap2.Asap2File tree = parser.DoParse();
            if (tree != null)
            {
                try
                {
                    tree.Validate(errorHandler);
                    tree.elements.Insert(0, comment);
                    var ms = new MemoryStream();
                    parser.Serialise(tree, ms);
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var myStr = sr.ReadToEnd();
                    Console.WriteLine(myStr);
                }
                catch (Asap2.ValidationErrorException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
