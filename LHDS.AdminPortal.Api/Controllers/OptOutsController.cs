// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptOutsController : RESTFulController
    {
        private readonly IOptOutProcessingService optOutProcessingService;

        public OptOutsController(IOptOutProcessingService optOutProcessingService) =>
            this.optOutProcessingService = optOutProcessingService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.OptOut")]
#endif
        public async ValueTask<ActionResult<OptOut>> PostOptOutAsync(
            OptOut optOut)
        {
            try
            {
                OptOut addedOptOut =
                    await this.optOutProcessingService.RetrieveOrAddOptOutAsync(optOut);

                return Created(addedOptOut);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
            {
                return BadRequest(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is InvalidOptOutReferenceException)
            {
                return FailedDependency(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingDependencyValidationException)
               when (optOutProcessingDependencyValidationException.InnerException is AlreadyExistsOptOutException)
            {
                return Conflict(optOutProcessingDependencyValidationException.InnerException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                return InternalServerError(optOutDependencyException);
            }
            catch (OptOutServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }

        [HttpGet("{nhsNumber}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.OptOut, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<OptOut>> GetOptOutByNhsNumberAsync(string nhsNumber)
        {
            try
            {
                OptOut getOptOutByNhs =
                    await this.optOutProcessingService.RetrieveOptOutByNhsNumberAsync(nhsNumber);

                return Ok(getOptOutByNhs);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
            {
                return BadRequest(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyException optOutProcessingDependencyException)
            {
                return InternalServerError(optOutProcessingDependencyException);
            }
            catch (OptOutProcessingServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.OptOut")]
#endif
        public async ValueTask<ActionResult<OptOut>> PutOptOutAsync(OptOut optOut)
        {
            try
            {
                OptOut modifiedOptOut =
                    await this.optOutProcessingService.AddOrModifyOptOutAsync(optOut);

                return Ok(modifiedOptOut);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is NotFoundOptOutException)
            {
                return NotFound(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingValidationException optOutProcessingValidationException)
            {
                return BadRequest(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingValidationException)
                when (optOutProcessingValidationException.InnerException is InvalidOptOutReferenceException)
            {
                return FailedDependency(optOutProcessingValidationException.InnerException);
            }
            catch (OptOutProcessingDependencyValidationException optOutProcessingDependencyValidationException)
               when (optOutProcessingDependencyValidationException.InnerException is AlreadyExistsOptOutException)
            {
                return Conflict(optOutProcessingDependencyValidationException.InnerException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                return InternalServerError(optOutDependencyException);
            }
            catch (OptOutServiceException optOutProcessingServiceException)
            {
                return InternalServerError(optOutProcessingServiceException);
            }
        }
    }
}