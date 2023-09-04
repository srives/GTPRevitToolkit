using Gtpx.ModelSync.DataModel.Enums;

namespace Gtpx.ModelSync.DataModel.Models
{
    [System.Serializable]
    public struct PropertyDefinition 
    {
        public string BatchIndex { get; set; }
        public PropertyDataType DisplayDataType { get; set; }
        public string Id { get; set; }
        public int Index { get; set; }
        public bool IsReadOnly { get; set; }
        public string Name { get; set; }
        public PropertyDataType StorageDataType { get; set; }
        public SourceSystemType SourceSystemTypeId { get; set; }
    }
}
