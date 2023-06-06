// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetreiveMessagesFromMeshAsync()
        {
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.ASCII.GetBytes(inputString);
        }
    }
}
