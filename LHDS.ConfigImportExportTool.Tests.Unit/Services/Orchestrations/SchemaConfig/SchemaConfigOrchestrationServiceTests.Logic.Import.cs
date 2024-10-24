// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfig
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldImportSchemaConfigAsync()
        {
            // given
            List<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();
            List<ObjectColumn> inputObjectColumns = randomObjectColumns;
            List<ObjectColumn> expectedObjectColumns = inputObjectColumns;

            foreach (ObjectColumn objectColumn in inputObjectColumns)
            {
                this.objectColumnServiceMock.Setup(service =>
                    service.AddObjectColumnAsync(objectColumn))
                        .ReturnsAsync(objectColumn);

                this.specificationObjectServiceMock.Setup(service =>
                    service.AddSpecificationObjectAsync(i))
                        .ReturnsAsync(expectedObjectColumns.ToList);
            }

            // when
            await this.schemaConfigOrchestrationService.Import(inputObjectColumns);

            // then
            foreach (ObjectColumn objectColumn in inputObjectColumns)
            {
                this.objectColumnServiceMock.Verify(service =>
                    service.AddObjectColumnAsync(objectColumn),
                        Times.Once);

                this.specificationObjectServiceMock.Verify(service =>
                    service.AddSpecificationObjectAsync(i),
                        Times.Once);
            }

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}