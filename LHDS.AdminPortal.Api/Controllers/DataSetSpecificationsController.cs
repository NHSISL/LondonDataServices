using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.DataSetSpecifications;
using LHDS.AdminPortal.Api.Models.Foundations.DataSetSpecifications.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.DataSetSpecifications;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSetSpecificationsController : RESTFulController
    {
        private readonly IDataSetSpecificationService dataSetSpecificationService;

        public DataSetSpecificationsController(IDataSetSpecificationService dataSetSpecificationService) =>
            this.dataSetSpecificationService = dataSetSpecificationService;

        [HttpPost]
        public async ValueTask<ActionResult<DataSetSpecification>> PostDataSetSpecificationAsync(DataSetSpecification dataSetSpecification)
        {
            try
            {
                DataSetSpecification addedDataSetSpecification =
                    await this.dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);

                return Created(addedDataSetSpecification);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                return BadRequest(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is InvalidDataSetSpecificationReferenceException)
            {
                return FailedDependency(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
               when (dataSetSpecificationDependencyValidationException.InnerException is AlreadyExistsDataSetSpecificationException)
            {
                return Conflict(dataSetSpecificationDependencyValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<DataSetSpecification>> GetAllDataSetSpecifications()
        {
            try
            {
                IQueryable<DataSetSpecification> retrievedDataSetSpecifications =
                    this.dataSetSpecificationService.RetrieveAllDataSetSpecifications();

                return Ok(retrievedDataSetSpecifications);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }

        [HttpGet("{dataSetSpecificationId}")]
        public async ValueTask<ActionResult<DataSetSpecification>> GetDataSetSpecificationByIdAsync(Guid dataSetSpecificationId)
        {
            try
            {
                DataSetSpecification dataSetSpecification = await this.dataSetSpecificationService.RetrieveDataSetSpecificationByIdAsync(dataSetSpecificationId);

                return Ok(dataSetSpecification);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is NotFoundDataSetSpecificationException)
            {
                return NotFound(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                return BadRequest(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }
    }
}