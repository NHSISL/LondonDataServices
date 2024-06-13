using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AuditValidationException : Xeption
    {
        public AuditValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}