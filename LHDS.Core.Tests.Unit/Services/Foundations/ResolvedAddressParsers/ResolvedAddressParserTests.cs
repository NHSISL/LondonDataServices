// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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
        private readonly Mock<ICsvMapperBroker> csvMapperBrokerMock;
        private readonly ResolvedAddressParserService addressParserService;
        private readonly ICompareLogic compareLogic;

        public ResolvedAddressParserTests()
        {
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.csvMapperBrokerMock = new Mock<ICsvMapperBroker>();
            this.compareLogic = new CompareLogic();

            this.addressParserService = new ResolvedAddressParserService(
                csvMapperBroker: this.csvMapperBrokerMock.Object,
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

        private static List<dynamic> GetRandomDynamicAddreses(Guid identifier, int count)
        {
            List<dynamic> addresses = new List<dynamic>();

            for (int i = 0; i < count; i++)
            {
                dynamic address = new
                {
                    Id = identifier,
                    UniqueReference = Guid.NewGuid(),
                    PostCode = GetRandomString(),
                    UnstructuredPostalAddress = GetRandomString()
                };

                addresses.Add(address);
            }

            return addresses;
        }

        private List<string[]> CreateStringArrayFromDynamic(List<dynamic> data)
        {
            List<string[]> addresses = new List<string[]>();

            foreach (dynamic item in data)
            {
                string[] address = new string[]
                {
                    item.UniqueReference.ToString(),
                    item.PostCode,
                    item.UnstructuredPostalAddress
                };

                addresses.Add(address);
            }

            return addresses;
        }

        private string CreateStringAddressFromDynamic(List<dynamic> data, bool hasHeaderRecord)
        {
            StringBuilder addresses = new StringBuilder();

            if (hasHeaderRecord)
            {
                addresses.AppendLine("UniqueReference,PostCode,UnstructuredPostalAddress");
            }

            foreach (dynamic item in data)
            {
                string uniqueReference = item.UniqueReference.ToString();
                string postCode = item.PostCode;
                string unstructuredPostalAddress = item.UnstructuredPostalAddress;

                addresses.AppendLine($"{uniqueReference},{postCode},{unstructuredPostalAddress}");
            }

            return addresses.ToString();
        }

        private List<ResolvedAddress> CreateResolvedAddressFromDynamic(List<dynamic> data)
        {
            List<ResolvedAddress> addresses = new List<ResolvedAddress>();

            foreach (dynamic item in data)
            {
                ResolvedAddress address = new ResolvedAddress
                {
                    Id = item.Id,
                    UniqueReference = item.UniqueReference,
                    PostCode = item.PostCode,
                    UnstructuredPostalAddress = item.UnstructuredPostalAddress
                };

                addresses.Add(address);
            }

            return addresses;
        }
    }
}
