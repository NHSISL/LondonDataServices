// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOptOutService optOutService;

        public OptOutServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.optOutService = new OptOutService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomMessage(int length) =>
            new MnemonicString(wordCount: length).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static string GenerateValidNhsNumber()
        {
            var randomNumber = new LongRange(0, 999999999);
            string formattedNhsNumber = randomNumber.GetValue().ToString().PadLeft(9, '0');
            int[] multiplers = new int[9];
            multiplers[0] = 10;
            multiplers[1] = 9;
            multiplers[2] = 8;
            multiplers[3] = 7;
            multiplers[4] = 6;
            multiplers[5] = 5;
            multiplers[6] = 4;
            multiplers[7] = 3;
            multiplers[8] = 2;
            int currentNumber;
            int currentSum = 0;
            int currentMultipler;
            string currentString;
            int remainder;
            int total;

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

            string checkNumber = total.ToString();

            return $"{formattedNhsNumber}{checkNumber}";
        }

        private static string GenerateInvalidNhsNumber()
        {
            string nhsNumber = GenerateValidNhsNumber();
            int checkDigit = Convert.ToInt32(nhsNumber.Substring(nhsNumber.Length - 1, 1));
            Random random = new Random();
            int randomNumber = random.Next(9);

            if (randomNumber == checkDigit)
            {
                if (randomNumber == 9)
                {
                    randomNumber = randomNumber - 1;
                }
                else
                {
                    randomNumber = randomNumber + 1;
                }
            }

            string invalidNhsNumber = $"{nhsNumber.Substring(0, nhsNumber.Length - 1)}{randomNumber}";

            return invalidNhsNumber;
        }


        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static OptOut CreateRandomModifyOptOut(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            OptOut randomOptOut = CreateRandomOptOut(dateTimeOffset);

            randomOptOut.CreatedDate =
                randomOptOut.CreatedDate.AddDays(randomDaysInPast);

            return randomOptOut;
        }

        private static IQueryable<OptOut> CreateRandomOptOuts()
        {
            return CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static OptOut CreateRandomOptOut() =>
            CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
            CreateOptOutFiller(dateTimeOffset).Create();

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }
    }
}