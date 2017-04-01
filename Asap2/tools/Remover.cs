using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asap2.tools
{
    public class Remover
    {
        /// <summary>
        /// Removes all elements of the types specified by the parameter <paramref name="elementsToDelete"/>.
        /// </summary>
        /// <param name="module">Asap2 <see cref="MODULE"/> to delete elements from.</param>
        /// <param name="elementsToDelete">Element types to remove</param>
        /// <param name="options">Common options</param>
        /// <param name="moduleName">Name of module to remove elements from. If not specified the first module is used.</param>
        static public void RemoveAllElements(ref MODULE module, Options.ElementTypes elementsToDelete, Options options)
        {
            {
                List<String> deletedNames = new List<string>();
                if (elementsToDelete.HasFlag(Options.ElementTypes.CHARACTERISTIC))
                {
                    List<CHARACTERISTIC> characteristicsToDelete = new List<CHARACTERISTIC>();
                    elementsToDelete &= ~(Options.ElementTypes.CHARACTERISTIC);
                    foreach (var obj in module.AxisPtsCharacteristicMeasurement.Where(x => x.Value.GetType() == typeof(CHARACTERISTIC)))
                    {
                        characteristicsToDelete.Add(obj.Value as CHARACTERISTIC);
                    }

                    foreach (var obj in characteristicsToDelete)
                    {
                        module.AxisPtsCharacteristicMeasurement.Remove(obj.Name);
                        deletedNames.Add(obj.Name);
                    }
                }

                if (elementsToDelete.HasFlag(Options.ElementTypes.MEASUREMENT))
                {
                    List<MEASUREMENT> measurementsToDelete = new List<MEASUREMENT>();
                    elementsToDelete &= ~(Options.ElementTypes.MEASUREMENT);
                    foreach (var obj in module.AxisPtsCharacteristicMeasurement.Where(x => x.Value.GetType() == typeof(MEASUREMENT)))
                    {
                        measurementsToDelete.Add(obj.Value as MEASUREMENT);
                    }

                    foreach (var obj in measurementsToDelete)
                    {
                        module.AxisPtsCharacteristicMeasurement.Remove(obj.Name);
                        deletedNames.Add(obj.Name);
                    }
                }

                if (elementsToDelete.HasFlag(Options.ElementTypes.AXIS_PTS))
                {
                    List<AXIS_PTS> axis_ptsToDelete = new List<AXIS_PTS>();
                    elementsToDelete &= ~(Options.ElementTypes.AXIS_PTS);
                    foreach (var obj in module.AxisPtsCharacteristicMeasurement.Where(x => x.Value.GetType() == typeof(AXIS_PTS)))
                    {
                        axis_ptsToDelete.Add(obj.Value as AXIS_PTS);
                    }

                    foreach (var obj in axis_ptsToDelete)
                    {
                        module.AxisPtsCharacteristicMeasurement.Remove(obj.Name);
                        deletedNames.Add(obj.Name);
                    }
                }
                deleteReferencesToCharacteristicMeasurementAxis_pts(ref module, deletedNames);
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.FRAME))
            {
                elementsToDelete &= ~(Options.ElementTypes.FRAME);
                module.Frames.Clear();
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.GROUP))
            {
                elementsToDelete &= ~(Options.ElementTypes.GROUP);
                module.Groups.Clear();
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.UNIT))
            {
                elementsToDelete &= ~(Options.ElementTypes.UNIT);
                module.Units.Clear();
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.USER_RIGHTS))
            {
                elementsToDelete &= ~(Options.ElementTypes.USER_RIGHTS);
                module.User_rights.Clear();
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.A2ML))
            {
                List<Asap2Base> objToDelete = new List<Asap2Base>();
                elementsToDelete &= ~(Options.ElementTypes.A2ML);
                foreach (var obj in module.elements.Where(x => x.GetType() == typeof(A2ML)))
                {
                    objToDelete.Add(obj);
                }

                foreach (var obj in objToDelete)
                {
                    module.elements.Remove(obj);
                }
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.IF_DATA))
            {
                List<Asap2Base> objToDelete = new List<Asap2Base>();
                elementsToDelete &= ~(Options.ElementTypes.IF_DATA);
                foreach (var obj in module.elements.Where(x => x.GetType() == typeof(IF_DATA)))
                {
                    objToDelete.Add(obj);
                }

                foreach (var obj in objToDelete)
                {
                    module.elements.Remove(obj);
                }
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.MOD_COMMON))
            {
                List<Asap2Base> objToDelete = new List<Asap2Base>();
                elementsToDelete &= ~(Options.ElementTypes.MOD_COMMON);
                foreach (var obj in module.elements.Where(x => x.GetType() == typeof(MOD_COMMON)))
                {
                    objToDelete.Add(obj);
                }

                foreach (var obj in objToDelete)
                {
                    module.elements.Remove(obj);
                }
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.MOD_PAR))
            {
                List<Asap2Base> objToDelete = new List<Asap2Base>();
                elementsToDelete &= ~(Options.ElementTypes.MOD_PAR);
                foreach (var obj in module.elements.Where(x => x.GetType() == typeof(MOD_PAR)))
                {
                    objToDelete.Add(obj);
                }

                foreach (var obj in objToDelete)
                {
                    module.elements.Remove(obj);
                }
            }

            if (elementsToDelete.HasFlag(Options.ElementTypes.VARIANT_CODING))
            {
                List<Asap2Base> objToDelete = new List<Asap2Base>();
                elementsToDelete &= ~(Options.ElementTypes.VARIANT_CODING);
                foreach (var obj in module.elements.Where(x => x.GetType() == typeof(VARIANT_CODING)))
                {
                    objToDelete.Add(obj);
                }

                foreach (var obj in objToDelete)
                {
                    module.elements.Remove(obj);
                }
            }

            if (elementsToDelete != Options.ElementTypes.NONE)
            {
                throw new ErrorException(ErrorException.ErrorCodes.ParameterError, String.Format("Specified elemenents to delete is not supported to delete '{0}'", elementsToDelete));
            }
        }

        /// <summary>
        /// Search base elements (elements that is handled by the <see cref="MODULE"/>) and delete references to named <see cref="CHARACTERISTIC"/>, <see cref="MEASUREMENT"/> or <see cref="AXIS_PTS"/>.
        /// </summary>
        /// <param name="module">Module to delete references in.</param>
        /// <param name="name">Name to search for.</param>
        /// <returns>List of base elements that contains references to the named <see cref="CHARACTERISTIC"/>.</returns>
        static void deleteReferencesToCharacteristicMeasurementAxis_pts(ref MODULE module, List<String> name)
        {
            foreach (var function in module.Functions.Values)
            {
                if (function.def_characteristic != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in function.def_characteristic.def_characteristics.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        function.def_characteristic.def_characteristics.Remove(obj);
                    }
                }
                
                if (function.ref_characteristic != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in function.ref_characteristic.reference.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        function.ref_characteristic.reference.Remove(obj);
                    }
                }
                
                if (function.in_measurement != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in function.in_measurement.measurements.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        function.in_measurement.measurements.Remove(obj);
                    }
                }
                
                if (function.loc_measurement != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in function.loc_measurement.measurements.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        function.loc_measurement.measurements.Remove(obj);
                    }
                }

                if (function.out_measurement != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in function.out_measurement.measurements.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        function.out_measurement.measurements.Remove(obj);
                    }
                }
            }

            foreach (var group in module.Groups.Values)
            {
                if (group.ref_characteristic != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in group.ref_characteristic.reference.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        group.ref_characteristic.reference.Remove(obj);
                    }
                }

                if (group.ref_measurement != null)
                {
                    List<String> objToDelete = new List<String>();
                    foreach (var obj in group.ref_measurement.reference.Where(x => name.Contains(x)))
                    {
                        objToDelete.Add(obj);
                    }

                    foreach (var obj in objToDelete)
                    {
                        group.ref_measurement.reference.Remove(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Removes empty <see cref="FUNCTION"/>s and <see cref="GROUP"/>s. Does also remove not referenced <see cref="COMPU_METHOD"/>s, <see cref="COMPU_TAB"/>s,
        /// <see cref="COMPU_VTAB"/>s, <see cref="COMPU_VTAB_RANGE"/>s and <see cref="UNIT"/>s
        /// </summary>
        /// <param name="module">Module to purge in.</param>
        /// <param name="options">Common options</param>
        static public void purgeEmptyOrNotReferencedElements(ref MODULE module, Options options)
        {
            {
                List<FUNCTION> functionToDelete = new List<FUNCTION>();
                List<string> functionsNotToDelete = new List<string>();
                foreach (var function in module.Functions.Values)
                {
                    bool doDelete = true;
                    if (function.def_characteristic != null && function.def_characteristic.def_characteristics.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (function.ref_characteristic != null && function.ref_characteristic.reference.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (function.in_measurement != null && function.in_measurement.measurements.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (function.loc_measurement != null && function.loc_measurement.measurements.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (function.out_measurement != null && function.out_measurement.measurements.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (!doDelete)
                    {
                        // Not empty, store it so it is not deleted in next part.
                        functionsNotToDelete.Add(function.Name);
                    }

                    if (function.sub_function != null && function.sub_function.sub_functions.Count > 0)
                    {
                        // There is subfunctions.
                        doDelete = false;
                    }

                    if (doDelete)
                    {
                        functionToDelete.Add(function);
                    }
                }

                foreach (var function in functionToDelete)
                {
                    module.Functions.Remove(function.Name);
                }

                // Search for functions refered in subfunctions, remove not existing functions from subfunctions. Delete function if subfunctions is empty.
                Dictionary<string, FUNCTION> Functions = module.Functions;
                functionToDelete.Clear();
                foreach (var function in module.Functions.Values)
                {
                    bool doDelete = true;

                    List<String> sub_functionToDelete = new List<String>();
                    if (function.sub_function != null)
                    {
                        foreach (var obj in function.sub_function.sub_functions.Where(x => !Functions.Keys.Contains(x)))
                        {
                            sub_functionToDelete.Add(obj);
                        }
                    }

                    foreach (var obj in sub_functionToDelete)
                    {
                        function.sub_function.sub_functions.Remove(obj);
                    }

                    if (function.sub_function != null && function.sub_function.sub_functions.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (doDelete)
                    {
                        functionToDelete.Add(function);
                    }
                }

                foreach (var obj in functionToDelete)
                {
                    if (!functionsNotToDelete.Contains(obj.Name))
                    {
                        module.Functions.Remove(obj.Name);
                    }
                }
            }


            {
                List<GROUP> groupsToDelete = new List<GROUP>();
                List<string> groupsNotToDelete = new List<string>();
                foreach (var group in module.Groups.Values)
                {
                    bool doDelete = true;

                    if (group.ref_characteristic != null && group.ref_characteristic.reference.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (group.ref_measurement != null && group.ref_measurement.reference.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (!doDelete)
                    {
                        // Not empty, store it so it is not deleted in next part.
                        groupsNotToDelete.Add(group.Name);
                    }

                    if (group.sub_group != null && group.sub_group.groups.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (doDelete)
                    {
                        groupsToDelete.Add(group);
                    }
                }

                foreach (var function in groupsToDelete)
                {
                    module.Functions.Remove(function.Name);
                }

                // Search for Groups refered in subGroups, remove not existing functions from subGroups. Delete group if subGroups is empty.
                Dictionary<string, GROUP> Groups = module.Groups;
                groupsToDelete.Clear();
                foreach (var group in module.Groups.Values)
                {
                    bool doDelete = true;

                    List<String> sub_functionToDelete = new List<String>();
                    if (group.sub_group != null)
                    {
                        foreach (var obj in group.sub_group.groups.Where(x => !Groups.Keys.Contains(x)))
                        {
                            sub_functionToDelete.Add(obj);
                        }
                    }

                    foreach (var obj in sub_functionToDelete)
                    {
                        group.sub_group.groups.Remove(obj);
                    }

                    if (group.sub_group != null && group.sub_group.groups.Count > 0)
                    {
                        doDelete = false;
                    }

                    if (doDelete)
                    {
                        groupsToDelete.Add(group);
                    }
                }

                foreach (var obj in groupsToDelete)
                {
                    if (!groupsNotToDelete.Contains(obj.Name))
                    {
                        module.Groups.Remove(obj.Name);
                    }
                }
            }
            {
                List<String> referedCompuMethods = new List<String>();
                List<String> referedRecord_layouts = new List<String>();
                List<String> referedUnits = new List<String>();
                foreach (var obj in module.AxisPtsCharacteristicMeasurement.Values)
                {
                    if (obj.GetType() == typeof(CHARACTERISTIC))
                    {
                        var tmp = obj as CHARACTERISTIC;
                        if (tmp.Conversion != "NO_COMPU_METHOD")
                        {
                            referedCompuMethods.Add(tmp.Conversion);
                        }
                        referedRecord_layouts.Add(tmp.Deposit);

                        if (tmp.phys_unit != null && tmp.phys_unit.Count() > 0)
                        {
                            referedUnits.Add(tmp.phys_unit);
                        }
                    }
                    if (obj.GetType() == typeof(AXIS_PTS))
                    {
                        var tmp = obj as AXIS_PTS;
                        if (tmp.Conversion != "NO_COMPU_METHOD")
                        {
                            referedCompuMethods.Add(tmp.Conversion);
                        }
                        referedRecord_layouts.Add(tmp.Deposit);
                        if (tmp.phys_unit != null && tmp.phys_unit.Count() > 0)
                        {
                            referedUnits.Add(tmp.phys_unit);
                        }
                    }
                    if (obj.GetType() == typeof(MEASUREMENT))
                    {
                        var tmp = obj as MEASUREMENT;
                        if (tmp.Conversion != "NO_COMPU_METHOD")
                        {
                            referedCompuMethods.Add(tmp.Conversion);
                        }
                        if (tmp.phys_unit != null && tmp.phys_unit.Count() > 0)
                        {
                            referedUnits.Add(tmp.phys_unit);
                        }
                    }
                }

                {
                    List<String> objsToDelete = new List<String>();
                    foreach (var obj in module.CompuMethods.Keys.Where(x => !referedCompuMethods.Contains(x)))
                    {
                        objsToDelete.Add(obj);
                    }

                    foreach (var obj in objsToDelete)
                    {
                        module.CompuMethods.Remove(obj);
                    }
                }
                {
                    List<String> objsToDelete = new List<String>();
                    foreach (var obj in module.Record_layouts.Keys.Where(x => !referedRecord_layouts.Contains(x)))
                    {
                        objsToDelete.Add(obj);
                    }

                    foreach (var obj in objsToDelete)
                    {
                        module.Record_layouts.Remove(obj);
                    }
                }

                List<String> referedCompu_tabs = new List<String>();
                foreach (var obj in module.CompuMethods.Values)
                {
                    if (obj.ref_unit != null && obj.ref_unit.Count() > 0)
                    {
                        referedUnits.Add(obj.ref_unit);
                    }
                    if (obj.compu_tab_ref != null && obj.compu_tab_ref.Count() > 0)
                    {
                        referedCompu_tabs.Add(obj.compu_tab_ref);
                    }
                }
                {
                    List<String> objsToDelete = new List<String>();
                    foreach (var obj in module.CompuTabCompuVtabCompuVtabRanges.Keys.Where(x => !referedCompu_tabs.Contains(x)))
                    {
                        objsToDelete.Add(obj);
                    }

                    foreach (var obj in objsToDelete)
                    {
                        module.CompuTabCompuVtabCompuVtabRanges.Remove(obj);
                    }
                }
                {
                    List<String> objsToDelete = new List<String>();
                    foreach (var obj in module.Units.Keys.Where(x => !referedUnits.Contains(x)))
                    {
                        objsToDelete.Add(obj);
                    }

                    foreach (var obj in objsToDelete)
                    {
                        module.Units.Remove(obj);
                    }
                }
            }
        }
    }
}
