// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests : RESTFulController
    {
        private readonly Mock<ITerminologyArtifactService> terminologyArtifactsServiceMock;
        private readonly TerminologyArtifactsController terminologyArtifactsController;

        public TerminologyArtifactsControllerTests()
        {
            this.terminologyArtifactsServiceMock = new Mock<ITerminologyArtifactService>();
            this.terminologyArtifactsController = new TerminologyArtifactsController(this.terminologyArtifactsServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<TerminologyArtifact> CreateRandomTerminologyArtifacts() =>
            CreateTerminologyArtifactFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static TerminologyArtifact CreateRandomTerminologyArtifact() =>
            CreateTerminologyArtifactFiller().Create();

        private static Filler<TerminologyArtifact> CreateTerminologyArtifactFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

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
                new TerminologyArtifactDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new TerminologyArtifactServiceException(
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
                new TerminologyArtifactValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new TerminologyArtifactDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
