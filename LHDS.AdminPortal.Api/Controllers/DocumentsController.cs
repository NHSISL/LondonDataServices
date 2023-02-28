using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Documents;
using LHDS.AdminPortal.Api.Models.Documents.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.Documents;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : RESTFulController
    {
        private readonly IDocumentService documentService;

        public DocumentsController(IDocumentService documentService) =>
            this.documentService = documentService;

        [HttpPost]
        public async ValueTask<ActionResult<Document>> PostDocumentAsync(Document document)
        {
            try
            {
                Document addedDocument =
                    await this.documentService.AddDocumentAsync(document);

                return Created(addedDocument);
            }
            catch (DocumentValidationException documentValidationException)
            {
                return BadRequest(documentValidationException.InnerException);
            }
            catch (DocumentDependencyValidationException documentValidationException)
                when (documentValidationException.InnerException is InvalidDocumentReferenceException)
            {
                return FailedDependency(documentValidationException.InnerException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
               when (documentDependencyValidationException.InnerException is AlreadyExistsDocumentException)
            {
                return Conflict(documentDependencyValidationException.InnerException);
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