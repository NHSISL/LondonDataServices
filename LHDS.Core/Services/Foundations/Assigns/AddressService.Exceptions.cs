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
                throw CreateAndLogValidationException(invalidArgumentAssignAssignException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw CreateAndLogDependencyException(assignServiceException);
            }
            catch (Exception exception)
            {
                var failedAssignServiceException =
                    new FailedAssignServiceException(
                        message: "Failed assign service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAssignServiceException);
            }
        }

        private AssignValidationException CreateAndLogValidationException(Xeption exception)
        {
            var assignValidationException =
                new AssignValidationException(
                    message: "Assign validation errors occurred, please try again.",
                    innerException: exception);

            loggingBroker.LogError(assignValidationException);

            return assignValidationException;
        }

        private AssignDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var assignDependencyValidationException =
                new AssignDependencyValidationException(
                    message: "Assign dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            loggingBroker.LogError(assignDependencyValidationException);

            return assignDependencyValidationException;
        }

        private AssignDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var assignDependencyException =
                new AssignDependencyException(
                    message: "Assign dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            loggingBroker.LogError(assignDependencyException);

            throw assignDependencyException;
        }

        private AssignServiceException CreateAndLogServiceException(Xeption exception)
        {
            var assignServiceException = new
                AssignServiceException(
                    message: "Assign service error occurred, please contact support.",
                    innerException: exception);

            loggingBroker.LogError(assignServiceException);

            return assignServiceException;
        }

    }
}
