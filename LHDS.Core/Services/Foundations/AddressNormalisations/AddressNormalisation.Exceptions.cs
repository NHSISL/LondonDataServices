// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationService
    {
        private delegate ValueTask<AddressNormalisation> ReturningAddressNormalisationFunction();

        private async ValueTask<AddressNormalisation> TryCatch(ReturningAddressNormalisationFunction returningAddressNormalisationFunction)
        {
            try
            {
                return await returningAddressNormalisationFunction();
            }
            catch (InvalidAddressNormalisationArgumentException invalidAddressNormalisationArgumentException)
            {
                throw CreateAndLogValidationException(invalidAddressNormalisationArgumentException);
            }
        }

        private AddressNormalisationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressLoadingAuditValidationException =
                new AddressNormalisationValidationException(
                    message: "AddressNormalisation validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditValidationException);

            return addressLoadingAuditValidationException;
        }
    }
}