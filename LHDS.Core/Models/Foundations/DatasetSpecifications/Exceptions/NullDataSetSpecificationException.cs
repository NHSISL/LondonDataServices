using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class NullDataSetSpecificationException : Xeption
    {
        public NullDataSetSpecificationException(string message)
            : base(message)
        { }
    }
}