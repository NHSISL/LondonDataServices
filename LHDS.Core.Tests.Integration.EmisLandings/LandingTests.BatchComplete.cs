// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.EmisLandings
{
    public partial class LandingTests
    {
        [Fact]
        public async Task ShouldCheckForBatchCompleteAsync()
        {
            try
            {
                // given

                // when
                await this.decryptionClient.ProcessDecryptedItemsForBatchCompleteAsync();

                // then
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                Console.WriteLine(ex.Message);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"{ex.Message}, {ex?.InnerException?.Message}");
            }
        }
    }
}