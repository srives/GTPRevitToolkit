using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.UI;
using System.Diagnostics;

namespace Gtpx.ModelSync.Export.Revit.Services
{

    public static class ParameterValueService
    {
        public static string GetValue(Autodesk.Revit.DB.Document document, Notifier notifier, Element revitElement, string parameterName)
        {
            var parameter = revitElement.LookupParameter(parameterName);
            var value = string.Empty;

            if (parameter != null)
            {
                GetValue(document, notifier, revitElement, parameter, parameterName, out value);
            }

            return value;
        }

        public static bool GetValue(Autodesk.Revit.DB.Document document, Notifier notifier, Element revitElement, Parameter parameter, string parameterName, out string parameterValue)
        {
            parameterValue = string.Empty;

            try
            {
                if (!parameter.HasValue)
                {
                    return false;
                }
            }
            catch
            {
                // SPVR-10470 : The HasValue call was throwing an exception,
                // when this happens just eat the exception and continue processing
                notifier.Warning($"Failed to get value for Element: {revitElement.Id}, Parameter named: {parameterName}.");
                return false;
            }

            // passing in parameterName since accessing parameter.Definition.Name is costly
            var storageType = parameter.StorageType;
            switch (storageType)
            {
                case StorageType.Double:
                    var doubleValue = parameter.AsDouble();
                    if (parameterName == "Weight") // assumes Imperial
                    {
                        // Revit expresses the weight in kgs, so need to convert to lbs
                        doubleValue *= 2.20462262185;
                    }
                    parameterValue = StringFormatService.FormatDouble(doubleValue);
                    break;

                case StorageType.ElementId:
                    parameterValue = GetElementIdValue(document, notifier, parameter, parameterName) ?? string.Empty;
                    break;

                case StorageType.Integer:
                    parameterValue = LookupIntegerValue(document, revitElement, parameter) ?? string.Empty;
                    break;

                case StorageType.None:
                    break;

                case StorageType.String:
                    parameterValue = parameter.AsString() ?? string.Empty;
                    break;

                default:
                    Debug.Assert(false, $"Unrecognized storage type: {parameter.StorageType}.");
                    break;
            }

            return true;
        }

        public static string GetElementIdValue(Autodesk.Revit.DB.Document document, Notifier notifier, Parameter parameter, string parameterName)
        {
            var elementId = parameter.AsElementId();
            string parameterValue;

            if (parameterName == "Category")
            {
                // To make this code behave like V1 need to initialize the parameter value to -1 string
#if Revit2024
                parameterValue = elementId.Value.ToString();
#else
                parameterValue = elementId.IntegerValue.ToString();
#endif
                try
                {
#if Revit2024
                    parameterValue = document.Settings.Categories.get_Item((BuiltInCategory)elementId.Value).Name;
#else
                    parameterValue = document.Settings.Categories.get_Item((BuiltInCategory)elementId.IntegerValue).Name;
#endif
                }
                catch
                {
                    // Ignore these, not sure why some categories aren't found
                }
            }
            else if (parameterName == "Family")
            {
                // To make this code behave like V1 need to initialize the parameter value to -1 string
#if Revit2024
                parameterValue = elementId.Value.ToString();
#else
                parameterValue = elementId.IntegerValue.ToString();
#endif
                if (document.GetElement(elementId) is ElementType elementType)
                {
                    parameterValue = elementType.FamilyName;
                }
            }
            else if (parameterName == "Family and Type")
            {
                // To make this code behave like V1 need to initialize the parameter value to empty string
                parameterValue = string.Empty;
                if (document.GetElement(elementId) is ElementType elementType)
                {
                    if (!string.IsNullOrEmpty(elementType.Name))
                    {
                        parameterValue = $"{elementType.FamilyName} - {elementType.Name}";
                    }
                    else
                    {
                        parameterValue = $"{elementType.FamilyName}";
                    }
                }
            }
            else if (parameterName == "Design Option")
            {
                // To make this code behave like V1 need to initialize the parameter value to empty string
                parameterValue = string.Empty;
                if (elementId != ElementId.InvalidElementId)
                {
                    var element = document.GetElement(elementId);
                    if (element != null)
                    {
                        parameterValue = element.Name;
                    }
                }
            }
            else
            {
                // To make this code behave like V1 need to initialize the parameter value to -1 string
#if Revit2024
                parameterValue = elementId.Value.ToString();
#else
                parameterValue = elementId.IntegerValue.ToString();
#endif
                var element = document.GetElement(elementId);
                if (element != null)
                {
                    parameterValue = element.Name;
                }
            }

            return parameterValue;
        }

        public static string LookupIntegerValue(Autodesk.Revit.DB.Document document, Element element, Parameter parameter)
        {
            var index = parameter.AsInteger();
            var parameterDefinitionName = parameter.Definition.Name;

            // handle special cases for certain parameter values that require lookup using integer value
            if (parameterDefinitionName == "Specification" ||
                parameterDefinitionName == "Insulation Specification" ||
                parameterDefinitionName == "Fabrication Service" ||
                parameterDefinitionName == "Part Material" ||
                parameterDefinitionName == "Cut Type")
            {
                var fabricationPart = element as FabricationPart;
                if (fabricationPart != null)
                {
                    var fc = FabricationConfiguration.GetFabricationConfiguration(document);
                    if (fc != null)
                    {
                        if (parameterDefinitionName == "Specification")
                        {
                            return fc.GetSpecificationName(index);
                        }
                        else if (parameterDefinitionName == "Insulation Specification")
                        {
                            return fc.GetInsulationSpecificationName(index);
                        }
                        else if (parameterDefinitionName == "Fabrication Service")
                        {
                            var fs = fc.GetService(index);
                            if (fs != null)
                            {
                                return fs.Name;
                            }
                        }
                        else if (parameterDefinitionName == "Part Material")
                        {
                            return fc.GetMaterialName(index);
                        }
                        else if (parameterDefinitionName == "Cut Type")
                        {
                            switch (index)
                            {
                                case 0:
                                    return "NotSet";
                                case 1:
                                    return "MachineCut";
                                case 2:
                                    return "DecoiledStraight";
                                case 3:
                                    return "SpiralStraight";
                                case 4:
                                    return "OvalStraight";
                                case 5:
                                    return "DrawOnly";
                                case 6:
                                    return "Guillotine";
                                case 7:
                                    return "Equipment";
                                case 8:
                                    return "Pipework";
                                case 9:
                                    return "Wrapped";
                                case 10:
                                    return "Rotary";
                                case 11:
                                    return "Limit";
                                default:
                                    return string.Empty;
                            }
                        }
                    }
                }
            }
            return index.ToString();
        }
    }
}
