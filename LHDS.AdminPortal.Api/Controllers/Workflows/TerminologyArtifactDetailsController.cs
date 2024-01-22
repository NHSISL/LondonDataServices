// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
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

        [HttpPost]
#if RELEASE
            [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult> ProcessArtifactDetailsAsync()
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