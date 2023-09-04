namespace Gtpx.ModelSync.CAD.Services
{
    public static class PropertyNameService
    {
        public static string CleanName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }

            // replace any characters that cause issues as an element name in json
            return propertyName.Replace("$", string.Empty)
                               .Replace(".", string.Empty)
                               .Trim();
        }
    }
}
