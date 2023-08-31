using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.DataSets;
using LHDS.AdminPortal.Api.Models.Foundations.DataSets.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.DataSets;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSetsController : RESTFulController
    {
        private readonly IDataSetService dataSetService;

        public DataSetsController(IDataSetService dataSetService) =>
            this.dataSetService = dataSetService;

        [HttpPost]
        public async ValueTask<ActionResult<DataSet>> PostDataSetAsync(DataSet dataSet)
        {
            try
            {
                DataSet addedDataSet =
                    await this.dataSetService.AddDataSetAsync(dataSet);

                return Created(addedDataSet);
            }
            catch (DataSetValidationException dataSetValidationException)
            {
                return BadRequest(dataSetValidationException.InnerException);
            }
            catch (DataSetDependencyValidationException dataSetValidationException)
                when (dataSetValidationException.InnerException is InvalidDataSetReferenceException)
            {
                return FailedDependency(dataSetValidationException.InnerException);
            }
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
               when (dataSetDependencyValidationException.InnerException is AlreadyExistsDataSetException)
            {
                return Conflict(dataSetDependencyValidationException.InnerException);
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                return InternalServerError(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                return InternalServerError(dataSetServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<DataSet>> GetAllDataSets()
        {
            try
            {
                IQueryable<DataSet> retrievedDataSets =
                    this.dataSetService.RetrieveAllDataSets();

                return Ok(retrievedDataSets);
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                return InternalServerError(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                return InternalServerError(dataSetServiceException);
            }
        }

        [HttpGet("{dataSetId}")]
        public async ValueTask<ActionResult<DataSet>> GetDataSetByIdAsync(Guid dataSetId)
        {
            try
            {
                DataSet dataSet = await this.dataSetService.RetrieveDataSetByIdAsync(dataSetId);

                return Ok(dataSet);
            }
            catch (DataSetValidationException dataSetValidationException)
                when (dataSetValidationException.InnerException is NotFoundDataSetException)
            {
                return NotFound(dataSetValidationException.InnerException);
            }
            catch (DataSetValidationException dataSetValidationException)
            {
                return BadRequest(dataSetValidationException.InnerException);
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                return InternalServerError(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                return InternalServerError(dataSetServiceException);
            }
        }
    }
}