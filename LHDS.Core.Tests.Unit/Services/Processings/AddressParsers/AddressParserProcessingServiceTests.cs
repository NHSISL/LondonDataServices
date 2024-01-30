// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Processings.AddressParsers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock =
            new Mock<IAddressParserService>();

        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAddressParserProcessingService addressParserProcessingService;

        public AddressParserProcessingServiceTests()
        {
            this.addressParserProcessingService = new AddressParserProcessingService(
                this.addressParserServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
          new DateTimeRange(earliestDate: new DateTime()).GetValue();

        //private static async Task<List<Address>> CreateRandomAddressesAsync()
        //{
        //    var dateTimeOffset = GetRandomDateTimeOffset();
        //    var count = GetRandomNumber();

        //    var filler = CreateAddressFiller(dateTimeOffset);
        //    var addresses = filler.Create(count: GetRandomNumber()).AsQueryable();

        //    return await addresses.ToListAsync();
        //}

        //private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        //{
        //    string user = Guid.NewGuid().ToString();
        //    var filler = new Filler<Address>();

        //    filler.Setup()
        //        .OnType<DateTimeOffset>().Use(dateTimeOffset)
        //        .OnProperty(address => address.CreatedBy).Use(user)
        //        .OnProperty(address => address.UpdatedBy).Use(user);

        //    return filler;
        //}
    }
}
