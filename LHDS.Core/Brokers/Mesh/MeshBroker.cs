// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Mesh;
using NEL.MESH.Clients;
using NEL.MESH.Models.Foundations.Mesh;

namespace LHDS.Core.Brokers.Mesh
{
    public class MeshBroker : IMeshBroker
    {
        private readonly IMeshClient meshClient;
        private readonly MeshConfiguration meshConfiguration;

        public MeshBroker(MeshConfiguration meshConfiguration)
        {
            this.meshConfiguration = meshConfiguration;

            var config = new NEL.MESH.Models.Configurations.MeshConfiguration
            {
                MailboxId = this.meshConfiguration.MailboxId,
                Password = this.meshConfiguration.Password,
                Key = this.meshConfiguration.Key,
                Url = this.meshConfiguration.Url,
                RootCertificate = this.meshConfiguration.RootCertificate,
                IntermediateCertificates = this.meshConfiguration.IntermediateCertificates,
                ClientCertificate = this.meshConfiguration.ClientCertificate,
                MexClientVersion = this.meshConfiguration.MexClientVersion,
                MexOSName = this.meshConfiguration.MexOSName,
                MexOSVersion = this.meshConfiguration.MexOSVersion
            };

            this.meshClient = new MeshClient(config);
        }

        public ValueTask<bool> HandshakeAsync() =>
            this.meshClient.Mailbox.HandshakeAsync();

        public ValueTask<Message> SendMessageAsync(Message message) =>
            this.meshClient.Mailbox.SendMessageAsync(message);

        public ValueTask<Message> SendFileAsync(Message message) =>
            this.meshClient.Mailbox.SendFileAsync(message);

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            this.meshClient.Mailbox.TrackMessageAsync(messageId);

        public ValueTask<List<string>> RetrieveMessagesAsync() =>
            this.meshClient.Mailbox.RetrieveMessagesAsync();

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            this.meshClient.Mailbox.RetrieveMessageAsync(messageId);

        public ValueTask<bool> AcknowledgeMessageAsync(string messageId) =>
            this.meshClient.Mailbox.AcknowledgeMessageAsync(messageId);
    }
}
