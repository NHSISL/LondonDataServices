// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Orchestrations.TerminologyMetadata.Exceptions;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers.Workflows
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerminologyArtifactMetadataController : RESTFulController
    {
        private readonly ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService;

        public LandingsController(ITerminologyMetadataOrchestrationService terminologyMetadataOrchestrationService) =>
            this.terminologyMetadataOrchestrationService = terminologyMetadataOrchestrationService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.TerminologyArtifact, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult> RetrieveArtifactMetadataAsync()
        {
            try
            {
                await this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync();

                return Ok();
            }
            catch (TerminologyMetadataOrchestrationValidationException terminologyMetadataOrchestrationValidationException)
            {
                return BadRequest(terminologyMetadataOrchestrationValidationException.InnerException);
            }
            catch (TerminologyMetadataOrchestrationDependencyException terminologyMetadataOrchestrationDependencyException)
            {
                return InternalServerError(terminologyMetadataOrchestrationDependencyException);
            }
            catch (TerminologyMetadataOrchestrationServiceException terminologyMetadataOrchestrationServiceException)
            {
                return InternalServerError(terminologyMetadataOrchestrationServiceException);
            }
        }
    }
}