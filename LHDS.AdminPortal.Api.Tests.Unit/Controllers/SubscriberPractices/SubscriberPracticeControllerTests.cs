// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberPractices
{
    public partial class SubscriberPracticeControllerTests : RESTFulController
    {
        private readonly Mock<ISubscriberPracticeService> subscriberPracticeServiceMock;
        private readonly SubscriberPracticesController subscriberPracticesController;

        public SubscriberPracticeControllerTests()
        {
            this.subscriberPracticeServiceMock = new Mock<ISubscriberPracticeService>();

            this.subscriberPracticesController = new SubscriberPracticesController(
                this.subscriberPracticeServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<SubscriberPractice> CreateRandomSubscriberPractices() =>
            CreateSubscriberPracticeFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static SubscriberPractice CreateRandomSubscriberPractice() =>
            CreateSubscriberPracticeFiller().Create();

        private static Filler<SubscriberPractice> CreateSubscriberPracticeFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberPractice>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.SubscriberAgreement).IgnoreIt();

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new SubscriberPracticeDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new SubscriberPracticeServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
       }
}
