using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class DataSetDependencyValidationException : Xeption
    {
        public DataSetDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}