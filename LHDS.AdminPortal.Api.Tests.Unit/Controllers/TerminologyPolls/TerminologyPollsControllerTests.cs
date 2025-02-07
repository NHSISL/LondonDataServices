// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests : RESTFulController
    {
        private readonly Mock<ITerminologyPollService> dataTypeServiceMock;
        private readonly TerminologyPollsController dataTypesController;

        public TerminologyPollsControllerTests()
        {
            this.dataTypeServiceMock = new Mock<ITerminologyPollService>();
            this.dataTypesController = new TerminologyPollsController(this.dataTypeServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<TerminologyPoll> CreateRandomTerminologyPolls() =>
            CreateTerminologyPollFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static TerminologyPoll CreateRandomTerminologyPoll() =>
            CreateTerminologyPollFiller().Create();

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyPoll>();

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
                new TerminologyPollDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new TerminologyPollServiceException(
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
                new TerminologyPollValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new TerminologyPollDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}