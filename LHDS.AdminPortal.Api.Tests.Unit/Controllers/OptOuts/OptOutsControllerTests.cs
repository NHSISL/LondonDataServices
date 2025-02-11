// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Processings.OptOuts;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests : RESTFulController
    {
        private readonly Mock<IOptOutProcessingService> optOutProcessingServiceMock;
        private readonly OptOutsController optOutsController;

        public OptOutsControllerTests()
        {
            this.optOutProcessingServiceMock = new Mock<IOptOutProcessingService>();

            this.optOutsController = new OptOutsController(
                this.optOutProcessingServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<OptOut> CreateRandomOptOuts() =>
            CreateOptOutFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static OptOut CreateRandomOptOut() =>
            CreateOptOutFiller().Create();

        private static Filler<OptOut> CreateOptOutFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

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
                new OptOutProcessingDependencyException(
                    message : someMessage,
                    innerException: someInnerException),

                new OptOutProcessingServiceException(
                    message : someMessage,
                    innerException: someInnerException)
            };
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new OptOutProcessingValidationException(
                    message : someMessage,
                    innerException: someInnerException),

                new OptOutProcessingDependencyValidationException(
                    message : someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
