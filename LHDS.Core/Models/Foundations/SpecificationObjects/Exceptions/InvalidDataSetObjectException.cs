using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class InvalidDataSetObjectException : Xeption
    {
        public InvalidDataSetObjectException(string message)
            : base(message)
        { }
    }
}