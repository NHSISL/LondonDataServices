using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifacts
{
    public class TerminologyArtifact
    {
        public Guid Id { get; set; }

        // TODO:  Add your properties here

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
