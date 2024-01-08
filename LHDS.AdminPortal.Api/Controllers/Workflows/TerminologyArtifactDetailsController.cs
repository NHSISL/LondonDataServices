// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions;
using LHDS.Core.Services.Orchestrations.TerminologyDetails;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/workflow/[controller]")]
    public class TerminologyArtifactDetailsController : RESTFulController
    {
        private readonly ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService;

        public TerminologyArtifactDetailsController(ITerminologyDetailOrchestrationService 
                terminologyDetailOrchestrationService) =>
            this.terminologyDetailOrchestrationService = terminologyDetailOrchestrationService;

        [HttpPut]
#if RELEASE
            [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult> RetrieveArtifactDetailsAsync()
        {
            try
            {
                await this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

                return Ok();
            }
            catch (TerminologyDetailOrchestrationDependencyException downloadOrchestrationValidationException)
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