using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.Cache;
using Gtpx.ModelSync.CAD.Services;
using Gtpx.ModelSync.CAD.Utilities;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Caches;
using Gtpx.ModelSync.Export.Revit.Interfaces;
using Gtpx.ModelSync.Export.Revit.Providers;
using Gtpx.ModelSync.Export.Revit.Services;
using Gtpx.ModelSync.Pipeline.Models;
using System.Collections.Generic;
using System.Windows.Markup;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using RevitElement = Autodesk.Revit.DB.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{
    public class ParameterExtractor : IElementSubExtractor
    {
        private readonly CurrentModelContext currentModelContext;
        private readonly DerivedPropertySubExtractor derivedPropertySubExtractor;
        private readonly Document document;
        private readonly ParameterValueService parameterValueService;
        private readonly PropertyDataTypesProvider propertyDataTypesProvider;
        private readonly PropertyNameService propertyNameService;
        private readonly PropertyDefinitionCache propertyDefinitionCache;
        private readonly ParameterBasedAssemblyCache parameterBasedAssemblyCache;
        private readonly RevitPropertyForAssembliesService revitPropertyForAssembliesService;
        private HashSet<string> propertyNamesToExclude;
        private bool cacheElementsForAssemblies;
        private string propertyNameForAssembly;
        private bool retrievedPropertyNameForAssembly;
        private readonly GTProfiler _profiler;

        public ParameterExtractor(CurrentModelContext currentModelContext,
                                  DerivedPropertySubExtractor derivedPropertySubExtractor,
                                  Document document,
                                  ParameterValueService parameterValueService,
                                  PropertyDataTypesProvider propertyDataTypesProvider,
                                  PropertyNameService propertyNameService,
                                  PropertyDefinitionCache propertyDefinitionCache,
                                  ParameterBasedAssemblyCache parameterBasedAssemblyCache,
                                  RevitPropertyForAssembliesService revitPropertyForAssembliesService)
        {
            this.currentModelContext = currentModelContext;
            this.derivedPropertySubExtractor = derivedPropertySubExtractor;
            this.document = document;
            this.parameterValueService = parameterValueService;
            this.propertyDataTypesProvider = propertyDataTypesProvider;
            this.propertyNameService = propertyNameService;
            this.propertyDefinitionCache = propertyDefinitionCache;
            this.parameterBasedAssemblyCache = parameterBasedAssemblyCache;
            this.revitPropertyForAssembliesService = revitPropertyForAssembliesService;
            this._profiler = new GTProfiler();
        }

        public void ProcessElement(RevitElement revitElement,
                                   GtpxElement element)
        {
            SetElementIdProperty(revitElement, element);

            var familyInstance = revitElement as FamilyInstance;
            if (familyInstance != null && familyInstance.Symbol != null)
            {
                // add family type parameters for family instances
                AddPropertiesForParameterSet(revitElement, element, familyInstance.Symbol.Parameters);
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
                        AddPropertiesForParameterSet(revitElement, element, typeElement.Parameters);
                    }
                }
            }

            // add all revit element parameters          
            var usedParameters = AddPropertiesForParameterSet(revitElement, element, revitElement.Parameters);

            // Save statistics for the log file (useful for GTP Service Desk diagnostics on publishes)
            _profiler.SaveValue("propertyDefinitionCacheSize", propertyDefinitionCache.Count);
            _profiler.Accum("Element.Parameters.UsedCount", usedParameters);

            // process derived properties, which will leverage element.Properties more efficiently than revit parameters
            derivedPropertySubExtractor.ProcessElement(revitElement, element);

            // Setting the element description requires the Description derived property to have already been extracted.  
            // So this call must happen after the derivedPropertySubExtractor.ProcessElement has been called.
            SetElementDescription(element);

            CacheElementForAssembly(revitElement, element);
        }

        private int AddPropertiesForParameterSet(RevitElement revitElement,
                                                          GtpxElement element,
                                                          ParameterSet parameterSet)
        {
            var ct = 0;
            foreach (Parameter parameter in parameterSet)
            {
                if (parameter != null && GetProperty(revitElement, element, parameter, out Property property))
                {
                    element.NameToPropertyMap[property.Name] = property;
                    ct++;
                }
            }
            return ct;
        }

        private void SetElementDescription(GtpxElement element)
        {
            // The element description is identical to the derived Description property value
            if (element.NameToDerivedPropertyMap.TryGetValue("Description", out Property property))
            {
                element.Description = property.Value;
            }
        }

        private void SetElementIdProperty(RevitElement revitElement,
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

        private bool GetProperty(RevitElement revitElement,
                                 GtpxElement element,
                                 Parameter parameter,
                                 out Property property)
        {
            property = null;
            var status = false;
            var definition = parameter.Definition;
            if (definition != null)
            {
                var definitionName = propertyNameService.CleanName(definition.Name);
                if (!IsExcluded(definitionName))
                {
                    var hasValue = parameterValueService.GetValue(revitElement, parameter, definitionName, out var value);

#if Revit2019 || Revit2020 || Revit2021
                    propertyDataTypesProvider.GetPropertyDataTypes(parameter,
                                                                   parameter.StorageType,
                                                                   definition.ParameterType,
                                                                   out PropertyDataType displayDataType,
                                                                   out PropertyDataType storageDataType);
#else
                    propertyDataTypesProvider.GetPropertyDataTypes(parameter,
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

        private bool IsExcluded(string propertyName)
        {
            if (propertyNamesToExclude == null)
            {
                propertyNamesToExclude = currentModelContext.Company.RevitAutoCADPropertyMappings.PartPropertyNamesToExclude;
            }
            return propertyNamesToExclude.Contains(propertyName);
        }

        private void CacheElementForAssembly(RevitElement revitElement,
                                             GtpxElement element)
        {
            if (!retrievedPropertyNameForAssembly)
            {
                var useRevitPropertyForAssemblies = revitPropertyForAssembliesService.UseRevitPropertiesForAssemblies(out var revitPropertyForAssemblies, out _);
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
                    parameterBasedAssemblyCache.Add(revitElement.Id, property.Value);
                }
            }
        }
    }
}
