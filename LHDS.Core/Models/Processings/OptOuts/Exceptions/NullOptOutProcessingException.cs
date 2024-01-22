// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class NullOptOutProcessingException : Xeption
    {
        public NullOptOutProcessingException()
            : base(message: $"Opt out processing is Null")
        { }
    }
}
