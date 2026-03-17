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
    [Authorize(Roles = "ISL.LDS.AdminApi.EmisLanding,ISL.LDS.AdminApi.Administrators")]
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
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                return BadRequest(invalidArgumentEmisLandingCoordinationException);
            }
            catch (EmisLandingCoordinationValidationException emisLandingCoordinationValidationException)
            {
                return BadRequest(emisLandingCoordinationValidationException);
            }
            catch (EmisLandingCoordinationDependencyValidationException 
                    emisLandingCoordinationDependencyValidationException)
            {
                return FailedDependency(emisLandingCoordinationDependencyValidationException);
            }
            catch (EmisLandingCoordinationDependencyException emisLandingCoordinationDependencyException)
            {
                return InternalServerError(emisLandingCoordinationDependencyException);
            }
            catch (EmisLandingCoordinationServiceException emisLandingCoordinationServiceException)
            {
                return InternalServerError(emisLandingCoordinationServiceException);
            }
            catch (FailedEmisLandingCoordinationServiceException failedEmisLandingCoordinationServiceException)
            {
                return InternalServerError(failedEmisLandingCoordinationServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminApi.EmisLanding,ISL.LDS.AdminApi.Administrators")]
        [HttpPut("decrypt/{ingestionTrackingId}")]
        public async ValueTask<ActionResult> RedecryptDocumentByIngestionTrackingIdAsync(Guid ingestionTrackingId)
        {
            try
            {
                await emisLandingCoordinationService
                    .RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

                return Ok();
            }
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                return BadRequest(invalidArgumentEmisLandingCoordinationException);
            }
            catch (EmisLandingCoordinationValidationException emisLandingCoordinationValidationException)
            {
                return BadRequest(emisLandingCoordinationValidationException);
            }
            catch (EmisLandingCoordinationDependencyValidationException 
                    emisLandingCoordinationDependencyValidationException)
            {
                return FailedDependency(emisLandingCoordinationDependencyValidationException);
            }
            catch (EmisLandingCoordinationDependencyException emisLandingCoordinationDependencyException)
            {
                return InternalServerError(emisLandingCoordinationDependencyException);
            }
            catch (EmisLandingCoordinationServiceException emisLandingCoordinationServiceException)
            {
                return InternalServerError(emisLandingCoordinationServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminApi.EmisLanding,ISL.LDS.AdminApi.Administrators")]
        [HttpGet("filename/{*fileName}")]
        public async ValueTask<ActionResult<string>> ReLandDocumentByFileNameAsync(string fileName)
        {
            try
            {
                string decryptedFileName =
                    await emisLandingCoordinationService.ReLandDocumentByFileNameAsync(fileName);

                return Ok(decryptedFileName);
            }
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                return BadRequest(invalidArgumentEmisLandingCoordinationException);
            }
            catch (EmisLandingCoordinationValidationException emisLandingCoordinationValidationException)
            {
                return BadRequest(emisLandingCoordinationValidationException);
            }
            catch (EmisLandingCoordinationDependencyValidationException
                    emisLandingCoordinationDependencyValidationException)
            {
                return FailedDependency(emisLandingCoordinationDependencyValidationException);
            }
            catch (EmisLandingCoordinationDependencyException emisLandingCoordinationDependencyException)
            {
                return InternalServerError(emisLandingCoordinationDependencyException);
            }
            catch (EmisLandingCoordinationServiceException emisLandingCoordinationServiceException)
            {
                return InternalServerError(emisLandingCoordinationServiceException);
            }
            catch (FailedEmisLandingCoordinationServiceException failedEmisLandingCoordinationServiceException)
            {
                return InternalServerError(failedEmisLandingCoordinationServiceException);
            }
        }
    }
}