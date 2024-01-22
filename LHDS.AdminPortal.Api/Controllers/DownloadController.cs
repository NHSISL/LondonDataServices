// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Services.Foundations.Downloads;
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
        private readonly IDownloadService downloadService;

        public DownloadsController(IDownloadService downloadService) =>
            this.downloadService = downloadService;

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
        public async ValueTask<ActionResult<List<Document>>> Get()
        {
            try
            {
                List<Document> retrievedDocuments =
                    await downloadService.RetrieveListOfDocumentsToProcessAsync();

                return Ok(retrievedDocuments);
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
        public async ValueTask<ActionResult<Document>> RetrieveDownloadByFileNameAsync(string fileName)
        {
            try
            {
                Document document = await downloadService.RetrieveDownloadByFileNameAsync(fileName);

                return Ok(document);
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