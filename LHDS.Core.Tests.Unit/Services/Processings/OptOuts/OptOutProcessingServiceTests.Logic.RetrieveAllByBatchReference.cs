// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllOptOutsByBatchReferenceAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputBatchReference = randomString;
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts(inputBatchReference);
            IQueryable<OptOut> retrievedOptOuts = randomOptOuts.DeepClone();

            List<OptOut> expectedOptOuts = retrievedOptOuts
                .Where(optOut => optOut.BatchReference == inputBatchReference)
                    .ToList();

            this.optOutServiceMock.Setup(service =>
                service.RetrieveAllOptOutsAsync())
                    .ReturnsAsync(retrievedOptOuts);

            // when
            List<OptOut> actualOptOuts =
                await this.optOutProcessingService.RetrieveAllOptOutsByBatchReferenceAsync(inputBatchReference);

            // then
            actualOptOuts.Count().Should().Be(3);
            actualOptOuts.Should().BeEquivalentTo(expectedOptOuts);

            this.optOutServiceMock.Verify(service =>
                service.RetrieveAllOptOutsAsync(),
                    Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}