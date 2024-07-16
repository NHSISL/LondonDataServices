// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Processings.AssignAddresses.Exceptions;
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
            catch (InvalidArgumentAssignAddressProcessingException invalidArgumentAssignAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAssignAddressProcessingException);
            }
        }

        private AssignAddressProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var assignAddressProcessingValidationException =
                new AssignAddressProcessingValidationException(
                    message: "Assign address validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(assignAddressProcessingValidationException);

            return assignAddressProcessingValidationException;
        }
    }
}
