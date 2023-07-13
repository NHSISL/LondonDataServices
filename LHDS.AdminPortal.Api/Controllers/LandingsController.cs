// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.Downloads;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandingsController : RESTFulController
    {
        private readonly IDownloadOrchestrationService downloadOrchestrationService;

        public LandingsController(IDownloadOrchestrationService downloadOrchestrationService) =>
            this.downloadOrchestrationService = downloadOrchestrationService;

        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhds.Api.IngestionTracking, lhdsApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> GetLandingDocumentByFileNameAsync(string fileName)
        {
            try
            {
                await this.downloadOrchestrationService.ProcessAsync(fileName);

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