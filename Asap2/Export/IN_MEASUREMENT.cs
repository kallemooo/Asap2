using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// References to <see cref="MEASUREMENT"/>s that are defined as inputs to this <see cref="FUNCTION"/>.
    /// </summary>
    [Base()]
    public class IN_MEASUREMENT : Asap2Base
    {
        [Element(0, IsList = true, IsArgument = true)]
        public List<string> measurements = new List<string>();
    }
}
