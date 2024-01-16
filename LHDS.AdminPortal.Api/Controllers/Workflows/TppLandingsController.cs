// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using LHDS.Core.Services.Orchestrations.TppLandings;
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
        private readonly ITppLandingOrchestrationService tppLandingOrchestrationService;

        public TppLandingsController(ITppLandingOrchestrationService tppLandingOrchestrationService) =>
            this.tppLandingOrchestrationService = tppLandingOrchestrationService;

        [HttpPut]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.TppLandings")]
#endif
        public async ValueTask<ActionResult<string>> ProcessDocumentByFileNameAsync([FromBody] Document document)
        {
            try
            {
                var returnFilePath = await tppLandingOrchestrationService
                    .ProcessAsync(document);

                return Ok(returnFilePath);
            }
            catch (TppLandingOrchestrationValidationException tppLandingOrchestrationValidationException)
            {
                return BadRequest(tppLandingOrchestrationValidationException.InnerException);
            }
            catch (TppLandingOrchestrationDependencyException tppLandingOrchestrationDependencyException)
            {
                return InternalServerError(tppLandingOrchestrationDependencyException);
            }
            catch (TppLandingOrchestrationServiceException tppLandingOrchestrationServiceException)
            {
                return InternalServerError(tppLandingOrchestrationServiceException);
            }
        }
    }
}