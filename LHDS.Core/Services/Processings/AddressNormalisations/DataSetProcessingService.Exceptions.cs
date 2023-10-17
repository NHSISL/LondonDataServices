// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingService
    {
        private delegate ValueTask<AddressNormalisation> ReturningAddressNormalisationFunction();

        private async ValueTask<AddressNormalisation> TryCatch(ReturningAddressNormalisationFunction returningAddressNormalisationFunction)
        {
            try
            {
                return await returningAddressNormalisationFunction();
            }
            catch (InvalidArgumentAddressNormalisationProcessingException
            invalidArgumentAddressNormalisationProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressNormalisationProcessingException);
            }
        }

        private AddressNormalisationProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressNormalisationProcessingValidationException =
                new AddressNormalisationProcessingValidationException(
                    message: "Address normalisation processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationProcessingValidationException);

            return addressNormalisationProcessingValidationException;
        }
    }
}