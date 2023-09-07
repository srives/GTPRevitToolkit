using Autodesk.Revit.DB;
using GTP.Providers;
using Gtpx.ModelSync.CAD.Cache;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.CAD.Utilities;
using Gtpx.ModelSync.Export.Revit.Caches;
using Gtpx.ModelSync.Export.Revit.Extractors;
using Gtpx.ModelSync.Export.Revit.Extractors.ElementSubExtractors;
using Gtpx.ModelSync.Export.Revit.Extractors.FamilyInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Documents;
using GtpxElement = Gtpx.ModelSync.DataModel.Models.Element;

namespace GTP.Extractors
{
    static public class ElementExtractor
    {
        static GTProfiler profiler = new GTProfiler();

        /// <summary>
        /// This function loops through all the elements in your model, you can loop through a set number (from start to stop).
        /// </summary>
        /// <param name="document"></param>
        /// <param name="notifier"></param>
        /// <param name="highRefreshRate">If true, we relinquish control to Revit more agressively for UI refresh</param>
        /// <param name="progressInterval"></param>
        /// <param name="start">0 based. -1 means ignore. We loop through X number of elements, and start is where to being that loop (with the starth element)</param>
        /// <param name="stop">Where to stop looping</param>
        /// <returns></returns>
        static public List<ProfilerStats> Execute(Document document, Notifier notifier, bool highRefreshRate, bool collectMemoryStats, int progressInterval, int start, int stop, CancellationToken cancellationToken)
        {
            // Fresh run
            profiler.Reset();
            PropertyDefinitionCache.Reset(); 
            ConnectorCache.Reset(); 

            var revitElements = ElementFilterProvider.GetFilteredElements(document);
            var numElements = revitElements.Count();
            var numFabElements = 0;
            var index = 0;
            var skip = (index < start && start > -1);
            foreach (Element revitElement in revitElements)
            {
                if (cancellationToken.IsCancellationRequested) break;

                if (highRefreshRate)
                {
                    System.Windows.Forms.Application.DoEvents(); // pump out the message queue
                }

                index++;
                if (index < start && start > -1)
                    continue;
                if (index > stop && stop > 0)
                    break;

                if (skip)
                {
                    notifier.Stats(null, index, (stop > 0) ? stop : numElements);
                    skip = false;
                }

                var element = new GtpxElement
                {
#if Revit2024
                    RevitId = revitElement.Id.Value,
                    RevitTypeId = revitElement.GetTypeId().Value,
#else
                    RevitId = revitElement.Id.IntegerValue,
                    RevitTypeId = revitElement.GetTypeId().IntegerValue,
#endif
                    CadType = revitElement.GetType().ToString(),
                    FabricationItemKey = revitElement.UniqueId,
                    Index = index,
                    ElementId = revitElement.UniqueId
                };

                profiler.RestartTimer(1);
                profiler.RestartTimer();
                PartTemplateIdSubExtractor.ProcessElement(document, notifier, revitElement, element);
                if (collectMemoryStats)
                    profiler.CatchTimeAndMemory($"{nameof(PartTemplateIdSubExtractor)}.{element.TemplateId}");
                else
                    profiler.CatchTime($"{nameof(PartTemplateIdSubExtractor)}.{element.TemplateId}");
                profiler.CatchTime($"TotalTime.{nameof(PartTemplateIdSubExtractor)}", 1);

                if (cancellationToken.IsCancellationRequested) break;
                if (revitElement is FamilyInstance familyInstance)
                {
                    FamilyInstanceSubExtractor.ProcessFamilyInstance(document, familyInstance, element);
                    if (collectMemoryStats)
                        profiler.CatchTimeAndMemory($"{nameof(FamilyInstanceSubExtractor)}.{element.TemplateId}");
                    else
                        profiler.CatchTime($"{nameof(FamilyInstanceSubExtractor)}.{element.TemplateId}");
                    profiler.CatchTime($"TotalTime.{nameof(FamilyInstanceSubExtractor)}", 1);
                }
                else if (revitElement is Wall wall)
                {
                    WallExtractor.ProcessWall(wall, element);
                    if (collectMemoryStats)
                        profiler.CatchTimeAndMemory($"{nameof(WallExtractor)}.{element.TemplateId}");
                    else
                        profiler.CatchTime($"{nameof(WallExtractor)}.{element.TemplateId}");
                    profiler.CatchTime($"TotalTime.{nameof(WallExtractor)}", 1);
                }

                if (cancellationToken.IsCancellationRequested) break;
                ElementSubExtractor.ProcessElement(document, notifier, revitElement, element);
                if (collectMemoryStats)
                    profiler.CatchTimeAndMemory($"{nameof(ElementSubExtractor)}.{element.TemplateId}");
                else
                    profiler.CatchTime($"{nameof(ElementSubExtractor)}.{element.TemplateId}");
                profiler.CatchTime($"TotalTime.{nameof(ElementSubExtractor)}", 1);

                if (cancellationToken.IsCancellationRequested) break;
                PartTemplateExtractor.ProcessElement(revitElement, element);
                if (collectMemoryStats)
                    profiler.CatchTimeAndMemory($"{nameof(PartTemplateExtractor)}.{element.TemplateId}");
                else
                    profiler.CatchTime($"{nameof(PartTemplateExtractor)}.{element.TemplateId}");
                profiler.CatchTime($"TotalTime.{nameof(PartTemplateExtractor)}", 1);

                if (element.CadType == "Autodesk.Revit.DB.FabricationPart" || element.CadType == "Autodesk.Fabrication.Item")
                {
                    numFabElements++;
                }

                // elementStorageProvider.Add(element, activityEvent);

                if (cancellationToken.IsCancellationRequested) break;
                if (index % progressInterval == 0)
                {
                    var stats = profiler.SortedList();
                    notifier.Stats(stats, index, (stop > 0) ? stop : numElements);
                    profiler.CatchMemory("ElementExtractor");
                    notifier.Information($"Extracted {index} elements out of {numElements}.");
                    foreach (var time in profiler.ToStrings())
                    {
                        notifier.LogSilent(time);
                    }
                    profiler.CatchMemory("ElementExtractor");
                    if (!highRefreshRate)
                    {
                        System.Windows.Forms.Application.DoEvents(); // pump out the message queue
                    }
                }
            }

            profiler.CatchMemory("ElementExtractor");
            foreach (var time in profiler.ToStrings(GTProfiler.GTProfOptions.Memory))
            {
                notifier.LogSilent(time);
            }

            foreach (var time in profiler.ToStrings())
            {
                notifier.Information(time);
            }

            /*
             * creates the files to upload to Azure
             * 
            elementStorageProvider.Finish(activityEvent);
            partTemplateExtractor.Finish(activityEvent);
            propertyDefinitionStorageProvider.Finish(activityEvent);
            */
            notifier.Information($"Finished extracting {numElements} elements.");
            var ret = profiler.SortedList();
            return ret;
        }
    }
}
