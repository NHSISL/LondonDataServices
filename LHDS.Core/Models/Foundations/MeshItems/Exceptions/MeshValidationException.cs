// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.MeshItems.Exceptions
{
    public class MeshValidationException : Xeption
    {
        private const string validationMessage = "Mesh validation errors occurred, please try again.";

        public MeshValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}
