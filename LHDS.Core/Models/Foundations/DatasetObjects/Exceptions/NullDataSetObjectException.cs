using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetObjects.Exceptions
{
    public class NullDataSetObjectException : Xeption
    {
        public NullDataSetObjectException(string message)
            : base(message)
        { }
    }
}