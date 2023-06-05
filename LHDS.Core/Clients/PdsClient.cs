// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.LandingClient.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
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

        public async ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile, string fileName)
        {
            try
            {
                return await this.pdsOrchestrationService.PickupFileAndSendToMesh(pdsFile, fileName);
            }
            catch (PdsOrchestrationValidationException downloadOrchestrationValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyValidationException
                downloadOrchestrationDependencyValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyException
                downloadOrchestrationDependencyException)
            {
                throw new LandingClientDependencyException(
                    downloadOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (PdsOrchestrationServiceException
                downloadOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    downloadOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage()
        {
            try
            {
                return await this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();
            }
            catch (PdsOrchestrationValidationException downloadOrchestrationValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyValidationException
                downloadOrchestrationDependencyValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsOrchestrationDependencyException
                downloadOrchestrationDependencyException)
            {
                throw new LandingClientDependencyException(
                    downloadOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (PdsOrchestrationServiceException
                downloadOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    downloadOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
