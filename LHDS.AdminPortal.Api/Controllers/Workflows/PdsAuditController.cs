// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Models.Controllers.Workflows.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using LHDS.Core.Services.Orchestrations.Pds;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers.Workflows
{
    [ApiController]
    [Route("api/workflows/[controller]")]
    public class PdsAuditController : RESTFulController
    {
        private readonly IPdsOrchestrationService pdsOrchestrationService;

        public PdsAuditController(IPdsOrchestrationService pdsOrchestrationService) =>
            this.pdsOrchestrationService = pdsOrchestrationService;

        [HttpGet]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Pds")]
#endif
        public async ValueTask<ActionResult<bool>> ValidateMailboxAccessAsync()
        {
            try
            {
                return await this.pdsOrchestrationService.ValidateMailboxAccessAsync();
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                return BadRequest(pdsOrchestrationValidationException.InnerException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                return InternalServerError(pdsOrchestrationDependencyValidationException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                return InternalServerError(pdsOrchestrationServiceException);
            }
        }

        [HttpPost]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Pds")]
#endif
        [Route("api/workflows/[controller]/file")]
        public async ValueTask<ActionResult<PdsAudit>> PickupFileAndSendToMesh([FromBody] MeshFileModel meshFileModel)
        {
            try
            {
                return await this.pdsOrchestrationService.PickupFileAndSendToMesh(
                    meshFileModel.PdsFile,
                    meshFileModel.FileName);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                return BadRequest(pdsOrchestrationValidationException.InnerException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                return InternalServerError(pdsOrchestrationDependencyValidationException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                return InternalServerError(pdsOrchestrationServiceException);
            }
        }

        [HttpPost]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Pds")]
#endif
        public async ValueTask<ActionResult<List<PdsAudit>>> ProcessMeshMessages()
        {
            try
            {
                return await this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                return BadRequest(pdsOrchestrationValidationException.InnerException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                return InternalServerError(pdsOrchestrationDependencyValidationException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                return InternalServerError(pdsOrchestrationServiceException);
            }
        }
    }
}