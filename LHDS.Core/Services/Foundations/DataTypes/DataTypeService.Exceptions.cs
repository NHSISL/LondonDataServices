using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService
    {
        private delegate ValueTask<DataType> ReturningDataTypeFunction();

        private async ValueTask<DataType> TryCatch(ReturningDataTypeFunction returningDataTypeFunction)
        {
            try
            {
                return await returningDataTypeFunction();
            }
            catch (NullDataTypeException nullDataTypeException)
            {
                throw CreateAndLogValidationException(nullDataTypeException);
            }
            catch (InvalidDataTypeException invalidDataTypeException)
            {
                throw CreateAndLogValidationException(invalidDataTypeException);
            }
        }

        private DataTypeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataTypeValidationException);

            return dataTypeValidationException;
        }
    }
}