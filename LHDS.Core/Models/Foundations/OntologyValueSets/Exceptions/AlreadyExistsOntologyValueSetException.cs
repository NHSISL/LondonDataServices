using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class AlreadyExistsOntologyValueSetException : Xeption
    {
        public AlreadyExistsOntologyValueSetException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}