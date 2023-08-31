using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class InvalidDataSetObjectException : Xeption
    {
        public InvalidDataSetObjectException(string message)
            : base(message)
        { }
    }
}