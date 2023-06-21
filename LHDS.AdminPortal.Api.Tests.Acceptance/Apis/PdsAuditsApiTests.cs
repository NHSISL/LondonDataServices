// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.PdsAudits
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PdsAuditsApiTests
    {
        private readonly ApiBroker apiBroker;

        public PdsAuditsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static PdsAudit UpdatePdsAuditWithRandomValues(PdsAudit inputPdsAudit)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnProperty(pdsAudit => pdsAudit.Id).Use(inputPdsAudit.Id)
                .OnProperty(pdsAudit => pdsAudit.CorrelationId).Use(inputPdsAudit.CorrelationId)
                .OnProperty(pdsAudit => pdsAudit.FileName).Use(inputPdsAudit.FileName)
                .OnProperty(pdsAudit => pdsAudit.MessageId).Use(inputPdsAudit.MessageId)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(inputPdsAudit.CreatedBy)
                .OnProperty(pdsAudit => pdsAudit.CreatedDate).Use(inputPdsAudit.CreatedDate)
                .OnProperty(pdsAudit => pdsAudit.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<PdsAudit> PostRandomPdsAuditAsync()
        {
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            await this.apiBroker.PostPdsAuditAsync(randomPdsAudit);

            return randomPdsAudit;
        }

        private async ValueTask<List<PdsAudit>> PostRandomPdsAuditsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomPdsAudits = new List<PdsAudit>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomPdsAudits.Add(await PostRandomPdsAuditAsync());
            }

            return randomPdsAudits;
        }

        private static PdsAudit CreateRandomPdsAudit() =>
            CreateRandomPdsAuditFiller().Create();

        private static Filler<PdsAudit> CreateRandomPdsAuditFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(pdsAudit => pdsAudit.CreatedDate).Use(now)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(user)
                .OnProperty(pdsAudit => pdsAudit.UpdatedDate).Use(now)
                .OnProperty(pdsAudit => pdsAudit.UpdatedBy).Use(user);

            return filler;
        }

    }
}