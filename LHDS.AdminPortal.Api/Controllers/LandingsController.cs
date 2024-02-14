// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public LandingsController(IEmisLandingCoordinationService emisLandingCoordinationService) =>
            this.emisLandingCoordinationService = emisLandingCoordinationService;

        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<string>> GetLandingDocumentByFileNameAsync(string fileName)
        {
            try
            {
                var returnFilePath = await this.emisLandingCoordinationService
                    .ProcessAsync(HttpUtility.UrlDecode(fileName));

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