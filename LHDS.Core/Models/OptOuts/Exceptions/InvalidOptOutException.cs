using Xeptions;

namespace LHDS.Core.Models.OptOuts.Exceptions
{
    public class InvalidOptOutException : Xeption
    {
        public InvalidOptOutException()
            : base(message: "Invalid optOut. Please correct the errors and try again.")
        { }
    }
}