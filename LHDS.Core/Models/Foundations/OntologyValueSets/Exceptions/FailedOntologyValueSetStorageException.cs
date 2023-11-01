using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class FailedOntologyValueSetStorageException : Xeption
    {
        public FailedOntologyValueSetStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}