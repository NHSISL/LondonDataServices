using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class FailedOntologyValueSetServiceException : Xeption
    {
        public FailedOntologyValueSetServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}