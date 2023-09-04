using Autodesk.Revit.DB;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{
    public static class ElementSubExtractor
    {
        public static void ProcessElement(Element revitElement, GtpxElement element)
        {
            parameterExtractor.ProcessElement(revitElement, element);
            conduitRunPropetySubExtractor.ProcessElement(revitElement, element);
            oletPipeCutExtractor.ProcessElement(revitElement, element);
            pointSubExtractor.ProcessElement(revitElement, element);
            straightPipeCutExtractor.ProcessElement(revitElement, element);
            worksetExtractor.ProcessElement(revitElement, element);
        }
    }
}
