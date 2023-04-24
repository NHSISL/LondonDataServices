// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
                await optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }

        }
    }
}