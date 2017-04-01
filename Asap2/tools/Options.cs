using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2.tools
{
    /// <summary>
    /// Class for handling of common options for the tools.
    /// </summary>
    public class Options
    {
        public enum ModuleMergeType
        {
            /// <summary>
            /// Merge modules seperate in to the first project
            /// </summary>
            Multiple,
            /// <summary>
            /// Merge data from all modules in to the first A2L file module.
            /// </summary>
            One
        }

        public enum MergeConflictType
        {
            /// <summary>
            /// If data with the same name exists in multiple MODULES, use the version in the first. Do not warn.
            /// </summary>
            UseFromFirstModule = 0,
            /// <summary>
            /// If data with the same name exists in multiple MODULES, use the version in the first. Warn about the problem.
            /// </summary>
            UseFromFirstModuleAndWarn = 1,
            /// <summary>
            /// If data with the same name exists in multiple MODULES, report error and abort.
            /// </summary>
            AbortWithError = 2,
        }

        [Flags]
        public enum ElementTypes
        {
            NONE = 0,
            MEASUREMENT = 0x00001,
            CHARACTERISTIC = 0x00002,
            AXIS_PTS = 0x00004,
            COMPU_TAB = 0x00008,
            COMPU_VTAB = 0x00010,
            COMPU_VTAB_RANGE = 0x00020,
            COMPU_METHOD = 0x00040,
            FRAME = 0x00080,
            GROUP = 0x00100,
            FUNCTION = 0x00200,
            RECORD_LAYOUT = 0x00400,
            UNIT = 0x00800,
            USER_RIGHTS = 0x01000,
            A2ML = 0x02000,
            MOD_COMMON = 0x04000,
            MOD_PAR = 0x08000,
            VARIANT_CODING = 0x10000,
            IF_DATA = 0x20000,
            ALL = MEASUREMENT | CHARACTERISTIC | AXIS_PTS | COMPU_TAB | COMPU_VTAB | COMPU_VTAB_RANGE |
                COMPU_METHOD | FRAME | GROUP | FUNCTION | RECORD_LAYOUT | UNIT | USER_RIGHTS | A2ML |
                MOD_COMMON | MOD_PAR | VARIANT_CODING | IF_DATA
        }

#region MergeOptions
        public ModuleMergeType ModuleMerge;
        public MergeConflictType MergeConflict;
        public ElementTypes ElementToIgnoreWhenMerging;
#endregion

#region DeleteOptions
#endregion
    }

    [Serializable()]
    public class ErrorException : System.Exception
    {
        [Flags]
        public enum ErrorCodes : int
        {
            NoError = 0x00,
            ParameterError = 0x01,
            InputFileNotFoundOrNotReadable = 0x02,
            OutoutFileNotWritable = 0x04,
            MergeError = 0x08,
            InFileParsingError = 0x10,
            FileValidationError = 0x20,
            ModuleNotFoundError = 0x40,
            UnknownError = 0xFF
        }

        public ErrorCodes errorCode;

        public ErrorException(ErrorCodes errorCode)
            : base()
        {
            this.errorCode = errorCode;
        }
        public ErrorException(ErrorCodes errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
        public ErrorException(ErrorCodes errorCode, string message, System.Exception inner)
            : base(message, inner)
        {
            this.errorCode = errorCode;
        }

        public override string ToString()
        {
            return String.Format("Error: {0} : {1}", errorCode.ToString(), base.Message);
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

}
