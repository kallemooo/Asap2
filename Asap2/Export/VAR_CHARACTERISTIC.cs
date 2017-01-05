using System;
using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// Description of a tunable parameter with more than one value in ECU memory.
    /// The description consists of a reference to the tunable parameter (<see cref="CHARACTERISTIC"/>),
    /// references to variant criteria (<see cref="VAR_CRITERION"/>) defining the possible variants and a reference
    /// to the list of ECU addresses for the values for each variant (<see cref="VAR_ADDRESS"/>).
    /// </summary>
    [Base()]
    public class VAR_CHARACTERISTIC : Asap2Base
    {
        public VAR_CHARACTERISTIC(Location location, string Name) : base(location)
        {
            this.Name = Name;
        }
        [Element(1, IsArgument = true, Comment = " Name          ")]
        public string Name;
        [Element(2, IsArgument = true, Comment = " CriterionName ", IsList = true)]
        public List<string> CriterionNames = new List<string>();

        [Element(3)]
        public VAR_ADDRESS var_address;
    }

    /// <summary>
    /// List of ECU addresses for each value of a variant coded tunable parameter (<see cref="VAR_CHARACTERISTIC"/>).
    /// The number of addresses in this list must match the number of variants from the referenced variant criteria (<see cref="VAR_CRITERION"/>).
    /// </summary>
    [Base()]
    public class VAR_ADDRESS : Asap2Base
    {
        public VAR_ADDRESS(Location location) : base(location) { }
        [Element(0, IsArgument = true, IsList = true, CodeAsHex = true)]
        public List<UInt64> Addresses = new List<UInt64>();
    }
}
