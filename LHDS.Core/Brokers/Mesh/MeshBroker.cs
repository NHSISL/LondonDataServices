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

        public ValueTask<Message> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            byte[] fileContent,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json")
        {
            return this.meshClient.Mailbox.SendMessageAsync(
                mexTo: mexTo,
                mexWorkflowId: mexWorkflowId,
                fileContent: fileContent,
                mexSubject: mexSubject,
                mexLocalId: mexLocalId,
                mexFileName: mexFileName,
                mexContentChecksum: mexContentChecksum,
                contentType: contentType,
                contentEncoding: contentEncoding,
                accept: accept);
        }

        public ValueTask<Message> TrackMessageAsync(string messageId) =>
            this.meshClient.Mailbox.TrackMessageAsync(messageId);

        public ValueTask<List<string>> RetrieveMessageIdsAsync() =>
            this.meshClient.Mailbox.RetrieveMessagesAsync();

        public ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            this.meshClient.Mailbox.RetrieveMessageAsync(messageId);

        public ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId) =>
            this.meshClient.Mailbox.AcknowledgeMessageAsync(messageId);
    }
}
