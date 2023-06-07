// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Tests.Acceptance.Brokers.CsvMappers.Models;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class PdsTests
    {
        [Fact]
        public async Task ShouldMapCsvWithTrailingCommaToObjectAsync()
        {
            // given
            List<PdsAudit> expectedList = new List<PdsAudit>();

            // when
            List<PdsAudit> actualList = await this.pdsClient.;

            // then
            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}
