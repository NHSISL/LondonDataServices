// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Models.Foundations.Documents;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Downloads
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DownloadsApiTests
    {
        private readonly ApiBroker apiBroker;

        public DownloadsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static string GetRandomString() =>
             new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<Document> CreateRandomDocuments()
        {
            return CreateDocumentFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<Document> CreateDocumentFiller()
        {
            var filler = new Filler<Document>();
            string filename = GetRandomString();

            filler.Setup()
                .OnProperty(document => document.FileName).Use(filename);

            return filler;
        }
    }
}