// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.HealthChecks.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses
{
    public partial class ResolvedAddressFailedToExportHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressFailedToExportHealthCheckServiceException =
                new FailedResolvedAddressFailedToExportHealthCheckServiceException(

                    message: "Failed resolved address failed to export health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedResolvedAddressFailedToExportHealthCheckServiceException =
                new ResolvedAddressFailedToExportHealthCheckServiceException(

                    message: "Resolved address failed to export health check service error occurred, " +
                        "please contact support.",

                    innerException: failedResolvedAddressFailedToExportHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.resolvedAddressHealthItemService.GetHealthStatusAsync();

            ResolvedAddressFailedToExportHealthCheckServiceException
                actualResolvedAddressFailedToExportHealthCheckServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressFailedToExportHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualResolvedAddressFailedToExportHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressFailedToExportHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressFailedToExportHealthCheckServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}