using Autodesk.Revit.DB;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.DataModel.Enums;
using Gtpx.ModelSync.Export.Revit.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Gtpx.ModelSync.Export.Revit.Providers
{
    public static class PropertyDataTypesProvider
    {
        private static ILogger _logger;
        private static Dictionary<string, PropertyDataType> nameToPropertyDataTypeMap;
        private static List<PropertyDataTypesConfiguration> propertyDataTypesConfigurations;

#if Revit2019 || Revit2020
        private static Dictionary<DisplayUnitType, PropertyDataTypes> displayUnitTypeToPropertyDataTypesMap = null;
#else
        private static Dictionary<ForgeTypeId, PropertyDataTypes> forgeTypeIdToPropertyDataTypesMap = null;
#endif

        public static void Reset(ILogger logger)
        {
            _logger = logger;
            nameToPropertyDataTypeMap = new Dictionary<string, PropertyDataType>();
            propertyDataTypesConfigurations = new List<PropertyDataTypesConfiguration>();

#if Revit2019 || Revit2020
            displayUnitTypeToPropertyDataTypesMap = new Dictionary<DisplayUnitType, PropertyDataTypes>();
#else
            forgeTypeIdToPropertyDataTypesMap = new Dictionary<ForgeTypeId, PropertyDataTypes>();
#endif

            LoadNameToPropertyDataTypeMap();
            LoadPropertyDataTypesConfigurations();
            LoadUnitTypeIdToPropertyDataTypesMap();
        }

#if Revit2019 || Revit2020 || Revit2021
        public static void GetPropertyDataTypes(Parameter parameter, ILogger logger,
                                         StorageType storageType,
                                         ParameterType parameterType,
                                         out PropertyDataType displayDataType,
                                         out PropertyDataType storageDataType)
        {
            displayDataType = PropertyDataType.String;
            storageDataType = PropertyDataType.String;

            switch (storageType)
            {
                case StorageType.Double:
                    if (parameterType != ParameterType.Invalid &&
                        parameterType != ParameterType.Integer &&
                        parameterType != ParameterType.YesNo)
                    {
                        try
                        {
#if Revit2019 || Revit2020
                            if (displayUnitTypeToPropertyDataTypesMap == null)
#else
                            if (forgeTypeIdToPropertyDataTypesMap == null)
#endif
                            {
                                Reset(logger);
                            }

#if Revit2019 || Revit2020
                            if (displayUnitTypeToPropertyDataTypesMap.TryGetValue(parameter.DisplayUnitType, out PropertyDataTypes propertyDataTypes))
#else
                            if (forgeTypeIdToPropertyDataTypesMap.TryGetValue(parameter.GetUnitTypeId(), out PropertyDataTypes propertyDataTypes))
#endif
                            {
                                displayDataType = propertyDataTypes.DisplayDataType;
                                storageDataType = propertyDataTypes.StorageDataType;
                                return;
                            }
                            else
                            {
                                displayDataType = PropertyDataType.String;
                                storageDataType = PropertyDataType.Double;
#if Revit2019 || Revit2020
                                logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} Unrecognized unit type ID: {parameter.DisplayUnitType}.");
#else
                                _logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} Unrecognized unit type ID: {parameter.GetUnitTypeId()}.");
#endif
                                return;
                            }
                        }
                        catch
                        {
                            // For some reason calling parameter.DisplayUnitType sometimes causes an exception 
                            _logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} unit type ID does not exist.");
                            return;
                        }
                    }
                    break;

                case StorageType.Integer:
                    displayDataType = PropertyDataType.String;
                    if (parameterType == ParameterType.YesNo ||
                        parameterType == ParameterType.Integer ||
                        parameterType == ParameterType.NumberOfPoles ||
                        parameterType == ParameterType.Number)
                    {
                        displayDataType = PropertyDataType.Integer;
                    }
                    storageDataType = PropertyDataType.Integer;
                    break;

                case StorageType.ElementId:
                case StorageType.None:
                case StorageType.String:
                    displayDataType = PropertyDataType.String;
                    storageDataType = PropertyDataType.String;
                    break;

                default:
                    _logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} Unrecognized storage type: {storageType}.");
                    break;
            }
        }
#else
         public static void GetPropertyDataTypes(Parameter parameter, ILogger logger,
                                         StorageType storageType,
                                         ForgeTypeId forgeTypeId,
                                         out PropertyDataType displayDataType,
                                         out PropertyDataType storageDataType)
        {
            displayDataType = PropertyDataType.String;
            storageDataType = PropertyDataType.String;

            switch (storageType)
            {
                case StorageType.Double:
                    if (/*forgeTypeId != ParameterType.Invalid && There does not appear to be an equivalent SpecTypeId for this one*/
                        forgeTypeId != SpecTypeId.Int.Integer &&
                        forgeTypeId != SpecTypeId.Boolean.YesNo)
                    {
                        try
                        {
                            if (forgeTypeIdToPropertyDataTypesMap == null)
                            {
                                Reset(logger);
                            }
                            if (forgeTypeIdToPropertyDataTypesMap.TryGetValue(parameter.GetUnitTypeId(), out PropertyDataTypes propertyDataTypes))
                            {
                                displayDataType = propertyDataTypes.DisplayDataType;
                                storageDataType = propertyDataTypes.StorageDataType;
                                return;
                            }
                            else
                            {
                                displayDataType = PropertyDataType.String;
                                storageDataType = PropertyDataType.Double;
                                logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} Unrecognized unit type ID: {parameter.GetUnitTypeId()}.");
                                return;
                            }
                        }
                        catch
                        {
                            // For some reason calling parameter.DisplayUnitType sometimes causes an exception 
                            logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} unit type ID does not exist.");
                            return;
                        }
                    }
                    break;

                case StorageType.Integer:
                    displayDataType = PropertyDataType.String;
                    if (forgeTypeId == SpecTypeId.Boolean.YesNo ||
                        forgeTypeId == SpecTypeId.Int.Integer ||
                        forgeTypeId == SpecTypeId.Int.NumberOfPoles ||
                        forgeTypeId == SpecTypeId.Number)
                    {
                        displayDataType = PropertyDataType.Integer;
                    }
                    storageDataType = PropertyDataType.Integer;
                    break;

                case StorageType.ElementId:
                case StorageType.None:
                case StorageType.String:
                    displayDataType = PropertyDataType.String;
                    storageDataType = PropertyDataType.String;
                    break;

                default:
                    logger.Debug($"{nameof(PropertyDataTypesProvider)}.{nameof(GetPropertyDataTypes)} Unrecognized storage type: {storageType}.");
                    break;
            }
        }
#endif
        private static void LoadNameToPropertyDataTypeMap()
        {
            var names = Enum.GetNames(typeof(PropertyDataType));
            foreach (var name in names)
            {
                if (Enum.TryParse(name, out PropertyDataType propertyDataType))
                {
                    nameToPropertyDataTypeMap[name] = propertyDataType;
                }
                else
                {
                    _logger.Warning($"Matching PropertyDataType not found for name: {name}.");
                }
            }
        }
        
        private static void LoadPropertyDataTypesConfigurations()
        {
            var assembly = typeof(PropertyDataTypesProvider).GetTypeInfo().Assembly;

            var names = typeof(PropertyDataTypesProvider).GetTypeInfo().Assembly.GetManifestResourceNames();

#if Revit2019 || Revit2020
            var resourceName = $"GTP.Configuration.PropertyDataTypesConfigurationForRevit2017,2018,2019,2020.json";
#else
            var resourceName = $"GTP.Configuration.PropertyDataTypesConfigurationForRevit2021AndNewer.json";
#endif

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var streamReader = new StreamReader(stream))
            {
                propertyDataTypesConfigurations = JsonConvert.DeserializeObject<List<PropertyDataTypesConfiguration>>(streamReader.ReadToEnd());
            }
        }

        private static void LoadUnitTypeIdToPropertyDataTypesMap()
        {
            foreach (var propertyDataTypesConfiguration in propertyDataTypesConfigurations)
            {
#if Revit2019 || Revit2020
                if (Enum.TryParse(propertyDataTypesConfiguration.UnitTypeName, out DisplayUnitType displayUnitType))
                {
                    if (nameToPropertyDataTypeMap.ContainsKey(propertyDataTypesConfiguration.DisplayDataTypeName) &&
                        nameToPropertyDataTypeMap.ContainsKey(propertyDataTypesConfiguration.StorageDataTypeName))
                    {
                        displayUnitTypeToPropertyDataTypesMap[displayUnitType] = new PropertyDataTypes()
                        {
                            DisplayDataType = nameToPropertyDataTypeMap[propertyDataTypesConfiguration.DisplayDataTypeName],
                            StorageDataType = nameToPropertyDataTypeMap[propertyDataTypesConfiguration.StorageDataTypeName]
                        };
                    }
                }
                else
                {
                    logger.Warning($"Matching DisplayUnitType not found for name:{propertyDataTypesConfiguration.UnitTypeName}.");
                }
#else
                var value = typeof(UnitTypeId).GetProperty(propertyDataTypesConfiguration.UnitTypeName, BindingFlags.Public | BindingFlags.Static)
                                              .GetValue(null, null);
                if (value != null && value is ForgeTypeId forgeTypeId)
                {
                    if (nameToPropertyDataTypeMap.ContainsKey(propertyDataTypesConfiguration.DisplayDataTypeName) &&
                        nameToPropertyDataTypeMap.ContainsKey(propertyDataTypesConfiguration.StorageDataTypeName))
                    {
                        forgeTypeIdToPropertyDataTypesMap[forgeTypeId] = new PropertyDataTypes()
                        {
                            DisplayDataType = nameToPropertyDataTypeMap[propertyDataTypesConfiguration.DisplayDataTypeName],
                            StorageDataType = nameToPropertyDataTypeMap[propertyDataTypesConfiguration.StorageDataTypeName]
                        };
                    }
                }
                else
                {
                    _logger.Warning($"Matching ForgeTypeId not found for name:{propertyDataTypesConfiguration.UnitTypeName}.");
                }
#endif
            }
        }
    }
}
