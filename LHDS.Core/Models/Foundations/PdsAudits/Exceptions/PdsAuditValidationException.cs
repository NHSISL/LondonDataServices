using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class PdsAuditValidationException : Xeption
    {
        public PdsAuditValidationException(string message, Xeption innerException)
            : base(message: "PdsAudit validation errors occurred, please try again.",
                  innerException)
        { }
    }
}