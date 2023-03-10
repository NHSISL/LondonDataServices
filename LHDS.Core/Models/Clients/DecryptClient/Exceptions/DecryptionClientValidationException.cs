// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.DecryptClient.Exceptions {
    public class DecryptionClientValidationException : Xeption {
        private const string validationMessage =
            "Decryption client validation error occurred, fix errors and try again.";

        public DecryptionClientValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException) { }
    }
}
