// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Services.Foundations.Documents;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : RESTFulController
    {
        private readonly IDocumentService documentService;

        public DocumentsController(IDocumentService documentService) =>
            this.documentService = documentService;

        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<Document>> GetDownloadLinkAsync(string fileName)
        {
            try
            {
                string document = await this.documentService.GetDownloadLinkAsync(WebUtility.UrlDecode(fileName));

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
    }
}