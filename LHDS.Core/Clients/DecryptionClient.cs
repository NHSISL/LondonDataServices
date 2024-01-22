// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.DecryptClient.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Services.Orchestrations.Decryptions;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class DecryptionClient : IDecryptionClient
    {
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;

        public DecryptionClient(IDecryptionOrchestrationService decryptionOrchestrationService)
        {
            this.decryptionOrchestrationService = decryptionOrchestrationService;
        }

        public async ValueTask<string> DecryptAsync(string fileName)
        {
            try
            {
                return await decryptionOrchestrationService.DecryptAsync(fileName);
            }
            catch (DecryptionOrchestrationValidationException decryptionOrchestrationValidationException)
            {
                throw new DecryptionClientValidationException(
                    decryptionOrchestrationValidationException.InnerException as Xeption);
            }
            catch (DecryptionOrchestrationDependencyValidationException
                decryptionOrchestrationDependencyValidationException)
            {
                throw new DecryptionClientValidationException(
                    decryptionOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (DecryptionOrchestrationDependencyException
                decryptionOrchestrationDependencyException)
            {
                throw new DecryptionClientDependencyException(
                    message: "Decryption client dependency error occurred, contact support.",
                    innerException: decryptionOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException)
            {
                throw new DecryptionClientServiceException(
                    message: "Decryption client service error occurred, fix errors and try again.",
                    innerException: decryptionOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}