// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.OptOutClient.Exceptions;
using LHDS.Core.Models.Foundations.Mesh;
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

        public async ValueTask<bool> ValidateMailboxAccessAsync()
        {
            try
            {
                return await this.optOutOrchestrationService.ValidateMailboxAccessAsync();
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

        public async ValueTask<string> RetrieveOptOutStatusAsync(Stream input, string fileName)
        {
            try
            {
                return await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(input, fileName);
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

        public async ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync()
        {
            try
            {
                return await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();
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

        public async ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync()
        {
            try
            {
                return await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();
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
