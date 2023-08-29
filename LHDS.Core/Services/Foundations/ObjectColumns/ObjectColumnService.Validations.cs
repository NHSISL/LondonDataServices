using System;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnService
    {
        private void ValidateObjectColumnOnAdd(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);

            Validate(
                (Rule: IsInvalid(objectColumn.Id), Parameter: nameof(ObjectColumn.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(objectColumn.CreatedDate), Parameter: nameof(ObjectColumn.CreatedDate)),
                (Rule: IsInvalid(objectColumn.CreatedBy), Parameter: nameof(ObjectColumn.CreatedBy)),
                (Rule: IsInvalid(objectColumn.UpdatedDate), Parameter: nameof(ObjectColumn.UpdatedDate)),
                (Rule: IsInvalid(objectColumn.UpdatedBy), Parameter: nameof(ObjectColumn.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: objectColumn.UpdatedDate,
                    secondDate: objectColumn.CreatedDate,
                    secondDateName: nameof(ObjectColumn.CreatedDate)),
                Parameter: nameof(ObjectColumn.UpdatedDate)),

                (Rule: IsNotSame(
                    first: objectColumn.UpdatedBy,
                    second: objectColumn.CreatedBy,
                    secondName: nameof(ObjectColumn.CreatedBy)),
                Parameter: nameof(ObjectColumn.UpdatedBy)),

                (Rule: IsNotRecent(objectColumn.CreatedDate), Parameter: nameof(ObjectColumn.CreatedDate)));
        }

        private void ValidateObjectColumnOnModify(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);

            Validate(
                (Rule: IsInvalid(objectColumn.Id), Parameter: nameof(ObjectColumn.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(objectColumn.CreatedDate), Parameter: nameof(ObjectColumn.CreatedDate)),
                (Rule: IsInvalid(objectColumn.CreatedBy), Parameter: nameof(ObjectColumn.CreatedBy)),
                (Rule: IsInvalid(objectColumn.UpdatedDate), Parameter: nameof(ObjectColumn.UpdatedDate)),
                (Rule: IsInvalid(objectColumn.UpdatedBy), Parameter: nameof(ObjectColumn.UpdatedBy)));
        }

        public void ValidateObjectColumnId(Guid objectColumnId) =>
            Validate((Rule: IsInvalid(objectColumnId), Parameter: nameof(ObjectColumn.Id)));

        private static void ValidateStorageObjectColumn(ObjectColumn maybeObjectColumn, Guid objectColumnId)
        {
            if (maybeObjectColumn is null)
            {
                throw new NotFoundObjectColumnException(objectColumnId);
            }
        }

        private static void ValidateObjectColumnIsNotNull(ObjectColumn objectColumn)
        {
            if (objectColumn is null)
            {
                throw new NullObjectColumnException(message: "ObjectColumn is null.");
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
            var invalidObjectColumnException = 
                new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidObjectColumnException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidObjectColumnException.ThrowIfContainsErrors();
        }
    }
}