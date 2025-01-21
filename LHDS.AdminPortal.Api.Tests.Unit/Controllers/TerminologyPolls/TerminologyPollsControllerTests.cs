// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests : RESTFulController
    {

        private readonly Mock<ITerminologyPollService> terminologyPollsServiceMock;
        private readonly TerminologyPollsController terminologyPollsController;

        public TerminologyPollsControllerTests()
        {
            terminologyPollsServiceMock = new Mock<ITerminologyPollService>();
            terminologyPollsController = new TerminologyPollsController(terminologyPollsServiceMock.Object);
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

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static TerminologyPoll CreateRandomTerminologyPoll() =>
            CreateTerminologyPollFiller().Create();

        private static IQueryable<TerminologyPoll> CreateRandomTerminologyPolls()
        {
            return CreateTerminologyPollFiller()
                .Create(count: GetRandomNumber())
                .AsQueryable();
        }

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(poll => poll.ResourceType).Use(GetRandomStringWithLengthOf(50))
                .OnProperty(poll => poll.CreatedBy).Use(user)
                .OnProperty(poll => poll.UpdatedBy).Use(user)
                .OnProperty(poll => poll.CreatedDate).Use(dateTimeOffset)
                .OnProperty(poll => poll.UpdatedDate).Use(dateTimeOffset)
                .OnProperty(poll => poll.LastPoll).Use(dateTimeOffset);

            return filler;
        }
    }
}
