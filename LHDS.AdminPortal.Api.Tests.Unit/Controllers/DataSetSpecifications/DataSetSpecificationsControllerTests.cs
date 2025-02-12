// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests : RESTFulController
    {
        private readonly Mock<IDataSetSpecificationService> dataSetSpecificationServiceMock;
        private readonly DataSetSpecificationsController dataSetSpecificationController;

        public DataSetSpecificationsControllerTests()
        {
            this.dataSetSpecificationServiceMock = new Mock<IDataSetSpecificationService>();

            this.dataSetSpecificationController = new DataSetSpecificationsController(
                this.dataSetSpecificationServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications()
        {
            return CreateDataSetSpecificationFiller().Create(count: GetRandomNumber()).AsQueryable();
        }

        private static DataSetSpecification CreateRandomDataSetSpecification() =>
            CreateDataSetSpecificationFiller().Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user);

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new DataSetSpecificationDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new DataSetSpecificationServiceException(
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
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new DataSetSpecificationDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}