// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        private readonly Mock<IObjectColumnProcessingService> objectColumnProcessingServiceMock;
        private readonly Mock<ISpecificationObjectProcessingService> specificationObjectProcessingServiceMock;
        private readonly Mock<IDataSetProcessingService> dataSetProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISchemaConfigOrchestrationService schemaConfigOrchestrationService;

        public SchemaConfigOrchestrationServiceTests()
        {
            this.objectColumnProcessingServiceMock = new Mock<IObjectColumnProcessingService>();
            this.specificationObjectProcessingServiceMock = new Mock<ISpecificationObjectProcessingService>();
            this.dataSetProcessingServiceMock = new Mock<IDataSetProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.schemaConfigOrchestrationService = new SchemaConfigOrchestrationService(
                objectColumnService: this.objectColumnProcessingServiceMock.Object,
                specificationObjectService: this.specificationObjectProcessingServiceMock.Object,
                dataSetProcessingService: this.dataSetProcessingServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static dynamic GetRandomTrackingInfo()
        {
            return new
            {
                AddressType = GetRandomString(),
                Checksum = GetRandomString(),
                ChunkCount = GetRandomNumber(),
                CompressFlag = GetRandomString(),
                DownloadTimestamp = GetRandomString(),
                DtsId = GetRandomString(),
                EncryptedFlag = GetRandomString(),
                ExpiryTime = GetRandomString(),
                FileName = GetRandomString(),
                FileSize = GetRandomNumber(),
                IsCompressed = GetRandomString(),
                LocalId = GetRandomString(),
                MeshRecipientOdsCode = GetRandomString(),
                MessageId = GetRandomString(),
                MessageType = GetRandomString(),
                PartnerId = GetRandomString(),
                Recipient = GetRandomString(),
                RecipientName = GetRandomString(),
                RecipientOrgCode = GetRandomString(),
                RecipientSmtp = GetRandomString(),
                Sender = GetRandomString(),
                SenderName = GetRandomString(),
                SenderOdsCode = GetRandomString(),
                SenderOrgCode = GetRandomString(),
                SenderSmtp = GetRandomString(),
                Status = GetRandomString(),
                StatusSuccess = GetRandomString(),
                UploadTimestamp = GetRandomString(),
                Version = GetRandomString(),
                WorkflowId = GetRandomString()
            };
        }

        private static MessageTrackingInfo MaptToMeshMessageTrackingInfo(dynamic dynamicTrackingInfo)
        {
            return new MessageTrackingInfo
            {
                AddressType = dynamicTrackingInfo.AddressType,
                Checksum = dynamicTrackingInfo.Checksum,
                ChunkCount = dynamicTrackingInfo.ChunkCount,
                CompressFlag = dynamicTrackingInfo.CompressFlag,
                DownloadTimestamp = dynamicTrackingInfo.DownloadTimestamp,
                DtsId = dynamicTrackingInfo.DtsId,
                EncryptedFlag = dynamicTrackingInfo.EncryptedFlag,
                ExpiryTime = dynamicTrackingInfo.ExpiryTime,
                FileName = dynamicTrackingInfo.FileName,
                FileSize = dynamicTrackingInfo.FileSize,
                IsCompressed = dynamicTrackingInfo.IsCompressed,
                LocalId = dynamicTrackingInfo.LocalId,
                MeshRecipientOdsCode = dynamicTrackingInfo.MeshRecipientOdsCode,
                MessageId = dynamicTrackingInfo.MessageId,
                MessageType = dynamicTrackingInfo.MessageType,
                PartnerId = dynamicTrackingInfo.PartnerId,
                Recipient = dynamicTrackingInfo.Recipient,
                RecipientName = dynamicTrackingInfo.RecipientName,
                RecipientOrgCode = dynamicTrackingInfo.RecipientOrgCode,
                RecipientSmtp = dynamicTrackingInfo.RecipientSmtp,
                Sender = dynamicTrackingInfo.Sender,
                SenderName = dynamicTrackingInfo.SenderName,
                SenderOdsCode = dynamicTrackingInfo.SenderOdsCode,
                SenderOrgCode = dynamicTrackingInfo.SenderOrgCode,
                SenderSmtp = dynamicTrackingInfo.SenderSmtp,
                Status = dynamicTrackingInfo.Status,
                StatusSuccess = dynamicTrackingInfo.StatusSuccess,
                UploadTimestamp = dynamicTrackingInfo.UploadTimestamp,
                Version = dynamicTrackingInfo.Version,
                WorkflowId = dynamicTrackingInfo.WorkflowId,
            };
        }

        private static TrackingInfo MaptToMessageTrackingInfo(dynamic dynamicTrackingInfo)
        {
            return new TrackingInfo
            {
                AddressType = dynamicTrackingInfo.AddressType,
                Checksum = dynamicTrackingInfo.Checksum,
                ChunkCount = dynamicTrackingInfo.ChunkCount,
                CompressFlag = dynamicTrackingInfo.CompressFlag,
                DownloadTimestamp = dynamicTrackingInfo.DownloadTimestamp,
                DtsId = dynamicTrackingInfo.DtsId,
                EncryptedFlag = dynamicTrackingInfo.EncryptedFlag,
                ExpiryTime = dynamicTrackingInfo.ExpiryTime,
                FileName = dynamicTrackingInfo.FileName,
                FileSize = dynamicTrackingInfo.FileSize,
                IsCompressed = dynamicTrackingInfo.IsCompressed,
                LocalId = dynamicTrackingInfo.LocalId,
                MeshRecipientOdsCode = dynamicTrackingInfo.MeshRecipientOdsCode,
                MessageId = dynamicTrackingInfo.MessageId,
                MessageType = dynamicTrackingInfo.MessageType,
                PartnerId = dynamicTrackingInfo.PartnerId,
                Recipient = dynamicTrackingInfo.Recipient,
                RecipientName = dynamicTrackingInfo.RecipientName,
                RecipientOrgCode = dynamicTrackingInfo.RecipientOrgCode,
                RecipientSmtp = dynamicTrackingInfo.RecipientSmtp,
                Sender = dynamicTrackingInfo.Sender,
                SenderName = dynamicTrackingInfo.SenderName,
                SenderOdsCode = dynamicTrackingInfo.SenderOdsCode,
                SenderOrgCode = dynamicTrackingInfo.SenderOrgCode,
                SenderSmtp = dynamicTrackingInfo.SenderSmtp,
                Status = dynamicTrackingInfo.Status,
                StatusSuccess = dynamicTrackingInfo.StatusSuccess,
                UploadTimestamp = dynamicTrackingInfo.UploadTimestamp,
                Version = dynamicTrackingInfo.Version,
                WorkflowId = dynamicTrackingInfo.WorkflowId,
            };
        }

    }
}