// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class NullOptOutListProcessingException : Xeption
    {
        public NullOptOutListProcessingException(string message)
            : base(message)
        { }
    }
}
