// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Downloads
{
    public partial class DownloadsApiTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcessAsync()
        {
            // when
            List<Document> actualDownloads =
                await this.apiBroker.RetrieveListOfDocumentsToProcessAsync();

            // then
            actualDownloads.Count.Should().BeGreaterThan(0);
        }
    }
}