using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    class Asap2File
    {
        public ASAP2_VERSION asap2_version;
        public PROJECT project;
    }

    public class ASAP2_VERSION
    {
        public ASAP2_VERSION(UInt32 major, UInt32 minor)
        {
            this.major = major;
            this.minor = minor;
        }
        public UInt32 major;
        public UInt32 minor;
    }

    public class HEADER
    {
        public HEADER(string comment, string version, string version_no)
        {
            this.comment = comment;
            this.version = version;
            this.version_no = version_no;
        }
        public string comment;
        public string version;
        public string version_no;
    }

    public class PROJECT
    {
        public PROJECT(string name, string comment, HEADER header)
        {
            this.comment = comment;
            this.name = name;
            this.header = header;
        }
        public PROJECT(string name, string comment)
        {
            this.comment = comment;
            this.name = name;
        }
        public string name;
        public string comment;
        public HEADER header;
    }
}
