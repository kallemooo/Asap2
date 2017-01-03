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

    /// <summary>
    /// Data types defined by the standard.
    /// </summary>
    public enum DataType
    {
        UBYTE,
        SBYTE,
        UWORD,
        SWORD,
        ULONG,
        SLONG,
        A_UINT64,
        A_INT64,
        FLOAT32_IEEE,
        FLOAT64_IEEE
    }

    /// <summary>
    /// Data sizes defined by the standard.
    /// </summary>
    public enum DataSize
    {
        BYTE,
        WORD,
        LONG,
    }

    public enum AddrType
    {
        PBYTE,
        PWORD,
        PLONG,
        DIRECT
    }

    public enum IndexOrder
    {
        /// <summary>
        /// Increasing index with increasing address.
        /// </summary>
        INDEX_INCR,
        /// <summary>
        /// Decreasing index with increasing address.
        /// </summary>
        INDEX_DECR,
    }
    
    /// <summary>
    /// Conversion types used by CHARACTERISTICs and MEASUREMENT.
    /// </summary>
    public enum ConversionType
    {
        IDENTICAL,
        FORM,
        LINEAR,
        RAT_FUNC,
        TAB_INTP,
        TAB_NOINTP,
        TAB_VERB,
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

    [Base()]
    public class HEADER : Asap2Base
    {
        [Element(0, IsString = true)]
        public string longIdentifier;

        [Element(1, IsString = true, Name = "VERSION")]
        public string version;

        [Element(2, IsArgument = true, Name = "PROJECT_NO")]
        public string project_no;
    }

    [Base()]
    public class MODULE : Asap2Base
    {
        public MODULE()
        {
        }

        [Element(1, IsArgument = true)]
        public string name;

        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(3)]
        public A2ML A2ML;

        [Element(4, IsDictionary = true)]
        public Dictionary<string, IF_DATA> IF_DATAs = new Dictionary<string, IF_DATA>();

        [Element(5)]
        public MOD_COMMON mod_common;

        [Element(6)]
        public MOD_PAR mod_par;

        [Element(7, IsDictionary = true, Comment = " Measurment data for the module ")]
        public Dictionary<string, MEASUREMENT> measurements = new Dictionary<string, MEASUREMENT>();

        [Element(8, IsDictionary = true, Comment = " Module COMPU_METHODs ")]
        public Dictionary<string, COMPU_METHOD> compu_methods = new Dictionary<string, COMPU_METHOD>();

        [Element(9, IsDictionary = true, Comment = " Conversion tables for the module COMPU_METHODs ")]
        public Dictionary<string, COMPU_TAB> COMPU_TABs = new Dictionary<string, COMPU_TAB>();

        [Element(10, IsDictionary = true, Comment = " Verbal conversion tables for the module ")]
        public Dictionary<string, COMPU_VTAB> COMPU_VTABs = new Dictionary<string, COMPU_VTAB>();

        [Element(11, IsDictionary = true, Comment = " Verbal conversion tables with parameter ranges for the module ")]
        public Dictionary<string, COMPU_VTAB_RANGE> COMPU_VTAB_RANGEs = new Dictionary<string, COMPU_VTAB_RANGE>();

        [Element(12, IsDictionary = true, Comment = " Groups for the module ")]
        public Dictionary<string, GROUP> groups = new Dictionary<string, GROUP>();

        [Element(13, IsDictionary = true, Comment = " Characteristic data for the module ")]
        public Dictionary<string, CHARACTERISTIC> characteristics = new Dictionary<string, CHARACTERISTIC>();

        [Element(14, IsDictionary = true, Comment = " AXIS_PTS data for the module ")]
        public Dictionary<string, AXIS_PTS> axis_pts = new Dictionary<string, AXIS_PTS>();

        [Element(15, IsDictionary = true, Comment = " RECORD_LAYOUT data for the module ")]
        public Dictionary<string, RECORD_LAYOUT> record_layout = new Dictionary<string, RECORD_LAYOUT>();

        [Element(16, IsDictionary = true, Comment = " FUNCTIONs for the module ")]
        public Dictionary<string, FUNCTION> functions = new Dictionary<string, FUNCTION>();

        [Element(17, IsDictionary = true, Comment = " UNITs for the module ")]
        public Dictionary<string, UNIT> units = new Dictionary<string, UNIT>();

        [Element(18, IsDictionary = true, Comment = " USER_RIGHTS for the module ")]
        public Dictionary<string, USER_RIGHTS> user_rights = new Dictionary<string, USER_RIGHTS>();

        [Element(19, IsDictionary = true, Comment = " FRAMEs for the module ")]
        public Dictionary<string, FRAME> frames = new Dictionary<string, FRAME>();
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

    [Base()]
    public class AXIS_DESCR : Asap2Base
    {
        /// <summary>
        /// Specifies the properties of an axis that belongs to a tunable curve, map or cuboid.
        /// </summary>
        public enum Attribute
        {
            /// <summary>
            /// Axis shared by various tables and rescaled, i.e. normalized, by a curve (CURVE_AXIS_REF).
            /// </summary>
            CURVE_AXIS,
            /// <summary>
            /// Axis shared by various tables.
            /// </summary>
            COM_AXIS,
            /// <summary>
            /// Axis specific to one table with calculated axis points. Axis points are not stored in ECU memory
            /// </summary>
            FIX_AXIS,
            /// <summary>
            /// Axis shared by various tables and rescaled, i.e. normalized, by another axis (AXIS_PTS_REF).
            /// </summary>
            RES_AXIS,
            /// <summary>
            ///  Axis specific to one table.
            /// </summary>
            STD_AXIS,
        }

        public AXIS_DESCR(Attribute attribute, string InputQuantity, string Conversion, UInt64 MaxAxisPoints, decimal LowerLimit, decimal UpperLimit)
        {
            this.attribute = attribute;
            this.InputQuantity = InputQuantity;
            this.Conversion = Conversion;
            this.MaxAxisPoints = MaxAxisPoints;
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        [Element(1, IsArgument = true, Comment = " Type           ")]
        public Attribute attribute;
        [Element(2, IsArgument = true, Comment = " InputQuantity  ")]
        public string InputQuantity;
        [Element(3, IsArgument = true, Comment = " Conversion     ")]
        public string Conversion;
        [Element(4, IsArgument = true, Comment = " MaxAxisPoints  ")]
        public UInt64 MaxAxisPoints;
        [Element(5, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit;
        [Element(6, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit;
        [Element(7, IsList = true)]
        public List<ANNOTATION> annotation = new List<ANNOTATION>();

        /// <summary>
        /// Reference to <see cref="AXIS_PTS"/> in case the axis values are stored in a different memory location than the values of
        /// the <see cref="CHARACTERISTIC"/> the axis description belongs to.
        /// </summary>
        [Element(8, IsArgument = true, Name = "AXIS_PTS_REF")]
        public string axis_pts_ref;
        [Element(9)]
        public BYTE_ORDER byte_order;

        /// <summary>
        /// Reference to the curve's <see cref="CHARACTERISTIC"/> that is used to normalize or scale the axis.
        /// </summary>
        [Element(10, IsArgument = true, Name = "CURVE_AXIS_REF")]
        public string curve_axis_ref;
        [Element(11)]
        public DEPOSIT deposit;
        [Element(12)]
        public EXTENDED_LIMITS extended_limits;
        [Element(13)]
        public FIX_AXIS_PAR fix_axis_par;
        [Element(14)]
        public FIX_AXIS_PAR_DIST fix_axis_par_dist;
        [Element(15)]
        public FIX_AXIS_PAR_LIST fix_axis_par_list;
        [Element(16, IsString = true, Name = "FORMAT")]
        public string format;

        /// <summary>
        /// Specifies the maximum permissible gradient for this axis.
        /// </summary>
        [Element(17, IsArgument = true, Name = "MAX_GRAD")]
        public decimal? max_grad;
        [Element(18)]
        public MONOTONY monotony;
        /// <summary>
        /// Specifies the physical unit. Overrules the unit specified in the referenced <see cref="COMPU_METHOD"/>.
        /// </summary>
        [Element(19, IsString = true, Name = "PHYS_UNIT")]
        public string phys_unit;
        [Element(20, Comment = "Write-access is not allowed for this AXIS_DESCR")]
        public READ_ONLY read_only;
        /// <summary>
        /// Specifies an increment value that is added or subtracted when using the up/down keys while calibrating.
        /// </summary>
        [Element(21, IsArgument = true, Name = "STEP_SIZE")]
        public decimal? step_size;
    }

    /// <summary>
    /// Axes contain the sample point values for curves and maps.
    /// The keyword describes the properties of an axis, such as its address in memory, references to the input variable (<see cref="MEASUREMENT"/>),
    /// record layout and computation method, the maximum number of sample points and further properties.
    /// </summary>
    [Base()]
    public class AXIS_PTS : Asap2Base
    {
        public AXIS_PTS(string Name, string LongIdentifier, UInt64 Address, string InputQuantity, string Deposit, decimal MaxDiff, string Conversion, UInt64 MaxAxisPoints, decimal LowerLimit, decimal UpperLimit)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.Address = Address;
            this.InputQuantity = InputQuantity;
            this.Deposit = Deposit;
            this.MaxDiff = MaxDiff;
            this.Conversion = Conversion;
            this.MaxAxisPoints = MaxAxisPoints;
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true,   Comment = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " Address        ", CodeAsHex = true)]
        public UInt64 Address;
        [Element(4, IsArgument = true, Comment = " InputQuantity  ")]
        public string InputQuantity;
        [Element(5, IsArgument = true, Comment = " Deposit        ")]
        public string Deposit;
        [Element(6, IsArgument = true, Comment = " MaxDiff        ")]
        public decimal MaxDiff;
        [Element(7, IsArgument = true, Comment = " Conversion     ")]
        public string Conversion;
        [Element(8, IsArgument = true, Comment = " MaxAxisPoints  ")]
        public UInt64 MaxAxisPoints;
        [Element(9, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit;
        [Element(10, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit;

        [Element(11, IsList = true)]
        public List<ANNOTATION> annotation = new List<ANNOTATION>();

        [Element(12)]
        public BYTE_ORDER byte_order;

        [Element(13)]
        public CALIBRATION_ACCESS calibration_access;

        [Element(12)]
        public DEPOSIT deposit;

        [Element(18, IsArgument = true, Name = "DISPLAY_IDENTIFIER")]
        public string display_identifier;

        [Element(19)]
        public ECU_ADDRESS_EXTENSION ecu_address_extension;

        [Element(20)]
        public EXTENDED_LIMITS extended_limits;

        [Element(21, IsString = true, Name = "FORMAT")]
        public string format;

        [Element(22)]
        public FUNCTION_LIST function_list;

        [Element(23)]
        public GUARD_RAILS guard_rails;

        [Element(24, IsList = true)]
        public List<IF_DATA> if_data = new List<IF_DATA>();

        [Element(25)]
        public MONOTONY monotony;

        /// <summary>
        /// Specifies the physical unit. Overrules the unit specified in the referenced <see cref="COMPU_METHOD"/>.
        /// </summary>
        [Element(26, IsString = true, Name = "PHYS_UNIT")]
        public string phys_unit;

        [Element(20, Comment = "Write-access is not allowed for this AXIS_DESCR")]
        public READ_ONLY read_only;

        [Element(27, IsArgument = true, Name = "REF_MEMORY_SEGMENT")]
        public string ref_memory_segment;

        /// <summary>
        /// Specifies an increment value that is added or subtracted when using the up/down keys while calibrating.
        /// </summary>
        [Element(28, IsArgument = true, Name = "STEP_SIZE")]
        public decimal? step_size;

        [Element(29)]
        public SYMBOL_LINK symbol_link;
    }

    /// <summary>
    /// Specifies position, datatype, index increment and addressing method of the X, Y, Z, Z4 or Z5 axis points in memory.
    /// </summary>
    [Base(IsSimple = true)]
    public class AXIS_PTS_XYZ45 : Asap2Base
    {
        public AXIS_PTS_XYZ45(string Name, UInt64 Position, DataType dataType, IndexOrder indexIncr, AddrType addrType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
            this.indexIncr = indexIncr;
            this.addrType = addrType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position  ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " dataType  ")]
        public DataType dataType;
        [Element(3, IsArgument = true, Comment = " indexIncr ")]
        public IndexOrder indexIncr;
        [Element(4, IsArgument = true, Comment = " addrType  ")]
        public AddrType addrType;
    }

    /// <summary>
    /// Specifies the rescale mapping between stored axis points and used points for curve and maps.
    /// </summary>
    [Base(IsSimple = true)]
    public class AXIS_RESCALE_XYZ45 : Asap2Base
    {
        public AXIS_RESCALE_XYZ45(string Name, UInt64 Position, DataType dataType, UInt64 MaxNoOfRescalePairs, IndexOrder indexIncr, AddrType addrType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
            this.MaxNoOfRescalePairs = MaxNoOfRescalePairs;
            this.indexIncr = indexIncr;
            this.addrType = addrType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position            ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " dataType            ")]
        public DataType dataType;
        [Element(3, IsArgument = true, Comment = " MaxNoOfRescalePairs ")]
        public UInt64 MaxNoOfRescalePairs;
        [Element(4, IsArgument = true, Comment = " indexIncr           ")]
        public IndexOrder indexIncr;
        [Element(5, IsArgument = true, Comment = " addrType            ")]
        public AddrType addrType;
    }

    /// <summary>
    /// Specifies position and datatype of the distance (i.e. slope) value within the record layout.
    /// The distance value is used to calculate the axis points for the described FIX_AXIS.
    /// </summary>
    [Base(IsSimple = true)]
    public class DIST_OP_XYZ45 : Asap2Base
    {
        public DIST_OP_XYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position            ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " dataType            ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies the number of axis points. This number is fixed and not stored in memory.
    /// </summary>
    [Base(IsSimple = true)]
    public class FIX_NO_AXIS_PTS_XYZ45 : Asap2Base
    {
        public FIX_NO_AXIS_PTS_XYZ45(string Name, UInt64 NumberOfAxisPoints)
        {
            this.Name = Name;
            this.NumberOfAxisPoints = NumberOfAxisPoints;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " NumberOfAxisPoints ")]
        public UInt64 NumberOfAxisPoints;
    }

    /// <summary>
    /// Specifies position and data type of an identification number for the stored object.
    /// </summary>
    [Base(IsSimple = true)]
    public class IDENTIFICATION : Asap2Base
    {
        public IDENTIFICATION(UInt64 Position, DataType dataType)
        {
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " DataType ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies position and datatype of the number of axis points within the record layout.
    /// </summary>
    [Base(IsSimple = true)]
    public class NO_AXIS_PTS_XYZ45 : Asap2Base
    {
        public NO_AXIS_PTS_XYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " dataType ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies position and datatype of the number of rescaling values within the record layout.
    /// </summary>
    [Base(IsSimple = true)]
    public class NO_RESCALE_XYZ45 : Asap2Base
    {
        public NO_RESCALE_XYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " dataType ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies position and datatype of the offset value within the record layout.
    /// The offset value is used to calculate the axis points for the described FIX_AXIS.
    /// </summary>
    [Base(IsSimple = true)]
    public class OFFSET_XYZ45 : Asap2Base
    {
        public OFFSET_XYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " dataType ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies a position in this record layout that shall be ignored (i.e. not interpreted).
    /// </summary>
    [Base(IsSimple = true)]
    public class RESERVED : Asap2Base
    {
        public RESERVED(UInt64 Position, DataSize dataSize)
        {
            this.Position = Position;
            this.dataSize = dataSize;
        }

        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " DataSize ")]
        public DataSize dataSize;
    }

    /// <summary>
    /// Specifies position and datatype to store the result of interpolation for the X, Y, Z, Z4 or Z5 axis and the look-up table's output W.
    /// </summary>
    [Base(IsSimple = true)]
    public class RIP_ADDR_WXYZ45 : Asap2Base
    {
        public RIP_ADDR_WXYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " DataType ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies position and datatype of the power-of-two exponent of the distance (i.e. slope) value within the record layout.
    /// The distance value is used to calculate the axis points for the described FIX_AXIS.
    /// </summary>
    [Base(IsSimple = true)]
    public class SHIFT_OP_XYZ45 : Asap2Base
    {
        public SHIFT_OP_XYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " DataType ")]
        public DataType dataType;
    }

    /// <summary>
    /// Specifies position and datatype of the address of the axis' input value within the record layout.
    /// </summary>
    [Base(IsSimple = true)]
    public class SRC_ADDR_XYZ45 : Asap2Base
    {
        public SRC_ADDR_XYZ45(string Name, UInt64 Position, DataType dataType)
        {
            this.Name = Name;
            this.Position = Position;
            this.dataType = dataType;
        }

        [Element(0, IsName = true)]
        public string Name;
        [Element(1, IsArgument = true, Comment = " Position ")]
        public UInt64 Position;
        [Element(2, IsArgument = true, Comment = " DataType ")]
        public DataType dataType;
    }

    [Base(IsSimple = true)]
    public class STATIC_RECORD_LAYOUT : Asap2Base
    {
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

    /// <summary>
    /// Specifies which kind of monotony for the sample values is allowed for a <see cref="AXIS_DESCR"/> or <see cref="AXIS_PTS"/>.
    /// </summary>
    [Base(IsSimple = true)]
    public class MONOTONY : Asap2Base
    {
        public enum MONOTONY_type
        {
            /// <summary>
            /// Monotonously decreasing.
            /// </summary>
            MON_DECREASE,
            /// <summary>
            /// Monotonously increasing.
            /// </summary>
            MON_INCREASE,
            /// <summary>
            /// Strict monotonously decreasing.
            /// </summary>
            STRICT_DECREASE,
            /// <summary>
            /// Strict monotonously increasing.
            /// </summary>
            STRICT_INCREASE,
            /// <summary>
            /// Monotonously in- or decreasing.
            /// </summary>
            MONOTONOUS,
            /// <summary>
            /// Strict monotonously in- or decreasing.
            /// </summary>
            STRICT_MON,
            /// <summary>
            /// No monotony required.
            /// </summary>
            NOT_MON,
        }
        public MONOTONY(MONOTONY_type value)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public MONOTONY_type value;
    }


    /// <summary>
    /// Specifies the value of the first sample point, the power-of-two exponent of the increment value and total number of
    /// sample points for computing the sample point values of an equidistant axis of type FIX_AXIS.
    /// </summary>
    [Base(IsSimple = true)]
    public class FIX_AXIS_PAR : Asap2Base
    {
        public FIX_AXIS_PAR(Int64 Offset, Int64 Shift, UInt64 NumberAPo)
        {
            this.Offset = Offset;
            this.Shift = Shift;
            this.NumberAPo = NumberAPo;
        }

        [Element(0, IsArgument = true, Comment = " Offset                ")]
        public Int64 Offset;
        [Element(0, IsArgument = true, Comment = " Shift                 ")]
        public Int64 Shift;
        /// <summary>
        /// Number of axis points.
        /// </summary>
        [Element(0, IsArgument = true, Comment = " Number of axis points ")]
        public UInt64 NumberAPo;
    }

    /// <summary>
    /// Specifies the value of the first sample point, the increment value and the total number of sample points
    /// for computing the sample point values of an equidistant axis of type FIX_AXIS.
    /// </summary>
    [Base(IsSimple = true)]
    public class FIX_AXIS_PAR_DIST : Asap2Base
    {
        public FIX_AXIS_PAR_DIST(Int64 Offset, Int64 Distance, UInt64 NumberAPo)
        {
            this.Offset = Offset;
            this.Distance = Distance;
            this.NumberAPo = NumberAPo;
        }

        [Element(0, IsArgument = true, Comment = " Offset                ")]
        public Int64 Offset;
        [Element(0, IsArgument = true, Comment = " Distance              ")]
        public Int64 Distance;
        /// <summary>
        /// Number of axis points.
        /// </summary>
        [Element(0, IsArgument = true, Comment = " Number of axis points ")]
        public UInt64 NumberAPo;
    }

    /// <summary>
    /// Explicitly specifies the sample point values of the axis of type FIX_AXIS.
    /// </summary>
    [Base()]
    public class FIX_AXIS_PAR_LIST : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true, Comment = " Sample point values ")]
        public List<decimal> AxisPts_Values = new List<decimal>();
    }

    /// <summary>
    /// Specifies position, datatype, orientation and addressing method to store table data in the <see cref="RECORD_LAYOUT"/>.
    /// </summary>
    [Base(IsSimple = true)]
    public class FNC_VALUES : Asap2Base
    {
        public enum IndexMode
        {
            ALTERNATE_CURVES,
            ALTERNATE_WITH_X,
            ALTERNATE_WITH_Y,
            COLUMN_DIR,
            ROW_DIR,
        }
        public FNC_VALUES(UInt64 Position, DataType dataType, IndexMode indexMode, AddrType addrType)
        {
            this.Position = Position;
            this.dataType = dataType;
            this.indexMode = indexMode;
            this.addrType = addrType;
        }

        [Element(0, IsArgument = true)]
        public UInt64 Position;
        [Element(1, IsArgument = true)]
        public DataType dataType;
        [Element(2, IsArgument = true)]
        public IndexMode indexMode;
        [Element(3, IsArgument = true)]
        public AddrType addrType;
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

    [Base()]
    public class MOD_COMMON : Asap2Base
    {
        public MOD_COMMON(string Comment)
        {
            alignments = new Dictionary<string, ALIGNMENT>();
            this.Comment = Comment;
        }

        [Element(0, IsString = true, Comment = " Comment ")]
        public string Comment;

        [Element(1, IsDictionary = true)]
        public Dictionary<string, ALIGNMENT> alignments;

        [Element(2)]
        public BYTE_ORDER byte_order;

        [Element(3, IsArgument = true, Name = "DATA_SIZE")]
        public UInt64? data_size;

        [Element(4)]
        public DEPOSIT deposit;

        [Element(5, IsArgument = true, Name = "S_REC_LAYOUT")]
        public string s_rec_layout;
    }

    /// <summary>
    /// Describes how structures of tunable parameters (<see cref="CHARACTERISTIC"/>) and axes (<see cref="AXIS_PTS"/>) are stored in memory.
    /// It describes byte alignments, order and position of calibration objects in memory, their rescaling, memory offset and further properties.
    /// </summary>
    [Base()]
    public class RECORD_LAYOUT : Asap2Base
    {
        public RECORD_LAYOUT(string Name)
        {
            this.Name = Name;
        }

        [Element(0, IsArgument = true)]
        public string Name;

        [Element(1, IsDictionary = true)]
        public Dictionary<string, ALIGNMENT> alignments;

        [Element(2, IsDictionary = true)]
        public Dictionary<string, AXIS_PTS_XYZ45> axis_pts_xyz45 = new Dictionary<string, AXIS_PTS_XYZ45>();

        [Element(3, IsDictionary = true)]
        public Dictionary<string, AXIS_RESCALE_XYZ45> axis_rescale_xyz45 = new Dictionary<string, AXIS_RESCALE_XYZ45>();

        [Element(4, IsDictionary = true)]
        public Dictionary<string, DIST_OP_XYZ45> dist_op_xyz45 = new Dictionary<string, DIST_OP_XYZ45>();

        [Element(5, IsDictionary = true)]
        public Dictionary<string, FIX_NO_AXIS_PTS_XYZ45> fix_no_axis_pts_xyz45 = new Dictionary<string, FIX_NO_AXIS_PTS_XYZ45>();

        [Element(6)]
        public FNC_VALUES fnc_values;

        [Element(7)]
        public IDENTIFICATION identification;

        [Element(8)]
        public Dictionary<string, NO_AXIS_PTS_XYZ45> no_axis_pts_xyz45 = new Dictionary<string, NO_AXIS_PTS_XYZ45>();

        [Element(9)]
        public Dictionary<string, NO_RESCALE_XYZ45> no_rescale_xyz45 = new Dictionary<string, NO_RESCALE_XYZ45>();

        [Element(10)]
        public Dictionary<string, OFFSET_XYZ45> offset_xyz45 = new Dictionary<string, OFFSET_XYZ45>();

        [Element(11)]
        public RESERVED reserved;

        [Element(12)]
        public Dictionary<string, RIP_ADDR_WXYZ45> rip_addr_wxyz45 = new Dictionary<string, RIP_ADDR_WXYZ45>();

        [Element(13)]
        public Dictionary<string, SHIFT_OP_XYZ45> shift_op_xyz45 = new Dictionary<string, SHIFT_OP_XYZ45>();

        [Element(14)]
        public Dictionary<string, SRC_ADDR_XYZ45> src_addr_xyz45 = new Dictionary<string, SRC_ADDR_XYZ45>();

        /// <summary>
        /// Specifies that a tunable axis with a dynamic number of axis points does not compact or expand in memory when removing or inserting axis points.
        /// </summary>
        [Element(15)]
        public STATIC_RECORD_LAYOUT static_record_layout;
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

    [Base(IsSimple = true)]
    public class EXTENDED_LIMITS : Asap2Base
    {
        public EXTENDED_LIMITS(decimal LowerLimit, decimal UpperLimit)
        {
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        [Element(1, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit;
        [Element(2, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit;
    }

    [Base()]
    public class CHARACTERISTIC : Asap2Base
    {
        /// <summary>
        /// Characteristic types 
        /// </summary>
        public enum Type
        {
            ASCII,
            CURVE,
            MAP,
            CUBOID,
            CUBE_4,
            CUBE_5,
            VAL_BLK,
            VALUE,
        }

        public CHARACTERISTIC(string Name, string LongIdentifier, Type type, UInt64 Address, string Deposit, decimal MaxDiff, string Conversion, decimal LowerLimit, decimal UpperLimit)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.type = type;
            this.Address = Address;
            this.Deposit = Deposit;
            this.MaxDiff = MaxDiff;
            this.Conversion = Conversion;
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true, Comment   = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " Type           ")]
        public Type type;
        [Element(4, IsArgument = true, Comment = " Address        ", CodeAsHex = true)]
        public UInt64 Address;
        [Element(5, IsArgument = true, Comment = " Deposit        ")]
        public string Deposit;
        [Element(6, IsArgument = true, Comment = " MaxDiff        ")]
        public decimal MaxDiff;
        [Element(7, IsArgument = true, Comment = " Conversion     ")]
        public string Conversion;
        [Element(8, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit;
        [Element(9, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit;

        [Element(10, IsList = true)]
        public List<ANNOTATION> annotation = new List<ANNOTATION>();
        [Element(11, IsList = true)]
        public List<AXIS_DESCR> axis_descr = new List<AXIS_DESCR>();
        [Element(12, IsArgument = true, CodeAsHex = true, Name = "BIT_MASK")]
        public UInt64? bit_mask;
        [Element(13)]
        public BYTE_ORDER byte_order;
        [Element(14)]
        public CALIBRATION_ACCESS calibration_access;
        /// <summary>
        /// Reference to a <see cref="MEASUREMENT"/>, which represents the working point on a curve.
        /// </summary>
        [Element(15, IsArgument = true, Name = "COMPARISON_QUANTITY")]
        public string comparison_quantity;        
        [Element(16)]
        public DEPENDENT_CHARACTERISTIC dependent_characteristic;
        [Element(17)]
        public DISCRETE discrete;
        [Element(18, IsArgument = true, Name = "DISPLAY_IDENTIFIER")]
        public string display_identifier;
        [Element(19)]
        public ECU_ADDRESS_EXTENSION ecu_address_extension;
        [Element(20)]
        public EXTENDED_LIMITS extended_limits;
        [Element(21, IsString = true, Name = "FORMAT")]
        public string format;
        [Element(22)]
        public FUNCTION_LIST function_list;
        /// <summary>
        /// Determines that the outermost values of axes, curves and maps are calculated and cannot be adjusted.
        /// </summary>
        [Element(23)]
        public GUARD_RAILS guard_rails;
        [Element(24)]
        public MAP_LIST map_list;
        [Element(25)]
        public MATRIX_DIM matrix_dim;
        [Element(26)]
        public MAX_REFRESH max_refresh;
        /// <summary>
        /// Specifies the number of elements (ASCII characters or values) in a 1D array.
        /// Obsolete keyword. Please use <see cref="MATRIX_DIM"/> instead.
        /// </summary>
        /// <remarks>
        /// Obsolete keyword. Please use <see cref="MATRIX_DIM"/> instead.
        /// </remarks>
        [Element(27, IsArgument = true, Name = "NUMBER", IsObsolete = "Obsolete keyword. Please use MATRIX_DIM instead.")]
        public UInt64? number;
        /// <summary>
        /// Specifies the physical unit. Overrules the unit specified in the referenced <see cref="COMPU_METHOD"/>.
        /// </summary>
        [Element(28, IsString = true, Name = "PHYS_UNIT")]
        public string phys_unit;
        [Element(29, Comment = "Write-access is not allowed for this CHARACTERISTIC")]
        public READ_ONLY read_only;
        [Element(30, IsArgument = true, Name = "REF_MEMORY_SEGMENT")]
        public string ref_memory_segment;
        /// <summary>
        /// Specifies an increment value that is added or subtracted when using the up/down keys while calibrating.
        /// </summary>
        [Element(31, IsArgument = true, Name = "STEP_SIZE")]
        public decimal? step_size;
        [Element(32)]
        public SYMBOL_LINK symbol_link;
        [Element(33)]
        public VIRTUAL_CHARACTERISTIC virtual_characteristic;
        [Element(34, IsList = true)]
        public List<IF_DATA> if_data = new List<IF_DATA>();
    }

    [Base()]
    public class MEASUREMENT : Asap2Base
    {
        public enum LAYOUT
        {
            ROW_DIR,
            COLUMN_DIR,
        }

        public MEASUREMENT(string name, string LongIdentifier, DataType Datatype, string Conversion, uint Resolution, decimal Accuracy, decimal LowerLimit, decimal UpperLimit)
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
        [Element(3, IsArgument = true, Comment = " DataType       ")]
        public DataType Datatype;
        [Element(4, IsArgument = true, Comment = " Conversion     ")]
        public string Conversion;
        [Element(5, IsArgument = true, Comment = " Resolution     ")]
        public uint Resolution;
        [Element(6, IsArgument = true, Comment = " Accuracy       ")]
        public decimal Accuracy;
        [Element(7, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit;
        [Element(8, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit;
        [Element(9, IsList = true)]
        public List<ANNOTATION> annotation = new List<ANNOTATION>();
        [Element(10)]
        public ARRAY_SIZE array_size;
        [Element(11, IsArgument = true, CodeAsHex = true, Name = "BIT_MASK")]
        public UInt64? bit_mask;
        [Element(12)]
        public BIT_OPERATION bit_operation;
        [Element(13)]
        public BYTE_ORDER byte_order;
        [Element(14)]
        public DISCRETE discrete;
        [Element(15, IsArgument = true, Name = "DISPLAY_IDENTIFIER")]
        public string display_identifier;
        [Element(16)]
        public ECU_ADDRESS ecu_address;
        [Element(17)]
        public ECU_ADDRESS_EXTENSION ecu_address_extension;
        [Element(18, IsArgument = true, CodeAsHex = true, Name = "ERROR_MASK")]
        public UInt64? error_mask;
        [Element(19, IsString = true, Name = "FORMAT")]
        public string format;
        [Element(20)]
        public FUNCTION_LIST function_list;
        [Element(21, IsArgument = true, Name = "LAYOUT")]
        public LAYOUT? layout;
        [Element(22)]
        public MATRIX_DIM matrix_dim;
        [Element(23)]
        public MAX_REFRESH max_refresh;
        /// <summary>
        /// Specifies the physical unit. Overrules the unit specified in the referenced <see cref="COMPU_METHOD"/>.
        /// </summary>
        [Element(24, IsString = true, Name = "PHYS_UNIT")]
        public string phys_unit;
        [Element(25, Comment = "Write-access is allowed for this MEASUREMENT")]
        public READ_WRITE read_write;
        [Element(26, IsArgument = true, Name = "REF_MEMORY_SEGMENT")]
        public string ref_memory_segment;
        [Element(27)]
        public SYMBOL_LINK symbol_link;
        [Element(28)]
        public VIRTUAL Virtual;
        [Element(30, IsList = true)]
        public List<IF_DATA> if_data = new List<IF_DATA>();
    }

    [Base(IsObsolete = "Obsolete keyword. Please use FUNCTION instead.")]
    public class FUNCTION_LIST : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true, Comment = " List of functions. ")]
        public List<string> functions = new List<string>();
    }

    [Base()]
    public class VIRTUAL : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true, Comment = " MeasuringChannels ")]
        public List<string> MeasuringChannel = new List<string>();
    }


    [Base(IsSimple = true)]
    public class SYMBOL_LINK : Asap2Base
    {
        public SYMBOL_LINK(string SymbolName, UInt64 Offset)
        {
            this.SymbolName = SymbolName;
            this.Offset = Offset;
        }
        [Element(0, IsArgument = true, Comment = " SymbolName ")]
        public string SymbolName;

        [Element(1, IsArgument = true, Comment = " Offset     ")]
        public UInt64 Offset;
    }

    [Base(IsSimple = true)]
    public class MAX_REFRESH : Asap2Base
    {
        public MAX_REFRESH(UInt64 ScalingUnit, UInt64 Rate)
        {
            this.ScalingUnit = ScalingUnit;
            this.Rate = Rate;
        }
        [Element(0, IsArgument = true, Comment = " ScalingUnit ")]
        public UInt64 ScalingUnit;

        [Element(1, IsArgument = true, Comment = " Rate        ")]
        public UInt64 Rate;
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

    [Base(IsSimple = true, IsObsolete = "Obsolete keyword. Please use MATRIX_DIM instead.")]
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
    public class COMPU_METHOD : Asap2Base
    {
        public COMPU_METHOD(string Name, string LongIdentifier, ConversionType conversionType, string Format, string Unit)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.conversionType = conversionType;
            this.Format = Format;
        }

        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true, Comment   = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " ConversionType ")]
        public ConversionType conversionType;
        [Element(4, IsString = true, Comment   = " Display Format ")]
        public string Format;
        [Element(5, IsString = true, Comment   = " Physical Unit  ")]
        public string Unit;
        [Element(6)]
        public COEFFS coeffs;
        [Element(7)]
        public COEFFS_LINEAR coeffs_linear;

        /// <summary>
        /// Reference to conversion table to use.
        /// </summary>
        [Element(8, IsArgument = true, Name = "COMPU_TAB_REF")]
        public string compu_tab_ref;
        [Element(9)]
        public FORMULA formula;

        /// <summary>
        /// Reference to a physical unit.
        /// </summary>
        [Element(10, IsArgument = true, Name = "REF_UNIT")]
        public string ref_unit;

        /// <summary>
        /// Reference to a verbal conversion table (<see cref="COMPU_VTAB"/> or <see cref="COMPU_VTAB_RANGE"/>).
        /// Used to split up the value range of the measurement to a numerical part and a verbal part.
        /// The latter contains status information about the numerical part such as providing an error or describing the quality of the measurement.
        /// </summary>
        [Element(11, IsArgument = true, Name = "STATUS_STRING_REF")]
        public string status_string_ref;
    }

    [Base(IsSimple = true)]
    public class COEFFS : Asap2Base
    {
        public COEFFS(decimal a, decimal b, decimal c, decimal d, decimal e, decimal f)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }
        [Element(0, IsArgument = true, Comment = " Coefficients for the rational function (RAT_FUNC) ")]
        public decimal a;

        [Element(1, IsArgument = true)]
        public decimal b;

        [Element(2, IsArgument = true)]
        public decimal c;

        [Element(3, IsArgument = true)]
        public decimal d;

        [Element(4, IsArgument = true)]
        public decimal e;

        [Element(5, IsArgument = true)]
        public decimal f;
    }

    [Base(IsSimple = true)]
    public class COEFFS_LINEAR : Asap2Base
    {
        public COEFFS_LINEAR(decimal a, decimal b)
        {
            this.a = a;
            this.b = b;
        }
        [Element(0, IsArgument = true, Comment = " Coefficients for the linear function (LINEAR). ")]
        public decimal a;

        [Element(1, IsArgument = true)]
        public decimal b;
    }

    [Base()]
    public class FORMULA : Asap2Base
    {
        public FORMULA(string formula)
        {
            this.formula = formula;
        }

        /// <summary>
        /// Specifies a conversion formula to calculate the physical value from the ECU-internal value.
        /// Expression of the formula complies with ANSI C notation.
        /// Shall be used only, if linear or rational functions are not sufficient.
        /// </summary>
        [Element(1, IsString = true)]
        public string formula;

        /// <summary>
        /// Specifies a conversion formula to calculate the ECU-internal value from the physical value.
        /// Is the inversion of the referenced FORMULA.
        /// Expression of the formula complies to ANSI C notation.
        /// </summary>
        [Element(2, IsString = true, Name = "FORMULA_INV")]
        public string formula_inv;
    }

    [Base()]
    public class COMPU_TAB : Asap2Base
    {
        public COMPU_TAB(string Name, string LongIdentifier, ConversionType conversionType, uint NumberValuePairs)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.conversionType = conversionType;
            this.NumberValuePairs = NumberValuePairs;
            data = new List<COMPU_TAB_DATA>();
        }

        [Element(1, IsArgument = true, Comment = " Name             ")]
        public string Name;
        [Element(2, IsString = true, Comment   = " LongIdentifier   ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " ConversionType   ")]
        public ConversionType conversionType;
        [Element(4, IsArgument = true, Comment = " NumberValuePairs ")]
        public uint NumberValuePairs;
        [Element(5, IsList = true, Comment     = " ValuePairs       ")]
        public List<COMPU_TAB_DATA> data;
        [Element(6, IsString = true, Name = "DEFAULT_VALUE")]
        public string default_value;
        [Element(7, IsArgument = true, Name = "DEFAULT_VALUE_NUMERIC")]
        public decimal default_value_numeric;
    }

    [Base(IsSimple = true)]
    public class COMPU_TAB_DATA : Asap2Base
    {
        public COMPU_TAB_DATA(decimal InVal, decimal OutVal)
        {
            this.InVal = InVal;
            this.OutVal = OutVal;
        }

        [Element(0, IsName = true, ForceNewLine = true)]
        public string name = "";

        [Element(1, IsArgument = true, ForceNewLine = true)]
        public decimal InVal;

        [Element(2, IsString = true)]
        public decimal OutVal;
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

        [Element(1, IsArgument = true, Comment = " Name             ")]
        public string Name;
        [Element(2, IsString = true, Comment   = " LongIdentifier   ")]
        public string LongIdentifier;
        [Element(3, IsArgument = true, Comment = " ConversionType   ")]
        public string ConversionType = "TAB_VERB";
        [Element(4, IsArgument = true, Comment = " NumberValuePairs ")]
        public uint NumberValuePairs;
        [Element(5, IsList = true, Comment     = " ValuePairs       ")]
        public List<COMPU_VTAB_DATA> data;
        [Element(6, IsString = true, Name = "DEFAULT_VALUE")]
        public string default_value;
    }

    [Base(IsSimple = true)]
    public class COMPU_VTAB_DATA : Asap2Base
    {
        public COMPU_VTAB_DATA(decimal InVal, string OutVal)
        {
            this.InVal = InVal;
            this.OutVal = OutVal;
        }

        [Element(0, IsName = true, ForceNewLine = true)]
        public string name = "";

        [Element(1, IsArgument = true, ForceNewLine = true)]
        public decimal InVal;

        [Element(2, IsString = true)]
        public string OutVal;
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

        [Element(1, IsArgument = true, Comment = " Name               ")]
        public string Name;
        [Element(2, IsString = true, Comment   = " LongIdentifier     ")]
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

        [Element(0, IsName = true, ForceNewLine = true)]
        public string name = "";

        [Element(1, IsArgument = true, ForceNewLine = true)]
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

    [Base()]
    public class MEMORY_SEGMENT : Asap2Base
    {
        public enum PrgType
        {
            CALIBRATION_VARIABLES,
            CODE,
            DATA,
            EXCLUDED_FROM_FLASH,
            OFFLINE_DATA,
            RESERVED,
            SERAM,
            VARIABLES,
        }
        
        public enum MemoryType
        {
            EEPROM,
            EPROM,
            FLASH,
            RAM,
            ROM,
            REGISTER,
        }

        public enum Attribute
        {
            INTERN,
            EXTERN,
        }

        public MEMORY_SEGMENT(string name, string longIdentifier, PrgType prgType, MemoryType memoryType, Attribute attribute, UInt64 address, UInt64 size,
            long offset0, long offset1, long offset2, long offset3, long offset4)
        {
            this.name = name;
            this.longIdentifier = longIdentifier;
            this.prgType = prgType;
            this.memoryType = memoryType;
            this.attribute = attribute;
            this.address = address;
            this.size = size;
            this.offset0 = offset0;
            this.offset1 = offset1;
            this.offset2 = offset2;
            this.offset3 = offset3;
            this.offset4 = offset4;
        }

        [Element(0, IsArgument = true)]
        public string name;

        [Element(1, IsString = true)]
        public string longIdentifier;

        [Element(2, IsArgument = true, Comment = " PrgTypes   ")]
        public PrgType prgType;

        [Element(3, IsArgument = true, Comment = " MemoryType ")]
        public MemoryType memoryType;

        [Element(4, IsArgument = true, Comment = " Attribute  ")]
        public Attribute attribute;

        [Element(5, IsArgument = true, Comment = " Address    ", CodeAsHex = true)]
        public UInt64 address;

        [Element(6, IsArgument = true, Comment = " Size       ", CodeAsHex = true)]
        public UInt64 size;

        [Element(7, IsArgument = true, Comment = " offset     ")]
        public long offset0;

        [Element(8, IsArgument = true)]
        public long offset1;

        [Element(9, IsArgument = true)]
        public long offset2;

        [Element(10, IsArgument = true)]
        public long offset3;

        [Element(11, IsArgument = true)]
        public long offset4;
    
        [Element(12, IsList = true)]
        public List<IF_DATA> if_data = new List<IF_DATA>();
    }

    [Base()]
    public class MEMORY_LAYOUT : Asap2Base
    {
        public enum PrgType
        {
            PRG_CODE,
            PRG_DATA,
            PRG_RESERVED,
        }

        public MEMORY_LAYOUT(PrgType prgType, UInt64 Address, UInt64 Size,
            long offset0, long offset1, long offset2, long offset3, long offset4)
        {
            this.prgType = prgType;
            this.Address = Address;
            this.Size = Size;
            this.offset0 = offset0;
            this.offset1 = offset1;
            this.offset2 = offset2;
            this.offset3 = offset3;
            this.offset4 = offset4;
        }

        [Element(0, IsArgument = true, Comment = " Program segment type ")]
        public PrgType prgType;

        [Element(1, IsArgument = true, Comment = " Address              ", CodeAsHex = true)]
        public UInt64 Address;

        [Element(2, IsArgument = true, Comment = " Size                 ", CodeAsHex = true)]
        public UInt64 Size;

        [Element(3, IsArgument = true, Comment = " offset               ")]
        public long offset0;

        [Element(4, IsArgument = true)]
        public long offset1;

        [Element(5, IsArgument = true)]
        public long offset2;

        [Element(6, IsArgument = true)]
        public long offset3;

        [Element(7, IsArgument = true)]
        public long offset4;

        [Element(8, IsList = true)]
        public List<IF_DATA> if_data = new List<IF_DATA>();
    }

    [Base()]
    public class CALIBRATION_METHOD : Asap2Base
    {
        public CALIBRATION_METHOD(string Method, ulong Version)
        {
            this.Method = Method;
            this.Version = Version;
        }

        [Element(0, IsString = true,   Comment = " Method  ")]
        public string Method;

        [Element(1, IsArgument = true, Comment = " Version ")]
        public ulong Version;

        [Element(2)]
        public CALIBRATION_HANDLE calibration_handle;
    }

    [Base()]
    public class CALIBRATION_HANDLE : Asap2Base
    {
        public CALIBRATION_HANDLE()
        {
        }

        [Element(0, IsArgument = true, ForceNewLine = true, IsList = true, CodeAsHex = true, Comment = " Handles ")]
        public List<Int64> Handles = new List<Int64>();

        [Element(1, IsString = true, Name = "CALIBRATION_HANDLE_TEXT")]
        public string text;
    }

    [Base(IsSimple = true)]
    public class SYSTEM_CONSTANT : Asap2Base
    {
        public SYSTEM_CONSTANT(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        [Element(1, IsString = true)]
        public string name;

        [Element(1, IsString = true)]
        public string value;
    }

    [Base()]
    public class MOD_PAR : Asap2Base
    {
        public MOD_PAR(string comment)
        {
            this.comment = comment;
        }

        [Element(1, IsString = true)]
        public string comment;

        [Element(2, IsList = true)]
        public List<ADDR_EPK> addr_epk = new List<ADDR_EPK>();

        [Element(3, IsList = true)]
        public List<CALIBRATION_METHOD> calibration_method = new List<CALIBRATION_METHOD>();

        [Element(4, IsString = true, Name = "CPU_TYPE")]
        public string cpu_type;

        [Element(5, IsString = true, Name = "CUSTOMER")]
        public string customer;

        [Element(6, IsString = true, Name = "CUSTOMER_NO")]
        public string customer_no;

        [Element(7, IsString = true, Name = "ECU")]
        public string ecu;

        [Element(8, IsArgument = true, Name = "ECU_CALIBRATION_OFFSET")]
        public Int64? ecu_calibration_offset;

        [Element(9, IsString = true, Name = "EPK")]
        public string epk;

        [Element(10, IsList = true)]
        public List<MEMORY_LAYOUT> memory_layout = new List<MEMORY_LAYOUT>();

        [Element(11, IsList = true)]
        public List<MEMORY_SEGMENT> memory_segment = new List<MEMORY_SEGMENT>();

        [Element(12, IsArgument = true, Name = "NO_OF_INTERFACES")]
        public UInt64? no_of_interfaces;

        [Element(13, IsString = true, Name = "PHONE_NO")]
        public string phone_no;

        [Element(14, IsString = true, Name = "SUPPLIER")]
        public string supplier;

        [Element(15, IsDictionary = true)]
        public Dictionary<string, SYSTEM_CONSTANT> system_constants = new Dictionary<string, SYSTEM_CONSTANT>();

        [Element(16, IsString = true, Name = "USER")]
        public string user;

        [Element(17, IsString = true, Name = "VERSION")]
        public string version;
    }

    [Base(IsSimple = true)]
    public class DISCRETE : Asap2Base
    {
    }

    [Base(IsSimple = true)]
    public class READ_ONLY : Asap2Base
    {
    }

    [Base(IsSimple = true)]
    public class READ_WRITE : Asap2Base
    {
    }

    [Base(IsSimple = true)]
    public class GUARD_RAILS : Asap2Base
    {
    }

    [Base()]
    public class GROUP : Asap2Base
    {

        public GROUP(string GroupName, string GroupLongIdentifier)
        {
            this.GroupName = GroupName;
            this.GroupLongIdentifier = GroupLongIdentifier;
        }
        [Element(1, IsArgument = true, Comment = " GroupName           ")]
        public string GroupName;
        [Element(2, IsString = true, Comment   = " GroupLongIdentifier ")]
        public string GroupLongIdentifier;
        [Element(3, IsList = true)]
        public List<ANNOTATION> annotation = new List<ANNOTATION>();
        [Element(4)]
        public FUNCTION_LIST function_list;
        [Element(5, IsList = true)]
        public List<IF_DATA> if_data = new List<IF_DATA>();
        [Element(6)]
        public REF_CHARACTERISTIC ref_characteristic;
        [Element(7)]
        public REF_MEASUREMENT ref_measurement;
        [Element(8)]
        public ROOT root;
        [Element(9)]
        public SUB_GROUP sub_group;
    }

    [Base()]
    public class REF_MEASUREMENT : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true, Comment = " Measurement references ")]
        public List<string> reference = new List<string>();
    }

    [Base()]
    public class SUB_GROUP : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true, Comment = " Sub groups ")]
        public List<string> groups = new List<string>();
    }

    [Base(IsSimple = true)]
    public class ROOT : Asap2Base
    {
    }

    /// <summary>
    /// Lists the maps which comprise a cuboid.
    /// </summary>
    [Base()]
    public class MAP_LIST : Asap2Base
    {
        public MAP_LIST()
        {
        }
        [Element(1, IsArgument = true, IsList = true)]
        public List<string> MapList = new List<string>();
    }

    /// <summary>
    /// Specifies a formula to calculate the initialization value of this virtual characteristic based upon referenced <see cref="CHARACTERISTIC"/>s.
    /// The value of the virtual characteristic is not stored in ECU memory. It is typically used to calculate <see cref="DEPENDENT_CHARACTERISTIC"/>s.
    /// </summary>
    [Base()]
    public class VIRTUAL_CHARACTERISTIC : Asap2Base
    {
        public VIRTUAL_CHARACTERISTIC(string Formula)
        {
            this.Formula = Formula;
        }
        [Element(0, IsString = true)]
        public string Formula;
        [Element(1, IsArgument = true, IsList = true)]
        public List<string> Characteristic = new List<string>();
    }

    /// <summary>
    /// The value of the <see cref="CHARACTERISTIC"/>, which references this DEPENDENT_CHARACTERISTIC, is calculated instead of read from ECU memory.
    /// DEPENDENT_CHARACTERISTIC specifies a formula and references to other parameters (in memory or virtual) for the purpose to calculate the value.
    /// The value changes automatically, once one of the referenced parameters has changed its value.
    /// </summary>
    [Base()]
    public class DEPENDENT_CHARACTERISTIC : Asap2Base
    {
        public DEPENDENT_CHARACTERISTIC(string Formula)
        {
            this.Formula = Formula;
        }
        [Element(0, IsString = true)]
        public string Formula;
        [Element(1, IsArgument = true, IsList = true)]
        public List<string> Characteristic = new List<string>();
    }
}
