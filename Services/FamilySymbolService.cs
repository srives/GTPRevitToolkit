using Autodesk.Revit.DB;

namespace GTP.Services
{
    public static class FamilySymbolService
    {
        /// <summary>
        /// Gets the family name from the Family property of the family symbol.
        /// Note that if the Family is null this function will return an empty string.
        /// </summary>
        /// <param name="familySymbol">Family symbol.</param>
        /// <returns>Family name.</returns>
        public static string GetFamilyName(FamilySymbol familySymbol)
        {
            var familyName = string.Empty;
            if (familySymbol.Family != null)
            {
                familyName = familySymbol.Family.Name ?? string.Empty;
            }
            return familyName;
        }
    }
}
