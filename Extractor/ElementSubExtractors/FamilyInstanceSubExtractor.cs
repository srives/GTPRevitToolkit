using Autodesk.Revit.DB;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.FamilyInstances
{
    public static class FamilyInstanceSubExtractor
    {
        public static void ProcessFamilyInstance(Document document, FamilyInstance familyInstance,
                                          GtpxElement element)
        {
            if (familyInstance.SuperComponent != null)
            {
                element.ParentElementId = familyInstance.SuperComponent.UniqueId;
            }

            foreach (var subComponentId in familyInstance.GetSubComponentIds())
            {
                var subComponent = document.GetElement(subComponentId);
                if (subComponent != null && subComponent.IsValidObject)
                {
                    element.ChildElementIds.Add(subComponent.UniqueId);
                }
            }

            AnchorPointExtractor.ProcessFamilyInstance(familyInstance, element);
            DimensionReferencePointExtractor.ProcessFamilyInstance(document, familyInstance, element);
            EvPointExtractor.ProcessFamilyInstance(familyInstance, element);
            GtpPointExtractor.ProcessFamilyInstance(familyInstance, element);
        }
    }
}