using Autodesk.Revit.DB;
using GTP.Services;
using Gtpx.ModelSync.CAD;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Caches;
using Gtpx.ModelSync.Export.Revit.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using GtpxPoint = Gtpx.ModelSync.DataModel.Models.Point;

namespace Gtpx.ModelSync.Export.Revit.Extractors.FamilyInstances
{
    public static class DimensionReferencePointExtractor
    {
        private static AuditResults auditResults = new AuditResults();
        private static int dimensionPointThreshold = 40; // come from Settings.json file
        private static bool getSelectedViewInvoked = false;
        private static Options options;
        private static View3D selectedView;

        public static void Reset(AuditResults ar)
        {
            auditResults = ar;
            ConnectorCache.Reset();
            getSelectedViewInvoked = false;
        }

        public static void ProcessFamilyInstance(Document document, FamilyInstance familyInstance,
                                          GtpxElement element)
        {
            var view = GetSelectedView(document);
            var options = GetOptions(view);
            bool addedPoint = false;
            if (familyInstance.Symbol != null && familyInstance.Symbol.IsValidObject)
            {
                var symbolName = familyInstance.Symbol.Name;
                if (!string.IsNullOrEmpty(symbolName))
                {
                    if (!symbolName.StartsWith("Anchor - DEWALT - ") && FamilySymbolService.GetFamilyName(familyInstance.Symbol) != "GTP")
                    {
                        var subComponentIds = familyInstance.GetSubComponentIds();
                        if (subComponentIds != null)
                        {
                            var famIDocument = familyInstance.Document;
                            foreach (var subComponentId in subComponentIds)
                            {
                                var nestedElement = famIDocument.GetElement(subComponentId);
                                if (nestedElement != null && nestedElement.IsValidObject)
                                {
                                    if (nestedElement is FamilyInstance nestedFamilyInstance && nestedFamilyInstance.Symbol != null)
                                    {
                                        var nestedFamilyName = FamilySymbolService.GetFamilyName(nestedFamilyInstance.Symbol);
                                        if (!string.IsNullOrEmpty(nestedFamilyName))
                                        {
                                            if (nestedFamilyName == "sPoint")
                                            {
                                                if (nestedFamilyInstance.Location is LocationPoint locationPoint)
                                                {
                                                    element.Points.Add(new GtpxPoint()
                                                    {
                                                        Direction = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 },
                                                        Location = locationPoint.Point.ToPoint3D(),
                                                        PointType = PointType.DimensionReference,
                                                        // TODO : not sure why the UpVector is not being set here
                                                    });
                                                    addedPoint = true;
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

            if (!addedPoint)
            {
                if (familyInstance.Category != null)
                {
                    var categoryName = familyInstance.Category.Name;
                    if (!ConnectorCache.GetConnectors(familyInstance).Any() ||
                        categoryName == "Electrical Equipment" ||
                        categoryName == "Mechanical Equipment" ||
                        categoryName == "Specialty Equipment")
                    {
                        var partGeometryPointList = new List<XYZ>();
                        var geometryElement = familyInstance.get_Geometry(options);
                        int numDimensionPointsFound = 0;
                        AuditResult auditResult = null;
                        foreach (var geometryObject in geometryElement)
                        {
                            var geometryInstance = geometryObject as GeometryInstance;
                            if (geometryInstance != null && geometryInstance.SymbolGeometry != null)
                            {
                                var transform = geometryInstance.Transform;
                                foreach (var symbolGeometryObject in geometryInstance.SymbolGeometry)
                                {
                                    var solid = symbolGeometryObject as Solid;
                                    try
                                    {
                                        if (solid != null && solid.Volume > 0 && solid.Visibility.Equals(Visibility.Visible))
                                        {
                                            bool isHidden = false;
                                            if (familyInstance.Document.GetElement(solid.GraphicsStyleId) is GraphicsStyle graphicsStyle && view != null)
                                            {
                                                isHidden = view.GetCategoryHidden(graphicsStyle.GraphicsStyleCategory.Id);
                                            }
                                            if (!isHidden)
                                            {
                                                foreach (Edge edge in solid.Edges)
                                                {
                                                    var edgeCurve = edge.AsCurve();
                                                    var endPoint1 = edgeCurve.GetEndPoint(0);
                                                    var point1 = transform.OfPoint(endPoint1);
                                                    var endPoint2 = edgeCurve.GetEndPoint(1);
                                                    var point2 = transform.OfPoint(endPoint2);
                                                    var directionVector = point1.Subtract(point2).Normalize();
                                                    var direction = directionVector.ToVector3D();
                                                    if (!partGeometryPointList.Where(x => x.X.Equals(point1.X) &&
                                                                                          x.Y.Equals(point1.Y) &&
                                                                                          x.Z.Equals(point1.Z)).Any())
                                                    {
                                                        if (numDimensionPointsFound >= dimensionPointThreshold)
                                                        {
                                                            auditResult = new AuditResult()
                                                            {
                                                                AuditType = AuditType.DimensionPointThresholdExceeded,
                                                                CadId = familyInstance.UniqueId,
                                                                ElementId = familyInstance.Id.ToString()
                                                            };
                                                            break;
                                                        }

                                                        element.Points.Add(new GtpxPoint()
                                                        {
                                                            Direction = direction,
                                                            Location = point1.ToPoint3D(),
                                                            PointType = PointType.DimensionReference,
                                                            UpVector = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 }
                                                        });
                                                        partGeometryPointList.Add(point1);
                                                        numDimensionPointsFound++;

                                                    }

                                                    if (!partGeometryPointList.Where(x => x.X.Equals(point2.X) &&
                                                                                          x.Y.Equals(point2.Y) &&
                                                                                          x.Z.Equals(point2.Z)).Any())
                                                    {
                                                        if (numDimensionPointsFound > dimensionPointThreshold)
                                                        {
                                                            auditResult = new AuditResult()
                                                            {
                                                                AuditType = AuditType.DimensionPointThresholdExceeded,
                                                                CadId = familyInstance.UniqueId,
                                                                ElementId = familyInstance.Id.ToString()
                                                            };
                                                            break;
                                                        }

                                                        element.Points.Add(new GtpxPoint()
                                                        {
                                                            Direction = direction,
                                                            Location = point2.ToPoint3D(),
                                                            PointType = PointType.DimensionReference,
                                                            UpVector = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 }
                                                        });
                                                        partGeometryPointList.Add(point2);
                                                        numDimensionPointsFound++;
                                                    }
                                                }

                                                if (auditResult != null)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        // Rarely solid.Volume can throw an exception
                                        continue;
                                    }
                                }

                                if (auditResult != null)
                                {
                                    break;
                                }
                            }
                        }

                        if (auditResult != null)
                        {
                            auditResults.Add(auditResult);
                        }
                    }
                }
            }
        }

        private static Options GetOptions(View3D view)
        {
            if (options == null)
            {
                options = new Options();
                if (view != null)
                {
                    options.View = view;
                }
            }

            return options;
        }

        private static View3D GetSelectedView(Document doc)
        {
            if (!getSelectedViewInvoked)
            {
                var selectedViewId = doc.ActiveView.Id.ToString();
                // var selectedViewId = stratusSettingsProvider.GetPlatformSettings().SelectedViewId;
                if (!string.IsNullOrEmpty(selectedViewId))
                {
                    selectedView = doc.GetElement(selectedViewId) as View3D;
                }
                getSelectedViewInvoked = true;
            }

            return selectedView;
        }
    }
}
