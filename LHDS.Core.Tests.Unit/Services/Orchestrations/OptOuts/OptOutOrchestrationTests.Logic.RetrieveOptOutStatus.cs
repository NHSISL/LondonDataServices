// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public class OptOutOrchestrationTests
    {
        public async Task ShouldRetrieveOptOutStatusAsync()
        {
            // given
            var randomBytes = Encoding.ASCII.GetBytes(GetRandomString());
            var inputBytes = randomBytes;


            // when
            await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(inputBytes);


            // then
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
