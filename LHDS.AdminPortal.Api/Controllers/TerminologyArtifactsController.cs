using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.TerminologyArtifacts;
using LHDS.AdminPortal.Api.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.TerminologyArtifacts;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerminologyArtifactsController : RESTFulController
    {
        private readonly ITerminologyArtifactService terminologyArtifactService;

        public TerminologyArtifactsController(ITerminologyArtifactService terminologyArtifactService) =>
            this.terminologyArtifactService = terminologyArtifactService;

        [HttpPost]
        public async ValueTask<ActionResult<TerminologyArtifact>> PostTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact)
        {
            try
            {
                TerminologyArtifact addedTerminologyArtifact =
                    await this.terminologyArtifactService.AddTerminologyArtifactAsync(terminologyArtifact);

                return Created(addedTerminologyArtifact);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                return BadRequest(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactValidationException)
                when (terminologyArtifactValidationException.InnerException is InvalidTerminologyArtifactReferenceException)
            {
                return FailedDependency(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
               when (terminologyArtifactDependencyValidationException.InnerException is AlreadyExistsTerminologyArtifactException)
            {
                return Conflict(terminologyArtifactDependencyValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<TerminologyArtifact>> GetAllTerminologyArtifacts()
        {
            try
            {
                IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts =
                    this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

                return Ok(retrievedTerminologyArtifacts);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }
    }
}