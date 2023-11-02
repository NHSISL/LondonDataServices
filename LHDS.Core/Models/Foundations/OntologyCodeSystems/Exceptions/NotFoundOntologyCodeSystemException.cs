using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions
{
    public class NotFoundOntologyCodeSystemException : Xeption
    {
        public NotFoundOntologyCodeSystemException(Guid ontologyCodeSystemId)
            : base(message: $"Couldn't find ontologyCodeSystem with ontologyCodeSystemId: {ontologyCodeSystemId}.")
        { }
    }
}