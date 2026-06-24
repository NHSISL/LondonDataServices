// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.PdsClient.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using LHDS.Core.Services.Orchestrations.Pds;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class PdsClient : IPdsClient
    {
        private readonly IPdsOrchestrationService pdsOrchestrationService;

        public PdsClient(IPdsOrchestrationService pdsOrchestrationService)
        {
            this.pdsOrchestrationService = pdsOrchestrationService;
        }

        public async ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.pdsOrchestrationService.ValidateMailboxAccessAsync(cancellationToken);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw new PdsClientValidationException(
                    pdsOrchestrationValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyValidationException
                pdsOrchestrationDependencyValidationException)
            {
                throw new PdsClientValidationException(
                    pdsOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyException
                pdsOrchestrationDependencyException)
            {
                throw new PdsClientDependencyException(
                    pdsOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (PdsOrchestrationServiceException
                pdsOrchestrationServiceException)
            {
                throw new PdsClientServiceException(
                    pdsOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<PdsAudit> PickupFileAndSendToMesh
            (Stream pdsStream,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.pdsOrchestrationService
                    .PickupFileAndSendToMesh(pdsStream, fileName, cancellationToken);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw new PdsClientValidationException(
                    pdsOrchestrationValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyValidationException
                pdsOrchestrationDependencyValidationException)
            {
                throw new PdsClientValidationException(
                    pdsOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyException
                pdsOrchestrationDependencyException)
            {
                throw new PdsClientDependencyException(
                    pdsOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (PdsOrchestrationServiceException
                pdsOrchestrationServiceException)
            {
                throw new PdsClientServiceException(
                    pdsOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.pdsOrchestrationService
                    .RetreiveMessagesFromMeshAndUpdateStorage(cancellationToken);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw new PdsClientValidationException(
                    pdsOrchestrationValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyValidationException
                pdsOrchestrationDependencyValidationException)
            {
                throw new PdsClientValidationException(
                    pdsOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyException
                pdsOrchestrationDependencyException)
            {
                throw new PdsClientDependencyException(
                    pdsOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (PdsOrchestrationServiceException
                pdsOrchestrationServiceException)
            {
                throw new PdsClientServiceException(
                    pdsOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
