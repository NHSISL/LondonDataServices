using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.TerminologyPolls;
using LHDS.AdminPortal.Api.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.TerminologyPolls;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerminologyPollsController : RESTFulController
    {
        private readonly ITerminologyPollService terminologyPollService;

        public TerminologyPollsController(ITerminologyPollService terminologyPollService) =>
            this.terminologyPollService = terminologyPollService;

        [HttpPost]
        public async ValueTask<ActionResult<TerminologyPoll>> PostTerminologyPollAsync(TerminologyPoll terminologyPoll)
        {
            try
            {
                TerminologyPoll addedTerminologyPoll =
                    await this.terminologyPollService.AddTerminologyPollAsync(terminologyPoll);

                return Created(addedTerminologyPoll);
            }
            catch (TerminologyPollValidationException terminologyPollValidationException)
            {
                return BadRequest(terminologyPollValidationException.InnerException);
            }
            catch (TerminologyPollDependencyValidationException terminologyPollValidationException)
                when (terminologyPollValidationException.InnerException is InvalidTerminologyPollReferenceException)
            {
                return FailedDependency(terminologyPollValidationException.InnerException);
            }
            catch (TerminologyPollDependencyValidationException terminologyPollDependencyValidationException)
               when (terminologyPollDependencyValidationException.InnerException is AlreadyExistsTerminologyPollException)
            {
                return Conflict(terminologyPollDependencyValidationException.InnerException);
            }
            catch (TerminologyPollDependencyException terminologyPollDependencyException)
            {
                return InternalServerError(terminologyPollDependencyException);
            }
            catch (TerminologyPollServiceException terminologyPollServiceException)
            {
                return InternalServerError(terminologyPollServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<TerminologyPoll>> GetAllTerminologyPolls()
        {
            try
            {
                IQueryable<TerminologyPoll> retrievedTerminologyPolls =
                    this.terminologyPollService.RetrieveAllTerminologyPolls();

                return Ok(retrievedTerminologyPolls);
            }
            catch (TerminologyPollDependencyException terminologyPollDependencyException)
            {
                return InternalServerError(terminologyPollDependencyException);
            }
            catch (TerminologyPollServiceException terminologyPollServiceException)
            {
                return InternalServerError(terminologyPollServiceException);
            }
        }

        [HttpGet("{terminologyPollId}")]
        public async ValueTask<ActionResult<TerminologyPoll>> GetTerminologyPollByIdAsync(Guid terminologyPollId)
        {
            try
            {
                TerminologyPoll terminologyPoll = await this.terminologyPollService.RetrieveTerminologyPollByIdAsync(terminologyPollId);

                return Ok(terminologyPoll);
            }
            catch (TerminologyPollValidationException terminologyPollValidationException)
                when (terminologyPollValidationException.InnerException is NotFoundTerminologyPollException)
            {
                return NotFound(terminologyPollValidationException.InnerException);
            }
            catch (TerminologyPollValidationException terminologyPollValidationException)
            {
                return BadRequest(terminologyPollValidationException.InnerException);
            }
            catch (TerminologyPollDependencyException terminologyPollDependencyException)
            {
                return InternalServerError(terminologyPollDependencyException);
            }
            catch (TerminologyPollServiceException terminologyPollServiceException)
            {
                return InternalServerError(terminologyPollServiceException);
            }
        }
    }
}