namespace Gtpx.ModelSync.Pipeline.Models
{
    public class CurrentModelContext
    {
        public string Bim360UserId { get; set; }
        public string DesktopPlatform { get; set; }
        public int GtpModelVersionId { get; set; }
        public int GtpModelVersionNumber { get; set; }
        public bool IsPublishWithoutFabricationDb { get; set; } = false;
        public int JobId { get; set; }
        public int LastActivityDefinitionId { get; set; }
        public int LastActivityEventId { get; set; }
        public int LocaleId { get; set; }
        public string ModelFabConfigId { get; set; }
        public string ModelId { get; set; }
        public string ModelName { get; set; }
    }
}
