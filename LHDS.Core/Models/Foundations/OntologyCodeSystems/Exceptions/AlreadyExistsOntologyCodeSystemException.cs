using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class AlreadyExistsOntologyCodeSystemException : Xeption
    {
        public AlreadyExistsOntologyCodeSystemException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}