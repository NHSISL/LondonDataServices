// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService
    {
        private delegate ValueTask<SecureData> ReturningSecureDataFunction();

        private async ValueTask<SecureData> TryCatch(ReturningSecureDataFunction returningSecureDataFunction)
        {
            try
            {
                return await returningSecureDataFunction();
            }
            catch (NullSecureDataException nullSecureDataException)
            {
                throw CreateAndLogValidationException(nullSecureDataException);
            }
        }

        private SecureDataValidationException CreateAndLogValidationException(Xeption exception)
        {
            var secureDataValidationException = new SecureDataValidationException(
                message: "Secure data validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataValidationException);

            return secureDataValidationException;
        }
    }
}