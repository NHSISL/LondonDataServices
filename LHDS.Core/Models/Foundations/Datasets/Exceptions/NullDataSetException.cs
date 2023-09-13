using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class NullDataSetException : Xeption
    {
        public NullDataSetException(string message)
            : base(message)
        { }
    }
}