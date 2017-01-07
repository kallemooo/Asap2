using System;

namespace Asap2
{
    /// <summary>
    /// Specifies the exponents of the seven SI base units to express this derived SI unit.
    /// </summary>
    [Base(IsSimple = true)]
    public class SI_EXPONENTS : Asap2Base
    {
        public SI_EXPONENTS(Location location, Int64 Length, Int64 Mass, Int64 Time, Int64 ElectricCurrent, Int64 Temperature, Int64 AmountOfSubstance, Int64 LuminousIntensity) : base(location)
        {
            this.Length = Length;
            this.Mass = Mass;
            this.Time = Time;
            this.ElectricCurrent = ElectricCurrent;
            this.Temperature = Temperature;
            this.LuminousIntensity = LuminousIntensity;
        }
        [Element(0, IsArgument = true)]
        public Int64 Length;
        [Element(1, IsArgument = true)]
        public Int64 Mass;
        [Element(2, IsArgument = true)]
        public Int64 Time;
        [Element(3, IsArgument = true)]
        public Int64 ElectricCurrent;
        [Element(4, IsArgument = true)]
        public Int64 Temperature;
        [Element(5, IsArgument = true)]
        public Int64 AmountOfSubstance;
        [Element(6, IsArgument = true)]
        public Int64 LuminousIntensity;
    }
}
