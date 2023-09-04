using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.Cache;
using Gtpx.ModelSync.CAD.Services;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.CAD.Utilities;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Caches;
using Gtpx.ModelSync.Export.Revit.Providers;
using Gtpx.ModelSync.Export.Revit.Services;
using System.Collections.Generic;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using PropertyDefinition = Gtpx.ModelSync.DataModel.Models.PropertyDefinition;
using RevitElement = Autodesk.Revit.DB.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{
    public static class ParameterExtractor
    {
        public static HashSet<string> PartPropertyNamesToExclude { get; set; } // TO DO: Get this from input to user
        private static PropertyDefinitionCache propertyDefinitionCache;
        private static bool cacheElementsForAssemblies;
        private static string propertyNameForAssembly;
        private static bool retrievedPropertyNameForAssembly;
        private static GTProfiler _profiler = new GTProfiler();

        public static void ProcessElement(Document document, Notifier logger, RevitElement revitElement,
                                   GtpxElement element)
        {
            SetElementIdProperty(revitElement, element);

            var familyInstance = revitElement as FamilyInstance;
            if (familyInstance != null && familyInstance.Symbol != null)
            {
                // add family type parameters for family instances
                AddPropertiesForParameterSet(document, logger, revitElement, element, familyInstance.Symbol.Parameters);
            }
            else
            {
                // Not a family instance, so it should be a piece of straight duct, pipe, conduit, cable tray, etc.
                // For these types of elements we need to get the type element and extract its parameters
                ElementId typeId = revitElement.GetTypeId();
                if (typeId != null)
                {
                    RevitElement typeElement = document.GetElement(typeId);
                    if (typeElement != null)
                    {
                        AddPropertiesForParameterSet(document, logger, revitElement, element, typeElement.Parameters);
                    }
                }
            }

            // add all revit element parameters          
            var usedParameters = AddPropertiesForParameterSet(document, logger, revitElement, element, revitElement.Parameters);

            // Save statistics for the log file (useful for GTP Service Desk diagnostics on publishes)
            _profiler.SaveValue("propertyDefinitionCacheSize", propertyDefinitionCache.Count);
            _profiler.Accum("Element.Parameters.UsedCount", usedParameters);

            // process derived properties, which will leverage element.Properties more efficiently than revit parameters
            DerivedPropertySubExtractor.ProcessElement(revitElement, element);

            // Setting the element description requires the Description derived property to have already been extracted.  
            // So this call must happen after the derivedPropertySubExtractor.ProcessElement has been called.
            SetElementDescription(element);

            CacheElementForAssembly(revitElement, element);
        }

        private static int AddPropertiesForParameterSet(Document document, Notifier logger, RevitElement revitElement,
                                                          GtpxElement element,
                                                          ParameterSet parameterSet)
        {
            var ct = 0;
            foreach (Parameter parameter in parameterSet)
            {
                if (parameter != null && GetProperty(document, logger, revitElement, element, parameter, out Property property))
                {
                    element.NameToPropertyMap[property.Name] = property;
                    ct++;
                }
            }
            return ct;
        }

        private static void SetElementDescription(GtpxElement element)
        {
            // The element description is identical to the derived Description property value
            if (element.NameToDerivedPropertyMap.TryGetValue("Description", out Property property))
            {
                element.Description = property.Value;
            }
        }

        private static void SetElementIdProperty(RevitElement revitElement,
                                          GtpxElement element)
        {
            propertyDefinitionCache.Add(
                new PropertyDefinition
                {
                    DisplayDataType = PropertyDataType.String,
                    IsReadOnly = true,
                    Name = "ElementId",
                    StorageDataType = PropertyDataType.String
                },
                element);

            element.NameToPropertyMap["ElementId"] = new Property()
            {
                Name = "ElementId",
                Value = revitElement.Id.ToString()
            };
        }

        private static bool GetProperty(Document document, Notifier logger, RevitElement revitElement,
                                 GtpxElement element,
                                 Parameter parameter,
                                 out Property property)
        {
            property = null;
            var status = false;
            var definition = parameter.Definition;
            if (definition != null)
            {
                var definitionName = PropertyNameService.CleanName(definition.Name);
                if (!IsExcluded(definitionName))
                {
                    var hasValue = ParameterValueService.GetValue(document, logger, revitElement, parameter, definitionName, out var value);

#if Revit2019 || Revit2020 || Revit2021
                    PropertyDataTypesProvider.GetPropertyDataTypes(parameter,
                                                                   parameter.StorageType,
                                                                   definition.ParameterType,
                                                                   out PropertyDataType displayDataType,
                                                                   out PropertyDataType storageDataType);
#else
                    PropertyDataTypesProvider.GetPropertyDataTypes(parameter,
                                                                   parameter.StorageType,
                                                                   definition.GetDataType(),
                                                                   out PropertyDataType displayDataType,
                                                                   out PropertyDataType storageDataType);
#endif

                    propertyDefinitionCache.Add(
                        new PropertyDefinition
                        {
                            DisplayDataType = displayDataType,
                            IsReadOnly = parameter.IsReadOnly || storageDataType != PropertyDataType.String || displayDataType != PropertyDataType.String,
                            Name = definition.Name,
                            StorageDataType = storageDataType
                        },
                        element);

                    property = new Property
                    {
                        HasValue = hasValue,
                        Name = definitionName,
                        Value = value
                    };

                    status = true;
                }
            }
            return status;
        }

        private static bool IsExcluded(string propertyName)
        {
            return PartPropertyNamesToExclude.Contains(propertyName);
        }

        private static void CacheElementForAssembly(RevitElement revitElement,
                                             GtpxElement element)
        {
            if (!retrievedPropertyNameForAssembly)
            {
                var revitPropertyForAssemblies = string.Empty; // TO DO: Ask user for this value
                var useRevitPropertyForAssemblies = false; // TO DO: Ask user RevitPropertyForAssembliesService.UseRevitPropertiesForAssemblies(out var revitPropertyForAssemblies, out _);
                if (useRevitPropertyForAssemblies)
                {
                    propertyNameForAssembly = revitPropertyForAssemblies;
                    cacheElementsForAssemblies = true;
                }
                retrievedPropertyNameForAssembly = true;
            }

            if (cacheElementsForAssemblies)
            {
                if (element.NameToPropertyMap.TryGetValue(propertyNameForAssembly, out var property) &&
                    !string.IsNullOrWhiteSpace(property.Value))
                {
                    ParameterBasedAssemblyCache.Add(revitElement.Id, property.Value);
                }
            }
        }
    }
}
