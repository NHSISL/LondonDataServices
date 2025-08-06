// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldProcessDecryptedItemsForBatchCompleteAsync()
        {
            // given

            // when
            await decryptionClient.ProcessDecryptedItemsForBatchCompleteAsync();

            // then
        }
    }
}
