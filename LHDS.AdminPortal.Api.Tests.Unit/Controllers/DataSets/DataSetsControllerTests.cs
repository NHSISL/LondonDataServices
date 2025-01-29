// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSets;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;

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

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<DataSet> CreateRandomDataSet()
        {
            return CreateDataSetFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<DataSet> CreateDataSetFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user);

            return filler;
        }
    }
}
