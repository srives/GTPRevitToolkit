using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Gtpx.ModelSync.Export.Revit.Services;
using System.Collections.Generic;
using System.Linq;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using RevitElement = Autodesk.Revit.DB.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{
    public static class ColorSubExtractor 
    {
        public static void ProcessElement(RevitElement revitElement, GtpxElement element)
        {
            // only need to process non-FabricationParts because FabricationParts are handled by StratusCad FabMajToElement publish step
            if (revitElement as FabricationPart == null)
            {
                var color = string.Empty;
                // try to get color from assigned material first, if one exists
                var materialIds = revitElement.GetMaterialIds(false);
                if (materialIds != null)
                {
                    var materialAreaToColorMap = new Dictionary<double, string>();
                    foreach (var materialId in materialIds)
                    {
                        var material = revitElement.Document.GetElement(materialId) as Material;
                        if (material != null)
                        {
                            if (materialIds.Count == 1) // only one material
                            {
                                // don't need to actually calculate the area, just add the value
                                materialAreaToColorMap.Add(1, FormatColorAsHex(material.Color));
                            }
                            else // more than one material to consider
                            {
                                var area = revitElement.GetMaterialArea(materialId, false);
                                if (!materialAreaToColorMap.TryGetValue(area, out var value))
                                {
                                    materialAreaToColorMap.Add(area, FormatColorAsHex(material.Color));
                                }
                            }
                        }
                    }
                    if (materialAreaToColorMap.Any())
                    {
                        // return material with greatest area
                        color = materialAreaToColorMap.OrderByDescending(x => x.Key).First().Value;
                    }
                }
                // if we didn't get a color, try the system type and its color
                if (string.IsNullOrEmpty(color))
                {
                    var ductSystemTypeId = revitElement.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM)?.AsElementId();
                    var pipingSystemTypeId = revitElement.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM)?.AsElementId();
                    var ductSystemType = (ductSystemTypeId != null && ductSystemTypeId != ElementId.InvalidElementId) ? revitElement.Document.GetElement(ductSystemTypeId) : null;
                    var pipingSystemType = (pipingSystemTypeId != null && pipingSystemTypeId != ElementId.InvalidElementId) ? revitElement.Document.GetElement(pipingSystemTypeId) : null;
                    if (ductSystemType != null || pipingSystemType != null)
                    {
                        var mechanicalSystemType = ductSystemType != null ? ductSystemType as MechanicalSystemType : pipingSystemType as MechanicalSystemType;
                        if (mechanicalSystemType != null)
                        {
                            if (mechanicalSystemType.FillColor != null && mechanicalSystemType.FillColor.IsValid)
                            {
                                color = FormatColorAsHex(mechanicalSystemType.FillColor);
                            }
                            else if (mechanicalSystemType.LineColor != null && mechanicalSystemType.LineColor.IsValid)
                            {
                                color = FormatColorAsHex(mechanicalSystemType.LineColor);
                            }
                        }
                    }
                }
                DerivedPropertyDefinitionService.SetDerivedProperty(element, "Color", color);
            }
        }

        private static string FormatColorAsHex(Color color)
        {
            if (color != null && color.IsValid)
            {
                // format as #FFFFFF hex color string value
                return $"#{color.Red:X2}{color.Green:X2}{color.Blue:X2}";
            }
            return string.Empty;
        }
    }
}
