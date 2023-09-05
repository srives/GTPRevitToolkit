using Gtpx.ModelSync.DataModel.Enums;
using System.Collections.Generic;

namespace Gtpx.ModelSync.DataModel.Models
{
    public class PartTemplate
    {
        public string BatchIndex { get; set; }
        public string CadType { get; set; }
        public string Category { get; set; }
        public string FittingType { get; set; }
        public string Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string PatternNumber { get; set; }
        public SortedSet<string> PropertyDefinitionIds { get; set; } = new SortedSet<string>();
        public SourceSystemType SourceSystemTypeId { get; set; }
    }
}
