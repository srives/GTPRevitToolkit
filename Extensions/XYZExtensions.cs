using Autodesk.Revit.DB;
using Gtpx.ModelSync.DataModel.Models;
using System;

namespace Gtpx.ModelSync.Export.Revit.Extensions
{
    public static class Point3DExtensions
    {
        public static double DistanceTo(this Point3D firstPoint,
                                        Point3D secondPoint)
        {
            return Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) +
                             Math.Pow(firstPoint.Y - secondPoint.Y, 2) +
                             Math.Pow(firstPoint.Z - secondPoint.Z, 2));
        }

        public static bool IsAlmostEqualTo(this Point3D firstPoint,
                                          Point3D secondPoint,
                                          double tolerance = 1e-6)
        {
            return DistanceTo(firstPoint, secondPoint) < tolerance;
        }
    }

    public static class XYZExtensions
    {
        public static bool IsParallelTo(this XYZ sourceVector,
                                        XYZ vector,
                                        double tolerance = 0.0001)
        {
            var sourceUnitVector = sourceVector.Normalize();
            var unitVector = vector.Normalize();
            return sourceUnitVector.IsAlmostEqualTo(unitVector, tolerance) || sourceUnitVector.IsAlmostEqualTo(-unitVector, tolerance);
        }

        public static Point3D ToPoint3D(this XYZ point)
        {
            return new Point3D
            {
                X = point.X,
                Y = point.Y,
                Z = point.Z
            };
        }

        public static Vector3D ToVector3D(this XYZ vector)
        {
            return new Vector3D
            {
                X = vector.X,
                Y = vector.Y,
                Z = vector.Z
            };
        }
    }
}
