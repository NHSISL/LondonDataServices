// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectService
    {
        private void ValidateDataSetObjectOnAdd(SpecificationObject dataSetObject)
        {
            ValidateDataSetObjectIsNotNull(dataSetObject);

            Validate(
                (Rule: IsInvalid(dataSetObject.Id), Parameter: nameof(SpecificationObject.Id)),
                (Rule: IsInvalid(dataSetObject.DataSetSpecificationId), Parameter: nameof(dataSetObject.DataSetSpecificationId)),
                (Rule: IsInvalid(dataSetObject.SupplierObjectName), Parameter: nameof(dataSetObject.SupplierObjectName)),
                (Rule: IsInvalid(dataSetObject.OurObjectName), Parameter: nameof(dataSetObject.OurObjectName)),
                (Rule: IsInvalid(dataSetObject.CreatedDate), Parameter: nameof(SpecificationObject.CreatedDate)),
                (Rule: IsInvalid(dataSetObject.CreatedBy), Parameter: nameof(SpecificationObject.CreatedBy)),
                (Rule: IsInvalid(dataSetObject.UpdatedDate), Parameter: nameof(SpecificationObject.UpdatedDate)),
                (Rule: IsInvalid(dataSetObject.UpdatedBy), Parameter: nameof(SpecificationObject.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.SupplierObjectName, 255), Parameter: nameof(dataSetObject.SupplierObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.OurObjectName, 255), Parameter: nameof(dataSetObject.OurObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.ObjectDescription, 500), Parameter: nameof(dataSetObject.ObjectDescription)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.InterchangeProtocol, 255), Parameter: nameof(dataSetObject.InterchangeProtocol)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.DeletionHandling, 255), Parameter: nameof(dataSetObject.DeletionHandling)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.CreatedBy, 255), Parameter: nameof(dataSetObject.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.UpdatedBy, 255), Parameter: nameof(dataSetObject.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: dataSetObject.UpdatedDate,
                    secondDate: dataSetObject.CreatedDate,
                    secondDateName: nameof(SpecificationObject.CreatedDate)),
                Parameter: nameof(SpecificationObject.UpdatedDate)),

                (Rule: IsNotSame(
                    first: dataSetObject.UpdatedBy,
                    second: dataSetObject.CreatedBy,
                    secondName: nameof(SpecificationObject.CreatedBy)),
                Parameter: nameof(SpecificationObject.UpdatedBy)),

                (Rule: IsNotRecent(dataSetObject.CreatedDate), Parameter: nameof(SpecificationObject.CreatedDate)));
        }

        private void ValidateDataSetObjectOnModify(SpecificationObject dataSetObject)
        {
            ValidateDataSetObjectIsNotNull(dataSetObject);

            Validate(
                (Rule: IsInvalid(dataSetObject.Id), Parameter: nameof(SpecificationObject.Id)),
                (Rule: IsInvalid(dataSetObject.DataSetSpecificationId), Parameter: nameof(dataSetObject.DataSetSpecificationId)),
                (Rule: IsInvalid(dataSetObject.SupplierObjectName), Parameter: nameof(dataSetObject.SupplierObjectName)),
                (Rule: IsInvalid(dataSetObject.OurObjectName), Parameter: nameof(dataSetObject.OurObjectName)),
                (Rule: IsInvalid(dataSetObject.CreatedDate), Parameter: nameof(SpecificationObject.CreatedDate)),
                (Rule: IsInvalid(dataSetObject.CreatedBy), Parameter: nameof(SpecificationObject.CreatedBy)),
                (Rule: IsInvalid(dataSetObject.UpdatedDate), Parameter: nameof(SpecificationObject.UpdatedDate)),
                (Rule: IsInvalid(dataSetObject.UpdatedBy), Parameter: nameof(SpecificationObject.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.SupplierObjectName, 255), Parameter: nameof(dataSetObject.SupplierObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.OurObjectName, 255), Parameter: nameof(dataSetObject.OurObjectName)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.ObjectDescription, 500), Parameter: nameof(dataSetObject.ObjectDescription)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.InterchangeProtocol, 255), Parameter: nameof(dataSetObject.InterchangeProtocol)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.DeletionHandling, 255), Parameter: nameof(dataSetObject.DeletionHandling)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.CreatedBy, 255), Parameter: nameof(dataSetObject.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSetObject.UpdatedBy, 255), Parameter: nameof(dataSetObject.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: dataSetObject.UpdatedDate,
                    secondDate: dataSetObject.CreatedDate,
                    secondDateName: nameof(SpecificationObject.CreatedDate)),
                Parameter: nameof(SpecificationObject.UpdatedDate)),

                (Rule: IsNotRecent(dataSetObject.UpdatedDate), Parameter: nameof(dataSetObject.UpdatedDate)));
        }

        public void ValidateDataSetObjectId(Guid dataSetObjectId) =>
            Validate((Rule: IsInvalid(dataSetObjectId), Parameter: nameof(SpecificationObject.Id)));

        private static void ValidateStorageDataSetObject(SpecificationObject maybeDataSetObject, Guid dataSetObjectId)
        {
            if (maybeDataSetObject is null)
            {
                throw new NotFoundDataSetObjectException(dataSetObjectId);
            }
        }

        private static void ValidateDataSetObjectIsNotNull(SpecificationObject dataSetObject)
        {
            if (dataSetObject is null)
            {
                throw new NullDataSetObjectException(message: "DataSetObject is null.");
            }
        }

        private static void ValidateAgainstStorageDataSetObjectOnModify(SpecificationObject inputDataSetObject, SpecificationObject storageDataSetObject)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputDataSetObject.CreatedDate,
                    secondDate: storageDataSetObject.CreatedDate,
                    secondDateName: nameof(SpecificationObject.CreatedDate)),
                Parameter: nameof(SpecificationObject.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputDataSetObject.CreatedBy,
                    second: storageDataSetObject.CreatedBy,
                    secondName: nameof(SpecificationObject.CreatedBy)),
                Parameter: nameof(SpecificationObject.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputDataSetObject.UpdatedDate,
                    secondDate: storageDataSetObject.UpdatedDate,
                    secondDateName: nameof(SpecificationObject.UpdatedDate)),
                Parameter: nameof(SpecificationObject.UpdatedDate)));
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
            var invalidDataSetObjectException =
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataSetObjectException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataSetObjectException.ThrowIfContainsErrors();
        }
    }
}