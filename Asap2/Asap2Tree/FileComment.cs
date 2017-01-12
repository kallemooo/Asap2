using System;
using System.Text;

namespace Asap2
{
    /// <summary>
    /// Class for holding multi line comments. Use <see cref="Environment.NewLine"/> when adding a new line to the comment.
    /// Start ('/* ') and end (' */') of the comment block is added by the class. 
    /// </summary>
    public class FileComment : Asap2Base
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="comment">First comment line.</param>
        /// <param name="startWithStart">Indicates if each comment line shall start with a *.</param>
        public FileComment(string comment = null, bool startNewLineWithStar = false) : base(new Location())
        {
            if (comment != null)
            {
                this.comment = new StringBuilder();
                this.comment.Append(comment);
                this.startNewLineWithStar = startNewLineWithStar;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="comment">First comment line.</param>
        /// <param name="startWithStart">Indicates if each comment line shall start with a *.</param>
        public FileComment(Location location, string comment = null, bool startNewLineWithStar = false) : base(location)
        {
            if (comment != null)
            {
                this.comment = new StringBuilder();
                this.comment.Append(comment);
                this.startNewLineWithStar = startNewLineWithStar;
            }
        }

        private StringBuilder comment;
        public StringBuilder Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private bool startNewLineWithStar;
        public bool StartNewLineWithStar
        {
            get { return startNewLineWithStar; }
        }

        public void Append(object value)
        {
            Comment.Append(value);
        }

        public override string ToString()
        {
            comment.Insert(0, "/* ");
            if (StartNewLineWithStar)
            {
                comment.Replace(Environment.NewLine, Environment.NewLine + " * ");
                comment[comment.Length - 1] = '/';
            }
            else
            {
                comment.Append(" */");
            }
            return comment.ToString();
        }
    }
}
