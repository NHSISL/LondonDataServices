// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CsvMappers.Exceptions
{
    public class CsvMapperValidationException : Xeption
    {
        public CsvMapperValidationException(Xeption innerException)
            : base(
                message: "CSV mapper validation errors occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
