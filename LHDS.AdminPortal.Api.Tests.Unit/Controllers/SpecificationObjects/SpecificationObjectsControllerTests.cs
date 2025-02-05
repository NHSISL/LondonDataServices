// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using LHDS.Core.Services.Foundations.SpecificationObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SpecificationObjects
{
    public partial class SpecificationObjectsControllerTests
    {
        private readonly Mock<ISpecificationObjectService> specificationObjectServiceMock;
        private readonly SpecificationObjectsController specificationObjectsController;

        public SpecificationObjectsControllerTests()
        {
            this.specificationObjectServiceMock = new Mock<ISpecificationObjectService>();

            this.specificationObjectsController = new SpecificationObjectsController(
                this.specificationObjectServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<SpecificationObject> CreateRandomSpecificationObjects() =>
            CreateSpecificationObjectFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static SpecificationObject CreateRandomSpecificationObject() =>
            CreateSpecificationObjectFiller().Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SpecificationObject>();

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
                new SpecificationObjectDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new SpecificationObjectServiceException(
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
                new SpecificationObjectValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new SpecificationObjectDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}