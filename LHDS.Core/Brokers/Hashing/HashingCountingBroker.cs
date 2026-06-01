// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Hashing
{
    /// <summary>
    /// A read-only pass-through stream that incrementally computes a hash
    /// and counts the total number of bytes read from the underlying stream.
    /// Enables a single streaming pass to simultaneously hash and forward data
    /// (e.g. directly to a blob upload) without buffering the entire content.
    /// Supports MD5 and SHA-256 via the <see cref="HashAlgorithmName"/> parameter.
    /// </summary>
    public sealed class HashingCountingBroker : Stream, IHashingCountingBroker
    {
        private readonly Stream innerStream;
        private readonly IncrementalHash incrementalHash;
        private readonly string? pepper;
        private long bytesRead;
        private bool disposed;

        public HashingCountingBroker(
            Stream innerStream,
            HashAlgorithmName algorithmName,
            string? pepper = null)
        {
            this.innerStream = innerStream;
            this.incrementalHash = IncrementalHash.CreateHash(algorithmName);
            this.pepper = pepper;
            this.bytesRead = 0;
        }

        public long BytesRead => this.bytesRead;

        public string GetFinalHashHex()
        {
            if (!string.IsNullOrWhiteSpace(this.pepper))
            {
                this.incrementalHash.AppendData(Encoding.UTF8.GetBytes(this.pepper));
            }

            byte[] hashBytes = this.incrementalHash.GetHashAndReset();

            return Convert.ToHexString(hashBytes)
                .ToLowerInvariant();
        }

        public Stream AsStream() => this;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = this.innerStream.Read(buffer, offset, count);

            if (read > 0)
            {
                this.incrementalHash.AppendData(buffer, offset, read);
                this.bytesRead += read;
            }

            return read;
        }

        public override async Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            int read = await this.innerStream.ReadAsync(buffer, offset, count, cancellationToken);

            if (read > 0)
            {
                this.incrementalHash.AppendData(buffer, offset, read);
                this.bytesRead += read;
            }

            return read;
        }

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            int read = await this.innerStream.ReadAsync(buffer, cancellationToken);

            if (read > 0)
            {
                this.incrementalHash.AppendData(buffer.Span[..read]);
                this.bytesRead += read;
            }

            return read;
        }

        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.incrementalHash.Dispose();
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
