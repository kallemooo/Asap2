using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// Reference to other <see cref="FUNCTION"/>s, which are sub-functions of this <see cref="FUNCTION"/>.
    /// Can be used to reproduce the hierarchy of functions in the ECU software.
    /// </summary>
    [Base()]
    public class SUB_FUNCTION : Asap2Base
    {
        [Element(0, IsList = true, IsArgument = true)]
        public List<string> sub_functions = new List<string>();
    }
}
