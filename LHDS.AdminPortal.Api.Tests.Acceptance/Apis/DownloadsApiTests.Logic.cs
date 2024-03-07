// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Downloads;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Downloads
{
    public partial class DownloadsApiTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcessAsync()
        {
            //given 
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Document randomDocument = CreateRandomDocument();

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = randomDocument.FileName }
            };

            // when
            List<Download> actualDownloads =
                await this.apiBroker.RetrieveListOfDocumentsToProcessAsync(inputDownload);

            // then
            actualDownloads.Count.Should().BeGreaterThan(0);
        }
    }
}