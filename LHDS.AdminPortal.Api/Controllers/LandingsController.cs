// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
using System.Web;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.Downloads;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

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
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<string>> GetLandingDocumentByFileNameAsync(string fileName)
        {
            try
            {
                var returnFilePath = await this.downloadOrchestrationService.ProcessAsync(HttpUtility.UrlDecode(fileName));
                string jsonContent = JsonConvert.SerializeObject(new { path = returnFilePath });
                return jsonContent;
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