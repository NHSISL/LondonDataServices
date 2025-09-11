// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.Decisions.Exceptions
{
    public class DecisionCoordinationDependencyValidationException : Xeption
    {
        public DecisionCoordinationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}