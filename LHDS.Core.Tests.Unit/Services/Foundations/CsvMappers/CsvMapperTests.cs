// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Tests.Unit.Models.Foundations.CsvMappers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.CsvMappers
{
    public partial class CsvMapperTests
    {
        private readonly Mock<ICsvMapperBroker> csvMapperBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly CsvMapperService csvMapperService;

        public CsvMapperTests()
        {
            this.csvMapperBrokerMock = new Mock<ICsvMapperBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.csvMapperService = new CsvMapperService(
                csvMapperBroker: this.csvMapperBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<dynamic> CreateDynamicCars(List<Car> cars)
        {
            return cars
                .Select(car => new
                {
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Color = car.Color
                })
                .ToList<dynamic>();
        }


        private static List<Car> CreateRandomCars()
        {
            return CreateCarFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<Car> CreateCarFiller()
        {
            var filler = new Filler<Car>();
            filler.Setup();

            return filler;
        }

        public static TheoryData CsvToObjectMapperOptions()
        {
            return new TheoryData<bool, Dictionary<string, int>>
            {
                { true, null },
                { true, new Dictionary<string, int> { { nameof(OptOut.NhsNumber), 2 } } },
                { false, null },
                { false, new Dictionary<string, int>() }
            };
        }

        public static TheoryData ObjectToCsvMapperOptions()
        {
            return new TheoryData<bool, Dictionary<string, int>, bool>
            {
                { true, null, true },
                { true, new Dictionary<string, int> { { nameof(OptOut.NhsNumber), 2 } }, false },
                { false, null, false },
                { false, new Dictionary<string, int>(), true }
            };
        }

        private string GetCsvRepresentationOfCar(
            List<Car> cars,
            bool hasHeaderRow,
            bool shouldAddTrailingComma)
        {
            StringBuilder csvBuilder = new StringBuilder();

            if (hasHeaderRow)
            {
                csvBuilder.AppendLine("Make,Model,Year,Color");
            }

            foreach (var car in cars)
            {
                string line = $"{WrapInQuotesIfContainsComma(car.Make)}," +
                    $"{WrapInQuotesIfContainsComma(car.Model)}," +
                    $"{WrapInQuotesIfContainsComma(car.Year.ToString())}," +
                    $"{WrapInQuotesIfContainsComma(car.Color)}";

                if (shouldAddTrailingComma)
                {
                    line += ",";
                }

                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }

        private string GetCsvRepresentationOfCarInReverse(
            List<Car> cars,
            bool hasHeaderRow,
            bool shouldAddTrailingComma)
        {
            StringBuilder csvBuilder = new StringBuilder();

            if (hasHeaderRow)
            {
                csvBuilder.AppendLine("Color,Year,Model,Make");
            }

            foreach (var car in cars)
            {
                string line = $"{WrapInQuotesIfContainsComma(car.Color)}," +
                    $"{WrapInQuotesIfContainsComma(car.Year.ToString())}," +
                    $"{WrapInQuotesIfContainsComma(car.Model)}," +
                    $"{WrapInQuotesIfContainsComma(car.Make)}";

                if (shouldAddTrailingComma)
                {
                    line += ",";
                }

                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }

        private string WrapInQuotesIfContainsComma(string value)
        {
            if (value.Contains(","))
            {
                return $"\"{value}\"";
            }
            return value;
        }
    }
}
