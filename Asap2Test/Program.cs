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
            parser.DoParse();
            //Console.WriteLine("Press enter to close...");
            //Console.ReadLine();
        }
    }
}
