// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ObjectColumns.Exceptions
{
    public class ObjectColumnProcessingDependencyException : Xeption
    {
        public ObjectColumnProcessingDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
