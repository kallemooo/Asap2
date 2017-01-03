using System.Collections.Generic;

namespace Asap2
{
    /// <summary>
    /// Lists <see cref="GROUP"/>s and access rights for named users. Access rights might be read-only or read &amp; write. 
    /// </summary>
    [Base()]
    public class USER_RIGHTS : Asap2Base
    {
        public USER_RIGHTS(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// UserLevelId
        /// </summary>
        [Element(1, IsArgument = true, Comment = " UserLevelId ")]
        public string Name;

        [Element(2, IsList = true)]
        public List<REF_GROUP> ref_group = new List<REF_GROUP>();

        [Element(3)]
        public READ_ONLY read_only;
    }

    [Base()]
    public class REF_GROUP : Asap2Base
    {
        [Element(0, IsArgument = true, IsList = true, Comment = " Group references ")]
        public List<string> reference = new List<string>();
    }

}
