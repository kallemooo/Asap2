using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    public class Asap2File
    {
        [Element(IsComment = true, IsPreComment = true)]
        public string fileComment = " Start of A2L file ";

        [Element()]
        public ASAP2_VERSION asap2_version;

        [Element()]
        public A2ML_VERSION a2ml_version;

        [Element()]
        public PROJECT project;
    }

    [Base(IsSimple = true)]
    public class ASAP2_VERSION
    {
        public ASAP2_VERSION(UInt32 major, UInt32 minor)
        {
            this.major = major;
            this.minor = minor;
        }

        [Element(IsComment = true, IsPreComment = true)]
        public string comment;
    
        [Element(IsArgument = true)]
        public UInt32 major;
        
        [Element(IsArgument = true)]
        public UInt32 minor;
    }

    [Base(IsSimple = true)]
    public class A2ML_VERSION
    {
        public A2ML_VERSION(UInt32 VersionNo, UInt32 UpgradeNo)
        {
            this.VersionNo = VersionNo;
            this.UpgradeNo = UpgradeNo;
        }

        [Element(IsComment = true, IsPreComment = true)]
        public string comment;

        [Element(IsArgument = true)]
        public UInt32 VersionNo;

        [Element(IsArgument = true)]
        public UInt32 UpgradeNo;
    }

    [Base()]
    public class PROJECT
    {
        [Element(IsComment = true, IsPreComment = true)]
        public string comment = " The project ";

        [Element(IsArgument = true, Comment = " Name           ")]
        public string name;
        
        [Element(IsLongArg = true,  Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element()]
        public HEADER header;

        /// <summary>
        /// Dictionary with the project modules. The key is the name of the module.
        /// </summary>
        [Element(IsDictionary=true, Comment = " Project modules ")]
        public Dictionary<string, MODULE> modules = new Dictionary<string, MODULE>();
    }

    [Base(IsSimple = true)]
    public class PROJECT_NO
    {
        public PROJECT_NO(string project_no)
        {
            this.project_no = project_no;
        }
        [Element(IsArgument = true)]
        public string project_no;
    }

    [Base(IsSimple = true)]
    public class VERSION
    {
        public VERSION(string version)
        {
            this.version = version;
        }
        [Element(IsLongArg = true)]
        public string version;
    }

    [Base()]
    public class HEADER
    {
        [Element(IsComment = true, IsPreComment = true)]
        public string comment = " Project header ";

        [Element(IsLongArg = true)]
        public string longIdentifier;

        [Element()]
        public VERSION version;

        [Element()]
        public PROJECT_NO project_no;
    }

    [Base()]
    public class MODULE
    {
        [Element(IsComment = true, IsPreComment = true)]
        public string comment = " Project module ";

        [Element(IsArgument = true, Comment = " Name           ")]
        public string name;

        [Element(IsLongArg = true,  Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(IsDictionary = true)]
        public Dictionary<string, A2ML> A2MLs = new Dictionary<string, A2ML>();

        [Element(IsDictionary = true)]
        public Dictionary<string, IF_DATA> IF_DATAs = new Dictionary<string, IF_DATA>();

        [Element()]
        public MOD_COMMON mod_common;

        [Element(IsDictionary = true)]
        public Dictionary<string, MEASUREMENT> measurements = new Dictionary<string, MEASUREMENT>();
    }

    [Base(IsSimple = true)]
    public class ALIGNMENT
    {
        public enum ALIGNMENT_type
        {
            ALIGNMENT_BYTE,
            ALIGNMENT_WORD,
            ALIGNMENT_LONG,
            ALIGNMENT_INT64,
            ALIGNMENT_FLOAT32_IEEE,
            ALIGNMENT_FLOAT64_IEEE
        }
        public ALIGNMENT(ALIGNMENT_type type, uint value)
        {
            this.type = type;
            this.value = value;
            this.name = Enum.GetName(type.GetType(), type);
        }

        public ALIGNMENT_type type;
        [Element(IsName = true)]
        public string name;
        [Element(IsArgument = true)]
        public uint value;
    }

    [Base(IsSimple = true)]
    public class DEPOSIT
    {
        public enum DEPOSIT_type
        {
            ABSOLUTE,
            DIFFERENCE,
        }
        public DEPOSIT(DEPOSIT_type value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public DEPOSIT_type value;
    }

    [Base(IsSimple = true)]
    public class BYTE_ORDER
    {
        public enum BYTE_ORDER_type
        {
            MSB_FIRST,
            MSB_LAST,
        }
        public BYTE_ORDER(BYTE_ORDER_type value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public BYTE_ORDER_type value;
    }

    [Base(IsSimple = true)]
    public class DATA_SIZE
    {
        public DATA_SIZE(uint value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public uint value;
    }

    [Base()]
    public class MOD_COMMON
    {
        public MOD_COMMON(string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
        }

        [Element(IsComment = true, IsPreComment = true)]
        public string comment = " Module default values ";

        [Element(IsLongArg = true,  Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element()]
        public DEPOSIT deposit;
        
        [Element()]
        public BYTE_ORDER byte_order;

        [Element(IsDictionary = true)]
        public Dictionary<string, ALIGNMENT> alignments = new Dictionary<string, ALIGNMENT>();

        [Element()]
        public DATA_SIZE data_size;
    }

    [Base()]
    public class IF_DATA
    {
        public IF_DATA(string data)
        {
            this.data = data;
            char[] delimiterChars = { ' ', '\t' };
            string[] words = data.Split(delimiterChars);
            this.name = words[0];
        }
        public string name;
        [Element(IsArgument = true)]
        public string data;
    }

    [Base()]
    public class A2ML
    {
        public A2ML(string data)
        {
            this.data = data;
        }
        [Element(IsArgument = true)]
        public string data;
    }

    [Base()]
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
        [Element(IsComment = true, IsPreComment=true)]
        public string comment;
        [Element(IsArgument = true, Comment = " Name           ")]
        public string name;
        [Element(IsLongArg = true,  Comment = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(IsArgument = true, Comment = " Datatype       ")]
        public string Datatype;
        [Element(IsArgument = true, Comment = " Conversion     ")]
        public string Conversion;
        [Element(IsArgument = true, Comment = " Resolution     ")]
        public uint Resolution;
        [Element(IsArgument = true, Comment = " Accuracy       ")]
        public uint Accuracy;
        [Element(IsArgument = true, Comment = " LowerLimit     ")]
        public uint LowerLimit;
        [Element(IsArgument = true, Comment = " UpperLimit     ")]
        public uint UpperLimit;
        [Element()]
        public ECU_ADDRESS ecu_address;
        [Element()]
        public ECU_ADDRESS_EXTENSION ecu_address_extension;
        [Element()]
        public FORMAT format;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS_EXTENSION
    {
        public ECU_ADDRESS_EXTENSION(UInt64 value)
        {
            this.value = value;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS
    {
        public ECU_ADDRESS(UInt64 value)
        {
            this.value = value;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class FORMAT
    {
        public FORMAT(string value)
        {
            this.value = value;
        }

        [Element(IsLongArg = true)]
        public string value;
    }
}
