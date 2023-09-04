using Gtpx.ModelSync.DataModel.Enums;
namespace Gtpx.ModelSync.DataModel.Enums
{
    public enum UnitType
    {
        Imperial = 0,
        Metric = 1
    }
    public enum PropertyDefinitionSource
    {
        ElementProperty,
        Extractor
    }
    public enum AncillaryType
    {
        AirturnTrack = 0,
        AirturnVane = 1,
        AncillaryMaterial = 2,
        Clip = 3,
        Corner = 4,
        Fixing = 5,
        Gasket = 6,
        Isolator = 7,
        Sealant = 8,
        SeamMaterial = 9,
        SupportRod = 10,
        TieRod = 11,
        Unknown = 12,
        SupportSeismic = 13
    }
    public enum AncillaryUsageType
    {
        Airturn = 0,
        Connector = 1,
        Hanger = 2,
        Loose = 3,
        Seam = 4,
        Splitter = 5,
        Stiffener = 6,
        Undefined = 7
    }
    public enum PointType
    {
        Connector = 1,
        Anchor = 2,
        GTP = 3,
        Wall = 4,
        Centroid = 5,
        TapConnector = 6,
        CenterlineIntersection = 7,
        DimensionReference = 8,
        Positioning = 9
    }
    public enum PropertyDataType
    {
        AngleDegrees = 0,
        AngleRadians = 1,
        CurrencyUSD = 2,
        DateTimeHours = 3,
        DecimalFeet = 4,
        DecimalInches = 5,
        FeetInchesFraction = 6,
        String = 7,
        Double = 8,
        Integer = 9,
        DecimalMillimeters = 10,
        MassPounds = 11,
        MassKilograms = 12,
        Meters = 13,
        CurrencyAUSD = 14,
        CurrencyPoundsUK = 15,
        SquareFeet = 16,
        SquareMeters = 17,
        CubicFeet = 18,
        CubicMeters = 19,
        Amps = 20,
        VoltAmps = 21,
        Volts = 22,
        Percentage = 23,
        InchesFractions = 24,
        Gallons = 25,
        TemperatureFarenheit = 26,
        TemperatureCelsius = 27,
        Hertz = 28,
        Watts = 29,
        DateTimeMinutes = 30,
        DateTimeSeconds = 31,
        DateTimeDays = 32,
        DecimalCentimeters = 33,
        CubicCentimeters = 34,
        SquareCentimeters = 35,
        TemperatureKelvin = 36
    }

    public enum SourceSystemType
    {
        AddIn = 1
    }

    public enum PipeCutType
    {
        Straight = 1,
        Saddle = 4,
        Hole = 6
    }

    public enum AuditType
    {
        DimensionPointThresholdExceeded = 0
    }

    public enum PlatformType
    {
        Revit = 1,
        AutoCAD = 2
    }
    public enum ModelSyncMode
    {
        None = 0,
        Extract = 1,
        Load = 2,
        CreateAssembly = 3,
        SetProjectInfo = 4
    }

    public enum LogLevel
    {
        Debug = 0,
        Information,
        Warning,
        Error,
        Fatal
    }




}
