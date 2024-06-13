// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.TerminologyClient.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions;
using LHDS.Core.Services.Orchestrations.TerminologyDetails;
using LHDS.Core.Services.Orchestrations.TerminologyMetadata;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class TerminologyClient : ITerminologyClient
    {
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;
        private readonly ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService;

        public TerminologyClient(
            ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService,
            ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService)
        {
            this.terminologyMetadataOrchestrationService = terminologyMetadataOrchestrationService;
            this.terminologyDetailOrchestrationService = terminologyDetailOrchestrationService;
        }

        public async ValueTask RetrieveArtifactMetadataAsync(string[] resourceTypes)
        {
            try
            {
                await this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceTypes);
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
                    message: "Terminology client dependency error occurred, please contact support.",
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

        public async ValueTask RetrieveArtifactDetailsAsync()
        {
            try
            {
                await this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();
            }
            catch (TerminologyDetailOrchestrationValidationException
                terminologyDetailOrchestrationValidationException)
            {
                throw new TerminologyClientValidationException(
                    message: "Terminology client validation error occurred, fix errors and try again.",
                    innerException: terminologyDetailOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TerminologyDetailOrchestrationDependencyValidationException
                terminologyDetailOrchestrationDependencyValidationException)
            {
                throw new TerminologyClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException:
                        terminologyDetailOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TerminologyDetailOrchestrationDependencyException
                terminologyDetailOrchestrationDependencyException)
            {
                throw new TerminologyClientDependencyException(
                    message: "Terminology client dependency error occurred, please contact support.",
                    innerException: terminologyDetailOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TerminologyDetailOrchestrationServiceException
                terminologyDetailOrchestrationServiceException)
            {
                throw new TerminologyClientServiceException(
                    message: "Terminology client service error occurred, fix errors and try again.",
                    terminologyDetailOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
