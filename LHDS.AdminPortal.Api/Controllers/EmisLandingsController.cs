// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmisLandingsController : RESTFulController
    {
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public EmisLandingsController(IEmisLandingCoordinationService emisLandingCoordinationService) =>
            this.emisLandingCoordinationService = emisLandingCoordinationService;

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Downloads, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<List<string>>> Get(Guid subscriberAgreementId)
        {
            try
            {
                List<string> retrieveFileList =
                    await emisLandingCoordinationService.RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId);

                return Ok(retrieveFileList);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                return InternalServerError(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                return InternalServerError(downloadServiceException);
            }
        }

        [HttpGet("filename/{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<string>> ReLandDocumentByFileNameAsync(string fileName)
        {
            try
            {
                var returnFilePath = await this.emisLandingCoordinationService
                    .ProcessFileAsync(HttpUtility.UrlDecode(fileName));

                return Ok(returnFilePath);
            }
            catch (EmisLandingCoordinationValidationException emisLandingOrchestrationValidationException)
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

        [HttpGet("download/{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Downloads, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<Document>> DownloadDocumentByFileNameAsync(string filename)
        {
            try
            {
                Document retrieveDownload = await emisLandingCoordinationService
                    .RetrieveDownloadByFileNameAsync(filename);

                return Ok(retrieveDownload);
            }
            catch (DownloadValidationException downloadValidationException)
                when (downloadValidationException.InnerException is NotFoundDocumentException)
            {
                return NotFound(downloadValidationException.InnerException);
            }
            catch (DownloadValidationException dataSetValidationException)
            {
                return BadRequest(dataSetValidationException.InnerException);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                return InternalServerError(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                return InternalServerError(downloadServiceException);
            }
        }

        [HttpPut("decrypt")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Downloads, ISL.LDS.AdminApi.ReadOnly")]
#endif
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