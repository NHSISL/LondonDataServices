// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using CsvHelper.Configuration.Attributes;

namespace LHDS.Core.Models.Foundations.OptOuts
{
    public class OptOutIdentifier
    {
        [Name("UniqueReference")]
        public string UniqueReference { get; set; } = string.Empty;

        [Name("NHSNo")]
        public string NhsNumber { get; set; } = string.Empty;

        [Name("Status")]
        public string Status { get; set; } = "Unknown";

        [Name("StatusChangedDateTime")]
        [TypeConverter(typeof(NullableDateTimeConverter))]
        public DateTimeOffset? StatusChangedDateTime { get; set; } = null;
    }
}