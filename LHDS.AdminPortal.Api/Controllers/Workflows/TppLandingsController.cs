// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.Downloads;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers.Workflows
{
    [ApiController]
    [Route("api/[controller]")]
    public class TppLandingsController : RESTFulController
    {
        private readonly IDownloadOrchestrationService downloadOrchestrationService;

        public TppLandingsController(IDownloadOrchestrationService downloadOrchestrationService) =>
            this.downloadOrchestrationService = downloadOrchestrationService;

        [HttpPut]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.TppLandings")]
#endif
        public async ValueTask<ActionResult<string>> ProcessDocuments()
        {
            try
            {
                var returnFilePath = await downloadOrchestrationService
                    .ProcessAsync();

                return Ok(returnFilePath);
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

        [HttpPut]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.TppLandings")]
#endif
        public async ValueTask<ActionResult<string>> ProcessDocumentByFileNameAsync([FromBody] string fileName)
        {
            try
            {
                var returnFilePath = await downloadOrchestrationService
                    .ProcessAsync(fileName);

                return Ok(returnFilePath);
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