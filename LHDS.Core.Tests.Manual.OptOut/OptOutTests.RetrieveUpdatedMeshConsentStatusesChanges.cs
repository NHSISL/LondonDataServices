// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Manual.OptOut
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task RetrieveUpdatedMeshConsentStatusesChanges()
        {
            try
            {
                List<MeshMessage> messages = await optOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync();
                //message.Should().NotBeNull();
                //message.MessageId.Should().NotBeNullOrWhiteSpace();
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
            }

        }
    }
}