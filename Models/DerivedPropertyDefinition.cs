using Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors;
using Gtpx.ModelSync.DataModel.Enums;

namespace Gtpx.ModelSync.Export.Revit.Models
{
    public class DerivedPropertyDefinition
    {
        public string DefaultValue { get; set; }
        public PropertyDataType DisplayDataTypeImperial { get; set; } = PropertyDataType.String;
        public PropertyDataType DisplayDataTypeMetric { get; set; } = PropertyDataType.String;
        public bool IsReadOnly { get; set; } = true;
        public string Name { get; set; }
        public PropertyDefinitionSource PropertyDefinitionSource { get; set; }
        public string SourceKeys { get; set; }
        public PropertyDataType StorageDataTypeImperial { get; set; } = PropertyDataType.String;
        public PropertyDataType StorageDataTypeMetric { get; set; } = PropertyDataType.String;
    }
}
