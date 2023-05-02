// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CsvMappers.Exceptions
{
    public class CsvMapperProcessingDependencyValidationException : Xeption
    {
        public CsvMapperProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Csv Mapper processing dependency validation occurred, please try again.", innerException)
        { }
    }
}
