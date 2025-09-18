// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private static void ValidateOptOutProcessingOnRetrieveOrAdd(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingOnModify(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutProcessingException(message: "Opt out processing is Null");
            }
        }

        private static void ValidateCurrentOptOutListProcessingOnConsolidate(
            List<OptOut> optOutList,
            List<string> consentedItemsList)
        {
            Validate(
                createException: () => new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again."),

                (Rule: IsInvalid(optOutList), Parameter: "OptOutList"),
                (Rule: IsInvalid(consentedItemsList), Parameter: "ConsentedItemsList"));
        }

        public void ValidateOptOutId(Guid optOutId)
        {
            Validate(
                createException: () => new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again."),

                (Rule: IsInvalid(optOutId), Parameter: nameof(OptOut.Id)));

        }

        public void ValidateOptOutNhsNumber(string optOutNhsNumber)
        {
            Validate(
                createException: () => new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again."),

                (Rule: IsInvalid(optOutNhsNumber), Parameter: nameof(OptOut.NhsNumber)));

        }

        public void ValidateOptOutBatchReference(string batchReference)
        {
            Validate(
                createException: () => new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again."),

                (Rule: IsInvalid(batchReference), Parameter: nameof(OptOut.BatchReference)));
        }

        public void ValidateOlderThanDays(int olderThanDays)
        {
            Validate(
                createException: () => new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again."),

                (Rule: IsInvalid(olderThanDays), Parameter: "OlderThanDays"));

        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(int value) => new
        {
            Condition = value < 7,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(List<OptOut> optOutList) => new
        {
            Condition = optOutList == null,
            Message = "Opt out list is required"
        };

        private static dynamic IsInvalid(List<string> stringList) => new
        {
            Condition = stringList == null,
            Message = "String list is required"
        };

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = createException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
