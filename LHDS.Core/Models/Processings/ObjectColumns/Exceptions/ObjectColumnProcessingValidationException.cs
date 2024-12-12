// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ObjectColumns.Exceptions
{
    public class ObjectColumnProcessingValidationException : Xeption
    {
        public ObjectColumnProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
