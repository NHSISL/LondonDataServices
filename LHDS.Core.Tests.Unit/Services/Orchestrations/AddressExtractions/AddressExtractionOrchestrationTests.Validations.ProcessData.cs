// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessDataIfDataIsNullAndLogItAsync()
        {
            // given
            byte[] someData = null;

            var invalidArgumentAddressExtractionOrchestrationException =
                new InvalidArgumentAddressExtractionOrchestrationException(
                    message: "Invalid argument address extraction orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedAddressExtractionOrchestrationValidationException =
                new AddressExtractionOrchestrationValidationException(
                    message: "Address extraction orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentAddressExtractionOrchestrationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessDataAsync(someData);

            AddressExtractionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationValidationException>(processDataTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressExtractionOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionOrchestrationValidationException))),
                        Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        //[Fact]
        //public async Task ShouldThrowValidationExceptionOnProcessDataIfCsvDataIsNullAndLogItAsync()
        //{
        //    // given
        //    byte[] inputData = Encoding.UTF8.GetBytes(GetRandomString());

        //    var invalidArchiveAddressExctractionOrchestrationException =
        //        new InvalidArchiveAddressExtractionOrchestrationException(
        //            message: "Invalid address extraction orchestration archive, " +
        //                "please correct the errors and try again.");

        //    invalidArchiveAddressExctractionOrchestrationException.AddData(
        //       key: "MemoryStream",
        //       values: "File(s) required");

        //    var expectedAddressExtractionOrchestrationValidationException =
        //        new AddressExtractionOrchestrationValidationException(
        //            message: "Address extraction orchestration validation error occurred, please try again.",
        //            innerException: invalidArchiveAddressExctractionOrchestrationException);

        //    // when
        //    ValueTask<List<Address>> processDataTask =
        //        this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

        //    AddressExtractionOrchestrationValidationException actualException =
        //        await Assert.ThrowsAsync<AddressExtractionOrchestrationValidationException>(processDataTask.AsTask);

        //    // then
        //    actualException.Should()
        //        .BeEquivalentTo(expectedAddressExtractionOrchestrationValidationException);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            expectedAddressExtractionOrchestrationValidationException))),
        //                Times.Once);

        //    this.addressParserServiceMock.VerifyNoOtherCalls();
        //    this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
        //    this.dateTimeBrokerMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}
    }
}
