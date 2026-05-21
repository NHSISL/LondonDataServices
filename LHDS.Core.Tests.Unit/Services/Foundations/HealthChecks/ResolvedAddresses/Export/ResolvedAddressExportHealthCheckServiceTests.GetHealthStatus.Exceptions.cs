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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.Export
{
    public partial class ResolvedAddressExportHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetHealthStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressExportHealthCheckServiceException =
                new FailedResolvedAddressExportHealthCheckServiceException(

                    message:
                        "Failed resolved address export health check service error occurred, " +
                        "please contact support.",

                    innerException: serviceException);

            var expectedResolvedAddressExportHealthCheckServiceException =
                new ResolvedAddressExportHealthCheckServiceException(

                    message:
                        "Resolved address export health check service error occurred, " +
                        "please contact support.",

                    innerException: failedResolvedAddressExportHealthCheckServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<HealthCheckResult> getHealthStatusTask =
                this.resolvedAddressHealthItemService.GetHealthStatusAsync();

            ResolvedAddressExportHealthCheckServiceException
                actualResolvedAddressExportHealthCheckServiceException =
                    await Assert.ThrowsAsync<ResolvedAddressExportHealthCheckServiceException>(
                        getHealthStatusTask.AsTask);

            // then
            actualResolvedAddressExportHealthCheckServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressExportHealthCheckServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressExportHealthCheckServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
