// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
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
        public async ValueTask<ActionResult<IngestionTracking>> DecryptDocumentAsync(
            [FromBody] string fileName)
        {
            try
            {
                await decryptionCoordinationService.DecryptAsync(fileName);

                return Ok();
            }
            catch (EmisLandingOrchestrationValidationException emisLandingOrchestrationValidationException)
            {
                return BadRequest(emisLandingOrchestrationValidationException.InnerException);
            }
            catch (EmisLandingOrchestrationDependencyException emisLandingOrchestrationDependencyException)
            {
                return InternalServerError(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException emisLandingOrchestrationServiceException)
            {
                return InternalServerError(emisLandingOrchestrationServiceException);
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
                await decryptionCoordinationService.DecryptAsync(HttpUtility.UrlDecode(fileName));

                return Ok();
            }
            catch (EmisLandingOrchestrationValidationException emisLandingOrchestrationValidationException)
            {
                return BadRequest(emisLandingOrchestrationValidationException.InnerException);
            }
            catch (EmisLandingOrchestrationDependencyException emisLandingOrchestrationDependencyException)
            {
                return InternalServerError(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException emisLandingOrchestrationServiceException)
            {
                return InternalServerError(emisLandingOrchestrationServiceException);
            }
        }
    }
}