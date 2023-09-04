using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace Gtpx.ModelSync.Export.Revit.Caches
{
    public static class ParameterBasedAssemblyCache
    {
        private static  Dictionary<ElementId, string> elementIdToAssemblyNameMap = new Dictionary<ElementId, string>();

        public static void Reset()
        {
            elementIdToAssemblyNameMap = new Dictionary<ElementId, string>();
        }

        public static void Add(ElementId elementId, string assemblyName)
        {
            elementIdToAssemblyNameMap.Add(elementId, assemblyName);
        }

        public static Dictionary<ElementId, string> GetElementIdToAssemblyNameMap()
        {
            return elementIdToAssemblyNameMap;
        }
    }
}
