// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Tests.Integration.IDecide
{
    public partial class IDecideTests
    {
        [Fact]
        public async Task ShouldGetPatientDecisionsAsync()
        {
            // given

            // when
            var patientDecisions = await this.iDecideClient.GetPatientDecisions();

            // then
            Assert.NotNull(patientDecisions);
        }
    }
}
