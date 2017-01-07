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
            SI_CONVERSION,
        }
        public UNIT(Location location, string Name, string LongIdentifier, string Display, Type type) : base(location)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.Display = Display;
            this.type = type;
        }
        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name;
        [Element(2, IsString = true,   Comment = " LongIdentifier ")]
        public string LongIdentifier;
        [Element(3, IsString = true,   Comment = " Display        ")]
        public string Display;
        [Element(4, IsArgument = true, Comment = " Type           ")]
        public Type type;

        /// <summary>
        /// Reference to a physical unit.
        /// </summary>
        [Element(5, IsArgument = true, Name = "REF_UNIT")]
        public string ref_unit;

        [Element(6)]
        public SI_EXPONENTS si_exponents;

        [Element(7)]
        public UNIT_CONVERSION unit_conversion;
    }

}
