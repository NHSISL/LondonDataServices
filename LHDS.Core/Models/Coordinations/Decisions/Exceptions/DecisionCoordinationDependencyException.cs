// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.Decisions.Exceptions
{
    public class DecisionCoordinationDependencyException : Xeption
    {
        public DecisionCoordinationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
