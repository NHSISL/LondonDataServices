// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Orchestrations.TerminologyDetail.Exceptions;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers.Workflows
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerminologyArtifactDetailsController : RESTFulController
    {
        private readonly ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService;

        public TerminologyArtifactDetailsController(ITerminologyDetailOrchestrationService terminologyDetailOrchestrationService) =>
            this.terminologyDetailOrchestrationService = terminologyDetailOrchestrationService;

        [HttpPut]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.TerminologyArtifacts, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult> RetrieveArtifactDetailsAsync()
        {
            try
            {
                await this.terminologyDetailOrchestrationService.RetrieveArtifactDetailsAsync();

                return Ok();
            }
            catch (TerminologyDetailOrchestrationValidationException terminologyDetailOrchestrationValidationException)
            {
                return BadRequest(terminologyDetailOrchestrationValidationException.InnerException);
            }
            catch (TerminologyDetailOrchestrationDependencyException terminologyDetailOrchestrationDependencyException)
            {
                return InternalServerError(terminologyDetailOrchestrationDependencyException);
            }
            catch (TerminologyDetailOrchestrationServiceException terminologyDetailOrchestrationServiceException)
            {
                return InternalServerError(terminologyDetailOrchestrationServiceException);
            }
        }
    }
}