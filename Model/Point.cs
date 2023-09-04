namespace Gtpx.ModelSync.DataModel.Models
{
    public class Point
    {
        public Vector3D Direction { get; set; }
        public Point3D Location { get; set; }
        public PointType PointType { get; set; }
        public Vector3D UpVector { get; set; }
    }
}
