using System.Collections.Generic;

namespace Gtpx.ModelSync.CAD
{
    public enum AuditType
    {
        DimensionPointThresholdExceeded = 0
    }

    public class AuditResult
    {
        public AuditType AuditType { get; set; }
        public string CadId { get; set; }
        public string ElementId { get; set; } // Revit: Element.Id, AutoCAD: handle
    }

    public class AuditResults
    {
        private readonly List<AuditResult> auditResults;

        public AuditResults()
        {
            auditResults = new List<AuditResult>();
        }

        public void Add(AuditResult auditResult)
        {
            auditResults.Add(auditResult);
        }

        public IEnumerable<AuditResult> GetAuditResults()
        {
            return auditResults;
        }
    }
}
