// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.DataTypes;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests : RESTFulController
    {
        private readonly Mock<IDataTypeService> dataTypeServiceMock;
        private readonly DataTypesController dataTypesController;

        public DataTypesControllerTests()
        {
            this.dataTypeServiceMock = new Mock<IDataTypeService>();
            this.dataTypesController = new DataTypesController(this.dataTypeServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<DataType> CreateRandomDataTypes() =>
            CreateDataTypeFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static DataType CreateRandomDataType() =>
            CreateDataTypeFiller().Create();

        private static Filler<DataType> CreateDataTypeFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataType>();

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
                new DataTypeDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new DataTypeServiceException(
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
                new DataTypeValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new DataTypeDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
