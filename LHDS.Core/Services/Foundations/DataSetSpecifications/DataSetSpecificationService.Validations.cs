using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private void ValidateDataSetSpecificationOnAdd(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);
        }

        private static void ValidateDataSetSpecificationIsNotNull(DataSetSpecification dataSetSpecification)
        {
            if (dataSetSpecification is null)
            {
                throw new NullDataSetSpecificationException(message: "DataSetSpecification is null.");
            }
        }
    }
}