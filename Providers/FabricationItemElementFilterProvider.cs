using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace GTP.Providers
{
    public static class FabricationItemElementFilterProvider
    {

        public static List<ElementFilter> GetElementFilters(Document document)
        {
            var fabricationElementFilters = new List<ElementFilter>();

            foreach (Category category in document.Settings.Categories)
            {
                if (category.CategoryType.Equals(CategoryType.Model))
                {
                    fabricationElementFilters.Add(new ElementCategoryFilter(category.Id));
                }
            }

            return fabricationElementFilters;
        }
    }
}
