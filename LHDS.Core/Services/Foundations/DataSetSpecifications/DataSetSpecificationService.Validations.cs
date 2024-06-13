// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private void ValidateDataSetSpecificationOnAdd(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);

            Validate(
                (Rule: IsInvalid(dataSetSpecification.Id), Parameter: nameof(DataSetSpecification.Id)),
                (Rule: IsInvalid(dataSetSpecification.DataSetId), Parameter: nameof(DataSetSpecification.DataSetId)),

                (Rule: IsInvalid(dataSetSpecification.SupplierSpecificationVersion),
                    Parameter: nameof(DataSetSpecification.SupplierSpecificationVersion)),

                (Rule: IsInvalid(dataSetSpecification.OurSpecificationVersion),
                    Parameter: nameof(DataSetSpecification.OurSpecificationVersion)),

                (Rule: IsInvalid(dataSetSpecification.CreatedDate),
                    Parameter: nameof(DataSetSpecification.CreatedDate)),

                (Rule: IsInvalid(dataSetSpecification.CreatedBy), Parameter: nameof(DataSetSpecification.CreatedBy)),

                (Rule: IsInvalid(dataSetSpecification.UpdatedDate),
                    Parameter: nameof(DataSetSpecification.UpdatedDate)),

                (Rule: IsInvalid(dataSetSpecification.UpdatedBy), Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.SupplierSpecificationVersion, 10),
                    Parameter: nameof(dataSetSpecification.SupplierSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.OurSpecificationVersion, 10),
                    Parameter: nameof(dataSetSpecification.OurSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.CreatedBy, 255),
                    Parameter: nameof(dataSetSpecification.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.UpdatedBy, 255),
                    Parameter: nameof(dataSetSpecification.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: dataSetSpecification.UpdatedDate,
                    secondDate: dataSetSpecification.CreatedDate,
                    secondDateName: nameof(DataSetSpecification.CreatedDate)),
                Parameter: nameof(DataSetSpecification.UpdatedDate)),

                (Rule: IsNotSame(
                    first: dataSetSpecification.UpdatedBy,
                    second: dataSetSpecification.CreatedBy,
                    secondName: nameof(DataSetSpecification.CreatedBy)),
                Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsNotRecent(dataSetSpecification.CreatedDate),
                    Parameter: nameof(DataSetSpecification.CreatedDate)));
        }

        private void ValidateDataSetSpecificationOnModify(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);

            Validate(
                (Rule: IsInvalid(dataSetSpecification.Id), Parameter: nameof(DataSetSpecification.Id)),
                (Rule: IsInvalid(dataSetSpecification.DataSetId), Parameter: nameof(DataSetSpecification.DataSetId)),

                (Rule: IsInvalid(dataSetSpecification.SupplierSpecificationVersion),
                    Parameter: nameof(DataSetSpecification.SupplierSpecificationVersion)),

                (Rule: IsInvalid(dataSetSpecification.OurSpecificationVersion),
                    Parameter: nameof(DataSetSpecification.OurSpecificationVersion)),

                (Rule: IsInvalid(dataSetSpecification.CreatedDate),
                    Parameter: nameof(DataSetSpecification.CreatedDate)),

                (Rule: IsInvalid(dataSetSpecification.CreatedBy), Parameter: nameof(DataSetSpecification.CreatedBy)),

                (Rule: IsInvalid(dataSetSpecification.UpdatedDate),
                    Parameter: nameof(DataSetSpecification.UpdatedDate)),

                (Rule: IsInvalid(dataSetSpecification.UpdatedBy), Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.SupplierSpecificationVersion, 10),
                    Parameter: nameof(dataSetSpecification.SupplierSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.OurSpecificationVersion, 10),
                    Parameter: nameof(dataSetSpecification.OurSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.CreatedBy, 255),
                    Parameter: nameof(dataSetSpecification.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.UpdatedBy, 255),
                    Parameter: nameof(dataSetSpecification.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: dataSetSpecification.UpdatedDate,
                    secondDate: dataSetSpecification.CreatedDate,
                    secondDateName: nameof(DataSetSpecification.CreatedDate)),
                        Parameter: nameof(DataSetSpecification.UpdatedDate)),

                (Rule: IsNotRecent(dataSetSpecification.UpdatedDate),
                    Parameter: nameof(dataSetSpecification.UpdatedDate)));
        }

        public void ValidateDataSetSpecificationId(Guid dataSetSpecificationId) =>
            Validate((Rule: IsInvalid(dataSetSpecificationId), Parameter: nameof(DataSetSpecification.Id)));

        private static void ValidateStorageDataSetSpecification(
            DataSetSpecification maybeDataSetSpecification,
            Guid dataSetSpecificationId)
        {
            if (maybeDataSetSpecification is null)
            {
                throw new NotFoundDataSetSpecificationException(dataSetSpecificationId);
            }
        }

        private static void ValidateDataSetSpecificationIsNotNull(DataSetSpecification dataSetSpecification)
        {
            if (dataSetSpecification is null)
            {
                throw new NullDataSetSpecificationException(message: "DataSetSpecification is null.");
            }
        }

        private static void ValidateAgainstStorageDataSetSpecificationOnModify(
            DataSetSpecification inputDataSetSpecification,
            DataSetSpecification storageDataSetSpecification)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputDataSetSpecification.CreatedDate,
                    secondDate: storageDataSetSpecification.CreatedDate,
                    secondDateName: nameof(DataSetSpecification.CreatedDate)),
                Parameter: nameof(DataSetSpecification.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputDataSetSpecification.CreatedBy,
                    second: storageDataSetSpecification.CreatedBy,
                    secondName: nameof(DataSetSpecification.CreatedBy)),
                Parameter: nameof(DataSetSpecification.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputDataSetSpecification.UpdatedDate,
                    secondDate: storageDataSetSpecification.UpdatedDate,
                    secondDateName: nameof(DataSetSpecification.UpdatedDate)),
                Parameter: nameof(DataSetSpecification.UpdatedDate)));
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
            Condition = LengthIsEqualOrSmallerThan(text, maxLength),
            Message = "Text is exceeding max length"
        };

        private static bool LengthIsEqualOrSmallerThan(string text, int maxLength)
        {
            return (text ?? string.Empty).Length > maxLength;
        }

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

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDataSetSpecificationException =
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataSetSpecificationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataSetSpecificationException.ThrowIfContainsErrors();
        }
    }
}