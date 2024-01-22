// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CsvMappers.Exceptions
{
    public class CsvMapperProcessingDependencyValidationException : Xeption
    {
        public CsvMapperProcessingDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
