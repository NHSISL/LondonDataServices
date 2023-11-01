using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class LockedOntologyValueSetException : Xeption
    {
        public LockedOntologyValueSetException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}