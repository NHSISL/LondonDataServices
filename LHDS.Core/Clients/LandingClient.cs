// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async ValueTask ProcessAsync()
        {
            try
            {
                await this.downloadOrchestrationService.ProcessAsync();
            }
            catch (DecryptionOrchestrationValidationException downloadOrchestrationValidationException)
            {
                string validationSummary = GetValidationSummary(
                    downloadOrchestrationValidationException.InnerException.Data);

                throw new LandingClientValidationException(
                    downloadOrchestrationValidationException.InnerException as Xeption, validationSummary);
            }
            catch (DownloadOrchestrationDependencyValidationException
                downloadOrchestrationDependencyValidationException)
            {
                string validationSummary = GetValidationSummary(
                    downloadOrchestrationDependencyValidationException.InnerException.Data);

                throw new LandingClientValidationException(
                    downloadOrchestrationDependencyValidationException.InnerException as Xeption, validationSummary);
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

        public async ValueTask ProcessAsync(string fileName)
        {
            try
            {
                await this.downloadOrchestrationService.ProcessAsync(fileName);
            }
            catch (DecryptionOrchestrationValidationException downloadOrchestrationValidationException)
            {
                string validationSummary = GetValidationSummary(
                    downloadOrchestrationValidationException.InnerException.Data);

                throw new LandingClientValidationException(
                    downloadOrchestrationValidationException.InnerException as Xeption, validationSummary);
            }
            catch (DownloadOrchestrationDependencyValidationException
                downloadOrchestrationDependencyValidationException)
            {
                string validationSummary = GetValidationSummary(
                    downloadOrchestrationDependencyValidationException.InnerException.Data);

                throw new LandingClientValidationException(
                    downloadOrchestrationDependencyValidationException.InnerException as Xeption, validationSummary);
            }
            catch (DownloadOrchestrationDependencyException downloadOrchestrationDependencyException)
            {
                throw new LandingClientDependencyException(
                    downloadOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DownloadOrchestrationServiceException downloadOrchestrationServiceException)
            {
                throw new LandingClientServiceException(
                    downloadOrchestrationServiceException.InnerException as Xeption);
            }
        }

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}
