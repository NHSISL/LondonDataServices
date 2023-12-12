// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.TerminologyClient.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyMedata.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyMetadata.Exceptions;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class TerminologyClient : ITerminologyClient
    {
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;

        public TerminologyClient(ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService)
        {
            this.terminologyMetadataOrchestrationService = terminologyMetadataOrchestrationService;
        }

        public async ValueTask RetrieveArtifactMetadataAsync(string resourceType)
        {
            try
            {
                await this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceType);
            }
            catch (TerminologyMetadataOrchestrationValidationException
                terminologyMetadataOrchestrationValidationException)
            {
                throw new TerminologyClientValidationException(
                    message: "Terminology client validation error occurred, fix errors and try again.",
                    innerException: terminologyMetadataOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TerminologyMetadataOrchestrationDependencyValidationException
                terminologyMetadataOrchestrationDependencyValidationException)
            {
                throw new TerminologyClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException:
                        terminologyMetadataOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TerminologyMetadataOrchestrationDependencyException
                terminologyMetadataOrchestrationDependencyException)
            {
                throw new TerminologyClientDependencyException(
                    message: "Terminology client dependency error occurred, contact support.",
                    innerException: terminologyMetadataOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TerminologyMetadataOrchestrationServiceException
                terminologyMetadataOrchestrationServiceException)
            {
                throw new TerminologyClientServiceException(
                    message: "Terminology client service error occurred, fix errors and try again.",
                    terminologyMetadataOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
