using System;
using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// Defines units that can be referenced by <see cref="MEASUREMENT"/>, <see cref="CHARACTERISTIC"/>, <see cref="AXIS_PTS"/> and <see cref="COMPU_METHOD"/>.
    /// Units shall be stated according to the International System of Units (SI).
    /// UNIT supports SI based units described by exponents of the seven base units as well as derived units
    /// described by a reference unit and a linear conversion method.
    /// </summary>
    [Base()]
    public class UNIT : Asap2Base
    {
        public enum Type
        {
            DERIVED,
            EXTENDED_SI,
        }
        public UNIT(Location location, string Name, string LongIdentifier, string Display, Type type) : base(location)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.Display = Display;
            this.type = type;
        }
        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name { get; private set; }
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier { get; private set; }
        [Element(3, IsString = true, Comment = " Display        ")]
        public string Display { get; private set; }
        [Element(4, IsArgument = true, Comment = " Type           ")]
        public Type type { get; private set; }

        /// <summary>
        /// Reference to a physical unit.
        /// </summary>
        [Element(5, IsArgument = true, Name = "REF_UNIT")]
        public string ref_unit;

        [Element(6)]
        public SI_EXPONENTS si_exponents;

        [Element(7)]
        public UNIT_CONVERSION unit_conversion;

        public void Validate(IErrorReporter errorReporter, MODULE module)
        {
            base.ValidateIdentifier(Name, errorReporter);

            if (type == Type.DERIVED)
            {
                if (ref_unit == null || ref_unit == "")
                {
                    base.reportErrorOrWarning("DERIVED UNIT must have a referenced UNIT in REF_UNIT", false, errorReporter);
                }

                if (unit_conversion == null)
                {
                    base.reportErrorOrWarning("DERIVED UNIT must have a UNIT_CONVERSION", false, errorReporter);
                }
            }
            else
            {
                if (si_exponents == null)
                {
                    base.reportErrorOrWarning("EXTENDED_SI UNIT must have a SI_EXPONENTS", false, errorReporter);
                }
                if (ref_unit != null && ref_unit != "")
                {
                    base.reportErrorOrWarning("EXTENDED_SI UNIT shall not have a REF_UNIT", false, errorReporter);
                }
            }

            if (ref_unit != null && ref_unit != "")
            {
                /* Validate that refered UNIT exists */
                if (!module.Units.ContainsKey(ref_unit))
                {
                    base.reportErrorOrWarning(string.Format("Referenced UNIT '{0}' in REF_UNIT not found", ref_unit), false, errorReporter);
                }
            }
        }
    }
}
