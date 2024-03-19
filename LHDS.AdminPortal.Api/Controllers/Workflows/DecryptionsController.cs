// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using LHDS.Core.Services.Coordinations.Decryptions;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers.Workflows
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecryptionsController : RESTFulController
    {
        private readonly IDecryptionCoordinationService decryptionCoordinationService;

        public DecryptionsController(IDecryptionCoordinationService decryptionCoordinationService) =>
            this.decryptionCoordinationService = decryptionCoordinationService;

        [HttpPut()]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Decryptions")]
#endif
        public async ValueTask<ActionResult<string>> DecryptDocumentAsync(
            [FromBody] string fileName)
        {
            try
            {
                string decryptedItem =
                    await decryptionCoordinationService.DecryptAsync(fileName);

                return Ok(decryptedItem);
            }
            catch (DecryptionCoordinationDependencyValidationException
                decryptionCoordinationDependencyValidationException)
            {
                return BadRequest(decryptionCoordinationDependencyValidationException.InnerException);
            }
            catch (DecryptionCoordinationDependencyException decryptionCoordinationDependencyException)
            {
                return InternalServerError(decryptionCoordinationDependencyException);
            }
            catch (FailedDecryptionCoordinationServiceException failedDecryptionCoordinationServiceException)
            {
                return InternalServerError(failedDecryptionCoordinationServiceException);
            }
        }
    }
}