using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2
{

    [Base()]
    public class COMPU_METHOD : Asap2Base
    {
        public COMPU_METHOD(Location location, string Name, string LongIdentifier, ConversionType conversionType, string Format, string Unit) : base(location)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
            this.conversionType = conversionType;
            this.Format = Format;
            this.Unit = Unit;
        }

        [Element(1, IsArgument = true, Comment = " Name           ")]
        public string Name { get; private set; }
        [Element(2, IsString = true, Comment = " LongIdentifier ")]
        public string LongIdentifier { get; private set; }
        [Element(3, IsArgument = true, Comment = " ConversionType ")]
        public ConversionType conversionType { get; private set; }
        [Element(4, IsString = true, Comment = " Display Format ")]
        public string Format { get; private set; }
        /// <summary>
        /// Reference to a physical unit. Reduntant if <see cref="ref_unit"/> is defined.
        /// </summary>
        [Element(5, IsString = true, Comment = " Physical Unit  ")]
        public string Unit { get; private set; }
        [Element(6)]
        public COEFFS coeffs;
        [Element(7)]
        public COEFFS_LINEAR coeffs_linear;

        /// <summary>
        /// Reference to conversion table to use.
        /// </summary>
        [Element(8, IsArgument = true, Name = "COMPU_TAB_REF")]
        public string compu_tab_ref;

        /// <summary>
        /// Formula to use if <see cref="conversionType"/> == <see cref="ConversionType.FORM"/>.
        /// </summary>
        [Element(9)]
        public FORMULA formula;

        /// <summary>
        /// Reference to a physical unit.
        /// </summary>
        [Element(10, IsArgument = true, Name = "REF_UNIT")]
        public string ref_unit;

        /// <summary>
        /// Reference to a verbal conversion table (<see cref="COMPU_VTAB"/> or <see cref="COMPU_VTAB_RANGE"/>).
        /// Used to split up the value range of the measurement to a numerical part and a verbal part.
        /// The latter contains status information about the numerical part such as providing an error or describing the quality of the measurement.
        /// </summary>
        [Element(11, IsArgument = true, Name = "STATUS_STRING_REF")]
        public string status_string_ref;

        public void Validate(IErrorReporter errorReporter, MODULE module)
        {
            base.ValidateIdentifier(Name, errorReporter);

            if (conversionType == ConversionType.FORM)
            {
                if (formula != null)
                {
                    formula.Validate(errorReporter, module);
                }
                else
                {
                    base.reportErrorOrWarning("COMPU_METHOD with ConversionType FORM requires a FORMULA definition but no FORMULA where defined", false, errorReporter);
                }
            }
            else if (conversionType == ConversionType.LINEAR)
            {
                if (coeffs_linear == null)
                {
                    base.reportErrorOrWarning("COMPU_METHOD with ConversionType LINEAR requires a COEFFS_LINEAR definition but no COEFFS_LINEAR where defined", false, errorReporter);
                }
            }
            else if (conversionType == ConversionType.RAT_FUNC)
            {
                if (coeffs == null)
                {
                    base.reportErrorOrWarning("COMPU_METHOD with ConversionType RAT_FUNC requires a COEFFS definition but no COEFFS where defined", false, errorReporter);
                }
            }
            else if ((conversionType == ConversionType.TAB_INTP) || (conversionType == ConversionType.TAB_NOINTP) || (conversionType == ConversionType.TAB_VERB))
            {
                if (compu_tab_ref == null)
                {
                    base.reportErrorOrWarning(string.Format("COMPU_METHOD with conversionType {0} requires a COMPU_TAB_REF definition but no COMPU_TAB_REF where defined", ConversionType.TAB_VERB.ToString()), false, errorReporter);
                }
                else
                {
                    Asap2Base tab;
                    if (module.CompuTabCompuVtabCompuVtabRanges.TryGetValue(compu_tab_ref, out tab))
                    {
                        if ((conversionType == ConversionType.TAB_INTP) || (conversionType == ConversionType.TAB_NOINTP))
                        {
                            if (tab.GetType() == typeof(COMPU_TAB))
                            {
                                var tmp = (tab as COMPU_TAB);
                                if (tmp.conversionType != conversionType)
                                {
                                    tmp.reportErrorOrWarning(string.Format("ConversionType '{0}' is not the expeced ConversionType '{1}'", tmp.conversionType, conversionType), false, errorReporter);
                                }
                            }
                            else
                            {
                                base.reportErrorOrWarning(string.Format("Reference '{0}' in COMPU_TAB_REF is not of type COMPU_TAB", compu_tab_ref), false, errorReporter);
                            }
                        }
                        else
                        {
                            if (tab.GetType() == typeof(COMPU_VTAB))
                            {
                                var tmp = (tab as COMPU_VTAB);
                                if (tmp.conversionType != conversionType)
                                {
                                    tmp.reportErrorOrWarning(string.Format("ConversionType '{0}' is not the expeced ConversionType '{1}'", tmp.conversionType, conversionType), false, errorReporter);
                                }
                            }
                            else if (tab.GetType() != typeof(COMPU_VTAB_RANGE))
                            {
                                base.reportErrorOrWarning(string.Format("Reference '{0}' in COMPU_TAB_REF is not of type COMPU_VTAB or COMPU_VTAB_RANGE", compu_tab_ref), false, errorReporter);
                            }
                        }
                    }
                    else
                    {
                        base.reportErrorOrWarning(string.Format("Referenced COMPU_TAB '{0}' in COMPU_TAB_REF not found", compu_tab_ref), false, errorReporter);
                    }
                }
            }

            if (ref_unit != null && ref_unit != "")
            {
                /* Validate that refered UNIT exists */
                if (!module.Units.ContainsKey(ref_unit))
                {
                    base.reportErrorOrWarning(string.Format("Referenced UNIT '{0}' in REF_UNIT not found", ref_unit), false, errorReporter);
                }
                if (Unit != "")
                {
                    base.reportErrorOrWarning(string.Format("Both Unit and REF_UNIT is specified, value from REF_UNIT will be used"), false, errorReporter, true);
                }
            }

            if (status_string_ref != null && status_string_ref != "")
            {
                Asap2Base tab;
                if (module.CompuTabCompuVtabCompuVtabRanges.TryGetValue(status_string_ref, out tab))
                {
                    if (tab.GetType() == typeof(COMPU_VTAB))
                    {
                        var tmp = (tab as COMPU_VTAB);
                        if (tmp.conversionType != conversionType)
                        {
                            tmp.reportErrorOrWarning(string.Format("ConversionType '{0}' is not the expected ConversionType '{1}'", tmp.conversionType, ConversionType.TAB_VERB), false, errorReporter);
                        }
                    }
                    else if (tab.GetType() != typeof(COMPU_VTAB_RANGE))
                    {
                        base.reportErrorOrWarning(string.Format("Reference '{0}' in STATUS_STRING_REF is not of type COMPU_VTAB or COMPU_VTAB_RANGE", status_string_ref), false, errorReporter);
                    }
                }
                else
                {
                    base.reportErrorOrWarning(string.Format("Referenced COMPU_VTAB or COMPU_VTAB_RANGE '{0}' in STATUS_STRING_REF not found", status_string_ref), false, errorReporter);
                }
            }
        }
    }
}
