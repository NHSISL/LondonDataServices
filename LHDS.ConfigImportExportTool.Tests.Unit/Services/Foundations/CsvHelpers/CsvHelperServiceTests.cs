// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.CsvHelpers

{
    public partial class CsvHelperServiceTests
    {
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICsvHelperService csvHelperService;

        public CsvHelperServiceTests()
        {
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.csvHelperService = new CsvHelperService<dynamic>(
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static List<string> GetRandomStringList()
        {
            List<string> stringList = new List<string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                stringList.Add(GetRandomString());
            }

            return stringList;
        }
    }
}