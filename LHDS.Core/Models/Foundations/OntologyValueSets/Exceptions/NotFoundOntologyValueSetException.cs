using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions
{
    public class NotFoundOntologyValueSetException : Xeption
    {
        public NotFoundOntologyValueSetException(Guid ontologyValueSetId)
            : base(message: $"Couldn't find ontologyValueSet with ontologyValueSetId: {ontologyValueSetId}.")
        { }
    }
}