using System;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService
    {
        private void ValidateTerminologyArtifactOnAdd(TerminologyArtifact terminologyArtifact)
        {
            ValidateTerminologyArtifactIsNotNull(terminologyArtifact);

            Validate(
                (Rule: IsInvalid(terminologyArtifact.Id), Parameter: nameof(TerminologyArtifact.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(terminologyArtifact.CreatedDate), Parameter: nameof(TerminologyArtifact.CreatedDate)),
                (Rule: IsInvalid(terminologyArtifact.CreatedBy), Parameter: nameof(TerminologyArtifact.CreatedBy)),
                (Rule: IsInvalid(terminologyArtifact.UpdatedDate), Parameter: nameof(TerminologyArtifact.UpdatedDate)),
                (Rule: IsInvalid(terminologyArtifact.UpdatedBy), Parameter: nameof(TerminologyArtifact.UpdatedBy)));
        }

        private static void ValidateTerminologyArtifactIsNotNull(TerminologyArtifact terminologyArtifact)
        {
            if (terminologyArtifact is null)
            {
                throw new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTerminologyArtifactException = 
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTerminologyArtifactException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTerminologyArtifactException.ThrowIfContainsErrors();
        }
    }
}