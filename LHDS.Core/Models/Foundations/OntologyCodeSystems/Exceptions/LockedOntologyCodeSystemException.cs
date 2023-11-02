using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class LockedOntologyCodeSystemException : Xeption
    {
        public LockedOntologyCodeSystemException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}