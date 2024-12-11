// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate
    {
        private static List<IngestionTrackingAudit> CreateRandomAuditRecords(
            int auditCount,
            List<IngestionTracking> ingestionTrackingItems)
        {
            List<IngestionTrackingAudit> audits = new List<IngestionTrackingAudit>();

            foreach (IngestionTracking item in ingestionTrackingItems)
            {
                List<IngestionTrackingAudit> generatedItems =
                    CreateAuditFiller(dateTimeOffset: DateTimeOffset.UtcNow, item)
                        .Create(count: auditCount)
                            .ToList();

                audits.AddRange(generatedItems);
            }

            return audits;
        }

        private static Filler<IngestionTrackingAudit> CreateAuditFiller(
            DateTimeOffset dateTimeOffset,
            IngestionTracking item)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTrackingAudit>();

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
