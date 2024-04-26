// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Brokers.CsvMappers
{
    public class CsvMapperBroker : ICsvMapperBroker
    {
        public CsvReader CreateCsvReader(StringReader reader, bool hasHeaderRecord)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                MissingFieldFound = null
            };

            return new CsvReader(reader, config);
        }

        public CsvWriter CreateCsvWriter(StringWriter writer, bool hasHeaderRecord)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                MissingFieldFound = null
            };

            return new CsvWriter(writer, config);
        }

        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings = null)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
                MissingFieldFound = null
            };

            using (var reader = new StringReader(data))
            using (var csv = new CsvReader(reader, config))
            {
                if (fieldMappings != null)
                {
                    csv.Context.RegisterClassMap(new CustomMap<T>(fieldMappings));
                }

                var records = csv.GetRecords<T>().ToList();

                return await ValueTask.FromResult(records);
            }
        }

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false)
        {
            var csvWriterConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = addHeaderRecord,
                NewLine = Environment.NewLine
            };

            using (var stringWriter = new StringWriter())
            using (var csvWriter = new CsvWriter(stringWriter, csvWriterConfig))
            {
                if (addHeaderRecord)
                {
                    csvWriter.WriteHeader<Address>();
                    csvWriter.NextRecord();
                }

                foreach (var item in @object)
                {
                    csvWriter.WriteRecord(item);

                    if (shouldAddTrailingComma.HasValue && shouldAddTrailingComma.Value == true)
                    {
                        csvWriter.WriteField("");
                    }

                    csvWriter.NextRecord();
                }

                var csv = stringWriter.ToString();

                return await ValueTask.FromResult(csv);
            }
        }
    }

    internal class CustomMap<T> : ClassMap<T>
    {
        public CustomMap(Dictionary<string, int> fieldMappings)
        {
            foreach (var mapping in fieldMappings)
            {
                try
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, mapping.Key);
                    var funcType = typeof(Func<,>).MakeGenericType(typeof(T), property.Type);
                    var lambda = Expression.Lambda(funcType, property, parameter);
                    var mapMethods = typeof(ClassMap<T>).GetMethods().Where(m => m.Name == "Map" && m.IsGenericMethod);

                    var mapMethod = mapMethods.First(m => m.GetParameters().First()
                        .ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

                    mapMethod = mapMethod.MakeGenericMethod(property.Type);
                    var memberMap = (MemberMap)mapMethod.Invoke(this, new object[] { lambda, true });
                    memberMap.Index(mapping.Value);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
