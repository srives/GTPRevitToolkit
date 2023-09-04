using Autodesk.Revit.DB;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Extensions;
using System.Collections.Generic;
using System.Linq;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
using GtpxPoint = Gtpx.ModelSync.DataModel.Models.Point;

namespace Gtpx.ModelSync.Export.Revit.Extractors
{
    public static class WallExtractor
    {
        private static readonly Options options = new Options() { ComputeReferences = true, DetailLevel = ViewDetailLevel.Fine };

        public static void ProcessWall(Wall wall,
                                GtpxElement element)
        {
            var points = new List<GtpxPoint>();
            var geometryElement = wall.get_Geometry(options);
            foreach (var geometryObject in geometryElement)
            {
                // A wall is really a solid, so process any solids
                var solid = geometryObject as Solid;
                if (solid != null)
                {
                    // Iterate though all the faces in the solid
                    foreach (Face face in solid.Faces)
                    {
                        var planarFace = face as PlanarFace;
                        if (planarFace != null)
                        {
                            var normal = planarFace.FaceNormal;

                            // Only interested in faces that have a normal aligned to the Z axis (face at the top of the wall)
                            if (normal.IsAlmostEqualTo(XYZ.BasisZ))
                            {
                                // Iterate through every loop in the face
                                var edgeLoops = planarFace.EdgeLoops;
                                foreach (EdgeArray edgeLoop in edgeLoops)
                                {
                                    // Iterate through every edge in the loop
                                    foreach (Edge edge in edgeLoop)
                                    {
                                        // Extract the end points from the edge
                                        var curve = edge.AsCurve();

                                        var startPoint = curve.GetEndPoint(0);
                                        if (!points.Any(p => p.Location.IsAlmostEqualTo(startPoint.ToPoint3D())))
                                        {
                                            // The start point is not already in the array, so add it
                                            points.Add(new GtpxPoint
                                            {
                                                Direction = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 },
                                                Location = startPoint.ToPoint3D(),
                                                PointType = PointType.Wall,
                                                // TODO : not sure why the UpVector is not being set here
                                            });
                                        }

                                        var endPoint = curve.GetEndPoint(1);
                                        if (!points.Any(p => p.Location.IsAlmostEqualTo(endPoint.ToPoint3D())))
                                        {
                                            // The end point is not already in the array, so add it
                                            points.Add(new GtpxPoint
                                            {
                                                Direction = new Vector3D() { X = 0.0, Y = 0.0, Z = 1.0 },
                                                Location = endPoint.ToPoint3D(),
                                                PointType = PointType.Wall,
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
            element.Points.AddRange(points);
        }
    }
}
