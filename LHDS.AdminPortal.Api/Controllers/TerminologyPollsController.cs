// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Attrify.Attributes;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerminologyPollsController : RESTFulController
    {
        private readonly ITerminologyPollService terminologyPollService;

        public TerminologyPollsController(ITerminologyPollService terminologyPollService) =>
            this.terminologyPollService = terminologyPollService;

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators")]
        [InvisibleApi]
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

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<TerminologyPoll>>> GetAllTerminologyPollsAsync()
        {
            try
            {
                IQueryable<TerminologyPoll> retrievedTerminologyPolls =
                    await this.terminologyPollService.RetrieveAllTerminologyPollsAsync();

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

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
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

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations")]
        [HttpPut]
        public async ValueTask<ActionResult<TerminologyPoll>> PutTerminologyPollAsync(TerminologyPoll terminologyPoll)
        {
            try
            {
                TerminologyPoll modifiedTerminologyPoll =
                    await this.terminologyPollService.ModifyTerminologyPollAsync(terminologyPoll);

                return Ok(modifiedTerminologyPoll);
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

        [HttpDelete("{terminologyPollId}")]
        public async ValueTask<ActionResult<TerminologyPoll>> DeleteTerminologyPollByIdAsync(Guid terminologyPollId)
        {
            try
            {
                TerminologyPoll deletedTerminologyPoll =
                    await this.terminologyPollService.RemoveTerminologyPollByIdAsync(terminologyPollId);

                return Ok(deletedTerminologyPoll);
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
            catch (TerminologyPollDependencyValidationException terminologyPollDependencyValidationException)
                when (terminologyPollDependencyValidationException.InnerException is LockedTerminologyPollException)
            {
                return Locked(terminologyPollDependencyValidationException.InnerException);
            }
            catch (TerminologyPollDependencyValidationException terminologyPollDependencyValidationException)
            {
                return BadRequest(terminologyPollDependencyValidationException);
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