// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        private readonly Mock<IResolvedAddressService> resolvedAddressServiceMock;
        private readonly ResolvedAddressesController resolvedAddressesController;

        public ResolvedAddressesControllerTests()
        {
            this.resolvedAddressServiceMock = new Mock<IResolvedAddressService>();
            this.resolvedAddressesController = new ResolvedAddressesController(this.resolvedAddressServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<ResolvedAddress> CreateRandomResolvedAddresses() =>
            CreateResolvedAddressesFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static ResolvedAddress CreateRandomResolvedAddress() =>
            CreateResolvedAddressesFiller().Create();

        private static Filler<ResolvedAddress> CreateResolvedAddressesFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

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
                new ResolvedAddressDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new ResolvedAddressServiceException(
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
                new ResolvedAddressValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new ResolvedAddressDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}