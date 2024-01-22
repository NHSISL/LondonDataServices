// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TppLandings.Exceptions
{
    public class NullDocumentTppLandingException : Xeption
    {
        public NullDocumentTppLandingException(string message)
            : base(message)
        { }
    }
}
