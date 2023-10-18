// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Processings.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
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
            catch (AddressLoadingAuditValidationException addressLoadingAuditProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressLoadingAuditProcessingValidationException);
            }
            catch (AddressLoadingAuditDependencyValidationException
                addressLoadingAuditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressLoadingAuditDependencyValidationException);
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

        private AddressLoadingAuditProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var addressLoadingAuditProcessingDependencyValidationException =
                new AddressLoadingAuditProcessingDependencyValidationException(
                    message: "Address loading audit processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressLoadingAuditProcessingDependencyValidationException);

            return addressLoadingAuditProcessingDependencyValidationException;
        }
    }
}