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
        public ASAP2_VERSION(UInt32 VersionNo, UInt32 UpgradeNo)
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

        [Element(IsDictionary = true, Comment = " MEASUREMENTs      ")]
        public Dictionary<string, MEASUREMENT> measurements = new Dictionary<string, MEASUREMENT>();

        [Element(IsDictionary = true, Comment = " COMPU_VTABs       ")]
        public Dictionary<string, COMPU_VTAB> COMPU_VTABs = new Dictionary<string, COMPU_VTAB>();

        [Element(IsDictionary = true, Comment = " COMPU_VTAB_RANGEs ")]
        public Dictionary<string, COMPU_VTAB_RANGE> COMPU_VTAB_RANGEs = new Dictionary<string, COMPU_VTAB_RANGE>();
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
            LITTLE_ENDIAN,
            BIG_ENDIAN,
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

        [Element(IsArgument = true, Name = "DISPLAY_IDENTIFIER")]
        public string display_identifier;
        [Element()]
        public ECU_ADDRESS ecu_address;
        [Element()]
        public ECU_ADDRESS_EXTENSION ecu_address_extension;
        [Element()]
        public ARRAY_SIZE array_size;
        [Element()]
        public FORMAT format;
        [Element()]
        public BIT_MASK bit_mask;
        [Element()]
        public BIT_OPERATION bit_operation;
        [Element()]
        public MATRIX_DIM matrix_dim;
        [Element()]
        public ANNOTATION annotation;
    }

    [Base(IsSimple = true)]
    public class BIT_MASK
    {
        public BIT_MASK(UInt64 value)
        {
            this.value = value;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
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
    public class ADDR_EPK
    {
        public ADDR_EPK(UInt64 Address)
        {
            this.Address = Address;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 Address;
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

    [Base()]
    public class ANNOTATION
    {
        [Element()]
        public ANNOTATION_LABEL annotation_label;
        [Element()]
        public ANNOTATION_ORIGIN annotation_origin;
        [Element()]
        public ANNOTATION_TEXT annotation_text;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_LABEL
    {
        public ANNOTATION_LABEL(string value)
        {
            this.value = value;
        }

        [Element(IsLongArg = true)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_ORIGIN
    {
        public ANNOTATION_ORIGIN(string value)
        {
            this.value = value;
        }

        [Element(IsLongArg = true)]
        public string value;
    }

    [Base()]
    public class ANNOTATION_TEXT
    {
        [Element(IsDictionary = true)]
        public Dictionary<string, ANNOTATION_TEXT_DATA> data = new Dictionary<string, ANNOTATION_TEXT_DATA>();
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_TEXT_DATA
    {
        public ANNOTATION_TEXT_DATA(string value)
        {
            this.value = value;
        }

        [Element(IsName = true)]
        public const string name = "";

        [Element(IsLongArg = true, Name = "")]
        public string value;
    }


    [Base(IsSimple = true)]
    public class ARRAY_SIZE
    {
        public ARRAY_SIZE(ulong value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public ulong value;
    }

    [Base()]
    public class BIT_OPERATION
    {
        [Element()]
        public RIGHT_SHIFT right_shift;
        [Element()]
        public LEFT_SHIFT left_shift;
        [Element()]
        public SIGN_EXTEND sign_extend;
    }

    [Base(IsSimple = true)]
    public class RIGHT_SHIFT
    {
        public RIGHT_SHIFT(ulong value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public ulong value;
    }
    
    [Base(IsSimple = true)]
    public class LEFT_SHIFT
    {
        public LEFT_SHIFT(ulong value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public ulong value;
    }

    [Base(IsSimple = true)]
    public class SIGN_EXTEND
    {
    }

    [Base(IsSimple = true)]
    public class CALIBRATION_ACCESS
    {
        public enum CALIBRATION_ACCESS_type
        {
            CALIBRATION,
            NO_CALIBRATION,
            NOT_IN_MCD_SYSTEM,
            OFFLINE_CALIBRATION,
        }
        public CALIBRATION_ACCESS(CALIBRATION_ACCESS_type value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public CALIBRATION_ACCESS_type value;
    }

    [Base()]
    public class COMPU_VTAB
    {
        public COMPU_VTAB(string Name, string LongIdentifier, uint NumberValuePairs)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.NumberValuePairs = NumberValuePairs;
        }

        [Element(IsComment = true, IsPreComment = true)]
        public string comment;
        [Element(IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(IsLongArg = true, Comment =  " LongIdentifier ")]
        public string LongIdentifier;
        [Element(IsArgument = true, Comment = " ConversionType ")]
        public string ConversionType = "TAB_VERB";
        [Element(IsArgument = true, Comment = " NumberValuePairs ")]
        public uint NumberValuePairs;
        [Element(IsDictionary = true)]
        public Dictionary<string, COMPU_VTAB_DATA> data = new Dictionary<string, COMPU_VTAB_DATA>();
        [Element(IsLongArg = true, Name = "DEFAULT_VALUE")]
        public string default_value;
    }

    [Base(IsSimple = true)]
    public class COMPU_VTAB_DATA
    {
        public COMPU_VTAB_DATA(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        [Element(IsName = true)]
        public string name;

        [Element(IsLongArg = true)]
        public string value;
    }

    [Base()]
    public class COMPU_VTAB_RANGE
    {
        public COMPU_VTAB_RANGE(string Name, string LongIdentifier, uint NumberValueTriples)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.NumberValueTriples = NumberValueTriples;
        }

        [Element(IsComment = true, IsPreComment = true)]
        public string comment;
        [Element(IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(IsLongArg = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(IsArgument = true, Comment = " NumberValueTriples ")]
        public uint NumberValueTriples;
        [Element(IsDictionary = true)]
        public Dictionary<string, COMPU_VTAB_RANGE_DATA> data = new Dictionary<string, COMPU_VTAB_RANGE_DATA>();
        [Element(IsLongArg = true, Name = "DEFAULT_VALUE")]
        public string default_value;
    }

    [Base(IsSimple = true)]
    public class COMPU_VTAB_RANGE_DATA
    {
        public COMPU_VTAB_RANGE_DATA(decimal InValMin, decimal InValMax, string value)
        {
            this.InValMin = InValMin;
            this.InValMax = InValMax;
            this.value = value;
        }

        [Element(IsName = true)]
        public string name = "";

        [Element(IsArgument = true)]
        public decimal InValMin;

        [Element(IsArgument = true)]
        public decimal InValMax;

        [Element(IsLongArg = true)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class DEFAULT_VALUE
    {
        public DEFAULT_VALUE(string value)
        {
            this.value = value;
        }

        [Element(IsLongArg = true)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class MATRIX_DIM
    {
        public MATRIX_DIM(uint xDim, uint yDim, uint zDim)
        {
            this.xDim = xDim;
            this.yDim = yDim;
            this.zDim = zDim;
        }

        [Element(IsArgument = true)]
        public uint xDim;
        [Element(IsArgument = true)]
        public uint yDim;
        [Element(IsArgument = true)]
        public uint zDim;
    }
}
