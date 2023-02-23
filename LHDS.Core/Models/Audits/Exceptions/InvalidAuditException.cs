using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class InvalidAuditException : Xeption
    {
        public InvalidAuditException()
            : base(message: "Invalid audit. Please correct the errors and try again.")
        { }
    }
}