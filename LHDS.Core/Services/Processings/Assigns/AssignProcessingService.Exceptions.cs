// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using LHDS.Core.Models.Processings.Assigns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Assigns
{
    public partial class AssignProcessingService
    {
        private delegate ValueTask<AssignAddress> ReturningAssignAddressFunction();

        private async ValueTask<AssignAddress> TryCatch(ReturningAssignAddressFunction returningAssignAddressFunction)
        {
            try
            {
                return await returningAssignAddressFunction();
            }
            catch (InvalidArgumentAssignProcessingException invalidArgumentAssignAssignProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAssignAssignProcessingException);
            }
            catch (AssignValidationException assignValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(assignValidationException);
            }
            catch (AssignDependencyValidationException assignDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(assignDependencyValidationException);
            }
            catch (AssignDependencyException assignDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignDependencyException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignServiceException);
            }
            catch (Exception exception)
            {
                var failedAssignProcessingServiceException =
                    new FailedAssignProcessingServiceException(
                        message: "Failed assign processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAssignProcessingServiceException);
            }
        }

        private async ValueTask<AssignProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var assignProcessingValidationException =
                new AssignProcessingValidationException(
                    message: "Assign processing validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(assignProcessingValidationException);

            return assignProcessingValidationException;
        }

        private async ValueTask<AssignProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var assignProcessingDependencyValidationException =
                new AssignProcessingDependencyValidationException(
                    message: "Assign processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(assignProcessingDependencyValidationException);

            return assignProcessingDependencyValidationException;
        }

        private async ValueTask<AssignProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var assignProcessingDependencyException =
                new AssignProcessingDependencyException(
                    message: "Assign processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(assignProcessingDependencyException);

            return assignProcessingDependencyException;
        }

        private async ValueTask<AssignProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var assignProcessingServiceException = new
                AssignProcessingServiceException(
                    message: "Assign processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(assignProcessingServiceException);

            return assignProcessingServiceException;
        }
    }
}
