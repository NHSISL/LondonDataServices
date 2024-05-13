using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationDependencyException : Xeption
    {
        public DataSetSpecificationDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}