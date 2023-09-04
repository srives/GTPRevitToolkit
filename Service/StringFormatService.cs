using System;

namespace Gtpx.ModelSync.Export.Revit.Services
{
    public static class StringFormatService
    {
        public static string FormatDouble(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return "0";
            }

            // avoid overflow situations
            if (value > (double)Decimal.MaxValue)
            {
                value = (double)Decimal.MaxValue;
            }
            else if (value < (double)Decimal.MinValue)
            {
                value = (double)Decimal.MinValue;
            }

            return Normalize(Convert.ToDecimal(value.ToString("F5"))).ToString();
        }

        public static decimal Normalize(decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
    }
}
