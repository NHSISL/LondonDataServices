// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Services.Foundations.Mesh;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService : IMeshProcessingService
    {
        private readonly IMeshService meshService;
        private readonly ILoggingBroker loggingBroker;

        public MeshProcessingService(
            IMeshService meshService,
            ILoggingBroker loggingBroker)
        {
            this.meshService = meshService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateMailboxAccessAsync() =>
           TryCatch(async () =>
           {
               return await this.meshService.ValidateMailboxAccessAsync();
           });

        public ValueTask<List<string>> RetrieveMessagesFromInboxAsync() =>
         TryCatch(async () =>
            {
                return await this.meshService.RetrieveMessagesFromInboxAsync();
            });

        public ValueTask<MeshMessage> RetrieveAndAcknowledgeMessageByIdAsync(MeshMessage message) =>
            TryCatch(async () =>
            {
                ValidateMeshArgs(message.MessageId);

                MeshMessage retrievedMessage = await meshService.RetrieveMessageByIdAsync(message.MessageId);

                ValidateMeshMessageIsNotNull(retrievedMessage);

                bool ackResult = await meshService.AcknowledgeMessageByIdAsync(message.MessageId);

                return retrievedMessage;
            });

        public ValueTask<Message> SendMessageAsync(Message message) =>
            TryCatch(async () =>
            {

                ValidateMeshArgs(message.MessageId);
                Message trackMessage = await this.meshService.RetrieveTrackingStatusAsync(message.MessageId);
                await this.meshService.SendMessageAsync(message);

                return trackMessage;
                //ValidateMeshArgs(message.MessageId);

                //Message sendMessage;


                //sendMessage = await this.meshService.SendMessageAsync(message);

                //var trackMessage = await this.meshService.RetrieveTrackingStatusAsync(message.MessageId);

                //return trackMessage;
            });
    }
}
