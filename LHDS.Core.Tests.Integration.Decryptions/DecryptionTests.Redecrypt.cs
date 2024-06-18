// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldRedecryptAsync()
        {
            // given
            DateTimeOffset olderThanDateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-15);

            var itemsThatRequireDecryption = ingestionTrackingService.RetrieveAllIngestionTrackings()
                .Where(ingestionTrackingItem =>
                        ingestionTrackingItem.IsDownloaded == true
                        && ingestionTrackingItem.Decrypted == false
                        && ingestionTrackingItem.IsProcessing == false
                        && ingestionTrackingItem.RetryCount < 4
                        && ingestionTrackingItem.LastAttempt <= olderThanDateTimeOffset)
                .Select(ingestionTrackingItem => ingestionTrackingItem.Id)
                .ToList();

            // when
            await decryptionClient.RetryDecryptAsync();

            // then
            var remainingItems = ingestionTrackingService.RetrieveAllIngestionTrackings()
                .Where(ingestionTrackingItem =>
                        ingestionTrackingItem.IsDownloaded == true
                        && ingestionTrackingItem.Decrypted == false
                        && ingestionTrackingItem.IsProcessing == false
                        && ingestionTrackingItem.RetryCount < 4
                        && ingestionTrackingItem.LastAttempt <= olderThanDateTimeOffset)
                .Select(ingestionTrackingItem => ingestionTrackingItem.Id)
                .ToList();

            if (remainingItems.Count > 0)
            {
                remainingItems.Should().NotContain(itemsThatRequireDecryption);
            }
        }
    }
}
