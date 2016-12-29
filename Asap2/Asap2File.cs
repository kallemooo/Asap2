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

        public ulong OrderID { get; protected set; }

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
        [Element(0, IsComment = true, IsPreComment = true)]
        public string fileComment = " Start of A2L file ";

        [Element(1)]
        public ASAP2_VERSION asap2_version;

        [Element(2)]
        public A2ML_VERSION a2ml_version;

        [Element(3)]
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

        [Element(0, IsArgument = true)]
        public UInt32 VersionNo;

        [Element(1, IsArgument = true)]
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

        [Element(0, IsArgument = true)]
        public UInt32 VersionNo;

        [Element(1, IsArgument = true)]
        public UInt32 UpgradeNo;
    }

    [Base()]
    public class PROJECT : Asap2Base
    {
        public PROJECT()
        {
            modules = new Dictionary<string, MODULE>();
        }

        [Element(0, IsArgument = true, Comment = " Name           ")]
        public string name;

        [Element(1, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(2)]
        public HEADER header;

        /// <summary>
        /// Dictionary with the project modules. The key is the name of the module.
        /// </summary>
        [Element(3, IsDictionary = true)]
        public Dictionary<string, MODULE> modules;
    }

    [Base(IsSimple = true)]
    public class PROJECT_NO : Asap2Base
    {
        public PROJECT_NO(string project_no)
        {
            this.project_no = project_no;
        }
        [Element(0, IsArgument = true)]
        public string project_no;
    }

    [Base(IsSimple = true)]
    public class VERSION : Asap2Base
    {
        public VERSION(string version)
        {
            this.version = version;
        }
        [Element(0, IsString = true)]
        public string version;
    }

    [Base()]
    public class HEADER : Asap2Base
    {
        [Element(0, IsString = true)]
        public string longIdentifier;

        [Element(1)]
        public VERSION version;

        [Element(2)]
        public PROJECT_NO project_no;
    }

    [Base()]
    public class MODULE : Asap2Base
    {
        public MODULE()
        {
            measurements = new Dictionary<string, MEASUREMENT>();
            COMPU_VTABs = new Dictionary<string, COMPU_VTAB>();
            COMPU_VTAB_RANGEs = new Dictionary<string, COMPU_VTAB_RANGE>();
        }

        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string name;

        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(3, IsList = true)]
        public List<A2ML> A2MLs = new List<A2ML>();

        [Element(4, IsDictionary = true)]
        public Dictionary<string, IF_DATA> IF_DATAs = new Dictionary<string, IF_DATA>();

        [Element(5)]
        public MOD_COMMON mod_common;

        [Element(6, IsDictionary = true, Comment = " Measurment data for the module ")]
        public Dictionary<string, MEASUREMENT> measurements;

        [Element(7, IsDictionary = true, Comment = " Verbal conversion tables for the module ")]
        public Dictionary<string, COMPU_VTAB> COMPU_VTABs;

        [Element(8, IsDictionary = true, Comment = " Verbal conversion tables with parameter ranges for the module ")]
        public Dictionary<string, COMPU_VTAB_RANGE> COMPU_VTAB_RANGEs;
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
        [Element(0, IsName = true)]
        public string name;
        [Element(1, IsArgument = true)]
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

        [Element(0, IsArgument = true)]
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

        [Element(0, IsArgument = true)]
        public BYTE_ORDER_type value;
    }

    [Base(IsSimple = true)]
    public class DATA_SIZE : Asap2Base
    {
        public DATA_SIZE(uint value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public uint value;
    }

    [Base()]
    public class MOD_COMMON : Asap2Base
    {
        public MOD_COMMON(string LongIdentifier)
        {
            alignments = new Dictionary<string, ALIGNMENT>();
            this.LongIdentifier = LongIdentifier;
        }

        [Element(0, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(1)]
        public DEPOSIT deposit;

        [Element(2)]
        public BYTE_ORDER byte_order;

        [Element(3, IsDictionary = true)]
        public Dictionary<string, ALIGNMENT> alignments;

        [Element(4)]
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
        [Element(0, IsArgument = true)]
        public string data;
    }

    [Base()]
    public class A2ML : Asap2Base
    {
        public A2ML(string data)
        {
            this.data = data;
        }
        [Element(0, IsArgument = true)]
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
        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string name;
        [Element(2, IsString = true, Comment   = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " Datatype       ")]
        public string Datatype;
        [Element(4, IsArgument = true, Comment = " Conversion     ")]
        public string Conversion;
        [Element(5, IsArgument = true, Comment = " Resolution     ")]
        public uint Resolution;
        [Element(6, IsArgument = true, Comment = " Accuracy       ")]
        public uint Accuracy;
        [Element(7, IsArgument = true, Comment = " LowerLimit     ")]
        public uint LowerLimit;
        [Element(8, IsArgument = true, Comment = " UpperLimit     ")]
        public uint UpperLimit;

        [Element(9, IsArgument = true, Name = "DISPLAY_IDENTIFIER")]
        public string display_identifier;
        [Element(10)]
        public ECU_ADDRESS ecu_address;
        [Element(11)]
        public ECU_ADDRESS_EXTENSION ecu_address_extension;
        [Element(12)]
        public ARRAY_SIZE array_size;
        [Element(13)]
        public FORMAT format;
        [Element(14)]
        public BIT_MASK bit_mask;
        [Element(15)]
        public BIT_OPERATION bit_operation;
        [Element(16)]
        public MATRIX_DIM matrix_dim;
        [Element(17)]
        public ANNOTATION annotation;
    }

    [Base(IsSimple = true)]
    public class BIT_MASK : Asap2Base
    {
        public BIT_MASK(UInt64 value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS_EXTENSION : Asap2Base
    {
        public ECU_ADDRESS_EXTENSION(UInt64 value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS : Asap2Base
    {
        public ECU_ADDRESS(UInt64 value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ADDR_EPK : Asap2Base
    {
        public ADDR_EPK(UInt64 Address)
        {
            this.Address = Address;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 Address;
    }

    [Base(IsSimple = true)]
    public class FORMAT : Asap2Base
    {
        public FORMAT(string value)
        {
            this.value = value;
        }

        [Element(0, IsString = true)]
        public string value;
    }

    [Base()]
    public class ANNOTATION : Asap2Base
    {
        [Element(0)]
        public ANNOTATION_LABEL annotation_label;
        [Element(1)]
        public ANNOTATION_ORIGIN annotation_origin;
        [Element(2)]
        public ANNOTATION_TEXT annotation_text;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_LABEL : Asap2Base
    {
        public ANNOTATION_LABEL(string value)
        {
            this.value = value;
        }

        [Element(0, IsString = true)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_ORIGIN : Asap2Base
    {
        public ANNOTATION_ORIGIN(string value)
        {
            this.value = value;
        }

        [Element(0, IsString = true)]
        public string value;
    }

    [Base()]
    public class ANNOTATION_TEXT : Asap2Base
    {
        [Element(0, IsString = true, IsList = true)]
        public List<string> data = new List<string>();
    }

    [Base(IsSimple = true)]
    public class ARRAY_SIZE : Asap2Base
    {
        public ARRAY_SIZE(ulong value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public ulong value;
    }

    [Base()]
    public class BIT_OPERATION : Asap2Base
    {
        [Element(0)]
        public RIGHT_SHIFT right_shift;
        [Element(2)]
        public LEFT_SHIFT left_shift;
        [Element(3)]
        public SIGN_EXTEND sign_extend;
    }

    [Base(IsSimple = true)]
    public class RIGHT_SHIFT : Asap2Base
    {
        public RIGHT_SHIFT(ulong value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public ulong value;
    }

    [Base(IsSimple = true)]
    public class LEFT_SHIFT : Asap2Base
    {
        public LEFT_SHIFT(ulong value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
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

        [Element(0, IsArgument = true)]
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
            data = new List<COMPU_VTAB_DATA>();
        }

        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " ConversionType ")]
        public string ConversionType = "TAB_VERB";
        [Element(4, IsArgument = true, Comment = " NumberValuePairs ")]
        public uint NumberValuePairs;
        [Element(5, IsList = true)]
        public List<COMPU_VTAB_DATA> data;
        [Element(6, IsString = true, Name = "DEFAULT_VALUE")]
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

        [Element(0, IsName = true)]
        public string name;

        [Element(1, IsString = true)]
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
            data = new List<COMPU_VTAB_RANGE_DATA>();
        }

        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " NumberValueTriples ")]
        public uint NumberValueTriples;
        [Element(4, IsList = true)]
        public List<COMPU_VTAB_RANGE_DATA> data;
        [Element(5, IsString = true, Name = "DEFAULT_VALUE")]
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

        [Element(0, IsName = true)]
        public string name = "";

        [Element(1, IsArgument = true)]
        public decimal InValMin;

        [Element(2, IsArgument = true)]
        public decimal InValMax;

        [Element(3, IsString = true)]
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

        [Element(0, IsArgument = true)]
        public uint xDim;
        [Element(1, IsArgument = true)]
        public uint yDim;
        [Element(2, IsArgument = true)]
        public uint zDim;
    }
}
