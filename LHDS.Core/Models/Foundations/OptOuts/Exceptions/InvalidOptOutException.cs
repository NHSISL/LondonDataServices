using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class InvalidOptOutException : Xeption
    {
        public InvalidOptOutException()
            : base(message: "Invalid optOut. Please correct the errors and try again.")
        { }
    }
}