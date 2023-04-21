// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.OptOutClient.Exceptions;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
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

        public async ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName)
        {
            try
            {
                await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(optOutFile, fileName);
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

        public async ValueTask PushExpiredOptOutsToMeshForRenewalAsync()
        {
            try
            {
                await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();
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

        public async ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync()
        {
            try
            {
                await this.optOutOrchestrationService.RetrieveUpdatedMeshOptOutStatusChangesAsync();
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
