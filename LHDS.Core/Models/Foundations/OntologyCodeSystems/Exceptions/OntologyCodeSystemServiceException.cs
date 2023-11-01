using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class OntologyCodeSystemServiceException : Xeption
    {
        public OntologyCodeSystemServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}