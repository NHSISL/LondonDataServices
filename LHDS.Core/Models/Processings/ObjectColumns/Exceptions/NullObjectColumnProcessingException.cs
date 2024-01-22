// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ObjectColumns.Exceptions
{
    public class NullObjectColumnProcessingException : Xeption
    {
        public NullObjectColumnProcessingException(string message)
            : base(message)
        { }
    }
}
