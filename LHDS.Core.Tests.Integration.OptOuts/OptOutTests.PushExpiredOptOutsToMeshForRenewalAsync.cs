// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    public partial class OptOutTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldPushExpiredOptOutsToMeshForRenewalAsyncsAsync()
        {
            try
            {
                // Given 
                string batchReference = string.Empty;
                await SetupExpiredTestNhsNumbersForRetrieveUpdatedMesh(batchReference);

                // When
                List<MeshMessage?> messages = await optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();

                // Then
                foreach (MeshMessage message in messages)
                {
                    message.Should().NotBeNull();
                    message.MessageId.Should().NotBeNullOrWhiteSpace();
                    var messageId = message.MessageId;
                    bool messageAcked = await meshService.AcknowledgeMessageByIdAsync(messageId);
                    messageAcked.Should().BeTrue();

                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }
        }
    }
}