// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadsController : RESTFulController
    {
        private readonly IEmisLandingCoordinationService emisLandingCoordinationService;

        public DownloadsController(IEmisLandingCoordinationService emisLandingCoordinationService) =>
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

        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Downloads, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<Document>> RetrieveDownloadByFileNameAsync(string filename)
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
    }
}