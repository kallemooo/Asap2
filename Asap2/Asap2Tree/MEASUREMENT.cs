using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    [Base()]
    public class MEASUREMENT : Asap2Base, IAxisPtsCharacteristicMeasurement
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
        public string Name { get; private set; }
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier { get; private set; }
        [Element(3, IsArgument = true, Comment = " DataType       ")]
        public DataType Datatype { get; private set; }

        /// <summary>
        /// Reference to the relevant record of the description of the conversion method (see <see cref="COMPU_METHOD"/>).
        /// If there is no conversion method, as in the case of <see cref="CURVE_AXIS"/>,
        /// the parameter ‘Conversion’ should be set to “NO_COMPU_METHOD" (measurement and calibration systems must be able to handle this case). 
        /// </summary>
        [Element(4, IsArgument = true, Comment = " Conversion     ")]
        public string Conversion { get; private set; }
        [Element(5, IsArgument = true, Comment = " Resolution     ")]
        public uint Resolution { get; private set; }
        [Element(6, IsArgument = true, Comment = " Accuracy       ")]
        public decimal Accuracy { get; private set; }
        [Element(7, IsArgument = true, Comment = " LowerLimit     ")]
        public decimal LowerLimit { get; private set; }
        [Element(8, IsArgument = true, Comment = " UpperLimit     ")]
        public decimal UpperLimit { get; private set; }
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

#region IAxisPtsCharacteristicMeasurement
        public String GetName()
        {
            return Name;
        }

        public UInt64 GetEcuAddress()
        {
            UInt64 Address = 0;
            if (ecu_address != null)
            {
                Address = ecu_address.value;
            }
            return Address;
        }

        public void SetEcuAddress(UInt64 address)
        {
            if (ecu_address == null)
            {
                ecu_address = new ECU_ADDRESS(this.location, address);
            }
            else
            {
                ecu_address.value = address;
            }
        }

        public ulong orderID()
        {
            return OrderID;
        }
#endregion

        public void Validate(IErrorReporter errorReporter, MODULE module)
        {
            base.ValidateIdentifier(Name, errorReporter);

            if (Conversion != "NO_COMPU_METHOD")
            {
                /* Validate that refered conversion method exists */
                if (!module.CompuMethods.ContainsKey(Conversion))
                {
                    base.reportErrorOrWarning(String.Format("Referenced conversion method '{0} not found", Conversion), false, errorReporter);
                }
            }

            if (ref_memory_segment != null && ref_memory_segment != "")
            {
                /* Validate that refered conversion method exists */

                var mod_par = module.elements.FirstOrDefault(x => x.GetType() == typeof(MOD_PAR)) as MOD_PAR;
                if (mod_par != null)
                {
                    if (!mod_par.memory_segment.ContainsKey(ref_memory_segment))
                    {
                        base.reportErrorOrWarning(String.Format("Referenced MEMORY_SEGMENT '{0} not found", ref_memory_segment), false, errorReporter);
                    }
                }
            }
        }
    }
}
