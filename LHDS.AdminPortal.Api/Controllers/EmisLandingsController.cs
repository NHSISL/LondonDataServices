// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Attrify.Attributes;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Services.Coordinations.EmisLandings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.EmisLanding,ISL.LDS.AdminSpa.Administrators")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmisLandingsController : RESTFulController
    {
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public EmisLandingsController(IEmisLandingCoordinationService emisLandingCoordinationService) =>
            this.emisLandingCoordinationService = emisLandingCoordinationService;

        [HttpGet("{subscriberAgreementId}")]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [InvisibleApi]
        public async ValueTask<ActionResult<List<string>>> Get(Guid subscriberAgreementId)
        {
            try
            {
                List<string> retrieveFileList =
                    await emisLandingCoordinationService.RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId);

                return Ok(retrieveFileList);
            }
            catch (InvalidArgumentEmisLandingCoordinationException ex)
            {
                return BadRequest(ex);
            }
            catch (EmisLandingCoordinationValidationException ex)
            {
                return BadRequest(ex);
            }
            catch (EmisLandingCoordinationDependencyValidationException ex)
            {
                return FailedDependency(ex);
            }
            catch (EmisLandingCoordinationDependencyException ex)
            {
                return InternalServerError(ex);
            }
            catch (EmisLandingCoordinationServiceException ex)
            {
                return InternalServerError(ex);
            }
            catch (FailedEmisLandingCoordinationServiceException ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.EmisLanding,ISL.LDS.AdminSpa.Administrators")]
        [HttpPut("decrypt/{ingestionTrackingId}")]
        public async ValueTask<ActionResult> RedecryptDocumentByIngestionTrackingIdAsync(Guid ingestionTrackingId)
        {
            try
            {
                await emisLandingCoordinationService
                    .RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

                return Ok();
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }
    }
}