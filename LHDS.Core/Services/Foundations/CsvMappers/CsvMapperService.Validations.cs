// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using Xeptions;

namespace LHDS.Core.Brokers.CsvMappers
{
    public partial class CsvMapperService : ICsvMapperService
    {
        private static void ValidateMapCsvToObjectArguments(string data, bool hasHeaderRecord)
        {
            Validate<InvalidCsvMapperArgumentsException>(
                    (Rule: IsInvalid(data), Parameter: "Data"));
        }

        private static void ValidateMapObjectToCsvArguments<T>(T @object, bool addHeaderRecord)
        {
            Validate<InvalidCsvMapperArgumentsException>(
                    (Rule: IsInvalid(@object), Parameter: "Object"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Object is required"
        };

        private static void Validate<T>(params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T));

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
