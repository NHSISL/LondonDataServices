using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.DataSetObjects;
using LHDS.AdminPortal.Api.Models.Foundations.DataSetObjects.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.DataSetObjects;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSetObjectsController : RESTFulController
    {
        private readonly IDataSetObjectService dataSetObjectService;

        public DataSetObjectsController(IDataSetObjectService dataSetObjectService) =>
            this.dataSetObjectService = dataSetObjectService;

        [HttpPost]
        public async ValueTask<ActionResult<DataSetObject>> PostDataSetObjectAsync(DataSetObject dataSetObject)
        {
            try
            {
                DataSetObject addedDataSetObject =
                    await this.dataSetObjectService.AddDataSetObjectAsync(dataSetObject);

                return Created(addedDataSetObject);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
            {
                return BadRequest(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectDependencyValidationException dataSetObjectValidationException)
                when (dataSetObjectValidationException.InnerException is InvalidDataSetObjectReferenceException)
            {
                return FailedDependency(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectDependencyValidationException dataSetObjectDependencyValidationException)
               when (dataSetObjectDependencyValidationException.InnerException is AlreadyExistsDataSetObjectException)
            {
                return Conflict(dataSetObjectDependencyValidationException.InnerException);
            }
            catch (DataSetObjectDependencyException dataSetObjectDependencyException)
            {
                return InternalServerError(dataSetObjectDependencyException);
            }
            catch (DataSetObjectServiceException dataSetObjectServiceException)
            {
                return InternalServerError(dataSetObjectServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<DataSetObject>> GetAllDataSetObjects()
        {
            try
            {
                IQueryable<DataSetObject> retrievedDataSetObjects =
                    this.dataSetObjectService.RetrieveAllDataSetObjects();

                return Ok(retrievedDataSetObjects);
            }
            catch (DataSetObjectDependencyException dataSetObjectDependencyException)
            {
                return InternalServerError(dataSetObjectDependencyException);
            }
            catch (DataSetObjectServiceException dataSetObjectServiceException)
            {
                return InternalServerError(dataSetObjectServiceException);
            }
        }

        [HttpGet("{dataSetObjectId}")]
        public async ValueTask<ActionResult<DataSetObject>> GetDataSetObjectByIdAsync(Guid dataSetObjectId)
        {
            try
            {
                DataSetObject dataSetObject = await this.dataSetObjectService.RetrieveDataSetObjectByIdAsync(dataSetObjectId);

                return Ok(dataSetObject);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
                when (dataSetObjectValidationException.InnerException is NotFoundDataSetObjectException)
            {
                return NotFound(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
            {
                return BadRequest(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectDependencyException dataSetObjectDependencyException)
            {
                return InternalServerError(dataSetObjectDependencyException);
            }
            catch (DataSetObjectServiceException dataSetObjectServiceException)
            {
                return InternalServerError(dataSetObjectServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<DataSetObject>> PutDataSetObjectAsync(DataSetObject dataSetObject)
        {
            try
            {
                DataSetObject modifiedDataSetObject =
                    await this.dataSetObjectService.ModifyDataSetObjectAsync(dataSetObject);

                return Ok(modifiedDataSetObject);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
                when (dataSetObjectValidationException.InnerException is NotFoundDataSetObjectException)
            {
                return NotFound(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
            {
                return BadRequest(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectDependencyValidationException dataSetObjectValidationException)
                when (dataSetObjectValidationException.InnerException is InvalidDataSetObjectReferenceException)
            {
                return FailedDependency(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectDependencyValidationException dataSetObjectDependencyValidationException)
               when (dataSetObjectDependencyValidationException.InnerException is AlreadyExistsDataSetObjectException)
            {
                return Conflict(dataSetObjectDependencyValidationException.InnerException);
            }
            catch (DataSetObjectDependencyException dataSetObjectDependencyException)
            {
                return InternalServerError(dataSetObjectDependencyException);
            }
            catch (DataSetObjectServiceException dataSetObjectServiceException)
            {
                return InternalServerError(dataSetObjectServiceException);
            }
        }

        [HttpDelete("{dataSetObjectId}")]
        public async ValueTask<ActionResult<DataSetObject>> DeleteDataSetObjectByIdAsync(Guid dataSetObjectId)
        {
            try
            {
                DataSetObject deletedDataSetObject =
                    await this.dataSetObjectService.RemoveDataSetObjectByIdAsync(dataSetObjectId);

                return Ok(deletedDataSetObject);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
                when (dataSetObjectValidationException.InnerException is NotFoundDataSetObjectException)
            {
                return NotFound(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectValidationException dataSetObjectValidationException)
            {
                return BadRequest(dataSetObjectValidationException.InnerException);
            }
            catch (DataSetObjectDependencyValidationException dataSetObjectDependencyValidationException)
                when (dataSetObjectDependencyValidationException.InnerException is LockedDataSetObjectException)
            {
                return Locked(dataSetObjectDependencyValidationException.InnerException);
            }
            catch (DataSetObjectDependencyValidationException dataSetObjectDependencyValidationException)
            {
                return BadRequest(dataSetObjectDependencyValidationException);
            }
            catch (DataSetObjectDependencyException dataSetObjectDependencyException)
            {
                return InternalServerError(dataSetObjectDependencyException);
            }
            catch (DataSetObjectServiceException dataSetObjectServiceException)
            {
                return InternalServerError(dataSetObjectServiceException);
            }
        }
    }
}