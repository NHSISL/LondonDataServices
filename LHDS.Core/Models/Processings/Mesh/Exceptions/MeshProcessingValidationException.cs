// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class MeshProcessingValidationException : Xeption
    {
        private const string validationMessage = "Mesh processing validation errors occured, please try again";

        public MeshProcessingValidationException(Xeption innerException, string validationSummary = "")
           : base(
                 message: validationSummary.Length > 0
                   ? $"{validationMessage}  Validation errors: {validationSummary}"
                   : validationMessage,
                 innerException)
        { }
    }
}
