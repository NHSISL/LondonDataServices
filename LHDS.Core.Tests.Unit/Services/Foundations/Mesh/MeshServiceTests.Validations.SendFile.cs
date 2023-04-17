//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Foundations.Mesh;
//using LHDS.Core.Models.Foundations.Mesh.Exceptions;
//using Moq;
//using NEL.MESH.Models.Foundations.Mesh;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
//{
//    public partial class MeshServiceTests
//    {
//        [Fact]
//        public async Task ShouldThrowValidationExceptionOnSendFileIfMessageIsNullAsync()
//        {
//            // given
//            MeshMessage nullMeshMessage = null;
//            Message nullMessage = null;

//            var nullMessageException =
//                new NullMeshMessageException();

//            var expectedMeshValidationException =
//                new MeshValidationException(nullMessageException);

//            // when
//            ValueTask<MeshMessage> addMessageTask =
//                this.meshService.SendFileAsync(nullMeshMessage);

//            MeshValidationException actualMeshValidationException =
//                await Assert.ThrowsAsync<MeshValidationException>(() =>
//                    addMessageTask.AsTask());

//            // then
//            actualMeshValidationException.Should()
//                .BeEquivalentTo(expectedMeshValidationException);

//            this.meshBrokerMock.Verify(broker =>
//                broker.SendFileAsync(nullMessage),
//                        Times.Never);

//            this.meshBrokerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task ShouldThrowValidationExceptionOnSendFileIfHeadersDictionaryIsNullAsync()
//        {
//            // given
//            MeshMessage meshMessageWithNullHeaders = new MeshMessage
//            {
//                Headers = null
//            };

//            Message messageWithNullHeaders = new Message
//            {
//                Headers = null
//            };

//            var nullHeadersException =
//                new NullHeadersException();

//            var expectedMeshValidationException =
//                new MeshValidationException(nullHeadersException);

//            // when
//            ValueTask<MeshMessage> sendMessageTask =
//                this.meshService.SendFileAsync(meshMessageWithNullHeaders);

//            MeshValidationException actualMeshValidationException =
//                await Assert.ThrowsAsync<MeshValidationException>(() =>
//                    sendMessageTask.AsTask());

//            // then
//            actualMeshValidationException.Should()
//                .BeEquivalentTo(expectedMeshValidationException);

//            this.meshBrokerMock.Verify(broker =>
//                broker.SendFileAsync(messageWithNullHeaders),
//                        Times.Never);

//            this.meshBrokerMock.VerifyNoOtherCalls();
//        }

//        [Theory]
//        [InlineData(null)]
//        [InlineData("")]
//        [InlineData("   ")]
//        public async Task ShouldThrowValidationExceptionOnSendFileIfRequiredMessageItemsAreNullAsync(
//            string invalidInput)
//        {
//            // given
//            byte[] invalidContent = null;

//            dynamic dynamicMeshMessageProperties =
//                CreateRandomMeshMessageProperties();

//            var randomMessage = new Message
//            {
//                MessageId = invalidInput,
//                Headers = dynamicMeshMessageProperties.Headers,
//                FileContent = invalidContent,
//                TrackingInfo = MaptToMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
//            };

//            var inputMessage = randomMessage;

//            MeshMessage randomMeshMessage = new MeshMessage
//            {
//                MessageId = dynamicMeshMessageProperties.MessageId,
//                Headers = dynamicMeshMessageProperties.Headers,
//                StringContent = dynamicMeshMessageProperties.StringContent,
//                FileContent = dynamicMeshMessageProperties.FileContent,
//                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
//            };

//            var inputMeshMessage = randomMeshMessage;

//            var invalidMeshMessageException =
//                new InvalidMeshMessageException();

//            invalidMeshMessageException.AddData(
//                key: nameof(Message.FileContent),
//                values: "Content is required");

//            invalidMeshMessageException.AddData(
//                key: "Content-Type",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Mex-FileName",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Mex-From",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Mex-To",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Mex-WorkflowID",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Mex-Content-Checksum",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Mex-Content-Encrypted",
//                values: "Header value is required");

//            invalidMeshMessageException.AddData(
//                key: "Token",
//                values: "Text is required");

//            var expectedMeshValidationException =
//                new MeshValidationException(
//                innerException: invalidMeshMessageException,
//                validationSummary: GetValidationSummary(invalidMeshMessageException.Data));

//            // when
//            ValueTask<MeshMessage> sendFileTask =
//                this.meshService.SendFileAsync(inputMeshMessage);

//            MeshValidationException actualMeshValidationException =
//                await Assert.ThrowsAsync<MeshValidationException>(() =>
//                    sendFileTask.AsTask());

//            // then
//            actualMeshValidationException.Should()
//                .BeEquivalentTo(expectedMeshValidationException);

//            this.meshBrokerMock.Verify(broker =>
//                broker.SendFileAsync(inputMessage),
//                        Times.Never);

//            this.meshBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
