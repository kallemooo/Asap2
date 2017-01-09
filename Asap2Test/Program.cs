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
                    if (errorHandler.warnings == 0)
                    {
                        Console.WriteLine("Parsed file with no warnings.");
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Parsed file with {0} warnings.", errorHandler.warnings));
                    }

                    tree.Validate(errorHandler);
                    
                    if (errorHandler.warnings == 0)
                    {
                        Console.WriteLine("Validated parsed data with no warnings.");
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Validated parsed data with {0} warnings.", errorHandler.warnings));
                    }

                    Console.WriteLine("Press enter to serialise data.");
                    Console.ReadLine();

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
                    Console.WriteLine("Validation of parsed data failed!");
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Console.WriteLine("Parsing failed!");
            }
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
