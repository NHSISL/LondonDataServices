// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisation.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
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
            catch (AddressNormalisationValidationException addressNormalisationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationValidationException);
            }
            catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
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

        private AddressNormalisationProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressNormalisationProcessingDependencyValidationException =
                new AddressNormalisationProcessingDependencyValidationException(
                    message: "Address normalisation processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressNormalisationProcessingDependencyValidationException);

            return addressNormalisationProcessingDependencyValidationException;
        }
    }
}