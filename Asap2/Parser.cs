using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Asap2
{
    public class Parser
    {
        public Parser(string fileName)
        {
            this.fileName = fileName;
        }

        public string fileName {get; private set;}

        public bool DoParse()
        {
            bool status = false;
            using (var file = new FileStream(fileName, FileMode.Open))
            {
                Asap2Scanner scanner = new Asap2Scanner(file);
                Asap2Parser parser = new Asap2Parser(scanner);
                if (parser.Parse())
                {
                    status = true;
                }
                else
                {
                    Console.WriteLine("Line {0} {1}", scanner.line_num, scanner.chars_num);
                    status = false;
                }
            }

            return status;
        }
    }
}
