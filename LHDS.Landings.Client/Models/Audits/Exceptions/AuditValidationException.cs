using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class AuditValidationException : Xeption
    {
        public AuditValidationException(Xeption innerException)
            : base(message: "Audit validation errors occurred, please try again.",
                  innerException)
        { }
    }
}