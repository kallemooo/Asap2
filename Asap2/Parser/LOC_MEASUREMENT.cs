using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// References to <see cref="MEASUREMENT"/>s that are defined as local variables in this <see cref="FUNCTION"/>.
    /// </summary>
    [Base()]
    public class LOC_MEASUREMENT : Asap2Base
    {
        public LOC_MEASUREMENT(Location location) : base(location) { }
        [Element(0, IsList = true, IsArgument = true)]
        public List<string> measurements = new List<string>();
    }
}
