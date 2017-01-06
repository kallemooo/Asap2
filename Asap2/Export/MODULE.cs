using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{
    [Base()]
    public class MODULE : Asap2Base , IValidator
    {
        public MODULE(Location location) : base(location)
        {
        }

        [Element(1, IsArgument = true)]
        public string name;

        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier;

        [Element(3, IsList = true)]
        public List<Asap2Base> elements = new List<Asap2Base>();

        public void Validate(IErrorReporter errorReporter)
        {
            {
                var list = elements.FindAll(x => x.GetType() == typeof(A2ML));
                if (list != null)
                {
                    if (list.Count > 1)
                    {
                        list[list.Count - 1].reportErrorOrWarning("Second ASAP2_VERSION found, shall only be one", false, errorReporter);
                    }
                }
            }
            {
                var list = elements.FindAll(x => x.GetType() == typeof(MOD_COMMON));
                if (list != null)
                {
                    if (list.Count > 1)
                    {
                        list[list.Count - 1].reportErrorOrWarning("Second MOD_COMMON found, shall only be one", false, errorReporter);
                    }
                }
            }
            {
                var list = elements.FindAll(x => x.GetType() == typeof(MOD_PAR));
                if (list != null)
                {
                    if (list.Count > 1)
                    {
                        list[list.Count - 1].reportErrorOrWarning("Second MOD_PAR found, shall only be one", false, errorReporter);
                    }
                }
            }
        }

        /*
        [Element(7, IsDictionary = true, Comment = " Measurment data for the module ")]
        public Dictionary<string, MEASUREMENT> measurements = new Dictionary<string, MEASUREMENT>();

        [Element(8, IsDictionary = true, Comment = " Module COMPU_METHODs ")]
        public Dictionary<string, COMPU_METHOD> compu_methods = new Dictionary<string, COMPU_METHOD>();

        [Element(9, IsDictionary = true, Comment = " Conversion tables for the module COMPU_METHODs ")]
        public Dictionary<string, COMPU_TAB> COMPU_TABs = new Dictionary<string, COMPU_TAB>();

        [Element(10, IsDictionary = true, Comment = " Verbal conversion tables for the module ")]
        public Dictionary<string, COMPU_VTAB> COMPU_VTABs = new Dictionary<string, COMPU_VTAB>();

        [Element(11, IsDictionary = true, Comment = " Verbal conversion tables with parameter ranges for the module ")]
        public Dictionary<string, COMPU_VTAB_RANGE> COMPU_VTAB_RANGEs = new Dictionary<string, COMPU_VTAB_RANGE>();

        [Element(12, IsDictionary = true, Comment = " Groups for the module ")]
        public Dictionary<string, GROUP> groups = new Dictionary<string, GROUP>();

        [Element(13, IsDictionary = true, Comment = " Characteristic data for the module ")]
        public Dictionary<string, CHARACTERISTIC> characteristics = new Dictionary<string, CHARACTERISTIC>();

        [Element(14, IsDictionary = true, Comment = " AXIS_PTS data for the module ")]
        public Dictionary<string, AXIS_PTS> axis_pts = new Dictionary<string, AXIS_PTS>();

        [Element(15, IsDictionary = true, Comment = " RECORD_LAYOUT data for the module ")]
        public Dictionary<string, RECORD_LAYOUT> record_layout = new Dictionary<string, RECORD_LAYOUT>();

        [Element(16, IsDictionary = true, Comment = " FUNCTIONs for the module ")]
        public Dictionary<string, FUNCTION> functions = new Dictionary<string, FUNCTION>();

        [Element(17, IsDictionary = true, Comment = " UNITs for the module ")]
        public Dictionary<string, UNIT> units = new Dictionary<string, UNIT>();

        [Element(18, IsDictionary = true, Comment = " USER_RIGHTS for the module ")]
        public Dictionary<string, USER_RIGHTS> user_rights = new Dictionary<string, USER_RIGHTS>();

        [Element(19, IsDictionary = true, Comment = " FRAMEs for the module ")]
        public Dictionary<string, FRAME> frames = new Dictionary<string, FRAME>();

        [Element(20, IsList = true, Comment = " VARIANT_CODING for the module ")]
        public VARIANT_CODING variant_coding;
        */
    }
}
