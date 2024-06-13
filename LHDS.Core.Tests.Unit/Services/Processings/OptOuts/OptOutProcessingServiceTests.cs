// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Processings.OptOuts;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        private readonly Mock<IOptOutService> optOutServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly OptOutProcessingService optOutProcessingService;

        public OptOutProcessingServiceTests()
        {
            optOutServiceMock = new Mock<IOptOutService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            compareLogic = new CompareLogic();

            this.optOutProcessingService = new OptOutProcessingService(
                optOutService: optOutServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
            CreateOptOutFiller(dateTimeOffset).Create();

        private static OptOut CreateRandomOptOut() =>
            CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomExpiryDays(int expiryMax) =>
            new IntRange(max: expiryMax - 1).GetValue();

        private static int GetRandomValidExpiryDays(int expiryMin) =>
            new IntRange(min: expiryMin + 1, max: 20).GetValue();

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException),

                new OptOutDependencyValidationException(
                    message: "OptOut dependency validation occurred, please try again.",
                    innerException)
            };
        }
        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OptOutDependencyException(
                    message: "OptOut dependency error occurred, please contact support.",
                    innerException),

                new OptOutServiceException(message: "OptOut service error occurred, please contact support.", innerException)
            };
        }

        private Expression<Func<OptOut, bool>> SameOptOutAs(
            OptOut expectedOptOut)
        {
            return actualOptOut =>
                this.compareLogic.Compare(expectedOptOut, actualOptOut)
                    .AreEqual;
        }

        private static List<string> RandomStringList(int count)
        {
            var list = new List<string>();

            for (int i = 0; i < count; i++)
            {
                list.Add(GetRandomString());
            }

            return list;
        }

        private static IQueryable<OptOut> CreateRandomOptOuts(string batchReference)
        {
            List<OptOut> optOuts = new List<OptOut>();

            for (int i = 0; i < 6; i++)
            {
                OptOut randomOptOut = CreateRandomOptOut(GetRandomDateTimeOffset());
                optOuts.Add(randomOptOut);

                if (i % 2 == 0)
                {
                    randomOptOut.BatchReference = batchReference;
                }
            }

            return optOuts.AsQueryable();
        }

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.Status).Use(GetRandomString(length: 20))
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }

        private static IQueryable<OptOut> CreateRandomOptOuts()
        {
            return CreateRandomOptOuts(GetRandomDateTimeOffset());
        }

        private static IQueryable<OptOut> CreateRandomOptOuts(DateTimeOffset expireDate)
        {
            List<OptOut> optOuts = new List<OptOut>();
            DateTimeOffset start = expireDate.AddDays(-3);

            for (int i = 0; i < 6; i++)
            {
                optOuts.Add(CreateRandomOptOut(start.AddDays(i)));
            }

            return optOuts.AsQueryable();
        }

        private static List<OptOut> CreateRandomOptOutList(string status)
        {
            List<OptOut> optOuts = new List<OptOut>();
            int count = GetRandomNumber();

            for (int i = 0; i < count; i++)
            {
                OptOut optOut = CreateRandomOptOut();
                optOut.Status = status;
                optOuts.Add(optOut);
            }

            return optOuts;
        }

        private static List<OptOut> CreateRandomOptOutListWithNullOptOut()
        {
            List<OptOut> optOuts = new List<OptOut>();
            int count = GetRandomNumber();

            for (int i = 0; i < count; i++)
            {
                OptOut optOut = null;
                optOuts.Add(optOut);
            }

            return optOuts;
        }

        private OptOut SelectRandomOptOut(IQueryable<OptOut> optOuts)
        {
            Random random = new Random();

            OptOut optOut = optOuts.OrderBy(item => random.Next())
                .Take(1)
                    .SingleOrDefault();

            return optOut;
        }

        private static string GenerateValidNhsNumber()
        {
            int total = 10;
            string formattedNhsNumber = string.Empty;

            while (total == 10)
            {
                var randomNumber = new LongRange(100000000, 999999999);
                formattedNhsNumber = randomNumber.GetValue().ToString();
                int[] multiplers = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int currentNumber;
                int currentSum = 0;
                int currentMultipler;
                string currentString;
                int remainder;

                for (int i = 0; i <= 8; i++)
                {
                    currentString = formattedNhsNumber.Substring(i, 1);

                    currentNumber = Convert.ToInt16(currentString);
                    currentMultipler = multiplers[i];
                    currentSum = currentSum + (currentNumber * currentMultipler);
                }

                remainder = currentSum % 11;
                total = 11 - remainder;

                if (total.Equals(11))
                {
                    total = 0;
                }

                if (total != 10)
                {
                    break;
                }
            }

            string checkNumber = total.ToString();

            return $"{formattedNhsNumber}{checkNumber}";
        }
    }
}
