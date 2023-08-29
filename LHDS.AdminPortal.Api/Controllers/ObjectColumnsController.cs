using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.ObjectColumns;
using LHDS.AdminPortal.Api.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.ObjectColumns;

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

        [HttpGet]
        public ActionResult<IQueryable<ObjectColumn>> GetAllObjectColumns()
        {
            try
            {
                IQueryable<ObjectColumn> retrievedObjectColumns =
                    this.objectColumnService.RetrieveAllObjectColumns();

                return Ok(retrievedObjectColumns);
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

        [HttpGet("{objectColumnId}")]
        public async ValueTask<ActionResult<ObjectColumn>> GetObjectColumnByIdAsync(Guid objectColumnId)
        {
            try
            {
                ObjectColumn objectColumn = await this.objectColumnService.RetrieveObjectColumnByIdAsync(objectColumnId);

                return Ok(objectColumn);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
                when (objectColumnValidationException.InnerException is NotFoundObjectColumnException)
            {
                return NotFound(objectColumnValidationException.InnerException);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                return BadRequest(objectColumnValidationException.InnerException);
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