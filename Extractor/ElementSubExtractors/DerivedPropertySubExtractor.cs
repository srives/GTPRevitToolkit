using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Pipeline.Models;
using System.Collections.Generic;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using RevitElement = Autodesk.Revit.DB.Element;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.Export.Revit.Services;
using Gtpx.ModelSync.Export.Revit.Models;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{    
    public static class DerivedPropertySubExtractor
    {
        private static CurrentModelContext currentModelContext;
        private static Dictionary<string, string> propertyNameMappings;

        public static void initDerivedPropertySubExtractor(CurrentModelContext cmc)
        {
            currentModelContext = cmc;
        }

        public static void ProcessElement(RevitElement revitElement, GtpxElement element)
        {
            ExtractElementPropertyBased(element);
            ColorSubExtractor.ProcessElement(revitElement, element);

            // TO DO: Fill in these
            /*
            drawingNameSubExtractor.ProcessElement(revitElement, element);
            productCodeSubExtractor.ProcessElement(revitElement, element);
            descriptionSubExtractor.ProcessElement(revitElement, element);
            fittingTypeSubExtractor.ProcessElement(revitElement, element);
            lengthSubExtractor.ProcessElement(revitElement, element);
            materialSubExtractor.ProcessElement(revitElement, element);
            nameSubExtractor.ProcessElement(revitElement, element);
            phaseSubExtractor.ProcessElement(revitElement, element);
            sectionSubExtractor.ProcessElement(revitElement, element);
            serviceSubExtractor.ProcessElement(revitElement, element);
            sourceSubExtractor.ProcessElement(revitElement, element);
            spoolSubExtractor.ProcessElement(revitElement, element);
            straightElementSubExtractor.ProcessElement(revitElement, element);
            */
        }

        private static void ExtractElementPropertyBased(GtpxElement element)
        {
            foreach (var derivedPropertyDefinition in DerivedPropertyDefinitionService.GetDefinitions(PropertyDefinitionSource.ElementProperty))
            {
                var value = GetValue(element, derivedPropertyDefinition);
                DerivedPropertyDefinitionService.SetDerivedProperty(element, derivedPropertyDefinition.Name, value);
            }
        }

        private static string GetValue(GtpxElement element, DerivedPropertyDefinition derivedPropertyDefinition)
        {
            var propertyNames = derivedPropertyDefinition.SourceKeys.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var propertyName in propertyNames)
            {
                var name = propertyName;
                if (GetPropertyNameMappings().ContainsKey(propertyName))
                {
                    name = GetPropertyNameMappings()[propertyName];
                }

                if (!string.IsNullOrEmpty(name))
                {
                    // Search the list of extracted Properties for match so we don't make expensive call into revit element properties again
                    if (element.NameToPropertyMap.TryGetValue(name, out Property match))
                    {
                        if (string.IsNullOrEmpty(match.Value) &&
                            !string.IsNullOrEmpty(derivedPropertyDefinition.DefaultValue))
                        {
                            return derivedPropertyDefinition.DefaultValue;
                        }
                        return match.Value;
                    }
                }
            }
            return derivedPropertyDefinition.DefaultValue;
        }

        private static Dictionary<string, string> GetPropertyNameMappings()
        {
            if (propertyNameMappings == null)
            {
                propertyNameMappings = new Dictionary<string, string>
                {
                    // TO DO: Ask user for these
                    { "{RevitAutoCadPropertyMappings.RevitPropertyForItemNumber}", "ItemNumber" },
                    { "{RevitAutoCadPropertyMappings.RevitPropertyForOrderName}",  "PackageName" },
                    { "{RevitAutoCadPropertyMappings.RevitPropertyForStatusName}", "TrackingStatus" }
                };
            }

            return propertyNameMappings;
        }
    }
}
