// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.LandingClient.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Services.Coordinations.EmisLandings;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class EmisLandingClient : IEmisLandingClient
    {
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public EmisLandingClient(
            IEmisLandingCoordinationService emisLandingCoordinationService)
        {
            this.emisLandingCoordinationService = emisLandingCoordinationService;
        }

        public async ValueTask<List<string>> ProcessAsync(Guid supplierId)
        {
            try
            {
                return await this.emisLandingCoordinationService.ProcessAsync(supplierId);
            }
            catch (EmisLandingOrchestrationValidationException emisLandingOrchestrationValidationException)
            {
                throw new LandingClientValidationException(
                    emisLandingOrchestrationValidationException.InnerException as Xeption);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw new LandingClientValidationException(
                    emisLandingOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw new LandingClientDependencyException(
                    message: "Landing client dependency error occurred, please contact support.",
                    innerException: emisLandingOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    message: "Landing client service error occurred, fix errors and try again.",
                    innerException: emisLandingOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
