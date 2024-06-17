// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.DecryptClient.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Services.Coordinations.Decryptions;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class DecryptionClient : IDecryptionClient
    {
        private readonly IDecryptionCoordinationService decryptionCoordinationService;

        public DecryptionClient(IDecryptionCoordinationService decryptionCoordinationService)
        {
            this.decryptionCoordinationService = decryptionCoordinationService;
        }

        public async ValueTask<string> DecryptAsync(string encryptedFileName)
        {
            try
            {
                return await decryptionCoordinationService.DecryptAsync(encryptedFileName);
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
                    message: "Decryption client dependency error occurred, please contact support.",
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

        public async ValueTask RetryDecryptOnAllAsync()
        {
            try
            {
                await decryptionCoordinationService.RetryDecryptOnAllAsync();
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
                    message: "Decryption client dependency error occurred, please contact support.",
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