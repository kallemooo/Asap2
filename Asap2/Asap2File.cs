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

    public class PROJECT
    {
        public PROJECT(string name, string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
            this.name = name;
        }
        public string name;
        public string LongIdentifier;
        public HEADER header;
        public Dictionary<string, MODULE> modules = new Dictionary<string, MODULE>();
    }

    public class HEADER
    {
        public HEADER(string comment, string version, string project_no)
        {
            this.comment = comment;
            this.version = version;
            this.project_no = project_no;
        }
        public string comment;
        public string version;
        public string project_no;
    }

    public class MODULE
    {
        public MODULE(string name, string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
            this.name = name;
        }
        public string name;
        public string LongIdentifier;
        public MOD_COMMON mod_common;
        public Dictionary<string, MEASUREMENT> measurements = new Dictionary<string, MEASUREMENT>();
    }

    public enum ALIGNMENT_type
    {
          ALIGNMENT_BYTE,
          ALIGNMENT_WORD,
          ALIGNMENT_LONG,
          ALIGNMENT_INT64,
          ALIGNMENT_FLOAT32_IEEE,
          ALIGNMENT_FLOAT64_IEEE        
    }

    public enum DEPOSIT
    {
        ABSOLUTE,
        DIFFERENCE,
    }

    public enum BYTE_ORDER
    {
        MSB_FIRST,
        MSB_LAST,
    }

    public class MOD_COMMON
    {
        public MOD_COMMON(string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
        }

        public string LongIdentifier;
        public DEPOSIT? deposit;
        public BYTE_ORDER? byte_order;
        public Dictionary<ALIGNMENT_type, uint> alignments = new Dictionary<ALIGNMENT_type, uint>();
        public uint? data_size;
    }

    public class MEASUREMENT
    {
        public MEASUREMENT(string name, string LongIdentifier, string Datatype, string Conversion, uint Resolution, uint Accuracy, uint LowerLimit, uint UpperLimit)
        {
            this.name = name;
            this.LongIdentifier = LongIdentifier;
            this.Datatype = Datatype;
            this.Conversion = Conversion;
            this.Resolution = Resolution;
            this.Accuracy = Accuracy;
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        public string name;
        public string LongIdentifier;
        public string Datatype;
        public string Conversion;
        public uint Resolution;
        public uint Accuracy;
        public uint LowerLimit;
        public uint UpperLimit;
        public UInt64? ECU_ADDRESS;
        public UInt64? ECU_ADDRESS_EXTENSION;
        public String FORMAT;
    }

}
