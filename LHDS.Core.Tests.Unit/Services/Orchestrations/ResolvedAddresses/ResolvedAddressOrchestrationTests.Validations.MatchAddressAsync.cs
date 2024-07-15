// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchIfAssignUPRNIsNullAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses();
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;
            string inputResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().UnstructuredPostalAddress;

            ValueTask<AssignAddress> randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            AssignAddress storageAssignAddressWithBlankUPRN = await randomAssignAddress;
            storageAssignAddressWithBlankUPRN.UPRN = 0;

            ValueTask<AssignAddress> storageAssignAddress = new ValueTask<AssignAddress>(storageAssignAddressWithBlankUPRN);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddresses())
                   .Returns(unmatchedResolvedAddresses.AsQueryable())
                   .Returns(new List<ResolvedAddress>().AsQueryable());

            this.assignProcessingServiceMock.Setup(processing =>
               processing.MatchAddressAsync(inputResolvedAddress))
                   .Returns(storageAssignAddress);

            var nullUPRNResolvedAddressOrchestrationException =
                new NullUPRNResolvedAddressOrchestrationException(
                    message: "Null UPRN Resolved Address orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedResolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.\"",
                    innerException: nullUPRNResolvedAddressOrchestrationException);

            // when
            await this.resolvedAddressOrchestrationService.MatchAddressDataAsync();


            // then
            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressOrchestrationValidationException))),
                       Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
