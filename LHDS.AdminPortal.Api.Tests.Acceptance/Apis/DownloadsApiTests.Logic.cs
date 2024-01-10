// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.Documents;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Downloads
{
    public partial class DownloadsApiTests
    {
        
        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcessAsync()
        {
            // given
            List<Document> randomDownloads = CreateRandomDocuments();
            List<Document> externalDownloads = randomDownloads;
            List<Document> expectedDownloads = externalDownloads;

            foreach (Document document in externalDownloads)
            {
                this.apiBroker.PostDocumentAsync()
            }

            // when
            List<Document> actualDownloads =
                await this.apiBroker.RetrieveListOfDocumentsToProcessAsync();

            // then
            actualDownloads.Should().BeEquivalentTo(expectedDownloads);
        }
    }
}