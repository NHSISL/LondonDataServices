// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.Files;
using LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.ReadSchema
{
    public partial class ReadSchemaOrchestrationServiceTests
    {
        private readonly Mock<IFileService> fileServiceMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly IReadSchemaOrchestrationService readSchemaOrchestrationService;

        public ReadSchemaOrchestrationServiceTests()
        {
            this.fileServiceMock = new Mock<IFileService>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();

            this.readSchemaOrchestrationService = new ReadSchemaOrchestrationService(
                fileService: this.fileServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<ObjectColumn> CreateRandomObjectColumns()
        {
            return CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static ObjectColumn CreateRandomObjectColumn() =>
            CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ObjectColumn CreateRandomObjectColumn(DateTimeOffset dateTimeOffset) =>
            CreateObjectColumnFiller(dateTimeOffset).Create();

        private static Filler<ObjectColumn> CreateObjectColumnFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
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