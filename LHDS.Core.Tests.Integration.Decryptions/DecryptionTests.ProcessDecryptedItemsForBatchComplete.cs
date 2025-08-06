// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldProcessDecryptedItemsForBatchCompleteAsync()
        {
            // given
            Guid supplierId = Guid.NewGuid(); // Replace with actual supplier ID as needed
            // when
            await decryptionClient.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);

            // then
        }
    }
}
