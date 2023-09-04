using Autodesk.Revit.DB;
using GTP.Services;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Extensions;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using GtpxPoint = Gtpx.ModelSync.DataModel.Models.Point;

namespace Gtpx.ModelSync.Export.Revit.Extractors.FamilyInstances
{
    public static class GtpPointExtractor
    {
        private static string gtpPointFamilyName = "GTP";

        public static void ProcessFamilyInstance(FamilyInstance familyInstance,
                                          GtpxElement element)
        {
            if (familyInstance.Symbol != null && familyInstance.Symbol.IsValidObject)
            {
                var symbolName = familyInstance.Symbol.Name;
                if (!string.IsNullOrEmpty(symbolName))
                {
                    if (FamilySymbolService.GetFamilyName(familyInstance.Symbol) == gtpPointFamilyName)
                    {
                        if (familyInstance.Location is LocationPoint locationPoint)
                        {
                            element.Points.Add(new GtpxPoint()
                            {
                                Direction = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 },
                                Location = locationPoint.Point.ToPoint3D(),
                                PointType = PointType.Anchor,
                                // TODO : not sure why the UpVector is not being set here
                            });
                        }
                    }
                    else if (!symbolName.StartsWith("Anchor - DEWALT - "))
                    {
                        var subComponentIds = familyInstance.GetSubComponentIds();
                        if (subComponentIds != null)
                        {
                            var document = familyInstance.Document;
                            foreach (var subComponentId in subComponentIds)
                            {
                                var nestedElement = document.GetElement(subComponentId);
                                if (nestedElement != null && nestedElement.IsValidObject)
                                {
                                    if (nestedElement is FamilyInstance nestedFamilyInstance &&
                                        nestedFamilyInstance.Symbol != null &&
                                        nestedFamilyInstance.Symbol.IsValidObject &&
                                        FamilySymbolService.GetFamilyName(nestedFamilyInstance.Symbol) == gtpPointFamilyName)
                                    {
                                        if (nestedFamilyInstance.Location is LocationPoint locationPoint)
                                        {
                                            element.Points.Add(new GtpxPoint()
                                            {
                                                Direction = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 },
                                                Location = locationPoint.Point.ToPoint3D(),
                                                PointType = PointType.GTP,
                                                // TODO : not sure why the UpVector is not being set here
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
