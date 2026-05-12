// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
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

                TlsRootCertificates =
                    this.meshConfiguration.TlsRootCertificates,

                TlsIntermediateCertificates =
                    this.meshConfiguration.TlsIntermediateCertificates,

                ClientSigningCertificate = this.meshConfiguration.ClientSigningCertificate,
                MexClientVersion = this.meshConfiguration.MexClientVersion,
                MexOSName = this.meshConfiguration.MexOSName,
                MexOSVersion = this.meshConfiguration.MexOSVersion,
                MaxChunkSizeInMegabytes = this.meshConfiguration.MaxChunkSizeInMegabytes,
                MaxRequestTimeoutInSeconds = this.meshConfiguration.MaxRequestTimeoutInSeconds,
            };

            this.meshClient = new MeshClient(config);
        }

        public async ValueTask<bool> HandshakeAsync(CancellationToken cancellationToken = default) =>
            await this.meshClient.Mailbox.HandshakeAsync(cancellationToken);

        public async ValueTask<Message> SendMessageAsync(
            string mexTo,
            string mexWorkflowId,
            Stream content,
            string mexSubject = "",
            string mexLocalId = "",
            string mexFileName = "",
            string mexContentChecksum = "",
            string contentType = "application/octet-stream",
            string contentEncoding = "",
            string accept = "application/json",
            CancellationToken cancellationToken = default)
        {
            return await this.meshClient.Mailbox.SendMessageAsync(
                mexTo: mexTo,
                mexWorkflowId: mexWorkflowId,
                content: content,
                mexSubject: mexSubject,
                mexLocalId: mexLocalId,
                mexFileName: mexFileName,
                mexContentChecksum: mexContentChecksum,
                contentType: contentType,
                contentEncoding: contentEncoding,
                accept: accept,
                cancellationToken: cancellationToken);
        }

        public async ValueTask<Message> TrackMessageAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
                await this.meshClient.Mailbox.TrackMessageAsync(messageId, cancellationToken);

        public async ValueTask<List<string>> RetrieveMessageIdsAsync(CancellationToken cancellationToken = default) =>
            await this.meshClient.Mailbox.RetrieveMessagesAsync(cancellationToken);

        public async ValueTask<Message> RetrieveMessageAsync(
            string messageId,
            Stream outputStream,
            CancellationToken cancellationToken = default) =>
            await this.meshClient.Mailbox.RetrieveMessageAsync(messageId, outputStream, cancellationToken);

        public async ValueTask<bool> AcknowledgeMessageByIdAsync(
            string messageId,
            CancellationToken cancellationToken = default) =>
                await this.meshClient.Mailbox.AcknowledgeMessageAsync(messageId, cancellationToken);
    }
}
