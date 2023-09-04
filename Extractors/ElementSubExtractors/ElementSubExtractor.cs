using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.UI;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{
    public static class ElementSubExtractor
    {
        public static void ProcessElement(Document document, Notifier logger, Element revitElement, GtpxElement element)
        {
            ParameterExtractor.ProcessElement(document, logger, revitElement, element);
            /*
             * To do: Finish these
            conduitRunPropetySubExtractor.ProcessElement(revitElement, element);
            oletPipeCutExtractor.ProcessElement(revitElement, element);
            pointSubExtractor.ProcessElement(revitElement, element);
            straightPipeCutExtractor.ProcessElement(revitElement, element);
            worksetExtractor.ProcessElement(revitElement, element);
            */
        }
    }
}
