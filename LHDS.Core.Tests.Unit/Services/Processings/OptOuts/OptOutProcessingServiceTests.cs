// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Moq;
using Xunit;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Processings.OptOuts;
using System;
using Tynamix.ObjectFiller;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        private readonly Mock<IOptOutService> optOutServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly OptOutProcessingService optOutProcessingService;

        public OptOutProcessingServiceTests()
        {
            optOutServiceMock = new Mock<IOptOutService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            this.optOutProcessingService = new OptOutProcessingService(
                optOutService: optOutServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
        new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
        CreateOptOutFiller(dateTimeOffset).Create();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();
        
        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.OptOutStatus).Use(GetRandomString(length: 20))
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
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
