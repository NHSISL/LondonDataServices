// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Services.Foundations.ObjectColumns;
using LHDS.Core.Services.Processings.ObjectColumns;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        private readonly Mock<IObjectColumnService> objectColumnServiceMock = new Mock<IObjectColumnService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IObjectColumnProcessingService objectColumnProcessingService;

        public ObjectColumnProcessingServiceTests()
        {
            objectColumnProcessingService = new ObjectColumnProcessingService(
                objectColumnService: objectColumnServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ObjectColumn CreateRandomObjectColumn() =>
            CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<ObjectColumn> CreateObjectColumnFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(objectColumn => objectColumn.CreatedBy).Use(user)
                .OnProperty(objectColumn => objectColumn.UpdatedBy).Use(user);

            return filler;
        }
    }
}
