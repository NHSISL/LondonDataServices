// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class DataSetProcessingDependencyException : Xeption
    {
        public DataSetProcessingDependencyException(string message, Xeption innerException) :
            base(message, innerException)
        { }
    }
}
