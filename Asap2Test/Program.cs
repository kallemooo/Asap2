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
            var parser = new Asap2.Parser("../../../testFile.a2l");
            Asap2.Asap2File tree = parser.DoParse();
            if (tree != null)
            {
                var ms = new MemoryStream();
                parser.Serialise(tree, ms);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                var myStr = sr.ReadToEnd();
                Console.WriteLine(myStr);
            }
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
