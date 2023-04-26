// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Manual.OptOut
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldPushExpiredOptOutsToMeshForRenewalAsyncsAsync()
        {
            try
            {
                MeshMessage message = await optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();
                message.Should().NotBeNull();
                message.MessageId.Should().NotBeNullOrWhiteSpace();
                var messageId = message.MessageId;
                //bool messageAcked = await meshService.AcknowledgeMessageByIdAsync(messageId);
                //messageAcked.Should().BeTrue();
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }
        }
    }
}