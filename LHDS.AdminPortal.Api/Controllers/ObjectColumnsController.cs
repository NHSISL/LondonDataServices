// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.Core.Services.Foundations.ObjectColumns;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectColumnsController : RESTFulController
    {
        private readonly IObjectColumnService objectColumnService;

        public ObjectColumnsController(IObjectColumnService objectColumnService) =>
            this.objectColumnService = objectColumnService;

        [HttpPost]
        public async ValueTask<ActionResult<ObjectColumn>> PostObjectColumnAsync(ObjectColumn objectColumn)
        {
            try
            {
                ObjectColumn addedObjectColumn =
                    await this.objectColumnService.AddObjectColumnAsync(objectColumn);

                return Created(addedObjectColumn);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                return BadRequest(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is InvalidObjectColumnReferenceException)
            {
                return FailedDependency(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
               when (objectColumnDependencyValidationException.InnerException is AlreadyExistsObjectColumnException)
            {
                return Conflict(objectColumnDependencyValidationException.InnerException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                return InternalServerError(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                return InternalServerError(objectColumnServiceException);
            }
        }
    }
}