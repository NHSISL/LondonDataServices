// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CsvMappers.Exceptions
{
    public class CsvMapperDependencyValidationException : Xeption
    {
        public CsvMapperDependencyValidationException(Xeption innerException)
            : base(message: "CSV mapper dependency validation occurred, please try again.", innerException)
        { }
    }
}
