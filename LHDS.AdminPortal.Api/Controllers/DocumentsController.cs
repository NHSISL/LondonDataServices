// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Models.Controllers.Documents;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
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

        [HttpPost]
        public async ValueTask<ActionResult> PostDataSetAsync(DocumentsModel documentsModel)
        {
            try
            {
                await this.documentService.AddDocumentAsync(documentsModel.Document, documentsModel.Container);

                return Ok();
            }
            catch (DocumentValidationException documentValidationException)
            {
                return BadRequest(documentValidationException.InnerException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
                when (documentDependencyValidationException.InnerException is InvalidDataSetReferenceException)
            {
                return FailedDependency(documentDependencyValidationException.InnerException);
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

        [HttpGet("{fileName}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
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

        [HttpDelete("{documentsFileModel}")]
        public async ValueTask<ActionResult<Document>> RetrieveDocumentByFileNameAsync(DocumentsFileModel 
            documentsFileModel)
        {
            try
            {
                Document document =
                    await this.documentService.RetrieveDocumentByFileNameAsync(documentsFileModel.FileName, 
                    documentsFileModel.Container);

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