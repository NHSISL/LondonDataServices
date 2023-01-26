using System;
using LHDS.Landings.Client.Models.Downloads;
using LHDS.Landings.Client.Models.Downloads.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private void ValidateDownloadOnAdd(Download download)
        {
            ValidateDownloadIsNotNull(download);

            Validate(
                (Rule: IsInvalid(download.Id), Parameter: nameof(Download.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(download.CreatedDate), Parameter: nameof(Download.CreatedDate)),
                (Rule: IsInvalid(download.CreatedByUserId), Parameter: nameof(Download.CreatedByUserId)),
                (Rule: IsInvalid(download.UpdatedDate), Parameter: nameof(Download.UpdatedDate)),
                (Rule: IsInvalid(download.UpdatedByUserId), Parameter: nameof(Download.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: download.UpdatedDate,
                    secondDate: download.CreatedDate,
                    secondDateName: nameof(Download.CreatedDate)),
                Parameter: nameof(Download.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: download.UpdatedByUserId,
                    secondId: download.CreatedByUserId,
                    secondIdName: nameof(Download.CreatedByUserId)),
                Parameter: nameof(Download.UpdatedByUserId)),

                (Rule: IsNotRecent(download.CreatedDate), Parameter: nameof(Download.CreatedDate)));
        }

        public void ValidateDownloadId(Guid downloadId) =>
            Validate((Rule: IsInvalid(downloadId), Parameter: nameof(Download.Id)));

        private static void ValidateStorageDownload(Download maybeDownload, Guid downloadId)
        {
            if (maybeDownload is null)
            {
                throw new NotFoundDownloadException(downloadId);
            }
        }

        private static void ValidateDownloadIsNotNull(Download download)
        {
            if (download is null)
            {
                throw new NullDownloadException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
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
            var invalidDownloadException = new InvalidDownloadException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDownloadException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDownloadException.ThrowIfContainsErrors();
        }
    }
}