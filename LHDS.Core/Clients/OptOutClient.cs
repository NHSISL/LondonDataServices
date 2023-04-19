// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.LandingClient.Exceptions;
using LHDS.Core.Models.Clients.OptOutClient.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Services.Orchestrations.Downloads;
using LHDS.Core.Services.Orchestrations.OptOuts;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class OptOutClient : IOptOutClient
    {
        private readonly IOptOutOrchestrationService optOutOrchestrationService;

        public OptOutClient(
            IOptOutOrchestrationService optOutOrchestrationService)
        {
            this.optOutOrchestrationService = optOutOrchestrationService;
        }

        public async ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string requestId)
        {
            try
            {
                await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(optOutFile, requestId);
            }
            catch (OptOutOrchestrationValidationException optOutOrchestrationValidationException)
            {
                throw new OptOutClientValidationException(
                    optOutOrchestrationValidationException.InnerException as Xeption);
            }
            catch (OptOutOrchestrationDependencyValidationException
                optOutOrchestrationDependencyValidationException)
            {
                throw new OptOutClientValidationException(
                    optOutOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (OptOutOrchestrationDependencyException
                optOutOrchestrationDependencyException)
            {
                throw new OptOutClientDependencyException(
                    optOutOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (OptOutOrchestrationServiceException
                optOutOrchestrationServiceException)
            {
                throw new OptOutClientServiceException(
                    optOutOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
