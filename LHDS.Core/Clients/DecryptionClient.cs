// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.DecryptClient.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Services.Orchestrations.Decryptions;
using Xeptions;

namespace LHDS.Core.Clients {
    public class DecryptionClient : IDecryptionClient {
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;

        public DecryptionClient(IDecryptionOrchestrationService decryptionOrchestrationService) {
            this.decryptionOrchestrationService = decryptionOrchestrationService;
        }

        public async ValueTask DecryptAsync(string fileName) {
            try {
                await decryptionOrchestrationService.DecryptAsync(fileName);
            }
            catch (DecryptionOrchestrationValidationException decryptionOrchestrationValidationException) {
                string validationSummary = GetValidationSummary(
                    decryptionOrchestrationValidationException.InnerException.Data);

                throw new DecryptionClientValidationException(
                    decryptionOrchestrationValidationException.InnerException as Xeption, validationSummary);
            }
            catch (DecryptionOrchestrationDependencyValidationException
                decryptionOrchestrationDependencyValidationException) {
                string validationSummary = GetValidationSummary(
                    decryptionOrchestrationDependencyValidationException.InnerException.Data);

                throw new DecryptionClientValidationException(
                    decryptionOrchestrationDependencyValidationException.InnerException as Xeption, validationSummary);
            }
            catch (DecryptionOrchestrationDependencyException
                decryptionOrchestrationDependencyException) {
                throw new DecryptionClientDependencyException(
                    decryptionOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException) {
                throw new DecryptionClientServiceException(
                    decryptionOrchestrationServiceException.InnerException as Xeption);
            }
        }

        private string GetValidationSummary(IDictionary data) {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data) {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}