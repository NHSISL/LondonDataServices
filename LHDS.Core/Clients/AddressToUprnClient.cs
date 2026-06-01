// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.AddressToUprnClient.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressToUprns;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class AddressToUprnClient : IAddressToUprnClient
    {
        private readonly IAddressToUprnOrchestrationService addressToUprnOrchestrationService;

        public AddressToUprnClient(IAddressToUprnOrchestrationService addressToUprnOrchestrationService)
        {
            this.addressToUprnOrchestrationService = addressToUprnOrchestrationService;
        }

        public async ValueTask MatchAddressToUprnAsync(
            Stream data,
            string filename,
            Guid correlationId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await this.addressToUprnOrchestrationService
                    .MatchAddressToUprnAsync(data, filename, correlationId, cancellationToken);
            }
            catch (AddressToUprnOrchestrationValidationException addressToUprnOrchestrationValidationException)
            {
                throw new AddressToUprnClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressToUprnOrchestrationValidationException.InnerException as Xeption);
            }
            catch (AddressToUprnOrchestrationDependencyValidationException
                addressToUprnOrchestrationDependencyValidationException)
            {
                throw new AddressToUprnClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressToUprnOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (AddressToUprnOrchestrationDependencyException
                addressToUprnOrchestrationDependencyException)
            {
                throw new AddressToUprnClientDependencyException(
                    message: "Address client dependency error occurred, please contact support.",
                    innerException: addressToUprnOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (AddressToUprnOrchestrationServiceException
                addressToUprnOrchestrationServiceException)
            {
                throw new AddressToUprnClientServiceException(
                    message: "Address client service error occurred, fix errors and try again.",
                    addressToUprnOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
