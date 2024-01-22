// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Tests.Acceptance.Brokers.CsvMappers.Models;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Acceptance.Brokers.CsvMappers
{
    public partial class CsvMapperTests
    {
        private readonly ICsvMapperBroker csvMapperBroker;

        public CsvMapperTests()
        {
            this.csvMapperBroker = new CsvMapperBroker();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

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

        private static Filler<OptOutCsv> CreateOptOutFiller()
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOutCsv>();

            filler.Setup()
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber());

            return filler;
        }

        private static string CreateRandomNhsNumberCsvList(List<OptOutCsv> data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (OptOutCsv optOut in data)
            {
                sb.AppendLine($"{optOut.NhsNumber}");
            }

            return sb.ToString();
        }

        private static string CreateRandomNhsNumberCsvListWithTrailingComma(List<OptOutCsv> data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (OptOutCsv optOut in data)
            {
                sb.AppendLine($"{optOut.NhsNumber},");
            }

            return sb.ToString();
        }

        private static List<OptOutCsv> CreateRandomOptOuts()
        {
            List<OptOutCsv> list = new List<OptOutCsv>();
            int count = GetRandomNumber();

            for (int i = 0; i < count; i++)
            {
                list.Add(new OptOutCsv { NhsNumber = GenerateValidNhsNumber() });
            }

            return list;
        }
    }
}
