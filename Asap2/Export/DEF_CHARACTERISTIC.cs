using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// References to <see cref="AXIS_PTS"/> or <see cref="CHARACTERISTIC"/> that belong to the function.
    /// </summary>
    [Base()]
    public class DEF_CHARACTERISTIC : Asap2Base
    {
        [Element(0, IsList = true, IsArgument = true)]
        public List<string> def_characteristics = new List<string>();
    }
}
