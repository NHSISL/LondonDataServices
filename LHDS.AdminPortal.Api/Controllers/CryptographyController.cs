// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Providers.Cryptography;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptographyController : RESTFulController
    {
        private readonly ICryptographyProvider cryptographyProvider;

        public CryptographyController(ICryptographyProvider cryptographyProvider) =>
            this.cryptographyProvider = cryptographyProvider;

        [HttpPost]
        [Route("api/[controller]/encrypt")]
        public async ValueTask<ActionResult<byte[]>> PostCryptographEncryptyAsync([FromBody] byte[] data)
        {
            try
            {
                byte[] encryptedData =
                    await this.cryptographyProvider.EncryptAsync(data);

                return Created(encryptedData);
            }
            catch (CryptographyValidationException cryptographyValidationException)
            {
                return BadRequest(cryptographyValidationException.InnerException);
            }
            catch (CryptographyDependencyValidationException cryptographyValidationException)
            {
                return FailedDependency(cryptographyValidationException.InnerException);
            }
            catch (CryptographyDependencyException cryptographyDependencyException)
            {
                return InternalServerError(cryptographyDependencyException);
            }
            catch (CryptographyServiceException cryptographyServiceException)
            {
                return InternalServerError(cryptographyServiceException);
            }
        }

        [HttpPost]
        [Route("api/[controller]/decrypt")]
        public async ValueTask<ActionResult<byte[]>> PostCryptographyDecryptAsync([FromBody] byte[] data)
        {
            try
            {
                byte[] encryptedData =
                    await this.cryptographyProvider.DecryptAsync(data);

                return Created(encryptedData);
            }
            catch (CryptographyValidationException cryptographyValidationException)
            {
                return BadRequest(cryptographyValidationException.InnerException);
            }
            catch (CryptographyDependencyValidationException cryptographyValidationException)
            {
                return FailedDependency(cryptographyValidationException.InnerException);
            }
            catch (CryptographyDependencyException cryptographyDependencyException)
            {
                return InternalServerError(cryptographyDependencyException);
            }
            catch (CryptographyServiceException cryptographyServiceException)
            {
                return InternalServerError(cryptographyServiceException);
            }
        }
    }
}