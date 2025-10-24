using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.CAD.Utilities;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;

namespace Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors
{
    public static class ElementSubExtractor
    {
        public static int ProcessElement(Document document, Notifier logger, Element revitElement, GtpxElement element, long tolerance, bool searchForComplexParts)
        {
            var numParameters = ParameterExtractor.ProcessElement(document, logger, revitElement, element, tolerance, searchForComplexParts);
            if (numParameters > 0)
            {
                GTProfiler.AddElementId($"{nameof(ElementSubExtractor)}.{element?.TemplateId}", $"{element.RevitId}");
            }
            /*
             * To do: Finish these
            conduitRunPropetySubExtractor.ProcessElement(revitElement, element);
            oletPipeCutExtractor.ProcessElement(revitElement, element);
            pointSubExtractor.ProcessElement(revitElement, element);
            straightPipeCutExtractor.ProcessElement(revitElement, element);
            worksetExtractor.ProcessElement(revitElement, element);
            */
            return numParameters;
        }
    }
}
