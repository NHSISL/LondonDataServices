// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Clients;
using LHDS.Core.Models.Clients.DecryptionClient;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Services.Orchestrations.Decryptions;
using Xeptions;

namespace LHDS.Decryptions.Client.Clients
{
    public class DecryptionClient : IDecryptionClient
    {
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;

        public DecryptionClient(IDecryptionOrchestrationService decryptionOrchestrationService)
        {
            this.decryptionOrchestrationService = decryptionOrchestrationService;
        }

        public async ValueTask DecryptAsync(string fileName)
        {
            try
            {
                await this.decryptionOrchestrationService.DecryptAsync(fileName);
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
                    decryptionOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException)
            {
                throw new DecryptionClientServiceException(
                    decryptionOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}