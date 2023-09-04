using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Export.Revit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Gtpx.ModelSync.CAD.Cache;
using PropertyDefinition = Gtpx.ModelSync.DataModel.Models.PropertyDefinition;

namespace Gtpx.ModelSync.Export.Revit.Services
{
  
    public static class DerivedPropertyDefinitionService
    {
        private static Dictionary<string, DerivedPropertyDefinition> derivedPropertyDefinitions = null;
        private static PropertyDefinitionCache propertyDefinitionCache;
        private static UnitType unitType = UnitType.Imperial;

        private static Dictionary<string, DerivedPropertyDefinition> LoadDefinitions()
        {
            var resourceName = $"Gtpx.ModelSync.Export.Revit.Configuration.DerivedPropertyDefinitionConfigurations.json";
            using (var stream = typeof(DerivedPropertyDefinitionService).Assembly.GetManifestResourceStream(resourceName))
            using (var streamReader = new System.IO.StreamReader(stream))
            {
                var defintions = JsonConvert.DeserializeObject<List<DerivedPropertyDefinition>>(streamReader.ReadToEnd());

                return defintions.ToDictionary(x => x.Name, x => x);
            }
        }

        public static IEnumerable<DerivedPropertyDefinition> GetDefinitions(PropertyDefinitionSource propertyDefinitionSource)
        {
            if (derivedPropertyDefinitions == null)
            {
                derivedPropertyDefinitions = LoadDefinitions();
            }
            return derivedPropertyDefinitions.Values.Where(x => x.PropertyDefinitionSource == propertyDefinitionSource);
        }

        public static PropertyDataType GetDisplayDataType(DerivedPropertyDefinition derivedPropertyDefinition)
        {
            switch (unitType)
            {
                case UnitType.Imperial:
                    return derivedPropertyDefinition.DisplayDataTypeImperial;
                case UnitType.Metric:
                    return derivedPropertyDefinition.DisplayDataTypeMetric;
                default:
                    throw new ArgumentException($"Unknown measurement units:{unitType}");
            }
        }

        public static PropertyDataType GetStorageDataType(DerivedPropertyDefinition derivedPropertyDefinition)
        {
            switch (unitType)
            {
                case UnitType.Imperial:               
                    return derivedPropertyDefinition.StorageDataTypeImperial;
                case UnitType.Metric:
                    return derivedPropertyDefinition.StorageDataTypeMetric;
                default:
                    throw new ArgumentException($"Unknown measurement units:{unitType}");
            }
        }

        public static PropertyDataType GetDisplayDataTypeForRevitFamily(DerivedPropertyDefinition derivedPropertyDefinition)
        {
            if (derivedPropertyDefinition.Name.Equals("Length"))
            {
                switch (unitType)
                {
                    case UnitType.Imperial:
                        return PropertyDataType.FeetInchesFraction;
                    case UnitType.Metric:
                        return PropertyDataType.DecimalMillimeters;
                    default:
                        throw new ArgumentException($"Unknown measurement units:{unitType}");
                }
            }
            else
            {
                switch (unitType)
                {
                    case UnitType.Imperial:
                        return derivedPropertyDefinition.DisplayDataTypeImperial;
                    case UnitType.Metric:
                        return derivedPropertyDefinition.DisplayDataTypeMetric;
                    default:
                        throw new ArgumentException($"Unknown measurement units:{unitType}");
                }
            }
        }

        public static void SetDerivedProperty(Element element, string name, string value, bool isRevitFamily = false)
        {
            var derivedPropertyDefinition = GetDefinition(name);
            propertyDefinitionCache.Add(
                new PropertyDefinition
                {
                    DisplayDataType = isRevitFamily ? GetDisplayDataTypeForRevitFamily(derivedPropertyDefinition) : GetDisplayDataType(derivedPropertyDefinition),
                    IsReadOnly = derivedPropertyDefinition.IsReadOnly,
                    Name = name,
                    StorageDataType = GetStorageDataType(derivedPropertyDefinition)
                },
                element);

            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(derivedPropertyDefinition.DefaultValue))
            {
                value = derivedPropertyDefinition.DefaultValue;
            }

            element.NameToDerivedPropertyMap[name] = new Property()
            {
                Name = name,
                Value = value
            };
        }

        private static DerivedPropertyDefinition GetDefinition(string name)
        {
            if (derivedPropertyDefinitions == null)
            {
                derivedPropertyDefinitions = LoadDefinitions();
            }
            return derivedPropertyDefinitions[name];
        }
    }
}
