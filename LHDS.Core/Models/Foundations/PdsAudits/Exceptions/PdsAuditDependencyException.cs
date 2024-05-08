using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class PdsAuditDependencyException : Xeption
    {
        public PdsAuditDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}