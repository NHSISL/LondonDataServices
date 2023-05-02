using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class PdsAuditValidationException : Xeption
    {
        public PdsAuditValidationException(Xeption innerException)
            : base(message: "PdsAudit validation errors occurred, please try again.",
                  innerException)
        { }
    }
}