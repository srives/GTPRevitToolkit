using System.Collections.Generic;
using Gtpx.ModelSync.DataModel.Enums;

namespace Gtpx.ModelSync.DataModel.Models
{
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
