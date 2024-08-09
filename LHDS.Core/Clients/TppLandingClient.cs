// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.TppLandingClient.Exceptions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using LHDS.Core.Services.Coordinations.TppLandings;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class TppLandingClient : ITppLandingClient
    {
        private readonly ITppLandingCoordinationService tppLandingCoordinationService;

        public TppLandingClient(
            ITppLandingCoordinationService tppLandingCoordinationService)
        {
            this.tppLandingCoordinationService = tppLandingCoordinationService;
        }

        public async ValueTask<Guid> ProcessAsync(Stream input, string fileName, Guid supplierId)
        {
            try
            {
                return await this.tppLandingCoordinationService.ProcessAsync(input, fileName, supplierId);
            }
            catch (TppLandingOrchestrationValidationException tppOrchestrationValidationException)
            {
                throw new TppLandingClientValidationException(
                    message: "TPP landing client dependency validation error occurred, please contact support.",
                    tppOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TppLandingOrchestrationDependencyValidationException tppOrchestrationDependencyValidationException)
            {
                throw new TppLandingClientValidationException(
                    message: "TPP landing client dependency validation error occurred, please contact support.",
                    innerException: tppOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TppLandingOrchestrationDependencyException tppOrchestrationDependencyException)
            {
                throw new TppLandingClientDependencyException(
                    message: "TPP landing client dependency error occurred, please contact support.",
                    innerException: tppOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TppLandingOrchestrationServiceException tppOrchestrationServiceException)
            {
                throw new TppLandingClientServiceException(
                    message: "TPP landing client service error occurred, fix errors and try again.",
                    innerException: tppOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
