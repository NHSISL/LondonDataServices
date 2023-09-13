// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate
    {
        private static List<Audit> CreateRandomAuditRecords(
            int auditCount,
            List<IngestionTracking> ingestionTrackingItems)
        {
            List<Audit> audits = new List<Audit>();

            foreach (IngestionTracking item in ingestionTrackingItems)
            {
                List<Audit> generatedItems =
                    CreateAuditFiller(dateTimeOffset: DateTimeOffset.UtcNow, item)
                        .Create(count: auditCount)
                            .ToList();

                audits.AddRange(generatedItems);
            }

            return audits;
        }

        private static Filler<Audit> CreateAuditFiller(
            DateTimeOffset dateTimeOffset,
            IngestionTracking item)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Audit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(audit => audit.IngestionTrackingId).Use(item.Id)
                .OnProperty(audit => audit.CreatedBy).Use(user)
                .OnProperty(audit => audit.UpdatedBy).Use(user);

            return filler;
        }
    }
}
