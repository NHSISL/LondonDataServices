// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.ResolvedAddressParsers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserTests
    {
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly ICsvMapperBroker csvMapperBroker;
        private readonly ResolvedAddressParserService addressParserService;
        private readonly ICompareLogic compareLogic;

        public ResolvedAddressParserTests()
        {
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.csvMapperBroker = new CsvMapperBroker();
            this.compareLogic = new CompareLogic();

            this.addressParserService = new ResolvedAddressParserService(
                csvMapperBroker: this.csvMapperBroker,
                identifierBroker: this.identifierBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Expression<Func<ResolvedAddress, bool>> SameResolvedAddressAs(ResolvedAddress expectedResolvedAddress)
        {
            return actualResolvedAddress =>
                this.compareLogic.Compare(expectedResolvedAddress, actualResolvedAddress)
                    .AreEqual;
        }

        private static string GetRandomString() =>
          new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static List<dynamic> GetRandomDynamicAddreses(Guid identifier)
        {
            return new List<dynamic>
            {
                new
                {
                    Id = identifier,
                    UniqueReference = Guid.NewGuid(),
                    PostCode = GetRandomString(),
                    UnstructuredPostalAddress = GetRandomString()
                },
                new
                {
                    Id = identifier,
                    UniqueReference = Guid.NewGuid(),
                    PostCode = GetRandomString(),
                    UnstructuredPostalAddress = GetRandomString()
                }
            };
        }

        private string CreateStringAddressFromDynamic(List<dynamic> data)
        {
            throw new NotImplementedException();
        }

        private List<ResolvedAddress> CreateResolvedAddressFromDynamic(List<dynamic> data)
        {
            throw new NotImplementedException();
        }


    }
}
