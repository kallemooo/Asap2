using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// References to <see cref="AXIS_PTS"/> or <see cref="CHARACTERISTIC"/> that are used but not owned by this FUNCTION or which belong to this GROUP.
    /// </summary>
    [Base()]
    public class REF_CHARACTERISTIC : Asap2Base
    {
        public REF_CHARACTERISTIC(Location location) : base(location) { }
        [Element(0, IsList = true, IsArgument = true)]
        public List<string> reference = new List<string>();
    }
}
