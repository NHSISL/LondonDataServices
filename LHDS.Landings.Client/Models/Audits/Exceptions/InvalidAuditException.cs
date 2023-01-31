using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class InvalidAuditException : Xeption
    {
        public InvalidAuditException()
            : base(message: "Invalid audit. Please correct the errors and try again.")
        { }
    }
}