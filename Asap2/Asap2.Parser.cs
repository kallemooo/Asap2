using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Asap2
{


    internal partial class Asap2Parser
    {
        public Asap2File Asap2File = new Asap2File();
        public Asap2Parser(Asap2Scanner scanner) : base(scanner)
        {
        }
    }
}
