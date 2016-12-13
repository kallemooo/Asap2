using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    public class IndentHandler
    {
        public IndentHandler(string indentType = "    ", string currentIndent = "", string newLine = "\r\n")
        {
            this.indentType = indentType;
            this.currentIndent = currentIndent;
            this.newLine = newLine;
        }

        public IndentHandler(IndentHandler iH)
        {
            this.indentType = iH.indentType;
            this.currentIndent = iH.currentIndent + iH.indentType;
            this.newLine = iH.newLine;
        }

        public string indentType { private set; get; }
        public string currentIndent { private set; get; }
        public string newLine { private set; get; }
    }

    public class Asap2File
    {
        public Asap2File() {}
        public string fileComment;
        public ASAP2_VERSION asap2_version;
        public PROJECT project;

        public string serialize(IndentHandler iH)
        {
            string outStr = "";
            if (this.fileComment != null)
            {
                outStr += iH.currentIndent;
                outStr += fileComment;
                outStr += iH.newLine;
            }
            if (this.asap2_version != null)
            {
                outStr += this.asap2_version.serialize(new IndentHandler(iH));
                outStr += iH.newLine;
            }
            outStr += this.project.serialize(new IndentHandler(iH));
            outStr += iH.newLine;
            return outStr;
        }
    }

    public class ASAP2_VERSION
    {
        public ASAP2_VERSION(UInt32 major, UInt32 minor)
        {
            this.major = major;
            this.minor = minor;
        }
        public string comment;
        public UInt32 major;
        public UInt32 minor;

        public string serialize(IndentHandler iH)
        {
            string outStr = "";
            if (this.comment != null)
            {
                outStr += comment;
                outStr += iH.newLine;
            }
            outStr += "ASAP2_VERSION" + " " + this.major.ToString() + " " + this.minor.ToString();
            return outStr;
        }
    }

    public class PROJECT
    {
        public PROJECT(string name, string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
            this.name = name;
        }
        public string comment;
        public string nameComment = "/* Name           */";
        public string name;
        public string LongIdentifierComment = "/* LongIdentifier */";
        public string LongIdentifier;
        public HEADER header;
        public Dictionary<string, MODULE> modules = new Dictionary<string, MODULE>();

        public string serialize(IndentHandler iH)
        {
            string outStr = "";
            if (this.comment != null)
            {
                outStr += comment;
                outStr += iH.newLine;
            }
            outStr += "/begin PROJECT" + iH.newLine;
            outStr += iH.currentIndent;
            if (this.nameComment != "")
            {
                outStr += this.nameComment + " ";
            }
            outStr += this.name + iH.newLine;

            outStr += iH.currentIndent;
            if (this.LongIdentifierComment != "")
            {
                outStr += this.LongIdentifierComment + " ";
            }
            outStr += "\"" + this.LongIdentifier + "\"" + iH.newLine;
            if (this.header != null)
            {
                outStr += this.header.serialize(new IndentHandler(iH));
            }
            
            foreach (MODULE module in this.modules.Values)
            {
                outStr += iH.newLine;
                outStr += module.serialize(new IndentHandler(iH));
            }
            outStr += iH.newLine;
            outStr += "/end PROJECT";
            return outStr;
        }
    }

    public class HEADER
    {
        public HEADER(string longIdentifier, string version, string project_no)
        {
            this.longIdentifier = longIdentifier;
            this.version = version;
            this.project_no = project_no;
        }
        public string longIdentifier;
        public string version;
        public string project_no;

        public string serialize(IndentHandler iH)
        {
            string outStr = "";
            outStr += iH.currentIndent + "/begin HEADER" + iH.newLine;
            outStr += iH.currentIndent + iH.indentType + "\"" + this.longIdentifier + "\"" + iH.newLine;
            outStr += iH.currentIndent + iH.indentType + "VERSION \"" + this.version + "\"" + iH.newLine;
            outStr += iH.currentIndent + iH.indentType + "PROJECT_NO " + this.project_no + iH.newLine;
            outStr += iH.currentIndent + "/end HEADER";
            return outStr;
        }
    }

    public class MODULE
    {
        public MODULE(string name, string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
            this.name = name;
        }

        public string nameComment = "/* Name           */";
        public string name;
        public string LongIdentifierComment = "/* LongIdentifier */";
        public string LongIdentifier;
        public MOD_COMMON mod_common;
        public Dictionary<string, MEASUREMENT> measurements = new Dictionary<string, MEASUREMENT>();
    
        public string serialize(IndentHandler iH)
        {
            string outStr = "";
            outStr += iH.currentIndent + "/begin MODULE" + iH.newLine;

            outStr += iH.currentIndent + iH.indentType;
            if (this.nameComment != "")
            {
                outStr += this.nameComment + " ";
            }
            outStr += this.name + iH.newLine;

            outStr += iH.currentIndent + iH.indentType;
            if (this.LongIdentifierComment != "")
            {
                outStr += this.LongIdentifierComment + " ";
            }
            outStr += "\"" + this.LongIdentifier + "\"" + iH.newLine;
            if (this.mod_common != null)
            {
                outStr += this.mod_common.serialize(new IndentHandler(iH));
            }
            /*
            foreach (MEASUREMENT measurement in this.measurements.Values)
            {
                outStr += iH.newLine;
                outStr += measurement.serialize(new IndentHandler(iH));
            }*/
            outStr += iH.newLine;
            outStr += iH.currentIndent + "/end MODULE";
            return outStr;
        }
    }

    public enum ALIGNMENT_type
    {
          ALIGNMENT_BYTE,
          ALIGNMENT_WORD,
          ALIGNMENT_LONG,
          ALIGNMENT_INT64,
          ALIGNMENT_FLOAT32_IEEE,
          ALIGNMENT_FLOAT64_IEEE        
    }

    public enum DEPOSIT
    {
        ABSOLUTE,
        DIFFERENCE,
    }

    public enum BYTE_ORDER
    {
        MSB_FIRST,
        MSB_LAST,
    }

    public class MOD_COMMON
    {
        public MOD_COMMON(string LongIdentifier)
        {
            this.LongIdentifier = LongIdentifier;
        }

        public string comment = "/* Module default values */";
        public string LongIdentifierComment = "/* LongIdentifier     */"; 
        public string LongIdentifier;
        public DEPOSIT? deposit;
        public BYTE_ORDER? byte_order;
        public Dictionary<ALIGNMENT_type, uint> alignments = new Dictionary<ALIGNMENT_type, uint>();
        public uint? data_size;
        public string serialize(IndentHandler iH)
        {
            string outStr = "";
            outStr += iH.currentIndent + "/begin MOD_COMMON" + iH.newLine;

            if (this.comment != "")
            {
                outStr += iH.currentIndent + iH.indentType + this.comment + iH.newLine;
            }

            outStr += iH.currentIndent + iH.indentType;
            if (this.LongIdentifierComment != "")
            {
                outStr += this.LongIdentifierComment + " ";
            }
            outStr += "\"" + this.LongIdentifier + "\"";

            if (this.deposit != null)
            {
                outStr += iH.newLine;
                outStr += iH.currentIndent + iH.indentType + "DEPOSIT " + Enum.GetName(typeof(DEPOSIT), this.deposit);
            }

            if (this.byte_order != null)
            {
                outStr += iH.newLine;
                outStr += iH.currentIndent + iH.indentType + "BYTE_ORDER " + Enum.GetName(typeof(BYTE_ORDER), this.byte_order);
            }

            if (this.data_size != null)
            {
                outStr += iH.newLine;
                outStr += iH.currentIndent + iH.indentType + "DATA_SIZE " + this.data_size.ToString();
            }

            foreach (KeyValuePair<ALIGNMENT_type, uint> alignment in this.alignments)
            {
                outStr += iH.newLine;
                outStr += iH.currentIndent + iH.indentType + Enum.GetName(typeof(ALIGNMENT_type), alignment.Key) + " " + alignment.Value;
            }
            outStr += iH.newLine;
            outStr += iH.currentIndent + "/end MOD_COMMON";
            return outStr;
        }
    }

    public class MEASUREMENT
    {
        public MEASUREMENT(string name, string LongIdentifier, string Datatype, string Conversion, uint Resolution, uint Accuracy, uint LowerLimit, uint UpperLimit)
        {
            this.name = name;
            this.LongIdentifier = LongIdentifier;
            this.Datatype = Datatype;
            this.Conversion = Conversion;
            this.Resolution = Resolution;
            this.Accuracy = Accuracy;
            this.LowerLimit = LowerLimit;
            this.UpperLimit = UpperLimit;
        }
        public string name;
        public string LongIdentifier;
        public string Datatype;
        public string Conversion;
        public uint Resolution;
        public uint Accuracy;
        public uint LowerLimit;
        public uint UpperLimit;
        public UInt64? ECU_ADDRESS;
        public UInt64? ECU_ADDRESS_EXTENSION;
        public String FORMAT;
    }

}
