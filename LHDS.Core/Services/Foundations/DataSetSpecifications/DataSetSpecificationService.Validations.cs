// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private async ValueTask ValidateDataSetSpecificationOnAddAsync(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

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
                    Parameter: nameof(DataSetSpecification.SupplierSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.OurSpecificationVersion, 10),
                    Parameter: nameof(DataSetSpecification.OurSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.CreatedBy, 255),
                    Parameter: nameof(DataSetSpecification.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.UpdatedBy, 255),
                    Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: dataSetSpecification.CreatedBy),
                    Parameter: nameof(DataSetSpecification.CreatedBy)),

                (Rule: IsNotSame(
                    first: dataSetSpecification.UpdatedBy,
                    second: dataSetSpecification.CreatedBy,
                    secondName: nameof(DataSetSpecification.CreatedBy)),
                Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsNotSame(
                    first: dataSetSpecification.UpdatedDate,
                    second: dataSetSpecification.CreatedDate,
                    secondName: nameof(DataSetSpecification.CreatedDate)),
                Parameter: nameof(DataSetSpecification.UpdatedDate)),

                (Rule: await IsNotRecentAsync(dataSetSpecification.CreatedDate),
                    Parameter: nameof(DataSetSpecification.CreatedDate))
            );
        }

        private async ValueTask ValidateDataSetSpecificationOnModifyAsync(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

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
                    Parameter: nameof(DataSetSpecification.SupplierSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.OurSpecificationVersion, 10),
                    Parameter: nameof(DataSetSpecification.OurSpecificationVersion)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.CreatedBy, 255),
                    Parameter: nameof(DataSetSpecification.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(dataSetSpecification.UpdatedBy, 255),
                    Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: dataSetSpecification.UpdatedBy),
                Parameter: nameof(DataSetSpecification.UpdatedBy)),

                (Rule: IsSameAs(
                    firstDate: dataSetSpecification.CreatedDate,
                    secondDate: dataSetSpecification.UpdatedDate,
                    secondDateName: nameof(DataSetSpecification.CreatedDate)),
                Parameter: nameof(DataSetSpecification.UpdatedDate)),

                (Rule: await IsNotRecentAsync(dataSetSpecification.UpdatedDate),
                    Parameter: nameof(DataSetSpecification.UpdatedDate))
            );
        }

        private static void ValidateAgainstStorageDataSetSpecificationOnDelete(
            DataSetSpecification storageDataSetSpecification,
            string currentUserId)
                {
                    Validate(
                        (Rule: IsInvalid(
                            storageDataSetSpecification.CreatedBy), 
                            Parameter: nameof(DataSetSpecification.CreatedBy)),

                        (Rule: IsInvalid(
                            storageDataSetSpecification.CreatedDate), 
                            Parameter: nameof(DataSetSpecification.CreatedDate)),

                        (Rule: IsNotSame(
                            first: currentUserId,
                            second: storageDataSetSpecification.UpdatedBy,
                            secondName: nameof(DataSetSpecification.UpdatedBy)),
                        Parameter: nameof(DataSetSpecification.UpdatedBy)),

                        (Rule: IsNotSame(
                            storageDataSetSpecification.UpdatedDate,
                            storageDataSetSpecification.CreatedDate, 
                            nameof(DataSetSpecification.CreatedDate)),
                        Parameter: nameof(DataSetSpecification.UpdatedDate))
                    );
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

/*        private static void ValidateAgainstStorageDataSetSpecificationOnModify(
            DataSetSpecification inputDataSetSpecification,
            DataSetSpecification storageDataSetSpecification)
        { }*/

        private static void ValidateAgainstStorageDataSetSpecificationOnModify(
        DataSetSpecification inputDataSetSpecification,
        DataSetSpecification storageDataSetSpecification)
            {
                Validate(
                    (Rule: IsNotSame(
                        first: inputDataSetSpecification.CreatedBy,
                        second: storageDataSetSpecification.CreatedBy,
                        secondName: nameof(DataSetSpecification.CreatedBy)),
                    Parameter: nameof(DataSetSpecification.CreatedBy)),

                    (Rule: IsNotSame(
                        first: inputDataSetSpecification.CreatedDate,
                        second: storageDataSetSpecification.CreatedDate,
                        secondName: nameof(DataSetSpecification.CreatedDate)),
                    Parameter: nameof(DataSetSpecification.CreatedDate)),

                    (Rule: IsSameAs(
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

        private static dynamic IsSameAs(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            string first,
            string second) => new
            {
                Condition = first != second,
                Message = $"Expected value to be '{first}' but found '{second}'."
            };

        private static dynamic IsNotSame(
            string first,
            string second,
            string secondName) => new
            {
                Condition = first != second,
                Message = $"Text is not the same as {secondName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset first,
            DateTimeOffset second,
            string secondName) => new
            {
                Condition = first != second,
                Message = $"Date is not the same as {secondName}"
            };

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date)
        {
            var (isNotRecent, startDate, endDate) = await IsDateNotRecentAsync(date);

            return new
            {
                Condition = isNotRecent,
                Message = $"Date is not recent. Expected a value between {startDate} and {endDate} but found {date}"
            };
        }

        private async ValueTask<(bool IsNotRecent, DateTimeOffset StartDate, DateTimeOffset EndDate)>
            IsDateNotRecentAsync(DateTimeOffset date)
        {
            int pastThreshold = 90;
            int futureThreshold = 0;
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            if (currentDateTime == default)
            {
                return (false, default, default);
            }

            DateTimeOffset startDate = currentDateTime.AddSeconds(-pastThreshold);
            DateTimeOffset endDate = currentDateTime.AddSeconds(futureThreshold);
            bool isNotRecent = date < startDate || date > endDate;

            return (isNotRecent, startDate, endDate);
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