// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSets.Exceptions
{
    public class DataSetProcessingValidationException : Xeption
    {
        public DataSetProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
