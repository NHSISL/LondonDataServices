// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private void ValidateOnRetrieveListOfDocumentsToProcessAsync(Download download)
        {
            ValidateDownloadIsNotNull(download);
            ValidateSubscriberCredentialsIsNotNull(download.SubscriberCredential);
        }

        private void ValidateOnRetrieveDownloadByFileName(Download download)
        {
            ValidateDownloadIsNotNull(download);
            ValidateSubscriberCredentialsIsNotNull(download.SubscriberCredential);
            ValidateDocumentIsNotNull(download.Document);
            ValidateDocument(download.Document);
        }

        private void ValidateDocument(Document document) =>
            Validate((Rule: IsInvalid(document.FileName), Parameter: nameof(Document.FileName)));

        private static void ValidateStorageDownload(Download maybeDownload, string fileName)
        {
            if (maybeDownload is null)
            {
                throw new NotFoundDownloadException(message: $"Couldn't find download with file name: {fileName}.");
            }
        }

        private static void ValidateDownloadIsNotNull(Download download)
        {
            if (download is null)
            {
                throw new NullDownloadException(message: "Download is null.");
            }
        }

        private static void ValidateSubscriberCredentialsIsNotNull(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential is null)
            {
                throw new NullSubscriberCredentialException(message: "SubscriberCredential is null.");
            }
        }

        private static void ValidateDocumentIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDocumentException(message: "Document is null.");
            }
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
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
            var invalidDownloadException = new InvalidDownloadException(
                message: "Invalid download. Please correct the errors and try again.");

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