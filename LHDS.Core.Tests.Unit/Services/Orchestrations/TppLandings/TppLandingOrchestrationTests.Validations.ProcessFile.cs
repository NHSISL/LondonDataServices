// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using LHDS.Core.Services.Orchestrations.Tpp;
using Moq;
using Xunit;


namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfDocumentFileNameIsNullAndLogItAsync(
            string invalidText)
        {
            // given
            Guid supplierId = Guid.Empty;
            Stream randomStream = new MemoryStream();
            Stream inputStream = new MemoryStream();
            string inputFileName = invalidText;

            var invalidArgumentException =
                new InvalidArgumentTppLandingOrchestrationException(
                    message: "Invalid TPP landing orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentException.AddData(
               key: "FileName",
               values: "Text is required");

            invalidArgumentException.AddData(
               key: "SupplierId",
               values: "Id is required");

            var expectedTppOrchestrationValidationException = invalidArgumentException;

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            // when
            ValueTask<Guid> returnedGuidTask = tppOrchestrationServiceMock.Object
                .ProcessFileAsync(fileName: inputFileName, supplierId);

            InvalidArgumentTppLandingOrchestrationException actualException =
                await Assert.ThrowsAsync<InvalidArgumentTppLandingOrchestrationException>(returnedGuidTask.AsTask);

            // then
            actualException.Should()
               .BeEquivalentTo(expectedTppOrchestrationValidationException);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}