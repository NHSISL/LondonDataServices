using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Foundations.DataTypes;
using LHDS.AdminPortal.Api.Models.Foundations.DataTypes.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.DataTypes;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTypesController : RESTFulController
    {
        private readonly IDataTypeService dataTypeService;

        public DataTypesController(IDataTypeService dataTypeService) =>
            this.dataTypeService = dataTypeService;

        [HttpPost]
        public async ValueTask<ActionResult<DataType>> PostDataTypeAsync(DataType dataType)
        {
            try
            {
                DataType addedDataType =
                    await this.dataTypeService.AddDataTypeAsync(dataType);

                return Created(addedDataType);
            }
            catch (DataTypeValidationException dataTypeValidationException)
            {
                return BadRequest(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeValidationException)
                when (dataTypeValidationException.InnerException is InvalidDataTypeReferenceException)
            {
                return FailedDependency(dataTypeValidationException.InnerException);
            }
            catch (DataTypeDependencyValidationException dataTypeDependencyValidationException)
               when (dataTypeDependencyValidationException.InnerException is AlreadyExistsDataTypeException)
            {
                return Conflict(dataTypeDependencyValidationException.InnerException);
            }
            catch (DataTypeDependencyException dataTypeDependencyException)
            {
                return InternalServerError(dataTypeDependencyException);
            }
            catch (DataTypeServiceException dataTypeServiceException)
            {
                return InternalServerError(dataTypeServiceException);
            }
        }
    }
}