using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    /// <summary>
    /// Class for handling location information.
    /// </summary>
    public class Location : QUT.Gppg.IMerge<Location>
    {
        private int startLine;   // start line
        private int startColumn; // start column
        private int endLine;     // end line
        private int endColumn;   // end column
        private string fileName; // current filename.

        /// <summary>
        /// The line at which the text span starts.
        /// </summary>
        public int StartLine { get { return startLine; } }

        /// <summary>
        /// The column at which the text span starts.
        /// </summary>
        public int StartColumn { get { return startColumn; } }

        /// <summary>
        /// The line on which the text span ends.
        /// </summary>
        public int EndLine { get { return endLine; } }

        /// <summary>
        /// The column of the first character
        /// beyond the end of the text span.
        /// </summary>
        public int EndColumn { get { return endColumn; } }

        /// <summary>
        /// The column at which the text span starts.
        /// </summary>
        public string FileName { get { return fileName; } }

        /// <summary>
        /// Default no-arg constructor.
        /// </summary>
        public Location() { startLine = 0; startColumn = 0; endLine = 0; endColumn = 0; fileName = ""; }

        /// <summary>
        /// Constructor for text-span with given start and end.
        /// </summary>
        /// <param name="sl">start line</param>
        /// <param name="sc">start column</param>
        /// <param name="el">end line </param>
        /// <param name="ec">end column</param>
        /// <param name="fn">file name</param>
        public Location(int sl, int sc, int el, int ec, string fn) { startLine = sl; startColumn = sc; endLine = el; endColumn = ec; fileName = fn; }

        /// <summary>
        /// Create a text location which spans from the 
        /// start of "this" to the end of the argument "last"
        /// </summary>
        /// <param name="last">The last location in the result span</param>
        /// <returns>The merged span</returns>
        public Location Merge(Location last) { return new Location(this.startLine, this.startColumn, last.endLine, last.endColumn, this.fileName); }
    }
}
