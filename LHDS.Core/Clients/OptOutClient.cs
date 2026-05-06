// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        public async ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.optOutOrchestrationService.ValidateMailboxAccessAsync(cancellationToken);
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

        public async ValueTask<string> RetrieveOptOutStatusAsync(
            Stream input,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.optOutOrchestrationService
                    .RetrieveOptOutStatusAsync(input, fileName, cancellationToken);
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

        public async ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.optOutOrchestrationService
                    .PushExpiredOptOutsToMeshForRenewalAsync(cancellationToken);
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

        public async ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.optOutOrchestrationService
                    .RetrieveUpdatedMeshConsentStatusesChangesAsync(cancellationToken);
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
