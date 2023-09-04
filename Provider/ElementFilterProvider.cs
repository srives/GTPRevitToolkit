using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using GTP.Services;
using Autodesk.Revit.Creation;
using Document = Autodesk.Revit.DB.Document;

namespace GTP.Providers
{
    public static class ElementFilterProvider
    {
        // private readonly ConnectorCache connectorCache;
        private static Dictionary<string, Element> uniqueIdToElementMap;
        private static Options options = new Options();

        public static IEnumerable<AssemblyInstance> GetAssemblies(Document document)
        {
            return new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_Assemblies)
                                                         .OfClass(typeof(AssemblyInstance))
                                                         .Cast<AssemblyInstance>()
                                                         .Where(x => !string.IsNullOrWhiteSpace(x.Name));
        }

        public static FilteredElementCollector GetFilteredElementCollector(Document document)
        {
            return new FilteredElementCollector(document).WherePasses(GetElementCategoryFilter(document))
                                                         .WhereElementIsNotElementType()
                                                         .WhereElementIsViewIndependent();
        }

        public static IEnumerable<Element> GetFilteredElements(Document document)
        {
            return GetUniqueIdToElementMap(document).Values;
        }

        public static bool IsExtracted(Document document, Element element)
        {
            return GetUniqueIdToElementMap(document).ContainsKey(element.UniqueId);
        }

        private static Dictionary<string, Element> GetUniqueIdToElementMap(Document document)
        {
            if (uniqueIdToElementMap == null)
            {
                // lazy load elements on first call
                uniqueIdToElementMap = new Dictionary<string, Element>();

                var filteredElementCollector = GetFilteredElementCollector(document);

                foreach (var element in filteredElementCollector.Where(x => IsValid(x)))
                {
                    uniqueIdToElementMap[element.UniqueId] = element;
                    
                    if (!connectorCache.CacheConnectors(element))
                    {
                        Debug.Assert(true);
                    }                    
                }
            }

            return uniqueIdToElementMap;
        }

        public static LogicalOrFilter GetElementCategoryFilter(Document document)
        {
            var elementCategoryFilters = new List<ElementCategoryFilter>()
            {
                new ElementCategoryFilter(BuiltInCategory.OST_Assemblies),
                new ElementCategoryFilter(BuiltInCategory.OST_CableTray),
                new ElementCategoryFilter(BuiltInCategory.OST_CableTrayFitting),
                new ElementCategoryFilter(BuiltInCategory.OST_Conduit),
                new ElementCategoryFilter(BuiltInCategory.OST_ConduitFitting),
                new ElementCategoryFilter(BuiltInCategory.OST_DuctAccessory),
                new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves),
                new ElementCategoryFilter(BuiltInCategory.OST_DuctFitting),
                new ElementCategoryFilter(BuiltInCategory.OST_DuctTerminal),
                new ElementCategoryFilter(BuiltInCategory.OST_ElectricalFixtures),
                new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves),
                new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves),
                new ElementCategoryFilter(BuiltInCategory.OST_GenericModel),
                new ElementCategoryFilter(BuiltInCategory.OST_Grids),
                new ElementCategoryFilter(BuiltInCategory.OST_Levels),
                new ElementCategoryFilter(BuiltInCategory.OST_LightingDevices),
                new ElementCategoryFilter(BuiltInCategory.OST_LightingFixtures),
                new ElementCategoryFilter(BuiltInCategory.OST_Lights),
                new ElementCategoryFilter(BuiltInCategory.OST_ElectricalEquipment),
                new ElementCategoryFilter(BuiltInCategory.OST_MechanicalEquipment),
                new ElementCategoryFilter(BuiltInCategory.OST_Parts),
                new ElementCategoryFilter(BuiltInCategory.OST_PipeAccessory),
                new ElementCategoryFilter(BuiltInCategory.OST_PipeConnections),
                new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves),
                new ElementCategoryFilter(BuiltInCategory.OST_PipeFitting),
                new ElementCategoryFilter(BuiltInCategory.OST_PipeSegments),
                new ElementCategoryFilter(BuiltInCategory.OST_PlumbingFixtures),
                new ElementCategoryFilter(BuiltInCategory.OST_Rooms),
                new ElementCategoryFilter(BuiltInCategory.OST_SpecialityEquipment),
                new ElementCategoryFilter(BuiltInCategory.OST_Sprinklers),
                new ElementCategoryFilter(BuiltInCategory.OST_Walls)
            };

            // Only add element category filters that are not already present in the list
            elementCategoryFilters.AddRange(
                FabricationItemElementFilterProvider.GetElementFilters(document)
                                                    .Cast<ElementCategoryFilter>()
                                                    .Where(x => !elementCategoryFilters.Any(y => y.CategoryId.Equals(x.CategoryId))));

            var elementFilters = new List<ElementFilter>();
            elementFilters.AddRange(elementCategoryFilters);

            return new LogicalOrFilter(elementFilters);
        }

        private static bool IsValid(Element element)
        {
            var result = true;

            if (element.IsValidObject)
            {
                if (element.Category == null || element.get_Geometry(options) == null)
                {
                    // Exclude elements that don't have a category or don't have any geometry
                    result = false;
                }
                else
                {
                    var familyInstance = element as FamilyInstance;
                    if (familyInstance != null &&
                        familyInstance.Symbol != null &&
                        FamilySymbolService.GetFamilyName(familyInstance.Symbol).Equals("GTP") &&
                        familyInstance.SuperComponent != null)
                    {
                        // Exclude APL points that are nested into a family instance
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

    }
}
