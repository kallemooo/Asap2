using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    /// <summary>
    /// Base class for the Asap2Tree.
    /// </summary>
    public abstract class Asap2Base
    {
        private static ulong orderId;

        public ulong OrderID { get; protected set; }

        // Static constructor to initialize the static member, orderId. This
        // constructor is called one time, automatically, before any instance
        // of Asap2Base is created, or currentID is referenced.
        static Asap2Base()
        {
            orderId = 0;
        }

        public Asap2Base(Location location)
        {
            this.location = location;
            this.OrderID = GetOrderID();
        }

        protected ulong GetOrderID()
        {
            // currentID is a static field. It is incremented each time a new
            // instance of Asap2Base is created.
            return ++orderId;
        }

        public Location location { get; set; }

        public void reportErrorOrWarning(string message, bool isError, IErrorReporter errorReporter)
        {
            if (isError)
            {
                string msg = string.Format("{0} : Line: {1} : Row: {2} : ValidationError : {3}", location.FileName, location.StartLine, location.StartColumn, message);
                errorReporter.reportError(msg);
                throw new ValidationErrorException(msg);
            }
            else
            {
                errorReporter.reportError(string.Format("{0} : Line: {1} : Row: {2} : ValidationWarning : {3}", location.FileName, location.StartLine, location.StartColumn, message));
            }

        }

    }

    /// <summary>
    /// Data types defined by the standard.
    /// </summary>
    public enum DataType
    {
        UBYTE,
        SBYTE,
        UWORD,
        SWORD,
        ULONG,
        SLONG,
        A_UINT64,
        A_INT64,
        FLOAT32_IEEE,
        FLOAT64_IEEE
    }

    /// <summary>
    /// Data sizes defined by the standard.
    /// </summary>
    public enum DataSize
    {
        BYTE,
        WORD,
        LONG,
    }

    public enum AddrType
    {
        PBYTE,
        PWORD,
        PLONG,
        DIRECT
    }

    public enum IndexOrder
    {
        /// <summary>
        /// Increasing index with increasing address.
        /// </summary>
        INDEX_INCR,
        /// <summary>
        /// Decreasing index with increasing address.
        /// </summary>
        INDEX_DECR,
    }

    /// <summary>
    /// Conversion types used by CHARACTERISTICs and MEASUREMENT.
    /// </summary>
    public enum ConversionType
    {
        IDENTICAL,
        FORM,
        LINEAR,
        RAT_FUNC,
        TAB_INTP,
        TAB_NOINTP,
        TAB_VERB,
    }

    [Serializable()]
    public class ValidationErrorException : System.Exception
    {
        public ValidationErrorException() : base() { }
        public ValidationErrorException(string message) : base(message) { }
        public ValidationErrorException(string message, System.Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return base.Message;
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ValidationErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    interface IValidator
    {
        /// <summary>
        /// Validates the class contents according to the requirement rules.
        /// </summary>
        /// <exception cref="ValidationErrorException">Fatal validation error.</exception>
        void Validate(IErrorReporter errorReporter);
    }
}
