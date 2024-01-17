// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Controllers.OptOuts;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Services.Orchestrations.OptOuts;
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
        [Route("api/workflows/[controller]/file")]
        public async ValueTask<ActionResult<string>> RetrieveOptOutStatusAsync(
            [FromBody] OptOutFileModel optOutFileModel)
        {
            try
            {
                string optOutStatus = await this.optOutOrchestrationService.RetrieveOptOutStatusAsync(
                    optOutFileModel.OptOutFile,
                    optOutFileModel.FileName);

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
        [Route("api/workflows/[controller]/PushExpiredOptOuts")]
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

        [HttpPost]
        [Route("api/workflows/[controller]/ProcessUpdatedMeshConsentStatuses")]
        public async ValueTask<ActionResult<List<MeshMessage>>> ProcessUpdatedMeshConsentStatuses()
        {
            try
            {
                List<MeshMessage> retrievedMeshMessages =
                    await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

                return Ok(retrievedMeshMessages);
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