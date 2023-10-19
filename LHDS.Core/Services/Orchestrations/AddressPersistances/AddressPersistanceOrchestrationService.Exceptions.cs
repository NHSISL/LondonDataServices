// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisation.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressPersistanceOrchestrationException
                invalidArgumentAddressPersistanceOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressPersistanceOrchestrationException);
            }
            catch (AddressNormalisationProcessingValidationException addressNormalisationProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationProcessingValidationException);
            }
            catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
            }
            catch (AddressProcessingValidationException addressProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressProcessingValidationException);
            }
            catch (AddressProcessingDependencyValidationException addressProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressProcessingDependencyValidationException);
            }
            catch (AddressLoadingAuditValidationException addressLoadingAuditValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressLoadingAuditValidationException);
            }
            catch (AddressLoadingAuditDependencyValidationException addressLoadingAuditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressLoadingAuditDependencyValidationException);
            }
        }
        private AddressPersistanceOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationValidationException =
                new AddressPersistanceOrchestrationValidationException(
                    message: "Address persistance orchestration validation errors occured, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(addressPersistanceOrchestrationValidationException);

            return addressPersistanceOrchestrationValidationException;
        }

        private AddressPersistanceOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationDependencyValidationException =
                new AddressPersistanceOrchestrationDependencyValidationException(
                    message: "Address persistance orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressPersistanceOrchestrationDependencyValidationException);

            return addressPersistanceOrchestrationDependencyValidationException;
        }
    }
}
