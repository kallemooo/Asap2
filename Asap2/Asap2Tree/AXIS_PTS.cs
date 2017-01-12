using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    /// <summary>
    /// Axes contain the sample point values for curves and maps.
    /// The keyword describes the properties of an axis, such as its address in memory, references to the input variable (<see cref="MEASUREMENT"/>),
    /// record layout and computation method, the maximum number of sample points and further properties.
    /// </summary>
    [Base()]
    public class AXIS_PTS : Asap2Base
    {
        public AXIS_PTS(Location location, string Name, string LongIdentifier, UInt64 Address, string InputQuantity, string Deposit, decimal MaxDiff, string Conversion, UInt64 MaxAxisPoints, decimal LowerLimit, decimal UpperLimit) : base(location)
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
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
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
}
