using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    /// <summary>
    /// Combination of variants that are not allowed.
    /// </summary>
    [Base()]
    public class VAR_FORBIDDEN_COMB : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true)]
        public List<Combo> combinations = new List<Combo>();

        /// <summary>
        /// Combination class.
        /// </summary>
        public class Combo
        {
            public Combo(string criterionName, string criterionValue)
            {
                this.criterionName = criterionName;
                this.criterionValue = criterionValue;
            }
            private string criterionName;
            private string criterionValue;

            public string CriterionName
            {
                get { return criterionName; }
            }

            public string CriterionValue
            {
                get { return criterionValue; }
            }

            public override string ToString()
            {
                return CriterionName + " " + CriterionValue;
            }
        }
    }
}
