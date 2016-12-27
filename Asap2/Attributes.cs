using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BaseAttribute : Attribute
    {
        // Private fields.
        private bool isSimple;

        public BaseAttribute()
        {
            this.isSimple = false;
        }

        /// <summary>
        /// Is this a simple type that shall not start with /begin. Default is complex type that starts with /begin.
        /// </summary>
        public virtual bool IsSimple
        {
            get { return isSimple; }
            set { isSimple = value; }
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ElementAttribute : Attribute
    {
        // Private fields.
        private string name;
        private uint? sortOrder;
        private string comment;
        private bool isName;
        private bool isArgument;
        private bool isString;
        private bool isComment;
        private bool isPreComment;
        private bool isDictionary;
        private bool forceNewLine;
        private bool codeAsHex;

        public ElementAttribute()
        {
            this.isName = false;
            this.isArgument = false;
            this.isString = false;
            this.isComment = false;
            this.isPreComment = false;
            this.isDictionary = false;
            this.forceNewLine = false;
            this.codeAsHex = false;
        }

        /// <summary>
        /// Name for the element. Replaces class name as element name.
        /// </summary>
        public virtual bool IsName
        {
            get { return isName; }
            set { isName = value; }
        }

        /// <summary>
        /// Is this an argument of an element.
        /// </summary>
        public virtual bool IsArgument
        {
            get { return isArgument; }
            set { isArgument = value; }
        }

        /// <summary>
        /// Is this a long argument of an element of string type.
        /// </summary>
        public virtual bool IsString
        {
            get { return isString; }
            set { isString = value; }
        }

        /// <summary>
        /// Is comment in an element.
        /// </summary>
        public virtual bool IsComment
        {
            get { return isComment; }
            set { isComment = value; }
        }

        /// <summary>
        /// Is comment to placed before an element.
        /// </summary>
        public virtual bool IsPreComment
        {
            get { return isPreComment; }
            set { isPreComment = value; }
        }

        /// <summary>
        /// Is a dictionary of type Dictionary&lt;string, object&gt;
        /// </summary>
        public virtual bool IsDictionary
        {
            get { return isDictionary; }
            set { isDictionary = value; }
        }

        /// <summary>
        /// Force extra newline before this element.
        /// </summary>
        public virtual bool ForceNewLine
        {
            get { return forceNewLine; }
            set { forceNewLine = value; }
        }

        /// <summary>
        /// Code this element as hex.
        /// </summary>
        public virtual bool CodeAsHex
        {
            get { return codeAsHex; }
            set { codeAsHex = value; }
        }

        /// <summary>
        /// Override serialization name.
        /// </summary>
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Set serialization sortorder.
        /// </summary>
        public virtual uint SortOrder
        {
            get { return sortOrder.Value; }
            set { sortOrder = value; }
        }

        /// <summary>
        /// Optional comment.
        /// </summary>
        public virtual string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }
}
