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
        public MODULE(Location location, string Name, string LongIdentifier) : base(location)
        {
            this.Name = Name;
            this.LongIdentifier = LongIdentifier;
        }

        [Element(1, IsArgument = true)]
        public string Name { get; private set; }

        [Element(2, IsString = true, ForceNewLine = true)]
        public string LongIdentifier { get; private set; }

        /// <summary>
        /// MODULE elements that is generic.
        /// </summary>
        public List<Asap2Base> elements = new List<Asap2Base>();

#region AXIS_PTS_MEASUREMENT_CHARACTERISTIC
        /// <summary>
        /// References to the <see cref="AXIS_PTS"/>s, <see cref="MEASUREMENT"/>s or <see cref="CHARACTERISTIC"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, IAxisPtsCharacteristicMeasurement> AxisPtsCharacteristicMeasurement = new Dictionary<string, IAxisPtsCharacteristicMeasurement>();

        /// <summary>
        /// Add <see cref="AXIS_PTS"/>, <see cref="MEASUREMENT"/> or <see cref="CHARACTERISTIC"/> to the lists.
        /// </summary>
        /// <param name="Name">Name of object</param>
        /// <param name="obj">The object</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated object is found.</exception>
        private void AddAxisPtsCharacteristicOrMeasurement(string Name,  IAxisPtsCharacteristicMeasurement obj)
        {
            try
            {
                AxisPtsCharacteristicMeasurement.Add(Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'AXIS_PTS's, 'MEASUREMENT's or 'CHARACTERISTIC's in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, Name));
            }
        }

        /// <summary>
        /// Add a <see cref="AXIS_PTS"/> to the lists.
        /// </summary>
        /// <param name="obj">AXIS_PTS object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="AXIS_PTS"/>, <see cref="MEASUREMENT"/> or <see cref="CHARACTERISTIC"/> is found.</exception>
        public void AddElement(AXIS_PTS obj)
        {
            AddAxisPtsCharacteristicOrMeasurement(obj.Name, obj);
        }

        /// <summary>
        /// Add a <see cref="CHARACTERISTIC"/> to the lists.
        /// </summary>
        /// <param name="obj">CHARACTERISTIC object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="AXIS_PTS"/>, <see cref="MEASUREMENT"/> or <see cref="CHARACTERISTIC"/> is found.</exception>
        public void AddElement(CHARACTERISTIC obj)
        {
            AddAxisPtsCharacteristicOrMeasurement(obj.Name, obj);
        }

        /// <summary>
        /// Add a <see cref="MEASUREMENT"/> to the lists.
        /// </summary>
        /// <param name="Characteristic">MEASUREMENT object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="AXIS_PTS"/>, <see cref="MEASUREMENT"/> or <see cref="CHARACTERISTIC"/> is found.</exception>
        public void AddElement(MEASUREMENT Characteristic)
        {
            AddAxisPtsCharacteristicOrMeasurement(Characteristic.Name, Characteristic);
        }

        #endregion

#region COMPU_TAB_COMPU_VTAB_COMPU_VTAB_RANGE
        /// <summary>
        /// References to the <see cref="COMPU_TAB"/>s, <see cref="COMPU_VTAB"/>s or <see cref="COMPU_VTAB_RANGE"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, Asap2Base> CompuTabCompuVtabCompuVtabRanges = new Dictionary<string, Asap2Base>();

        /// <summary>
        /// References to the <see cref="COMPU_TAB"/>s, <see cref="COMPU_VTAB"/>s or <see cref="COMPU_VTAB_RANGE"/>s based on the Name element.
        /// </summary>
        /// <param name="Name">Name of object</param>
        /// <param name="obj">The object</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated object is found.</exception>
        private void AddCompuTabCompuVtabCompuVtabRanges(string Name, Asap2Base obj)
        {
            try
            {
                CompuTabCompuVtabCompuVtabRanges.Add(Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'COMPU_TABs', 'COMPU_VTAB's or'COMPU_VTAB_RANGE's in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, Name));
            }
        }

        /// <summary>
        /// Add a <see cref="COMPU_TAB"/> to the lists.
        /// </summary>
        /// <param name="obj">COMPU_TAB object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="COMPU_TAB"/>, <see cref="COMPU_VTAB"/> or <see cref="COMPU_VTAB_RANGE"/> is found.</exception>
        public void AddElement(COMPU_TAB obj)
        {
            AddCompuTabCompuVtabCompuVtabRanges(obj.Name, obj);
        }

        /// <summary>
        /// Add a <see cref="COMPU_VTAB"/> to the lists.
        /// </summary>
        /// <param name="obj">COMPU_VTAB object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="COMPU_TAB"/>, <see cref="COMPU_VTAB"/> or <see cref="COMPU_VTAB_RANGE"/> is found.</exception>
        public void AddElement(COMPU_VTAB obj)
        {
            AddCompuTabCompuVtabCompuVtabRanges(obj.Name, obj);
        }

        /// <summary>
        /// Add a <see cref="COMPU_VTAB_RANGE"/> to the lists.
        /// </summary>
        /// <param name="Characteristic">COMPU_VTAB_RANGE object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="COMPU_TAB"/>, <see cref="COMPU_VTAB"/> or <see cref="COMPU_VTAB_RANGE"/> is found.</exception>
        public void AddElement(COMPU_VTAB_RANGE Characteristic)
        {
            AddCompuTabCompuVtabCompuVtabRanges(Characteristic.Name, Characteristic);
        }

        #endregion

#region COMPU_METHOD
        /// <summary>
        /// References to the <see cref="COMPU_METHOD"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, COMPU_METHOD> CompuMethods = new Dictionary<string, COMPU_METHOD>();

        /// <summary>
        /// Add <see cref="COMPU_METHOD"/>s to the lists.
        /// </summary>
        /// <param name="obj">COMPU_METHOD object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="COMPU_METHOD"/> is found.</exception>
        public void AddElement(COMPU_METHOD obj)
        {
            try
            {
                CompuMethods.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'COMPU_METHODs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
        #endregion

#region FRAME
        /// <summary>
        /// References to the <see cref="FRAME"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, FRAME> Frames = new Dictionary<string, FRAME>();

        /// <summary>
        /// Add <see cref="FRAME"/>s to the lists.
        /// </summary>
        /// <param name="obj">FRAME object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="FRAME"/> is found.</exception>
        public void AddElement(FRAME obj)
        {
            try
            {
                Frames.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'FRAMEs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
        #endregion

#region FUNCTION
        /// <summary>
        /// References to the <see cref="FUNCTION"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, FUNCTION> Functions = new Dictionary<string, FUNCTION>();

        /// <summary>
        /// Add <see cref="FUNCTION"/>s to the lists.
        /// </summary>
        /// <param name="obj">FUNCTION object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="FUNCTION"/> is found.</exception>
        public void AddElement(FUNCTION obj)
        {
            try
            {
                Functions.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'FUNCTIONs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
        #endregion

#region GROUP
        /// <summary>
        /// References to the <see cref="GROUP"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, GROUP> Groups = new Dictionary<string, GROUP>();

        /// <summary>
        /// Add <see cref="GROUP"/>s to the lists.
        /// </summary>
        /// <param name="obj">GROUP object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="GROUP"/> is found.</exception>
        public void AddElement(GROUP obj)
        {
            try
            {
                Groups.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'GROUPs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
        #endregion

#region RECORD_LAYOUT
        /// <summary>
        /// References to the <see cref="RECORD_LAYOUT"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, RECORD_LAYOUT> Record_layouts = new Dictionary<string, RECORD_LAYOUT>();

        /// <summary>
        /// Add <see cref="RECORD_LAYOUT"/>s to the lists.
        /// </summary>
        /// <param name="obj">RECORD_LAYOUT object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="RECORD_LAYOUT"/> is found.</exception>
        public void AddElement(RECORD_LAYOUT obj)
        {
            try
            {
                Record_layouts.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'RECORD_LAYOUTs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
        #endregion

#region UNIT
        /// <summary>
        /// References to the <see cref="UNIT"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, UNIT> Units = new Dictionary<string, UNIT>();

        /// <summary>
        /// Add <see cref="UNIT"/>s to the lists.
        /// </summary>
        /// <param name="obj">UNIT object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="UNIT"/> is found.</exception>
        public void AddElement(UNIT obj)
        {
            try
            {
                Units.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'UNITs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
        #endregion

#region USER_RIGHTS
        /// <summary>
        /// References to the <see cref="USER_RIGHTS"/>s based on the Name element.
        /// </summary>
        public Dictionary<string, USER_RIGHTS> User_rights = new Dictionary<string, USER_RIGHTS>();

        /// <summary>
        /// Add <see cref="USER_RIGHTS"/>s to the lists.
        /// </summary>
        /// <param name="obj">USER_RIGHTS object to add</param>
        /// <exception cref="ValidationErrorException">thrown if duplicated <see cref="USER_RIGHTS"/> is found.</exception>
        public void AddElement(USER_RIGHTS obj)
        {
            try
            {
                User_rights.Add(obj.Name, obj);
            }
            catch (ArgumentException)
            {
                throw new ValidationErrorException(String.Format("Warning: Duplicate '{0}' (Must be unique of all 'USER_RIGHTSs' in the MODULE) with name '{1}' found, ignoring", obj.GetType().Name, obj.Name));
            }
        }
#endregion

        public void Validate(IErrorReporter errorReporter)
        {
            {
                var list = elements.FindAll(x => x.GetType() == typeof(A2ML));
                if (list != null)
                {
                    if (list.Count > 1)
                    {
                        list[list.Count - 1].reportErrorOrWarning("Second A2ML found, shall only be one", false, errorReporter);
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
            {
                var list = elements.FindAll(x => x.GetType() == typeof(VARIANT_CODING));
                if (list != null)
                {
                    if (list.Count > 1)
                    {
                        list[list.Count - 1].reportErrorOrWarning("Second VARIANT_CODING found, shall only be one", false, errorReporter);
                    }
                }
            }

            foreach (var obj in AxisPtsCharacteristicMeasurement.Values)
            {
                if (obj.GetType() == typeof(Asap2.AXIS_PTS))
                {
                    (obj as AXIS_PTS).Validate(errorReporter, this);
                }
                if (obj.GetType() == typeof(Asap2.MEASUREMENT))
                {
                    (obj as MEASUREMENT).Validate(errorReporter, this);
                }
                if (obj.GetType() == typeof(Asap2.CHARACTERISTIC))
                {
                    (obj as CHARACTERISTIC).Validate(errorReporter, this);
                }
            }
            { 
                {
                    var compu_method = elements.FindAll(x => x.GetType() == typeof(COMPU_METHOD));
                    foreach (var item in compu_method)
                    {
                        (item as COMPU_METHOD).Validate(errorReporter, this);
                    }
                }
            }
        }
    }
}
