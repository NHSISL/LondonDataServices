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
    public partial class ResolvedAddressProcessingService : IResolvedAddressProcessingService
    {
        private delegate ValueTask ReturningNothingProcessingFunction();
        private delegate ValueTask<ResolvedAddress> ReturningResolvedAddressProcessingFunction();
        private delegate ValueTask<bool> ReturningBooleanProcessingFunction();
        private delegate IQueryable<ResolvedAddress> ReturningResolvedAddressesFunction();

        private async ValueTask TryCatch(
            ReturningNothingProcessingFunction returningNothingProcessingFunction)
        {
            try
            {
                await returningNothingProcessingFunction();
            }
            catch (NullResolvedAddressProcessingException nullResolvedAddressException)
            {
                throw CreateAndLogValidationException(nullResolvedAddressException);
            }
            catch (InvalidArgumentResolvedAddressProcessingException invalidArgumentResolvedAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressProcessingException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressProcessingServiceException);
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
                throw CreateAndLogValidationException(nullResolvedAddressException);
            }
            catch (InvalidArgumentResolvedAddressProcessingException invalidArgumentResolvedAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressProcessingException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressProcessingServiceException);
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
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressProcessingException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressProcessingServiceException);
            }
        }

        private IQueryable<ResolvedAddress> TryCatch(
            ReturningResolvedAddressesFunction returningResolvedAddressesFunction)
        {
            try
            {
                return returningResolvedAddressesFunction();
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressValidationException);
            }
            catch (ResolvedAddressDependencyValidationException resolvedAddressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressDependencyValidationException);
            }
            catch (ResolvedAddressDependencyException resolvedAddressDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressDependencyException);
            }
            catch (ResolvedAddressServiceException resolvedAddressServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressProcessingServiceException =
                    new FailedResolvedAddressProcessingServiceException(
                        message: "Failed resolved address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressProcessingServiceException);
            }
        }


        private ResolvedAddressProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var resolvedAddressProcessingValidationExceptionn =
                new ResolvedAddressProcessingValidationException(
                    message: "Resolved address processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressProcessingValidationExceptionn);

            return resolvedAddressProcessingValidationExceptionn;
        }

        private ResolvedAddressProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var resolvedAddressProcessingDependencyValidationException =
                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(resolvedAddressProcessingDependencyValidationException);

            return resolvedAddressProcessingDependencyValidationException;
        }

        private ResolvedAddressProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var resolvedAddressProcessingDependencyException =
                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(resolvedAddressProcessingDependencyException);

            throw resolvedAddressProcessingDependencyException;
        }

        private ResolvedAddressProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var resolvedAddressProcessingServiceException = new
                ResolvedAddressProcessingServiceException(
                    message: "Resolved address processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressProcessingServiceException);

            return resolvedAddressProcessingServiceException;
        }
    }
}
