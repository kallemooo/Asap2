using System;
using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// Description of tunable parameters, which have more than one value stored in ECU memory at different addresses.
    /// The description of variant-coded parameters and non-variant coded parameters does not differ in the <see cref="CHARACTERISTIC"/> keyword.
    /// If a parameter shall be variant coded, then <see cref="VARIANT_CODING"/> has a reference to this parameter and specifies additional properties
    /// that describe how to select the active variant (i.e. read the selected value from memory).
    /// The active variant might be selected by another tunable parameter (<see cref="VAR_SELECTION_CHARACTERISTIC"/>) or an ECU-internal variable (<see cref="VAR_MEASUREMENT"/>).
    /// </summary>
    [Base()]
    public class VARIANT_CODING : Asap2Base
    {
        public VARIANT_CODING(Location location) : base(location) { }

        /// <summary>   
        /// Indexing method to distinguish different variants, e.g. "NUMERIC" or "ALPHA".
        /// </summary>
        public enum VAR_NAMING
        {
            NUMERIC,
            ALPHA
        }

        [Element(1, IsList = true)]
        public List<VAR_CHARACTERISTIC> var_characteristic = new List<VAR_CHARACTERISTIC>();

        [Element(2, IsList = true)]
        public List<VAR_CRITERION> var_criterion = new List<VAR_CRITERION>();

        [Element(3, IsList = true)]
        public List<VAR_FORBIDDEN_COMB> forbidden_combinations = new List<VAR_FORBIDDEN_COMB>();

        /// <summary>
        /// Separation symbol between the name of a variant coded parameter and the variant extension.
        /// </summary>
        [Element(4, IsString = true, Name = "VAR_SEPERATOR")]
        public string var_seperator;

        [Element(5, IsArgument = true, Name = "VAR_NAMING")]
        public VAR_NAMING var_naming;
    }
}
