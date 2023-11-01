using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class OntologyValueSetServiceException : Xeption
    {
        public OntologyValueSetServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}