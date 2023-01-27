using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class AuditDependencyValidationException : Xeption
    {
        public AuditDependencyValidationException(Xeption innerException)
            : base(message: "Audit dependency validation occurred, please try again.", innerException)
        { }
    }
}