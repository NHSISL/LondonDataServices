using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings
{
    public class IngestionTracking
    {
        public Guid Id { get; set; }

        // TODO:  Add your properties here

        public Guid CreatedByUserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
