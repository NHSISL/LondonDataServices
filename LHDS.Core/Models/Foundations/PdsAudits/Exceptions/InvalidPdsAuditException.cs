using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class InvalidPdsAuditException : Xeption
    {
        public InvalidPdsAuditException()
            : base(message: "Invalid pdsAudit. Please correct the errors and try again.")
        { }
    }
}