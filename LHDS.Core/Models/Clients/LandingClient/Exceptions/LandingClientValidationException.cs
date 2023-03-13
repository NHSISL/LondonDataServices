// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.LandingClient.Exceptions
{
    public class LandingClientValidationException : Xeption
    {
        private const string validationMessage =
            "Landing client validation error occurred, fix errors and try again.";

        public LandingClientValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}
