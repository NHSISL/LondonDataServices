// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class OptOutDependencyValidationException : Xeption
    {
        public OptOutDependencyValidationException(Xeption innerException)
            : base(message: "OptOut dependency validation occurred, please try again.", innerException)
        { }
    }
}