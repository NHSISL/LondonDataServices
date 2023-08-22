using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService
    {
        private void ValidateDataTypeOnAdd(DataType dataType)
        {
            ValidateDataTypeIsNotNull(dataType);
        }

        private static void ValidateDataTypeIsNotNull(DataType dataType)
        {
            if (dataType is null)
            {
                throw new NullDataTypeException(message: "DataType is null.");
            }
        }
    }
}