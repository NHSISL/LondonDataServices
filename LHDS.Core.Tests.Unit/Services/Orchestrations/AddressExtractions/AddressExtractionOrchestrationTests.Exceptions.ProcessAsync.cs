// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressExtractionOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressExtractionIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputFilePath = @"c:\temp\TestNestedZip.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            var expectedDependencyException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

            AddressExtractionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressExtractionOrchestrationDependencyValidationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressParserServiceMock.Verify(service =>
             service.ProcessCsvAsync(It.IsAny<byte[]>()),
                 Times.Once);

            this.addressExtractionAuditServiceMock.Verify(service =>
             service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                 Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}