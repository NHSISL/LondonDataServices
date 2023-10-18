// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingService
    {
        private delegate ValueTask<AddressLoadingAudit> ReturningAddressLoadingAuditFunction();

        private async ValueTask<AddressLoadingAudit>
            TryCatch(ReturningAddressLoadingAuditFunction returningAddressLoadingAuditFunction)
        {
            try
            {
                return await returningAddressLoadingAuditFunction();
            }
            catch (NullAddressLoadingAuditException nullAddressLoadingAuditException)
            {
                throw CreateAndLogValidationException(nullAddressLoadingAuditException);
            }
            catch (InvalidAddressLoadingAuditException invalidAddressLoadingAuditException)
            {
                throw CreateAndLogValidationException(invalidAddressLoadingAuditException);
            }
        }

        private AddressLoadingAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "Address loading audit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditValidationException);

            return addressLoadingAuditValidationException;
        }
    }
}