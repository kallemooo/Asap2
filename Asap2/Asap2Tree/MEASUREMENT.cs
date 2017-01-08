using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    [Base()]
    public class MEASUREMENT : Asap2Base
    {
        public enum LAYOUT
        {
            ROW_DIR,
            COLUMN_DIR,
        }

        public MEASUREMENT(Location location, string Name, string LongIdentifier, DataType Datatype, string Conversion, uint Resolution, decimal Accuracy, decimal LowerLimit, decimal UpperLimit) : base(location)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.Datatype = Datatype;
            this.Conversion = Conversion;
            this.Resolution = Resolution;
            this.Accuracy = Accuracy;
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
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
}
