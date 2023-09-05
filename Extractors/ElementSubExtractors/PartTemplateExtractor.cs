using Gtpx.ModelSync.CAD.Cache;
using Gtpx.ModelSync.DataModel.Models;
using System.Collections.Generic;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors
{
    public static class PartTemplateExtractor
    {
        private static Dictionary<string, PartTemplate> idToPartTemplateMap = new Dictionary<string, PartTemplate>();

        public static void Reset()
        {
            idToPartTemplateMap = new Dictionary<string, PartTemplate>();
        }

        public static void ProcessElement(Autodesk.Revit.DB.Element revitElement,
                                   GtpxElement element)
        {
            if (!string.IsNullOrEmpty(element.TemplateId))
            {
                if (!idToPartTemplateMap.TryGetValue(element.TemplateId, out PartTemplate partTemplate))
                {
                    var category = GetRevitElementCategory(revitElement);
                    var familyName = GetFamilyName(element);
                    var fittingType = GetFittingType(element);

                    partTemplate = new PartTemplate
                    {
                        CadType = element.CadType,
                        Category = category,
                        FittingType = fittingType,
                        Id = element.TemplateId,
                        PatternNumber = string.Empty,
                        Name = $"{category} {familyName}"
                    };

                    idToPartTemplateMap[element.TemplateId] = partTemplate;
                }

                foreach (var propertyDefinitionId in PropertyDefinitionCache.GetPropertyDefinitionIds(element))
                {
                    partTemplate.PropertyDefinitionIds.Add(propertyDefinitionId);
                }
            }
        }

        ///Disabled to get the data to align with V1 publish. This version actually works better
        private static string GetCategory(GtpxElement element)
        {
            if (element.NameToPropertyMap.TryGetValue("Category", out Property categoryProperty))
            {
                return categoryProperty.Value;
            }
            return string.Empty;
        }

        //We may want to deprecate this function in favor of GetCategory
        private static string GetRevitElementCategory(Autodesk.Revit.DB.Element element)
        {
            var parameter = element.LookupParameter("Category");
            if (parameter != null && parameter.HasValue && parameter.Definition != null && !string.IsNullOrEmpty(parameter.Definition?.Name))
            {
                return parameter.AsValueString();
            }
            return string.Empty;
        }

        private static string GetFamilyName(GtpxElement element)
        {
            if (element.NameToPropertyMap.TryGetValue("Family", out Property familyProperty))
            {
                return familyProperty.Value;
            }
            return string.Empty;
        }

        private static string GetFittingType(GtpxElement element)
        {
            if (element.NameToDerivedPropertyMap.TryGetValue("FittingType", out Property fittingTypeProperty))
            {
                return fittingTypeProperty.Value;
            }
            return string.Empty;
        }
    }
}
