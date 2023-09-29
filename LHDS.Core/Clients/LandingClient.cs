// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.LandingClient.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.Downloads;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class LandingClient : ILandingClient
    {
        private readonly IDownloadOrchestrationService downloadOrchestrationService;

        public LandingClient(
            IDownloadOrchestrationService downloadOrchestrationService)
        {
            this.downloadOrchestrationService = downloadOrchestrationService;
        }

        public async ValueTask<List<string>> ProcessAsync()
        {
            try
            {
                return await this.downloadOrchestrationService.ProcessAsync();
            }
            catch (DownloadOrchestrationValidationException downloadOrchestrationValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationValidationException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationDependencyValidationException
                downloadOrchestrationDependencyValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationDependencyException
                downloadOrchestrationDependencyException)
            {
                throw new LandingClientDependencyException(
                    message: "Landing client dependency error occurred, contact support.",
                    innerException: downloadOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationServiceException
                downloadOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    message: "Landing client service error occurred, fix errors and try again.",
                    innerException: downloadOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<string> ProcessAsync(string fileName)
        {
            try
            {
                return await this.downloadOrchestrationService.ProcessAsync(fileName);
            }
            catch (DecryptionOrchestrationValidationException downloadOrchestrationValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationValidationException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationDependencyValidationException
                downloadOrchestrationDependencyValidationException)
            {
                throw new LandingClientValidationException(
                    downloadOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationDependencyException downloadOrchestrationDependencyException)
            {
                throw new LandingClientDependencyException(
                    message: "Landing client dependency error occurred, contact support.",
                    innerException: downloadOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationServiceException downloadOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    message: "Landing client service error occurred, fix errors and try again.",
                    innerException: downloadOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
