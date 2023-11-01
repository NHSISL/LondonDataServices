using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class FailedOntologyCodeSystemServiceException : Xeption
    {
        public FailedOntologyCodeSystemServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}