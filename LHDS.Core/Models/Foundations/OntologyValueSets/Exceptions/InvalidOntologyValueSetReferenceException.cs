using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class InvalidOntologyValueSetReferenceException : Xeption
    {
        public InvalidOntologyValueSetReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}