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
                throw CreateAndLogValidationException(invalidArgumentAssignAssignProcessingException);
            }
            catch (AssignValidationException assignValidationException)
            {
                throw CreateAndLogDependencyValidationException(assignValidationException);
            }
            catch (AssignDependencyValidationException assignDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(assignDependencyValidationException);
            }
            catch (AssignDependencyException assignDependencyException)
            {
                throw CreateAndLogDependencyException(assignDependencyException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw CreateAndLogDependencyException(assignServiceException);
            }
            catch (Exception exception)
            {
                var failedAssignProcessingServiceException =
                    new FailedAssignProcessingServiceException(
                        message: "Failed assign processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAssignProcessingServiceException);
            }
        }

        private AssignProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var assignProcessingValidationException =
                new AssignProcessingValidationException(
                    message: "Assign processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(assignProcessingValidationException);

            return assignProcessingValidationException;
        }

        private AssignProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var assignProcessingDependencyValidationException =
                new AssignProcessingDependencyValidationException(
                    message: "Assign processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(assignProcessingDependencyValidationException);

            return assignProcessingDependencyValidationException;
        }

        private AssignProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var assignProcessingDependencyException =
                new AssignProcessingDependencyException(
                    message: "Assign processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(assignProcessingDependencyException);

            throw assignProcessingDependencyException;
        }

        private AssignProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var assignProcessingServiceException = new
                AssignProcessingServiceException(
                    message: "Assign processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(assignProcessingServiceException);

            return assignProcessingServiceException;
        }

    }
}
