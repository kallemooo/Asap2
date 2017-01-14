using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    public class Asap2File : IValidator
    {
        /// <summary>
        /// Filename to use for base validation.
        /// </summary>
        public string baseFilename { get; set; }
        /// <summary>
        /// Default no-arg constructor.
        /// </summary>
        public Asap2File() : this("") { }

        /// <summary>
        /// Constructor with filename parameter.
        /// </summary>
        /// <param name="baseFilename"></param>
        public Asap2File(string baseFilename)
        {
            /* Default version if ASAP2_VERSION element is missing. */
            this.asap2_version = new ASAP2_VERSION(new Location(baseFilename), 1, 51);
        }

        public ASAP2_VERSION asap2_version {private set; get; }
        public void AddAsap2_version(ASAP2_VERSION asap2_version)
        {
            var version = elements.FirstOrDefault(x => x.GetType() == typeof(ASAP2_VERSION)) as ASAP2_VERSION;
            if (version != null)
            {
                elements.Remove(version);
            }
            elements.Add(asap2_version);
            this.asap2_version = asap2_version;
        }

        public A2ML_VERSION a2ml_version { private set; get; }
        public void AddA2ml_version(A2ML_VERSION a2ml_version)
        {
            var version = elements.FirstOrDefault(x => x.GetType() == typeof(A2ML_VERSION)) as A2ML_VERSION;
            if (version != null)
            {
                elements.Remove(version);
            }
            elements.Add(a2ml_version);
            this.a2ml_version = a2ml_version;
        }

        public List<Asap2Base> elements = new List<Asap2Base>();

        public void Validate(IErrorReporter errorReporter)
        {
            var projects = elements.FindAll(x => x.GetType() == typeof(PROJECT));

            if (projects == null || projects.Count == 0)
            {
                errorReporter.reportError(baseFilename + " : No PROJECT found, must be one");
                throw new ValidationErrorException(baseFilename + " : No PROJECT found, must be one");
            }
            else if (projects.Count > 1)
            {
                projects[projects.Count - 1].reportErrorOrWarning("Second PROJECT found, shall only be one", false, errorReporter);
            }

            var asap2_versions = elements.FindAll(x => x.GetType() == typeof(ASAP2_VERSION));
            if (asap2_versions != null && asap2_versions.Count > 0)
            {
                if (asap2_versions.Count > 1)
                {
                    asap2_versions[asap2_versions.Count - 1].reportErrorOrWarning("Second ASAP2_VERSION found, shall only be one", false, errorReporter);
                }
                if (asap2_versions[0].OrderID >= projects[0].OrderID)
                {
                    asap2_versions[0].reportErrorOrWarning("ASAP2_VERSION shall be placed before PROJECT", false, errorReporter);
                }
            }
            else
            {
                asap2_version.reportErrorOrWarning("Mandatory element ASAP2_VERSION is not found, version of the file is set to 1.5.1", false, errorReporter);
            }

            if (asap2_version.VersionNo != 1)
            {
                asap2_version.reportErrorOrWarning("ASAP2_VERSION.VersionNo is not 1. This parser is primarly designed for version 1.", false, errorReporter);
            }
            else if (asap2_version.UpgradeNo < 60)
            {
                asap2_version.reportErrorOrWarning("ASAP2_VERSION is less than 1.6.0. This parser is primarly designed for version 1.6.0 and newer.", false, errorReporter);
            }

            var a2ml_versions = elements.FindAll(x => x.GetType() == typeof(A2ML_VERSION));
            if (a2ml_versions != null && a2ml_versions.Count > 0)
            {
                if (a2ml_versions.Count > 1)
                {
                    a2ml_versions[a2ml_versions.Count - 1].reportErrorOrWarning("Second A2ML_VERSION found, shall only be one", false, errorReporter);
                }
                if (a2ml_versions[0].OrderID >= projects[0].OrderID)
                {
                    asap2_versions[0].reportErrorOrWarning("A2ML_VERSION shall be placed before PROJECT", false, errorReporter);
                }
            }

            var project = projects[0] as PROJECT;
            project.Validate(errorReporter);
        }
    }

    [Base(IsSimple = true)]
    public class ASAP2_VERSION : Asap2Base
    {
        public ASAP2_VERSION(Location location, UInt32 VersionNo, UInt32 UpgradeNo) : base(location)
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
        public A2ML_VERSION(Location location, UInt32 VersionNo, UInt32 UpgradeNo) : base(location)
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
    public class PROJECT : Asap2Base, IValidator
    {
        public PROJECT(Location location) : base(location)
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

        public void Validate(IErrorReporter errorReporter)
        {
            if (modules.Count == 0)
            {
                base.reportErrorOrWarning("No MODULE found, must be atleast one", true, errorReporter);
                throw new ValidationErrorException("No MODULE found, must be atleast one");
            }

            foreach ( MODULE mod in modules.Values)
            {
                mod.Validate(errorReporter);
            }
        }

    }

    [Base()]
    public class HEADER : Asap2Base
    {
        public HEADER(Location location) : base(location) { }

        [Element(0, IsString = true)]
        public string longIdentifier;

        [Element(1, IsString = true, Name = "VERSION")]
        public string version;

        [Element(2, IsArgument = true, Name = "PROJECT_NO")]
        public string project_no;
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
        public ALIGNMENT(Location location, ALIGNMENT_type type, uint value) : base(location)
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

        public AXIS_DESCR(Location location, Attribute attribute, string InputQuantity, string Conversion, UInt64 MaxAxisPoints, decimal LowerLimit, decimal UpperLimit) : base(location)
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
    /// Specifies position, datatype, index increment and addressing method of the X, Y, Z, Z4 or Z5 axis points in memory.
    /// </summary>
    [Base(IsSimple = true)]
    public class AXIS_PTS_XYZ45 : Asap2Base
    {
        public AXIS_PTS_XYZ45(Location location, string Name, UInt64 Position, DataType dataType, IndexOrder indexIncr, AddrType addrType) : base(location)
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
        public AXIS_RESCALE_XYZ45(Location location, string Name, UInt64 Position, DataType dataType, UInt64 MaxNoOfRescalePairs, IndexOrder indexIncr, AddrType addrType) : base(location)
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
        public DIST_OP_XYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public FIX_NO_AXIS_PTS_XYZ45(Location location, string Name, UInt64 NumberOfAxisPoints) : base(location)
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
        public IDENTIFICATION(Location location, UInt64 Position, DataType dataType) : base(location)
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
        public NO_AXIS_PTS_XYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public NO_RESCALE_XYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public OFFSET_XYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public RESERVED(Location location, UInt64 Position, DataSize dataSize) : base(location)
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
        public RIP_ADDR_WXYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public SHIFT_OP_XYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public SRC_ADDR_XYZ45(Location location, string Name, UInt64 Position, DataType dataType) : base(location)
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
        public STATIC_RECORD_LAYOUT(Location location) : base(location) { }
    }

    [Base(IsSimple = true)]
    public class DEPOSIT : Asap2Base
    {
        public enum DEPOSIT_type
        {
            ABSOLUTE,
            DIFFERENCE,
        }
        public DEPOSIT(Location location, DEPOSIT_type value) : base(location)
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
        public MONOTONY(Location location, MONOTONY_type value) : base(location)
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
        public FIX_AXIS_PAR(Location location, Int64 Offset, Int64 Shift, UInt64 NumberAPo) : base(location)
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
        public FIX_AXIS_PAR_DIST(Location location, Int64 Offset, Int64 Distance, UInt64 NumberAPo) : base(location)
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
        public FIX_AXIS_PAR_LIST(Location location) : base(location) { }
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
        public FNC_VALUES(Location location, UInt64 Position, DataType dataType, IndexMode indexMode, AddrType addrType) : base(location)
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
        public BYTE_ORDER(Location location, BYTE_ORDER_type value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public BYTE_ORDER_type value;
    }

    [Base()]
    public class MOD_COMMON : Asap2Base
    {
        public MOD_COMMON(Location location, string Comment) : base(location)
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
        public RECORD_LAYOUT(Location location, string Name) : base(location)
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
        public IF_DATA(Location location, string data) : base(location)
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
        public A2ML(Location location, string data) : base(location)
        {
            this.data = data;
        }
        [Element(0, IsArgument = true)]
        public string data;
    }

    [Base(IsSimple = true)]
    public class EXTENDED_LIMITS : Asap2Base
    {
        public EXTENDED_LIMITS(Location location, decimal LowerLimit, decimal UpperLimit) : base(location)
        {
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        [Element(1, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit;
        [Element(2, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit;
    }

    /// <summary>
    /// Lists the FUNCTIONs in which this object is listed. Obsolete keyword. Please use <see cref="FUNCTION"/> instead.
    /// </summary>
    [Base(IsObsolete = "Obsolete keyword. Please use FUNCTION instead.")]
    public class FUNCTION_LIST : Asap2Base
    {
        public FUNCTION_LIST(Location location) : base(location) { }

        [Element(0, IsArgument = true, IsList = true, Comment = " List of functions. ")]
        public List<string> functions = new List<string>();
    }

    [Base()]
    public class VIRTUAL : Asap2Base
    {
        public VIRTUAL(Location location) : base(location) { }

        [Element(0, IsArgument = true, IsList = true, Comment = " MeasuringChannels ")]
        public List<string> MeasuringChannel = new List<string>();
    }


    [Base(IsSimple = true)]
    public class SYMBOL_LINK : Asap2Base
    {
        public SYMBOL_LINK(Location location, string SymbolName, UInt64 Offset) : base(location)
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
        public MAX_REFRESH(Location location, UInt64 ScalingUnit, UInt64 Rate) : base(location)
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
        public ECU_ADDRESS_EXTENSION(Location location, UInt64 value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ECU_ADDRESS : Asap2Base
    {
        public ECU_ADDRESS(Location location, UInt64 value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 value;
    }

    [Base(IsSimple = true)]
    public class ADDR_EPK : Asap2Base
    {
        public ADDR_EPK(Location location, UInt64 Address) : base(location)
        {
            this.Address = Address;
        }

        [Element(0, IsArgument = true, CodeAsHex = true)]
        public UInt64 Address;
    }

    [Base()]
    public class ANNOTATION : Asap2Base
    {
        public ANNOTATION(Location location) : base(location) { }

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
        public ANNOTATION_LABEL(Location location, string value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsString = true)]
        public string value;
    }

    [Base(IsSimple = true)]
    public class ANNOTATION_ORIGIN : Asap2Base
    {
        public ANNOTATION_ORIGIN(Location location, string value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsString = true)]
        public string value;
    }

    [Base()]
    public class ANNOTATION_TEXT : Asap2Base
    {
        public ANNOTATION_TEXT(Location location) : base(location) { }

        [Element(0, IsString = true, IsList = true)]
        public List<string> data = new List<string>();
    }

    [Base(IsSimple = true, IsObsolete = "Obsolete keyword. Please use MATRIX_DIM instead.")]
    public class ARRAY_SIZE : Asap2Base
    {
        public ARRAY_SIZE(Location location, ulong value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public ulong value;
    }

    [Base()]
    public class BIT_OPERATION : Asap2Base
    {
        public BIT_OPERATION(Location location) : base(location) { }

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
        public RIGHT_SHIFT(Location location, ulong value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public ulong value;
    }

    [Base(IsSimple = true)]
    public class LEFT_SHIFT : Asap2Base
    {
        public LEFT_SHIFT(Location location, ulong value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public ulong value;
    }

    [Base(IsSimple = true)]
    public class SIGN_EXTEND : Asap2Base
    {
        public SIGN_EXTEND(Location location) : base(location) { }
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
        public CALIBRATION_ACCESS(Location location, CALIBRATION_ACCESS_type value) : base(location)
        {
            this.value = value;
        }

        [Element(0, IsArgument = true)]
        public CALIBRATION_ACCESS_type value;
    }

    [Base(IsSimple = true)]
    public class COEFFS : Asap2Base
    {
        public COEFFS(Location location, decimal a, decimal b, decimal c, decimal d, decimal e, decimal f) : base(location)
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
        public COEFFS_LINEAR(Location location, decimal a, decimal b) : base(location)
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
        public FORMULA(Location location, string formula) : base(location)
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

        public void Validate(IErrorReporter errorReporter, MODULE module)
        {
        }
    }

    [Base()]
    public class COMPU_TAB : Asap2Base
    {
        public COMPU_TAB(Location location, string Name, string LongIdentifier, ConversionType conversionType, uint NumberValuePairs) : base(location)
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
        public COMPU_TAB_DATA(Location location, decimal InVal, decimal OutVal) : base(location)
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
        public COMPU_VTAB(Location location, string Name, string LongIdentifier, ConversionType conversionType, uint NumberValuePairs) : base(location)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.conversionType = conversionType;
            this.NumberValuePairs = NumberValuePairs;
            data = new List<COMPU_VTAB_DATA>();
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
        public List<COMPU_VTAB_DATA> data;
        [Element(6, IsString = true, Name = "DEFAULT_VALUE")]
        public string default_value;
    }

    [Base(IsSimple = true)]
    public class COMPU_VTAB_DATA : Asap2Base
    {
        public COMPU_VTAB_DATA(Location location, decimal InVal, string OutVal) : base(location)
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
        public COMPU_VTAB_RANGE(Location location, string Name, string LongIdentifier, uint NumberValueTriples) : base(location)
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
        public COMPU_VTAB_RANGE_DATA(Location location, decimal InValMin, decimal InValMax, string value) : base(location)
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
        public MATRIX_DIM(Location location, uint xDim, uint yDim, uint zDim) : base(location)
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

        public MEMORY_SEGMENT(Location location, string Name, string longIdentifier, PrgType prgType, MemoryType memoryType, Attribute attribute, UInt64 address, UInt64 size,
            long offset0, long offset1, long offset2, long offset3, long offset4) : base(location)
        {
            this.Name = Name;
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
        public string Name;

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

    /// <summary>
    /// Description of the memory layout of an ECU. Obsolete keyword. Please use <see cref="MEMORY_SEGMENT"/> instead.
    /// </summary>
    [Base(IsObsolete = "Obsolete keyword. Please use MEMORY_SEGMENT instead.")]
    public class MEMORY_LAYOUT : Asap2Base
    {
        public enum PrgType
        {
            PRG_CODE,
            PRG_DATA,
            PRG_RESERVED,
        }

        public MEMORY_LAYOUT(Location location, PrgType prgType, UInt64 Address, UInt64 Size,
            long offset0, long offset1, long offset2, long offset3, long offset4) : base(location)
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
        public CALIBRATION_METHOD(Location location, string Method, ulong Version) : base(location)
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
        public CALIBRATION_HANDLE(Location location) : base(location)
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
        public SYSTEM_CONSTANT(Location location, string name, string value) : base(location)
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
        public MOD_PAR(Location location, string comment) : base(location)
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

        [Element(11, IsDictionary = true)]
        public Dictionary<string, MEMORY_SEGMENT> memory_segment = new Dictionary<string, MEMORY_SEGMENT>();

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
        public DISCRETE(Location location) : base(location) { }
    }

    [Base(IsSimple = true)]
    public class READ_ONLY : Asap2Base
    {
        public READ_ONLY(Location location) : base(location) { }
    }

    [Base(IsSimple = true)]
    public class READ_WRITE : Asap2Base
    {
        public READ_WRITE(Location location) : base(location) { }
    }

    [Base(IsSimple = true)]
    public class GUARD_RAILS : Asap2Base
    {
        public GUARD_RAILS(Location location) : base(location) { }
    }

    [Base()]
    public class GROUP : Asap2Base
    {
        public GROUP(Location location, string Name, string GroupLongIdentifier) : base(location)
        {
            this.Name = Name;
            this.GroupLongIdentifier = GroupLongIdentifier;
        }
        [Element(1, IsArgument = true, Comment = " GroupName           ")]
        public string Name;
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
        public REF_MEASUREMENT(Location location) : base(location) { }

        [Element(0, IsArgument = true, IsList = true, Comment = " Measurement references ")]
        public List<string> reference = new List<string>();
    }

    [Base()]
    public class SUB_GROUP : Asap2Base
    {
        public SUB_GROUP(Location location) : base(location) { }

        [Element(0, IsArgument = true, IsList = true, Comment = " Sub groups ")]
        public List<string> groups = new List<string>();
    }

    [Base(IsSimple = true)]
    public class ROOT : Asap2Base
    {
        public ROOT(Location location) : base(location) { }
    }

    /// <summary>
    /// Lists the maps which comprise a cuboid.
    /// </summary>
    [Base()]
    public class MAP_LIST : Asap2Base
    {
        public MAP_LIST(Location location) : base(location)
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
        public VIRTUAL_CHARACTERISTIC(Location location, string Formula) : base(location)
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
        public DEPENDENT_CHARACTERISTIC(Location location, string Formula) : base(location)
        {
            this.Formula = Formula;
        }
        [Element(0, IsString = true)]
        public string Formula;
        [Element(1, IsArgument = true, IsList = true)]
        public List<string> Characteristic = new List<string>();
    }
}
