using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    public abstract class Asap2Base
    {
        private static ulong orderId;

        protected ulong OrderID { get; set; }

        // Static constructor to initialize the static member, orderId. This
        // constructor is called one time, automatically, before any instance
        // of Asap2Base is created, or currentID is referenced.
        static Asap2Base()
        {
            orderId = 0;
        }

        public Asap2Base()
        {
            this.OrderID = GetOrderID();
        }

        protected ulong GetOrderID()
        {
            // currentID is a static field. It is incremented each time a new
            // instance of Asap2Base is created.
            return ++orderId;
        }
    }

    public class Asap2File : Asap2Base
    {
        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string fileComment = " Start of A2L file ";

        [Element(SortOrder = 2)]
        public ASAP2_VERSION asap2_version;

        [Element(SortOrder = 3)]
        public A2ML_VERSION a2ml_version;

        [Element(SortOrder = 4)]
        public PROJECT project;
    }

    [Base(IsSimple = true)]
    public class ASAP2_VERSION : Asap2Base
    {
        public ASAP2_VERSION(UInt32 VersionNo, UInt32 UpgradeNo)
        {
            this.VersionNo = VersionNo;
            this.UpgradeNo = UpgradeNo;
        }

        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment;
    
        [Element(IsArgument = true, SortOrder = 2)]
        public UInt32 VersionNo;
        
        [Element(IsArgument = true, SortOrder = 3)]
        public UInt32 UpgradeNo;
    }

    [Base(IsSimple = true)]
    public class A2ML_VERSION : Asap2Base
    {
        public A2ML_VERSION(UInt32 VersionNo, UInt32 UpgradeNo)
        {
            this.VersionNo = VersionNo;
            this.UpgradeNo = UpgradeNo;
        }

        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment;

        [Element(IsArgument = true, SortOrder = 2)]
        public UInt32 VersionNo;

        [Element(IsArgument = true, SortOrder = 3)]
        public UInt32 UpgradeNo;
    }

    [Base()]
    public class PROJECT : Asap2Base
    {
        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment = " The project ";

        [Element(IsArgument = true, SortOrder = 2, Comment = " Name           ")]
        public string name;
        
        [Element(IsString = true, SortOrder = 3,  Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(SortOrder = 4)]
        public HEADER header;

        /// <summary>
        /// Dictionary with the project modules. The key is the name of the module.
        /// </summary>
        [Element(IsDictionary=true, SortOrder = 5, Comment = " Project modules ")]
        public Dictionary<string, MODULE> modules = new Dictionary<string, MODULE>();
    }

    [Base(IsSimple = true)]
    public class PROJECT_NO : Asap2Base
    {
        public PROJECT_NO(string project_no)
        {
            this.project_no = project_no;
        }
        [Element(IsArgument = true)]
        public string project_no;
    }

    [Base(IsSimple = true)]
    public class VERSION : Asap2Base
    {
        public VERSION(string version)
        {
            this.version = version;
        }
        [Element(IsString = true)]
        public string version;
    }

    [Base()]
    public class HEADER : Asap2Base
    {
        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment = " Project header ";

        [Element(IsString = true, SortOrder = 2)]
        public string longIdentifier;

        [Element(SortOrder = 3)]
        public VERSION version;

        [Element(SortOrder = 4)]
        public PROJECT_NO project_no;
    }

    [Base()]
    public class MODULE : Asap2Base
    {
        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment = " Project module ";

        [Element(IsArgument = true, SortOrder = 2, Comment = " Name           ")]
        public string name;

        [Element(IsString = true, SortOrder = 3,  Comment = " LongIdentifier ")]
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
    public class ALIGNMENT : Asap2Base
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
        [Element(IsName = true, SortOrder = 1)]
        public string name;
        [Element(IsArgument = true, SortOrder = 2)]
        public uint value;
    }

    [Base(IsSimple = true)]
    public class DEPOSIT : Asap2Base
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
    public class BYTE_ORDER : Asap2Base
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
    public class DATA_SIZE : Asap2Base
    {
        public DATA_SIZE(uint value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public uint value;
    }

    [Base()]
    public class MOD_COMMON : Asap2Base
    {
        public MOD_COMMON(string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
        }

        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment = " Module default values ";

        [Element(IsString = true, SortOrder = 2, Comment = " LongIdentifier ")]
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
    public class IF_DATA : Asap2Base
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
    public class A2ML : Asap2Base
    {
        public A2ML(string data)
        {
            this.data = data;
        }
        [Element(IsArgument = true)]
        public string data;
    }

    [Base()]
    public class MEASUREMENT : Asap2Base
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
        [Element(IsComment = true, IsPreComment=true, SortOrder = 1)]
        public string comment;
        [Element(IsArgument = true, Comment = " Name           ", SortOrder = 2)]
        public string name;
        [Element(IsString = true,  Comment = " LongIdentifier ", SortOrder = 3)]
        public string LongIdentifier;
        [Element(IsArgument = true, Comment = " Datatype       ", SortOrder = 4)]
        public string Datatype;
        [Element(IsArgument = true, Comment = " Conversion     ", SortOrder = 5)]
        public string Conversion;
        [Element(IsArgument = true, Comment = " Resolution     ", SortOrder = 6)]
        public uint Resolution;
        [Element(IsArgument = true, Comment = " Accuracy       ", SortOrder = 7)]
        public uint Accuracy;
        [Element(IsArgument = true, Comment = " LowerLimit     ", SortOrder = 8)]
        public uint LowerLimit;
        [Element(IsArgument = true, Comment = " UpperLimit     ", SortOrder = 9)]
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
    public class BIT_MASK : Asap2Base
    {
        public BIT_MASK(UInt64 value)
        {
            this.value = value;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS_EXTENSION : Asap2Base
    {
        public ECU_ADDRESS_EXTENSION(UInt64 value)
        {
            this.value = value;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS : Asap2Base
    {
        public ECU_ADDRESS(UInt64 value)
        {
            this.value = value;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ADDR_EPK : Asap2Base
    {
        public ADDR_EPK(UInt64 Address)
        {
            this.Address = Address;
        }

        [Element(IsArgument = true, CodeAsHex = true)]
        public UInt64 Address;
    }
    
    [Base(IsSimple = true)]
    public class FORMAT : Asap2Base
    {
        public FORMAT(string value)
        {
            this.value = value;
        }

        [Element(IsString = true)]
        public string value;
    }

    [Base()]
    public class ANNOTATION : Asap2Base
    {
        [Element(SortOrder = 1)]
        public ANNOTATION_LABEL annotation_label;
        [Element(SortOrder = 2)]
        public ANNOTATION_ORIGIN annotation_origin;
        [Element(SortOrder = 3)]
        public ANNOTATION_TEXT annotation_text;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_LABEL : Asap2Base
    {
        public ANNOTATION_LABEL(string value)
        {
            this.value = value;
        }

        [Element(IsString = true)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_ORIGIN : Asap2Base
    {
        public ANNOTATION_ORIGIN(string value)
        {
            this.value = value;
        }

        [Element(IsString = true)]
        public string value;
    }

    [Base()]
    public class ANNOTATION_TEXT : Asap2Base
    {
        [Element(IsDictionary = true)]
        public Dictionary<string, ANNOTATION_TEXT_DATA> data = new Dictionary<string, ANNOTATION_TEXT_DATA>();
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_TEXT_DATA : Asap2Base
    {
        public ANNOTATION_TEXT_DATA(string value)
        {
            this.value = value;
        }

        [Element(IsName = true)]
        public const string name = "";

        [Element(IsString = true, Name = "")]
        public string value;
    }


    [Base(IsSimple = true)]
    public class ARRAY_SIZE : Asap2Base
    {
        public ARRAY_SIZE(ulong value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public ulong value;
    }

    [Base()]
    public class BIT_OPERATION : Asap2Base
    {
        [Element(SortOrder = 1)]
        public RIGHT_SHIFT right_shift;
        [Element(SortOrder = 2)]
        public LEFT_SHIFT left_shift;
        [Element(SortOrder = 3)]
        public SIGN_EXTEND sign_extend;
    }

    [Base(IsSimple = true)]
    public class RIGHT_SHIFT : Asap2Base
    {
        public RIGHT_SHIFT(ulong value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public ulong value;
    }
    
    [Base(IsSimple = true)]
    public class LEFT_SHIFT : Asap2Base
    {
        public LEFT_SHIFT(ulong value)
        {
            this.value = value;
        }

        [Element(IsArgument = true)]
        public ulong value;
    }

    [Base(IsSimple = true)]
    public class SIGN_EXTEND : Asap2Base
    {
    }

    [Base(IsSimple = true)]
    public class CALIBRATION_ACCESS : Asap2Base
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
    public class COMPU_VTAB : Asap2Base
    {
        public COMPU_VTAB(string Name, string LongIdentifier, uint NumberValuePairs)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.NumberValuePairs = NumberValuePairs;
        }

        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment;
        [Element(IsArgument = true, Comment = " Name           ", SortOrder = 2)]
        public string Name;
        [Element(IsString = true, Comment =  " LongIdentifier ", SortOrder = 3)]
        public string LongIdentifier;
        [Element(IsArgument = true, Comment = " ConversionType ", SortOrder = 4)]
        public string ConversionType = "TAB_VERB";
        [Element(IsArgument = true, Comment = " NumberValuePairs ", SortOrder = 5)]
        public uint NumberValuePairs;
        [Element(IsDictionary = true, SortOrder = 6)]
        public Dictionary<string, COMPU_VTAB_DATA> data = new Dictionary<string, COMPU_VTAB_DATA>();
        [Element(IsString = true, Name = "DEFAULT_VALUE", SortOrder = 7)]
        public string default_value;
    }

    [Base(IsSimple = true)]
    public class COMPU_VTAB_DATA : Asap2Base
    {
        public COMPU_VTAB_DATA(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        [Element(IsName = true)]
        public string name;

        [Element(IsString = true)]
        public string value;
    }

    [Base()]
    public class COMPU_VTAB_RANGE : Asap2Base
    {
        public COMPU_VTAB_RANGE(string Name, string LongIdentifier, uint NumberValueTriples)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.NumberValueTriples = NumberValueTriples;
        }

        [Element(IsComment = true, IsPreComment = true, SortOrder = 1)]
        public string comment;
        [Element(IsArgument = true, Comment = " Name           ", SortOrder = 2)]
        public string Name;
        [Element(IsString = true, Comment = " LongIdentifier ", SortOrder = 3)]
        public string LongIdentifier;
        [Element(IsArgument = true, Comment = " NumberValueTriples ", SortOrder = 4)]
        public uint NumberValueTriples;
        [Element(IsDictionary = true, SortOrder = 5)]
        public Dictionary<string, COMPU_VTAB_RANGE_DATA> data = new Dictionary<string, COMPU_VTAB_RANGE_DATA>();
        [Element(IsString = true, Name = "DEFAULT_VALUE", SortOrder = 6)]
        public string default_value;
    }

    [Base(IsSimple = true)]
    public class COMPU_VTAB_RANGE_DATA : Asap2Base
    {
        public COMPU_VTAB_RANGE_DATA(decimal InValMin, decimal InValMax, string value)
        {
            this.InValMin = InValMin;
            this.InValMax = InValMax;
            this.value = value;
        }

        [Element(IsName = true, SortOrder = 1)]
        public string name = "";

        [Element(IsArgument = true, SortOrder = 2)]
        public decimal InValMin;

        [Element(IsArgument = true, SortOrder = 3)]
        public decimal InValMax;

        [Element(IsString = true, SortOrder = 4)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class MATRIX_DIM : Asap2Base
    {
        public MATRIX_DIM(uint xDim, uint yDim, uint zDim)
        {
            this.xDim = xDim;
            this.yDim = yDim;
            this.zDim = zDim;
        }

        [Element(IsArgument = true, SortOrder = 1)]
        public uint xDim;
        [Element(IsArgument = true, SortOrder = 2)]
        public uint yDim;
        [Element(IsArgument = true, SortOrder = 3)]
        public uint zDim;
    }
}
