using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions
{
    public class NotFoundOntologyConceptMapException : Xeption
    {
        public NotFoundOntologyConceptMapException(Guid ontologyConceptMapId)
            : base(message: $"Couldn't find ontologyConceptMap with ontologyConceptMapId: {ontologyConceptMapId}.")
        { }
    }
}