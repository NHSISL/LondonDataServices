// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers.Workflows
{
    [ApiController]
    [Route("api/workflows/[controller]")]
    public class EmisLandingsController : RESTFulController
    {
        private readonly IEmisLandingOrchestrationService emisLandingOrchestrationService;

        public EmisLandingsController(IEmisLandingOrchestrationService emisLandingOrchestrationService) =>
            this.emisLandingOrchestrationService = emisLandingOrchestrationService;

        [HttpGet]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.EmisLandings")]
#endif
        public async ValueTask<ActionResult<List<string>>> ProcessDocuments()
        {
            try
            {
                var returnFilePaths = await emisLandingOrchestrationService.ProcessAsync();

                return Ok(returnFilePaths);
            }
            catch (EmisLandingOrchestrationValidationException emisLandingOrchestrationValidationException)
            {
                return BadRequest(emisLandingOrchestrationValidationException.InnerException);
            }
            catch (EmisLandingOrchestrationDependencyException emisLandingOrchestrationDependencyException)
            {
                return InternalServerError(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException emisLandingOrchestrationServiceException)
            {
                return InternalServerError(emisLandingOrchestrationServiceException);
            }
        }

        [HttpPost]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.EmisLandings")]
#endif
        public async ValueTask<ActionResult<string>> ProcessDocumentByFileNameAsync([FromBody] string fileName)
        {
            try
            {
                var returnFilePath = await emisLandingOrchestrationService
                    .ProcessAsync(fileName);

                return Ok(returnFilePath);
            }
            catch (EmisLandingOrchestrationValidationException emisLandingOrchestrationValidationException)
            {
                return BadRequest(emisLandingOrchestrationValidationException.InnerException);
            }
            catch (EmisLandingOrchestrationDependencyException emisLandingOrchestrationDependencyException)
            {
                return InternalServerError(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException emisLandingOrchestrationServiceException)
            {
                return InternalServerError(emisLandingOrchestrationServiceException);
            }
        }
    }
}