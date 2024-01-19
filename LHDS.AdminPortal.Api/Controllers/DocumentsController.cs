// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
using System.Text;
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

        [HttpGet]
        public async ValueTask<ActionResult> Get()
        {
            try
            {
                Document doc = new Document
                {
                    DocumentData = Encoding.UTF8.GetBytes("Hello world!"),
                    FileName = $"test-{DateTime.Now.ToString("yyyyHHmmss")}.txt"
                };

                await this.documentService.AddDocumentAsync(doc, "emislanding");

                return Ok();
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                Console.WriteLine(dataSetDependencyException.Message);

                return InternalServerError(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                Console.WriteLine(dataSetServiceException.Message);

                return InternalServerError(dataSetServiceException);
            }
        }


        [HttpPost]
        public async ValueTask<ActionResult> PostDocumentAsync([FromBody] DocumentsModel documentsModel)
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

        [HttpDelete("{container}/{fileName}")]
        public async ValueTask<ActionResult> RemoveDocumentByFileNameAsync(string fileName, string container) 
        {
            try
            {
                string decodedFileName = WebUtility.UrlDecode(fileName);
                await this.documentService.RemoveDocumentByFileNameAsync(fileName, container);

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