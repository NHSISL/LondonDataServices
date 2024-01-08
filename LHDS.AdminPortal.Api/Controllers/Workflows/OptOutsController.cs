// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions;
using LHDS.Core.Services.Orchestrations.OptOuts;
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
    public class OptOutController : RESTFulController
    {
        private readonly IOptOutOrchestrationService optOutOrchestrationService;

        public OptOutController(IOptOutOrchestrationService optOutOrchestrationService) =>
            this.optOutOrchestrationService = optOutOrchestrationService;

        [HttpGet]
#if RELEASE
            [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<bool>> ValidateMailboxAccessAsync()
        {
            try
            {
                bool validated = await this.optOutOrchestrationService.ValidateMailboxAccessAsync();

                return Ok(validated);
            }
            catch (OptOutOrchestrationValidationException optOutOrchestrationValidationException)
            {
                return BadRequest(optOutOrchestrationValidationException.InnerException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                return InternalServerError(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                return InternalServerError(optOutOrchestrationServiceException);
            }
        }

        [HttpGet]
        public async ValueTask<ActionResult<string>> RetrieveOptOutStatusAsync([FromBody] byte[] optOutFile, 
            [FromBody] string fileName)
        {
            try
            {
                string optOutStatus = await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(
                    optOutFile, 
                    fileName);

                return Ok(optOutStatus);
            }
            catch (OptOutOrchestrationValidationException optOutOrchestrationValidationException)
            {
                return BadRequest(optOutOrchestrationValidationException.InnerException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                return InternalServerError(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                return InternalServerError(optOutOrchestrationServiceException);
            }
        }

        [HttpPost]
        public async ValueTask<ActionResult<MeshMessage>> PushExpiredOptOutsToMeshForRenewalAsync()
        {
            try
            {
                MeshMessage meshMessage = 
                    await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

                return Ok(meshMessage);
            }
            catch (OptOutOrchestrationValidationException optOutOrchestrationValidationException)
            {
                return BadRequest(optOutOrchestrationValidationException.InnerException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                return InternalServerError(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                return InternalServerError(optOutOrchestrationServiceException);
            }
        }
    }
}