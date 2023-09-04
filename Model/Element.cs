using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gtpx.ModelSync.DataModel.Models
{
    public enum SourceSystemType
    {
        AddIn = 1
    }

    public class Element
    {
        public List<Ancillary> Ancillaries { get; set; } = new List<Ancillary>();
        public string BatchIndex { get; set; }
        public string CadType { get; set; }

        // This only applies for Revit FamilyInstances
        // This will be a list of element UniqueIds of the FamilyInstances (SubComponents) referenced by this FamilyInstance
        // The list will be empty if this FamilyInstance does not reference any FamilyInstances (SubComponents)
        public List<string> ChildElementIds { get; set; } = new List<string>();

        public List<ConnectorPoint> ConnectorPoints { get; set; } = new List<ConnectorPoint>();

        // Needed by serializer, do not remove
        public IEnumerable<Property> DerivedProperties
        {
            get
            {
                return NameToDerivedPropertyMap.Values;
            }
        }

        public string Description { get; set; }

        // For Revit this is the element UniqueId
        // For AutoCAD this is the Fabrication item handle
        public string ElementId { get; set; }

        public string FabricationItemKey { get; set; }
        public int Index { get; set; }

        // There should only be one derived property for each name.  Using a dictionary here 
        // guarantees that only the last derived property with a name will be maintained.  
        // This is how STRATUS V1 publishing behaves and V2 must behave the same way.
        // JsonIgnore attribute is used because this derived property must not be serialized.  
        // The DerivedProperties property contains what needs to be serialized.
        [JsonIgnore]
        public Dictionary<string, Property> NameToDerivedPropertyMap { get; set; } = new Dictionary<string, Property>();

        // There should only be one property for each name.  Using a dictionary here 
        // guarantees that only the last property with a name will be maintained.  
        // This is how STRATUS V1 publishing behaves and V2 must behave the same way.
        // JsonIgnore attribute is used because this property must not be serialized.  
        // The Properties property contains what needs to be serialized.
        [JsonIgnore]
        public Dictionary<string, Property> NameToPropertyMap { get; set; } = new Dictionary<string, Property>();

        // This only applies for Revit FamilyInstances
        // This will be the element UniqueId for the FamilyInstance (SuperComponent) that references this FamilyInstance
        // This will be null if this FamilyInstance is not referenced by another FamilyInstance (SuperComponent) 
        public string ParentElementId { get; set; }

        public List<PipeCut> PipeCuts { get; set; } = new List<PipeCut>();
        public List<Point> Points { get; set; } = new List<Point>();

        // Needed by serializer, do not remove
        public IEnumerable<Property> Properties
        {
            get
            {
                return NameToPropertyMap.Values;
            }
        }

        public SortedSet<string> PropertyDefinitionIds { get; set; } = new SortedSet<string>();
        // Revit 2024 requires us to go to long
        [DefaultValue(-1)]
        public long RevitId { get; set; } = -1;
        [DefaultValue(-1)]
        // Revit 2024 requires us to go to long
        public long RevitTypeId { get; set; } = -1;
        public SourceSystemType SourceSystemTypeId { get; set; }
        public string TemplateId { get; set; }
    }
}