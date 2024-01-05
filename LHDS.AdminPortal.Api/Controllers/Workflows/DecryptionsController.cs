// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.Decryptions;
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
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;

        public DecryptionsController(IDecryptionOrchestrationService decryptionOrchestrationService) =>
            this.decryptionOrchestrationService = decryptionOrchestrationService;


        [HttpPut()]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.Decryptions")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> DecryptDocumentAsync(
            [FromBody] string fileName)
        {
            try
            {
                await decryptionOrchestrationService.DecryptAsync(HttpUtility.UrlDecode(fileName));

                return Ok();
            }
            catch (DownloadOrchestrationValidationException downloadOrchestrationValidationException)
            {
                return BadRequest(downloadOrchestrationValidationException.InnerException);
            }
            catch (DownloadOrchestrationDependencyException downloadOrchestrationDependencyException)
            {
                return InternalServerError(downloadOrchestrationDependencyException);
            }
            catch (DownloadOrchestrationServiceException downloadOrchestrationServiceException)
            {
                return InternalServerError(downloadOrchestrationServiceException);
            }
        }

        // TODO: Remove this method in a seperate PR and update UI to use the above method
        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> GetDocumentByFileNameToDecryptAsync(string fileName)
        {
            try
            {
                await decryptionOrchestrationService.DecryptAsync(HttpUtility.UrlDecode(fileName));

                return Ok();
            }
            catch (DownloadOrchestrationValidationException downloadOrchestrationValidationException)
            {
                return BadRequest(downloadOrchestrationValidationException.InnerException);
            }
            catch (DownloadOrchestrationDependencyException downloadOrchestrationDependencyException)
            {
                return InternalServerError(downloadOrchestrationDependencyException);
            }
            catch (DownloadOrchestrationServiceException downloadOrchestrationServiceException)
            {
                return InternalServerError(downloadOrchestrationServiceException);
            }
        }
    }
}