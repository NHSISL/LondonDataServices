// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldBulkAddOrModifyBatchAsync()
        {
            // given
            int randomBatchSize = GetRandomNumber();
            int inputBatchSize = randomBatchSize;
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<OptOut> randomNewOptOuts = CreateRandomOptOuts(
                count: 1,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<OptOut> randomExistingOptOuts = CreateRandomOptOuts(
                count: 1,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<OptOut> inputOptOuts = new List<OptOut>();
            inputOptOuts.AddRange(randomNewOptOuts);
            inputOptOuts.AddRange(randomExistingOptOuts);
            int batchCount = GetBatchSize(inputOptOuts.Count, inputBatchSize);

            var optOutServiceMock = new Mock<OptOutService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityAuditBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync(default))
                    .ReturnsAsync(randomExistingOptOuts.AsQueryable());

            int totalRecords = inputOptOuts.Count;

            for (int i = 0; i < totalRecords; i += randomBatchSize)
            {
                var batch = inputOptOuts.Skip(i).Take(randomBatchSize).ToList();

                List<Guid> batchIds =
                    batch.Select(optOut => optOut.Id).ToList();

                List<Guid> storageIds =
                    randomExistingOptOuts.Select(optOut => optOut.Id).ToList();

                var existingIds = storageIds
                    .Where(storageId => batchIds.Contains(storageId))
                    .Select(storageId => storageId)
                    .ToList();

                var existingOptOuts = batch
                    .Where(optOut => existingIds.Contains(optOut.Id)).ToList();

                var newOptOuts = batch
                    .Where(optOut => !existingIds.Contains(optOut.Id)).ToList();

                optOutServiceMock.Setup(service =>
                    service.ValidateOptOutsAndAssignIdAndAuditOnAddAsync(
                        newOptOuts, randomFileName))
                            .ReturnsAsync(newOptOuts);

                this.storageBrokerMock.Setup(broker =>
                    broker.BulkInsertOptOutsAsync(newOptOuts, true, default))
                        .Returns(ValueTask.CompletedTask);

                optOutServiceMock.Setup(service =>
                    service.ValidateOptOutsAndAssignAuditOnModifyAsync(
                        existingOptOuts, randomFileName))
                            .ReturnsAsync(existingOptOuts);

                this.storageBrokerMock.Setup(broker =>
                    broker.BulkUpdateOptOutsAsync(existingOptOuts, true, default))
                        .Returns(ValueTask.CompletedTask);
            }

            // when
            await optOutServiceMock.Object
                .BulkAddOrModifyBatchAsync(
                    inputOptOuts, inputFileName, inputBatchSize);

            // then
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOutsAsync(default),
                    Times.Exactly(batchCount));

            for (int i = 0; i < totalRecords; i += randomBatchSize)
            {
                var batch = inputOptOuts.Skip(i).Take(randomBatchSize).ToList();

                List<Guid> batchIds =
                    batch.Select(optOut => optOut.Id).ToList();

                List<Guid> storageIds =
                    randomExistingOptOuts.Select(optOut => optOut.Id).ToList();

                var existingIds = storageIds
                    .Where(storageId => batchIds.Contains(storageId))
                    .Select(storageId => storageId)
                    .ToList();

                var existingOptOuts = batch
                    .Where(optOut => existingIds.Contains(optOut.Id)).ToList();

                var newOptOuts = batch
                    .Where(optOut => !existingIds.Contains(optOut.Id)).ToList();

                optOutServiceMock.Verify(service =>
                    service.ValidateOptOutsAndAssignIdAndAuditOnAddAsync(
                        newOptOuts, randomFileName),
                            Times.Once);

                this.storageBrokerMock.Verify(broker =>
                    broker.BulkInsertOptOutsAsync(newOptOuts, true, default),
                        Times.Once);

                optOutServiceMock.Verify(service =>
                    service.ValidateOptOutsAndAssignAuditOnModifyAsync(
                        existingOptOuts, randomFileName),
                            Times.Once);

                this.storageBrokerMock.Verify(broker =>
                    broker.BulkUpdateOptOutsAsync(
                        existingOptOuts, true, default),
                            Times.Once);
            }

            optOutServiceMock.Verify(service =>
                service.BulkAddOrModifyBatchAsync(
                    inputOptOuts, inputFileName, inputBatchSize),
                        Times.Once);

            optOutServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSkipAndAuditUnknownOptOutsWhenAllowInsertsIsFalseAsync()
        {
            // given
            int randomBatchSize = GetRandomNumber();
            int inputBatchSize = randomBatchSize;
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<OptOut> randomNewOptOuts = CreateRandomOptOuts(
                count: 1,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<OptOut> randomExistingOptOuts = CreateRandomOptOuts(
                count: 1,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<OptOut> inputOptOuts = new List<OptOut>();
            inputOptOuts.AddRange(randomNewOptOuts);
            inputOptOuts.AddRange(randomExistingOptOuts);
            int batchCount = GetBatchSize(inputOptOuts.Count, inputBatchSize);

            var optOutServiceMock = new Mock<OptOutService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityAuditBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync(default))
                    .ReturnsAsync(randomExistingOptOuts.AsQueryable());

            List<OptOut> capturedExistingOptOuts = null;

            optOutServiceMock.Setup(service =>
                service.ValidateOptOutsAndAssignAuditOnModifyAsync(
                    It.IsAny<List<OptOut>>(), randomFileName))
                        .Callback<List<OptOut>, string>((list, _) => capturedExistingOptOuts = list)
                        .ReturnsAsync(randomExistingOptOuts);

            this.storageBrokerMock.Setup(broker =>
                broker.BulkUpdateOptOutsAsync(It.IsAny<List<OptOut>>(), true, default))
                    .Returns(ValueTask.CompletedTask);

            this.auditBrokerMock.Setup(broker =>
                broker.BulkLogAsync(It.IsAny<List<Audit>>()))
                    .Returns(ValueTask.CompletedTask);

            this.loggingBrokerMock.Setup(broker =>
                broker.LogWarningAsync(It.IsAny<string>()))
                    .Returns(ValueTask.CompletedTask);

            // when
            await optOutServiceMock.Object
                .BulkAddOrModifyBatchAsync(
                    inputOptOuts, inputFileName, inputBatchSize, allowInserts: false);

            // then
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOutsAsync(default),
                    Times.Exactly(batchCount));

            this.storageBrokerMock.Verify(broker =>
                broker.BulkInsertOptOutsAsync(
                    It.IsAny<List<OptOut>>(), It.IsAny<bool>(), It.IsAny<System.Threading.CancellationToken>()),
                        Times.Never);

            optOutServiceMock.Verify(service =>
                service.ValidateOptOutsAndAssignAuditOnModifyAsync(
                    capturedExistingOptOuts, randomFileName),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.BulkUpdateOptOutsAsync(
                    It.IsAny<List<OptOut>>(), true, default),
                        Times.Exactly(batchCount));

            this.auditBrokerMock.Verify(broker =>
                broker.BulkLogAsync(It.Is<List<Audit>>(audits =>
                    audits.Count == randomNewOptOuts.Count
                    && audits.All(a => a.AuditType == "OptOut")
                    && audits.All(a => a.FileName == inputFileName)
                    && audits.All(a => a.LogLevel == "Error"))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarningAsync(It.IsAny<string>()),
                    Times.Exactly(randomNewOptOuts.Count));

            optOutServiceMock.Verify(service =>
                service.BulkAddOrModifyBatchAsync(
                    inputOptOuts, inputFileName, inputBatchSize, false),
                        Times.Once);
        }
    }
}
