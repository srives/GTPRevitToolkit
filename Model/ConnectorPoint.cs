using Autodesk.Revit.DB;

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

    public enum PointType
    {
        Connector = 1,
        Anchor = 2,
        GTP = 3,
        Wall = 4,
        Centroid = 5,
        TapConnector = 6,
        CenterlineIntersection = 7,
        DimensionReference = 8,
        Positioning = 9
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
