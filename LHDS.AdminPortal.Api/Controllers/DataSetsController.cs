// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using LHDS.Core.Services.Foundations.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

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
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.DataSets, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IQueryable<DataSet>>> Get()
        {
            try
            {
                IQueryable<DataSet> retrievedDataSets =
                    await this.dataSetService.RetrieveAllDataSetsAsync();

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

        [HttpPut]
        public async ValueTask<ActionResult<DataSet>> PutDataSetAsync(DataSet dataSet)
        {
            try
            {
                DataSet modifiedDataSet =
                    await this.dataSetService.ModifyDataSetAsync(dataSet);

                return Ok(modifiedDataSet);
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

        [HttpDelete("{dataSetId}")]
        public async ValueTask<ActionResult<DataSet>> DeleteDataSetByIdAsync(Guid dataSetId)
        {
            try
            {
                DataSet deletedDataSet =
                    await this.dataSetService.RemoveDataSetByIdAsync(dataSetId);

                return Ok(deletedDataSet);
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
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
                when (dataSetDependencyValidationException.InnerException is LockedDataSetException)
            {
                return Locked(dataSetDependencyValidationException.InnerException);
            }
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
            {
                return BadRequest(dataSetDependencyValidationException);
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