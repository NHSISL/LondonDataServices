// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
                SharedKey = this.meshConfiguration.SharedKey,
                Url = this.meshConfiguration.Url,
                TlsRootCertificates = this.meshConfiguration.TlsRootCertificates,
                TlsIntermediateCertificates = this.meshConfiguration.TlsIntermediateCertificates,
                ClientSigningCertificate = this.meshConfiguration.ClientSigningCertificate,
                MexClientVersion = this.meshConfiguration.MexClientVersion,
                MexOSName = this.meshConfiguration.MexOSName,
                MexOSVersion = this.meshConfiguration.MexOSVersion,
                MaxChunkSizeInMegabytes = this.meshConfiguration.MaxChunkSizeInMegabytes,
            };

            this.meshClient = new MeshClient(config);
        }

        public async ValueTask<bool> HandshakeAsync() =>
            await this.meshClient.Mailbox.HandshakeAsync();

        public async ValueTask<Message> SendMessageAsync(
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
            try
            {
                return await this.meshClient.Mailbox.SendMessageAsync(
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
            catch (Exception)
            {
                throw;
            }
        }

        public async ValueTask<Message> TrackMessageAsync(string messageId) =>
            await this.meshClient.Mailbox.TrackMessageAsync(messageId);

        public async ValueTask<List<string>> RetrieveMessageIdsAsync() =>
            await this.meshClient.Mailbox.RetrieveMessagesAsync();

        public async ValueTask<Message> RetrieveMessageAsync(string messageId) =>
            await this.meshClient.Mailbox.RetrieveMessageAsync(messageId);

        public async ValueTask<bool> AcknowledgeMessageByIdAsync(string messageId) =>
            await this.meshClient.Mailbox.AcknowledgeMessageAsync(messageId);
    }
}
