using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class DataSetDependencyException : Xeption
    {
        public DataSetDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}