using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AuditDependencyException : Xeption
    {
        public AuditDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}