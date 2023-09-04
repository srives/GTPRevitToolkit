using Autodesk.Revit.DB;
using Gtpx.ModelSync.DataModel.Enums;

namespace Gtpx.ModelSync.DataModel.Models
{
    public class Vector3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class ConnectorPoint
    {
        public Vector3D Direction { get; set; }
        public double Height { get; set; }
        public Point3D Location { get; set; }
        public string MatingElementUniqueId { get; set; } = string.Empty;
        public PointType PointType { get; set; }
        public Vector3D UpVector { get; set; }
        public double Width { get; set; }
    }
}
