// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.IDecideClient.Exceptions
{
    public class IDecideClientValidationException : Xeption
    {
        public IDecideClientValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
