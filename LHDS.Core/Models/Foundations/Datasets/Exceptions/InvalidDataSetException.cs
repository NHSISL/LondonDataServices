using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class InvalidDataSetException : Xeption
    {
        public InvalidDataSetException(string message)
            : base(message)
        { }
    }
}