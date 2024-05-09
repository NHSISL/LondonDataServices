// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingService
    {
        private void ValidateTerminologyArtifact(TerminologyArtifact terminologyArtifact)
        {
            ValidateTerminologyArtifactIsNotNull(terminologyArtifact);
        }

        private static void ValidateTerminologyArtifactIsNotNull(TerminologyArtifact terminologyArtifact)
        {
            if (terminologyArtifact is null)
            {
                throw new NullTerminologyArtifactProcessingException(message: "Terminology artifact is null.");
            }
        }

        public void ValidateId(Guid Id) =>
           Validate<InvalidArgumentTerminologyArtifactProcessingException>(
               message: "Invalid argument(s). Please correct the errors and try again.",
               (Rule: IsInvalid(Id), Parameter: nameof(Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T?)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException?.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException?.ThrowIfContainsErrors();
        }
    }
}
