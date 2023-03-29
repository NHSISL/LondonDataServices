// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Services.Foundations.OptOuts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptOutsController : RESTFulController
    {
        private readonly IOptOutService optOutService;

        public OptOutsController(IOptOutService optOutService) =>
            this.optOutService = optOutService;


        [HttpPost]
        public async ValueTask<ActionResult<OptOut>> PostOptOutAsync(
            OptOut optOut)
        {
            try
            {
                OptOut addedOptOut =
                    await this.optOutService.AddOptOutAsync(optOut);

                return Created(addedOptOut);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                return BadRequest(optOutValidationException.InnerException);
            }
            catch (OptOutDependencyValidationException optOutValidationException)
                when (optOutValidationException.InnerException is InvalidOptOutReferenceException)
            {
                return FailedDependency(optOutValidationException.InnerException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
               when (optOutDependencyValidationException.InnerException is AlreadyExistsOptOutException)
            {
                return Conflict(optOutDependencyValidationException.InnerException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                return InternalServerError(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                return InternalServerError(optOutServiceException);
            }
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public ActionResult<IQueryable<OptOut>> Get()
        {
            try
            {
                IQueryable<OptOut> retrievedOptOuts =
                    this.optOutService.RetrieveAllOptOuts();

                return Ok(retrievedOptOuts);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                return InternalServerError(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                return InternalServerError(optOutServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<OptOut>> PutOptOutAsync(OptOut optOut)
        {
            try
            {
                OptOut modifiedOptOut =
                    await this.optOutService.ModifyOptOutAsync(optOut);

                return Ok(modifiedOptOut);
            }
            catch (OptOutValidationException optOutValidationException)
                when (optOutValidationException.InnerException is NotFoundOptOutException)
            {
                return NotFound(optOutValidationException.InnerException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                return BadRequest(optOutValidationException.InnerException);
            }
            catch (OptOutDependencyValidationException optOutValidationException)
                when (optOutValidationException.InnerException is InvalidOptOutReferenceException)
            {
                return FailedDependency(optOutValidationException.InnerException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
               when (optOutDependencyValidationException.InnerException is AlreadyExistsOptOutException)
            {
                return Conflict(optOutDependencyValidationException.InnerException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                return InternalServerError(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                return InternalServerError(optOutServiceException);
            }
        }
    }
}