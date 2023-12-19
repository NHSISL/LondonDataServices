// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.TppLandingClient.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Orchestrations.Tpp.Exceptions;
using LHDS.Core.Services.Orchestrations.Tpp;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class TppLandingClient : ITppLandingClient
    {
        private readonly ITppOrchestrationService tppOrchestrationService;

        public TppLandingClient(
            ITppOrchestrationService tppOrchestrationService)
        {
            this.tppOrchestrationService = tppOrchestrationService;
        }

        public async ValueTask<Guid> ProcessAsync(Document fileName)
        {
            try
            {
                return await this.tppOrchestrationService.ProcessAsync(fileName);
            }
            catch (TppOrchestrationValidationException tppOrchestrationValidationException)
            {
                throw new TppLandingClientValidationException(
                    tppOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TppOrchestrationDependencyValidationException tppOrchestrationDependencyValidationException)
            {
                throw new TppLandingClientValidationException(
                    innerException: tppOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TppOrchestrationDependencyException tppOrchestrationDependencyException)
            {
                throw new TppLandingClientDependencyException(
                    message: "TPP Landing client dependency error occurred, contact support.",
                    innerException: tppOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TppOrchestrationServiceException tppOrchestrationServiceException)
            {
                throw new TppLandingClientServiceException(
                    message: "TPP Landing client service error occurred, fix errors and try again.",
                    innerException: tppOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
