using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
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

        public CHARACTERISTIC(Location location, string Name, string LongIdentifier, Type type, UInt64 Address, string Deposit, decimal MaxDiff, string Conversion, decimal LowerLimit, decimal UpperLimit) : base(location)
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
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
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
}
