using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NullDataSetObjectException : Xeption
    {
        public NullDataSetObjectException(string message)
            : base(message)
        { }
    }
}