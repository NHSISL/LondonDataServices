// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Assigns
{
    public partial class AssignService
    {
        private delegate ValueTask<AssignAddress> ReturningAssignAddressFunction();

        private async ValueTask<AssignAddress> TryCatch(ReturningAssignAddressFunction returningAssignAddressFunction)
        {
            try
            {
                return await returningAssignAddressFunction();
            }
            catch (InvalidArgumentAssignException invalidArgumentAssignAssignException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAssignAssignException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignServiceException);
            }
            catch (Exception exception)
            {
                var failedAssignServiceException =
                    new FailedAssignServiceException(
                        message: "Failed assign service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAssignServiceException);
            }
        }

        private async ValueTask<AssignValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var assignValidationException =
                new AssignValidationException(
                    message: "Assign validation errors occurred, please try again.",
                    innerException: exception);

            await loggingBroker.LogErrorAsync(assignValidationException);

            return assignValidationException;
        }

        private async ValueTask<AssignDependencyException> CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var assignDependencyException =
                new AssignDependencyException(
                    message: "Assign dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await loggingBroker.LogErrorAsync(assignDependencyException);

            throw assignDependencyException;
        }

        private async ValueTask<AssignServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var assignServiceException = new
                AssignServiceException(
                    message: "Assign service error occurred, please contact support.",
                    innerException: exception);

            await loggingBroker.LogErrorAsync(assignServiceException);

            return assignServiceException;
        }

    }
}
