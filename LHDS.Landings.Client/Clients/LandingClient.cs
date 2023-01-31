// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Clients.LandingClient;
using LHDS.Landings.Client.Services.Orchestrations.Download;
using Xeptions;

namespace LHDS.Landings.Client.Clients
{
    public class LandingClient : ILandingClient
    {
        private readonly IDownloadOrchestrationService downloadOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public LandingClient(
            IDownloadOrchestrationService downloadOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.downloadOrchestrationService = downloadOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask ProcessAsync()
        {
            try
            {
                await this.downloadOrchestrationService.ProcessAsync();
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
                    downloadOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationServiceException
                downloadOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    downloadOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
