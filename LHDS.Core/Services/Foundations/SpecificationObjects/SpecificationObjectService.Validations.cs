// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;

namespace LHDS.Core.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectService
    {
        private async ValueTask ValidateSpecificationObjectOnAddAsync(SpecificationObject specificationObject)
        {
            ValidateSpecificationObjectIsNotNull(specificationObject);

            Validate(
                (Rule: IsInvalid(specificationObject.Id), Parameter: nameof(SpecificationObject.Id)),
                (Rule: IsInvalid(specificationObject.DataSetSpecificationId), Parameter: nameof(specificationObject.DataSetSpecificationId)),
                (Rule: IsInvalid(specificationObject.SupplierObjectName), Parameter: nameof(specificationObject.SupplierObjectName)),
                (Rule: IsInvalid(specificationObject.OurObjectName), Parameter: nameof(specificationObject.OurObjectName)),
                (Rule: IsInvalid(specificationObject.CreatedDate), Parameter: nameof(SpecificationObject.CreatedDate)),
                (Rule: IsInvalid(specificationObject.CreatedBy), Parameter: nameof(SpecificationObject.CreatedBy)),
                (Rule: IsInvalid(specificationObject.UpdatedDate), Parameter: nameof(SpecificationObject.UpdatedDate)),
                (Rule: IsInvalid(specificationObject.UpdatedBy), Parameter: nameof(SpecificationObject.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.SupplierObjectName, 255), Parameter: nameof(specificationObject.SupplierObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.OurObjectName, 255), Parameter: nameof(specificationObject.OurObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.ObjectDescription, 500), Parameter: nameof(specificationObject.ObjectDescription)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.InterchangeProtocol, 255), Parameter: nameof(specificationObject.InterchangeProtocol)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.DeletionHandling, 255), Parameter: nameof(specificationObject.DeletionHandling)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.CreatedBy, 255), Parameter: nameof(specificationObject.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.UpdatedBy, 255), Parameter: nameof(specificationObject.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: specificationObject.UpdatedDate,
                    secondDate: specificationObject.CreatedDate,
                    secondDateName: nameof(SpecificationObject.CreatedDate)),
                Parameter: nameof(SpecificationObject.UpdatedDate)),

                (Rule: IsNotSame(
                    first: specificationObject.UpdatedBy,
                    second: specificationObject.CreatedBy,
                    secondName: nameof(SpecificationObject.CreatedBy)),
                Parameter: nameof(SpecificationObject.UpdatedBy)),

                (Rule: await IsNotRecentAsync(specificationObject.CreatedDate), Parameter: nameof(SpecificationObject.CreatedDate)));
        }

        private async ValueTask ValidateSpecificationObjectOnModifyAsync(SpecificationObject specificationObject)
        {
            ValidateSpecificationObjectIsNotNull(specificationObject);

            Validate(
                (Rule: IsInvalid(specificationObject.Id), Parameter: nameof(SpecificationObject.Id)),
                (Rule: IsInvalid(specificationObject.DataSetSpecificationId), Parameter: nameof(specificationObject.DataSetSpecificationId)),
                (Rule: IsInvalid(specificationObject.SupplierObjectName), Parameter: nameof(specificationObject.SupplierObjectName)),
                (Rule: IsInvalid(specificationObject.OurObjectName), Parameter: nameof(specificationObject.OurObjectName)),
                (Rule: IsInvalid(specificationObject.CreatedDate), Parameter: nameof(SpecificationObject.CreatedDate)),
                (Rule: IsInvalid(specificationObject.CreatedBy), Parameter: nameof(SpecificationObject.CreatedBy)),
                (Rule: IsInvalid(specificationObject.UpdatedDate), Parameter: nameof(SpecificationObject.UpdatedDate)),
                (Rule: IsInvalid(specificationObject.UpdatedBy), Parameter: nameof(SpecificationObject.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.SupplierObjectName, 255), Parameter: nameof(specificationObject.SupplierObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.OurObjectName, 255), Parameter: nameof(specificationObject.OurObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.ObjectDescription, 500), Parameter: nameof(specificationObject.ObjectDescription)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.InterchangeProtocol, 255), Parameter: nameof(specificationObject.InterchangeProtocol)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.DeletionHandling, 255), Parameter: nameof(specificationObject.DeletionHandling)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.CreatedBy, 255), Parameter: nameof(specificationObject.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    specificationObject.UpdatedBy, 255), Parameter: nameof(specificationObject.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: specificationObject.UpdatedDate,
                    secondDate: specificationObject.CreatedDate,
                    secondDateName: nameof(SpecificationObject.CreatedDate)),
                Parameter: nameof(SpecificationObject.UpdatedDate)),

                (Rule: await IsNotRecentAsync(specificationObject.UpdatedDate), Parameter: nameof(specificationObject.UpdatedDate)));
        }

        public void ValidateSpecificationObjectId(Guid specificationObjectId) =>
            Validate((Rule: IsInvalid(specificationObjectId), Parameter: nameof(SpecificationObject.Id)));

        private static void ValidateStorageSpecificationObject(SpecificationObject maybeSpecificationObject, Guid specificationObjectId)
        {
            if (maybeSpecificationObject is null)
            {
                throw new NotFoundSpecificationObjectException(specificationObjectId);
            }
        }

        private static void ValidateSpecificationObjectIsNotNull(SpecificationObject specificationObject)
        {
            if (specificationObject is null)
            {
                throw new NullSpecificationObjectException(message: "SpecificationObject is null.");
            }
        }

        private static void ValidateAgainstStorageSpecificationObjectOnModify(SpecificationObject inputSpecificationObject, SpecificationObject storageSpecificationObject)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputSpecificationObject.CreatedDate,
                    secondDate: storageSpecificationObject.CreatedDate,
                    secondDateName: nameof(SpecificationObject.CreatedDate)),
                Parameter: nameof(SpecificationObject.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputSpecificationObject.CreatedBy,
                    second: storageSpecificationObject.CreatedBy,
                    secondName: nameof(SpecificationObject.CreatedBy)),
                Parameter: nameof(SpecificationObject.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputSpecificationObject.UpdatedDate,
                    secondDate: storageSpecificationObject.UpdatedDate,
                    secondDateName: nameof(SpecificationObject.UpdatedDate)),
                Parameter: nameof(SpecificationObject.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsEqualOrSmallerThan(string text, int maxLength) => new
        {
            Condition = (text ?? string.Empty).Length > maxLength,
            Message = "Text is exceeding max length"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
           };

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date) => new
        {
            Condition = await IsDateNotRecentAsync(date),
            Message = "Date is not recent"
        };

        private async ValueTask<bool> IsDateNotRecentAsync(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSpecificationObjectException =
                new InvalidSpecificationObjectException(
                    message: "Invalid specificationObject. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSpecificationObjectException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSpecificationObjectException.ThrowIfContainsErrors();
        }
    }
}