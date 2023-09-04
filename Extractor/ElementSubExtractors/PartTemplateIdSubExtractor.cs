using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.Export.Revit.Services;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;
namespace Gtpx.ModelSync.Export.Revit.Extractors
{
    public static class PartTemplateIdSubExtractor
    {
        public static void ProcessElement(Document document, Notifier notifier, Element revitElement, GtpxElement element)
        {
            element.TemplateId = GetTemplateId(document, notifier, revitElement, element);
        }

        public static string GetTemplateId(Document document, Notifier notifier, Element revitElement, GtpxElement element)
        {
            // we handle both Revit parts and Fabrication items the same way here, using the Family name to identify the part template
            // logic for Fabrication items is handled on server side, but we must have Revit property data published for all parts here
            // because there may be Revit project parameters applied to Fabrication parts that need to get added to part templates too
            var familyName = ParameterValueService.GetValue(document, notifier, revitElement, "Family");
            if (string.IsNullOrEmpty(familyName))
            {
                return $"{element.CadType}.";
            }
            return $"{element.CadType}.{familyName}";
        }
    }
}
