using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class AddressExtractionAuditDependencyException : Xeption
    {
        public AddressExtractionAuditDependencyException(string message, Xeption innerException) 
            : base(message, innerException)
        { }
    }
}