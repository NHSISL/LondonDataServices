// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Services.Foundations.Documents;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : RESTFulController
    {
        private readonly IDocumentService documentService;
        private readonly BlobContainers blobContainers;

        public DocumentsController(IDocumentService documentService, BlobContainers blobContainers)
        {
            this.documentService = documentService;
            this.blobContainers = blobContainers;
        }

        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<Document>> GetDownloadLinkAsync(string fileName)
        {
            try
            {
                string document = await this.documentService
                    .GetDownloadLinkAsync(WebUtility.UrlDecode(fileName), blobContainers.EmisLanding);

                return Ok(document);
            }
            catch (DocumentValidationException documentValidationException)
                when (documentValidationException.InnerException is NotFoundDocumentException)
            {
                return NotFound(documentValidationException.InnerException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                return BadRequest(documentValidationException.InnerException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                return InternalServerError(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                return InternalServerError(documentServiceException);
            }
        }

        [HttpPost]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Documents")]
#endif
        public async ValueTask<ActionResult> ProcessDocumentByFileNameAsync([FromBody] Document document)
        {
            try
            {
                await documentService.AddDocumentAsync(document, blobContainers.EmisLanding);

                return Ok();
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

        [HttpDelete("{ingestionTrackingId}")]
#if RELEASE
[Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.Workflows.Documents")]
#endif
        public async ValueTask<ActionResult> DeleteDocumentByIdAsync(string fileName)
        {
            try
            {
                await this.documentService
                    .RemoveDocumentByFileNameAsync(WebUtility.UrlDecode(fileName), blobContainers.EmisLanding);

                return Ok();
            }
            catch (DocumentValidationException documentValidationException)
                when (documentValidationException.InnerException is NotFoundDocumentException)
            {
                return NotFound(documentValidationException.InnerException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                return BadRequest(documentValidationException.InnerException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                return InternalServerError(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                return InternalServerError(documentServiceException);
            }
        }
    }
}