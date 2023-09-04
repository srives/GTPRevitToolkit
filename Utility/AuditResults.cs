using System.Collections.Generic;
using Gtpx.ModelSync.DataModel.Enums;

namespace Gtpx.ModelSync.CAD
{
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
