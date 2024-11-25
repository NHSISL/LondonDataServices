// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;
using Force.DeepCloner;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddObjectColumnProcessingAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = inputObjectColumn.DeepClone();
            List<ObjectColumn> ObjectColumnlist = CreateRandomObjectColumns().ToList();

            this.ObjectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumnsAsync())
                    .ReturnsAsync(ObjectColumnlist.AsQueryable);

            this.ObjectColumnServiceMock.Setup(service =>
                service.AddObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(storageObjectColumn);

            // when
            await this.ObjectColumnProcessingService.ReadOrInsertObjectColumnAsync(
                inputObjectColumn);

            // then
            this.ObjectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            this.ObjectColumnServiceMock.Verify(service =>
                service.AddObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.ObjectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotAddObjectColumnWhenExistingObjectColumnIsFoundAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomSupplierObjectName = GetRandomString();
            ObjectColumn existingObjectColumn = CreateRandomObjectColumn();
            existingObjectColumn.SupplierColumnName = randomSupplierObjectName;
            ObjectColumn existingObjectColumnFound = existingObjectColumn;
            ObjectColumn storageObjectColumn = existingObjectColumn.DeepClone();
            List<ObjectColumn> ObjectColumnlist = CreateRandomObjectColumns().ToList();
            ObjectColumnlist.Add(storageObjectColumn);

            this.ObjectColumnServiceMock.Setup(processings =>
                processings.RetrieveAllObjectColumnsAsync())
                    .ReturnsAsync(ObjectColumnlist.AsQueryable);

            // when
            ObjectColumn retrievedObjectColumn = 
                await this.ObjectColumnProcessingService.ReadOrInsertObjectColumnAsync(
                    existingObjectColumn);

            // then
            retrievedObjectColumn.Should().BeEquivalentTo(existingObjectColumnFound);

            this.ObjectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            this.ObjectColumnServiceMock.Verify(service =>
                service.AddObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.ObjectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}