// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using LHDS.Core.Services.Foundations.DataSets;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSets
{
    public partial class DataSetsControllerTests : RESTFulController
    {
        private readonly Mock<IDataSetService> dataSetServiceMock;
        private readonly DataSetsController dataSetsController;

        public DataSetsControllerTests()
        {
            this.dataSetServiceMock = new Mock<IDataSetService>();
            this.dataSetsController = new DataSetsController(this.dataSetServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<DataSet> CreateRandomDataSets() =>
            CreateDataSetFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static DataSet CreateRandomDataSet() =>
            CreateDataSetFiller().Create();

        private static Filler<DataSet> CreateDataSetFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.Supplier).IgnoreIt()
                .OnProperty(accessAudit => accessAudit.DataSetSpecifications).IgnoreIt();

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new DataSetDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new DataSetServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new DataSetValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new DataSetDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}