// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingService
    {
        private delegate ValueTask ReturningNothingProcessingFunction();
        private delegate ValueTask<ResolvedAddress> ReturningResolvedAddressProcessingFunction();
        private delegate ValueTask<bool> ReturningBooleanProcessingFunction();
        private delegate ValueTask<IQueryable<ResolvedAddress>> ReturningResolvedAddressesFunction();

        private async ValueTask TryCatch(
            ReturningNothingProcessingFunction returningNothingProcessingFunction)
        {
            try
            {
                await returningNothingProcessingFunction();
            }
            catch (NullResolvedAddressProcessingException nullResolvedAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullResolvedAddressException);
            }
            catch (InvalidArgumentResolvedAddressProcessingException invalidArgumentResolvedAddressProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentResolvedAddressProcessingException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressProcessingServiceException);
            }
        }

        private async ValueTask<ResolvedAddress> TryCatch(
            ReturningResolvedAddressProcessingFunction returningResolvedAddressProcessingFunction)
        {
            try
            {
                return await returningResolvedAddressProcessingFunction();
            }
            catch (NullResolvedAddressProcessingException nullResolvedAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullResolvedAddressException);
            }
            catch (InvalidArgumentResolvedAddressProcessingException invalidArgumentResolvedAddressProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentResolvedAddressProcessingException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressProcessingServiceException);
            }
        }

        private async ValueTask<bool> TryCatch(
            ReturningBooleanProcessingFunction returningBooleanProcessingFunction)
        {
            try
            {
                return await returningBooleanProcessingFunction();
            }
            catch (InvalidArgumentResolvedAddressProcessingException invalidArgumentResolvedAddressProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentResolvedAddressProcessingException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<ResolvedAddress>> TryCatch(
            ReturningResolvedAddressesFunction returningResolvedAddressesFunction)
        {
            try
            {
                return await returningResolvedAddressesFunction();
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressProcessingServiceException);
            }
        }


        private async ValueTask<ResolvedAddressProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressProcessingValidationExceptionn =
                new ResolvedAddressProcessingValidationException(
                    message: "Ingestion tracking audit processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressProcessingValidationExceptionn);

            return resolvedAddressProcessingValidationExceptionn;
        }

        private async ValueTask<ResolvedAddressProcessingDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Ingestion tracking audit processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(resolvedAddressProcessingDependencyValidationException);

            return resolvedAddressProcessingDependencyValidationException;
        }

        private async ValueTask<ResolvedAddressProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Ingestion tracking audit processing dependency error occurred, please contact support.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(resolvedAddressProcessingDependencyException);

            throw resolvedAddressProcessingDependencyException;
        }

        private async ValueTask<ResolvedAddressProcessingServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressProcessingServiceException = new
                ResolvedAddressProcessingServiceException(
                    message: "Ingestion tracking audit processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressProcessingServiceException);

            return resolvedAddressProcessingServiceException;
        }
    }
}
