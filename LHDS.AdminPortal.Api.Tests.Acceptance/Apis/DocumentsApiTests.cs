// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Models.Foundations.Documents;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Documents
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DocumentsApiTests
    {
        private readonly ApiBroker apiBroker;

        public DocumentsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static string GetRandomString() =>
             new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Document CreateRandomDocument() =>
            CreateDocumentFiller().Create();

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