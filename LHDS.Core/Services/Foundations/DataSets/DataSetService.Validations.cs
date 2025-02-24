// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService
    {
        private async ValueTask ValidateDataSetOnAddAsync(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(dataSet.Id), Parameter: nameof(DataSet.Id)),
                (Rule: IsInvalid(dataSet.SupplierId), Parameter: nameof(DataSet.SupplierId)),
                (Rule: IsInvalid(dataSet.DataSetName), Parameter: nameof(DataSet.DataSetName)),
                (Rule: IsInvalid(dataSet.DataSetAliases), Parameter: nameof(DataSet.DataSetAliases)),
                (Rule: IsInvalid(dataSet.DataSetAuthor), Parameter: nameof(DataSet.DataSetAuthor)),
                (Rule: IsInvalid(dataSet.SpecifiedBy), Parameter: nameof(DataSet.SpecifiedBy)),
                (Rule: IsInvalid(dataSet.CollectedBy), Parameter: nameof(DataSet.CollectedBy)),
                (Rule: IsInvalid(dataSet.DataSourceType), Parameter: nameof(DataSet.DataSourceType)),
                (Rule: IsInvalid(dataSet.CreatedDate), Parameter: nameof(DataSet.CreatedDate)),
                (Rule: IsInvalid(dataSet.CreatedBy), Parameter: nameof(DataSet.CreatedBy)),
                (Rule: IsInvalid(dataSet.UpdatedDate), Parameter: nameof(DataSet.UpdatedDate)),
                (Rule: IsInvalid(dataSet.UpdatedBy), Parameter: nameof(DataSet.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSetName, 150), Parameter: nameof(dataSet.DataSetName)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSetAliases, 250), Parameter: nameof(dataSet.DataSetAliases)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSetAuthor, 150), Parameter: nameof(dataSet.DataSetAuthor)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSourceType, 50), Parameter: nameof(dataSet.DataSourceType)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.CreatedBy, 255), Parameter: nameof(dataSet.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.UpdatedBy, 255), Parameter: nameof(dataSet.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: dataSet.UpdatedDate,
                    secondDate: dataSet.CreatedDate,
                    secondDateName: nameof(DataSet.CreatedDate)),
                Parameter: nameof(DataSet.UpdatedDate)),

                (Rule: IsNotSame(
                    first: dataSet.UpdatedBy,
                    second: dataSet.CreatedBy,
                    secondName: nameof(DataSet.CreatedBy)),
                Parameter: nameof(DataSet.UpdatedBy)),

                (Rule: await IsNotRecentAsync(dataSet.CreatedDate), Parameter: nameof(DataSet.CreatedDate)));
        }

        private async ValueTask ValidateDataSetOnModifyAsync(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);

            Validate(
                (Rule: IsInvalid(dataSet.Id), Parameter: nameof(DataSet.Id)),
                (Rule: IsInvalid(dataSet.SupplierId), Parameter: nameof(DataSet.SupplierId)),
                (Rule: IsInvalid(dataSet.DataSetName), Parameter: nameof(DataSet.DataSetName)),
                (Rule: IsInvalid(dataSet.DataSetAliases), Parameter: nameof(DataSet.DataSetAliases)),
                (Rule: IsInvalid(dataSet.DataSetAuthor), Parameter: nameof(DataSet.DataSetAuthor)),
                (Rule: IsInvalid(dataSet.SpecifiedBy), Parameter: nameof(DataSet.SpecifiedBy)),
                (Rule: IsInvalid(dataSet.CollectedBy), Parameter: nameof(DataSet.CollectedBy)),
                (Rule: IsInvalid(dataSet.DataSourceType), Parameter: nameof(DataSet.DataSourceType)),
                (Rule: IsInvalid(dataSet.CreatedDate), Parameter: nameof(DataSet.CreatedDate)),
                (Rule: IsInvalid(dataSet.CreatedBy), Parameter: nameof(DataSet.CreatedBy)),
                (Rule: IsInvalid(dataSet.UpdatedDate), Parameter: nameof(DataSet.UpdatedDate)),
                (Rule: IsInvalid(dataSet.UpdatedBy), Parameter: nameof(DataSet.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSetName, 150), Parameter: nameof(dataSet.DataSetName)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSetAliases, 250), Parameter: nameof(dataSet.DataSetAliases)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSetAuthor, 150), Parameter: nameof(dataSet.DataSetAuthor)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.DataSourceType, 50), Parameter: nameof(dataSet.DataSourceType)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.CreatedBy, 255), Parameter: nameof(dataSet.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataSet.UpdatedBy, 255), Parameter: nameof(dataSet.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: dataSet.UpdatedDate,
                    secondDate: dataSet.CreatedDate,
                    secondDateName: nameof(DataSet.CreatedDate)),
                Parameter: nameof(DataSet.UpdatedDate)),

                (Rule: await IsNotRecentAsync(dataSet.UpdatedDate), Parameter: nameof(dataSet.UpdatedDate)));
        }

        public void ValidateDataSetId(Guid dataSetId) =>
            Validate((Rule: IsInvalid(dataSetId), Parameter: nameof(DataSet.Id)));

        private static void ValidateStorageDataSet(DataSet maybeDataSet, Guid dataSetId)
        {
            if (maybeDataSet is null)
            {
                throw new NotFoundDataSetException(dataSetId);
            }
        }

        private static void ValidateDataSetIsNotNull(DataSet dataSet)
        {
            if (dataSet is null)
            {
                throw new NullDataSetException(message: "DataSet is null.");
            }
        }

        private static void ValidateAgainstStorageDataSetOnModify(DataSet inputDataSet, DataSet storageDataSet)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputDataSet.CreatedDate,
                    secondDate: storageDataSet.CreatedDate,
                    secondDateName: nameof(DataSet.CreatedDate)),
                Parameter: nameof(DataSet.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputDataSet.CreatedBy,
                    second: storageDataSet.CreatedBy,
                    secondName: nameof(DataSet.CreatedBy)),
                Parameter: nameof(DataSet.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputDataSet.UpdatedDate,
                    secondDate: storageDataSet.UpdatedDate,
                    secondDateName: nameof(DataSet.UpdatedDate)),
                Parameter: nameof(DataSet.UpdatedDate)));
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
            var invalidDataSetException =
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataSetException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataSetException.ThrowIfContainsErrors();
        }
    }
}