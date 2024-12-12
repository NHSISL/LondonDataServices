// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace LHDS.Core.Models.Foundations.OptOuts
{
    internal class NullableDateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            DateTimeOffset result;

            if (DateTimeOffset.TryParseExact(text, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out result))
            {
                return result;
            }

            return null;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return string.Empty;

            return ((DateTimeOffset)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}