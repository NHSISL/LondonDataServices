// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Services.Foundations.Cryptographies;
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
        private readonly ICryptographyService cryptographyService;

        public CryptographyController(ICryptographyService cryptographyService) =>
            this.cryptographyService = cryptographyService;

        [HttpPost]
        [Route("encrypt")]
        public async ValueTask<ActionResult<byte[]>> PostCryptographEncryptyAsync([FromBody] byte[] data)
        {
            try
            {
                byte[] encryptedData =
                    await this.cryptographyService.EncryptAsync(data);

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
        [Route("decrypt")]
        public async ValueTask<ActionResult<byte[]>> PostCryptographyDecryptAsync([FromBody] byte[] data)
        {
            try
            {
                byte[] encryptedData =
                    await this.cryptographyService.DecryptAsync(data);

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