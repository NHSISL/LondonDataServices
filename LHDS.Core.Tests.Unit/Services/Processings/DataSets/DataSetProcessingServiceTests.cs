// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Processings.DataSets;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        private readonly Mock<IDataSetService> dataSetServiceMock;
        private readonly IDataSetProcessingService dataSetProcessingService;

        public DataSetProcessingServiceTests()
        {
            dataSetServiceMock = new Mock<IDataSetService>();

            dataSetProcessingService = new DataSetProcessingService(
                               dataSetService: dataSetServiceMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DataSet CreateRandomDataSet() =>
            CreateDataSetFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<DataSet> CreateDataSetFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user);

            return filler;
        }
    }
}