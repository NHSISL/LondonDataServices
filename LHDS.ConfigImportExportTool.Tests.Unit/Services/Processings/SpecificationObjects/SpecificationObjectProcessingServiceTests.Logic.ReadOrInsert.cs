// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;
using Force.DeepCloner;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddSpecificationObjectProcessingAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject storageSpecificationObject = inputSpecificationObject.DeepClone();
            List<SpecificationObject> specificationObjectlist = CreateRandomSpecificationObjects().ToList();

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjectsAsync())
                    .ReturnsAsync(specificationObjectlist.AsQueryable);

            this.specificationObjectServiceMock.Setup(service =>
                service.AddSpecificationObjectAsync(inputSpecificationObject))
                    .ReturnsAsync(storageSpecificationObject);

            // when
            await this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(
                inputSpecificationObject);

            // then
            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(inputSpecificationObject),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotAddSpecificationObjectWhenExistingSpecificationObjectIsFoundAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomSupplierObjectName = GetRandomString();
            SpecificationObject existingSpecificationObject = CreateRandomSpecificationObject();
            existingSpecificationObject.SupplierObjectName = randomSupplierObjectName;
            SpecificationObject existingSpecificationObjectFound = existingSpecificationObject;
            SpecificationObject storageSpecificationObject = existingSpecificationObject.DeepClone();
            List<SpecificationObject> specificationObjectlist = CreateRandomSpecificationObjects().ToList();
            specificationObjectlist.Add(storageSpecificationObject);

            this.specificationObjectServiceMock.Setup(processings =>
                processings.RetrieveAllSpecificationObjectsAsync())
                    .ReturnsAsync(specificationObjectlist.AsQueryable);

            // when
            SpecificationObject retrievedSpecificationObject = 
                await this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(
                    existingSpecificationObject);

            // then
            retrievedSpecificationObject.Should().BeEquivalentTo(existingSpecificationObjectFound);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Never);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}