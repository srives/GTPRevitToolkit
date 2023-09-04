namespace Gtpx.ModelSync.DataModel.Models
{
    public enum PipeCutType
    {
        Straight = 1,
        Saddle = 4,
        Hole = 6
    };

    public class PipeCut
    {
        public string ConnectorName { get; set; }
        public double Distance { get; set; } = -1.0;
        public double MateAngle { get; set; } = -1.0;
        public int MatingPartConnIndex { get; set; } = -1;
        public string MatingPartCadId { get; set; }
        public PipeCutType CutType { get; set; }
        public double RadialAngle { get; set; } = -1.0;
        public double MatingCLPipeOD { get; set; } = -1.0;
    }
}
