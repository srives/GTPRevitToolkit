using System.Collections.Generic;

namespace Gtpx.ModelSync.DataModel.Models
{
    public enum AncillaryType
    {
        AirturnTrack = 0,
        AirturnVane = 1,
        AncillaryMaterial = 2,
        Clip = 3,
        Corner = 4,
        Fixing = 5,
        Gasket = 6,
        Isolator = 7,
        Sealant = 8,
        SeamMaterial = 9,
        SupportRod = 10,
        TieRod = 11,
        Unknown = 12,
        SupportSeismic = 13
    }
    public enum AncillaryUsageType
    {
        Airturn = 0,
        Connector = 1,
        Hanger = 2,
        Loose = 3,
        Seam = 4,
        Splitter = 5,
        Stiffener = 6,
        Undefined = 7
    }

    public class Ancillary
    {
        public AncillaryType AncillaryType { get; set; }
        public AncillaryUsageType AncillaryUsageType { get; set; }
        public double Depth { get; set; }
        public double Length { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ProductManufacturer { get; set; }
        public string ProductMaterial { get; set; }
        public string ProductSize { get; set; }
        public double Quantity { get; set; }
        public List<double> RodLengths { get; set; } = new List<double>();
        public double WidthOrDiameter { get; set; }
    }
}
