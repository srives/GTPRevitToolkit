using Autodesk.Revit.DB;
using GTP.Services;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Extensions;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using GtpxPoint = Gtpx.ModelSync.DataModel.Models.Point;

namespace Gtpx.ModelSync.Export.Revit.Extractors.FamilyInstances
{
    public static class EvPointExtractor 
    {
        private static string evPointFamilyName = "eV_Point";
        
        public static void ProcessFamilyInstance(FamilyInstance familyInstance,
                                          GtpxElement element)
        {
            if (familyInstance.Symbol != null && familyInstance.Symbol.IsValidObject)
            {
                var symbolName = familyInstance.Symbol.Name;
                if (!string.IsNullOrEmpty(symbolName))
                {
                    if (FamilySymbolService.GetFamilyName(familyInstance.Symbol) == evPointFamilyName)
                    {
                        if (familyInstance.Location is LocationPoint locationPoint)
                        {
                            element.Points.Add(new GtpxPoint()
                            {
                                Direction = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 },
                                Location = locationPoint.Point.ToPoint3D(),
                                PointType = PointType.Positioning,
                                // TODO : not sure why the UpVector is not being set here
                            });
                        }
                    }
                }
            }
        }
    }
}
