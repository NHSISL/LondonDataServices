using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class AuditDependencyException : Xeption
    {
        public AuditDependencyException(Xeption innerException) :
            base(message: "Audit dependency error occurred, contact support.", innerException)
        { }
    }
}