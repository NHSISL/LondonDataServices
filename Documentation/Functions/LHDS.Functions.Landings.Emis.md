# LHDS.Functions.Landings.Emis

This document describes every Azure Function in the `LHDS.Functions.Landings.Emis` project,
including what each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The EMIS Landings function app manages the end-to-end ingestion pipeline for files delivered by
EMIS. It periodically polls for new files on an SFTP/FTP server, decrypts GPG-encrypted blobs as
they arrive in blob storage, retries failed decryptions, and marks batches as complete once all
files in a batch have been successfully decrypted and processed.

All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 EmisLandingTimerFunction

| Property | Value |
|---|---|
| **Function name** | `EmisLandingTimerFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `emisLandingTimerInterval` app setting (default: `0 0 */2 * * *` — every 2 hours) |

#### 2.1.1 Description

Runs on the schedule defined by the `emisLandingTimerInterval` application setting. Calls
`IEmisLandingClient.ProcessAsync(supplierId)` to connect to the EMIS FTP/SFTP server and download
any new files into the `emislanding/encrypted/` blob container. Files are associated with the
configured `landingSupplierId` so that downstream processing can attribute them to the correct
supplier. Any unhandled exception is logged and re-thrown.

#### 2.1.2 Configuration

| Key | Description |
|---|---|
| `emisLandingTimerInterval` | CRON expression controlling how often the FTP poll runs (default: `0 0 */2 * * *`) |
| `landingSettings:landingSupplierId` | GUID identifying the EMIS supplier in the system |
| `landingSettings:encryptedFolder` | Blob sub-folder where encrypted files are placed (default: `encrypted`) |
| `landingSettings:decryptedFolder` | Blob sub-folder for decrypted output (default: `inbox`) |
| `landingSettings:batchDownloadedFile` | Sentinel filename written when a batch download is complete (default: `LDSBatchDownloaded.txt`) |
| `landingSettings:batchReadyFile` | Sentinel filename written when a batch is ready for processing (default: `LDSBatchReady.txt`) |
| `landingSettings:lastSeenMinutes` | Minutes of inactivity before a file is considered stale (default: `60`) |
| `landingSettings:relandIntervalInMinutes` | Minutes between re-land attempts for failed files (default: `360`) |
| `ftpDownload:ftpServer` | FTP/SFTP host address |
| `ftpDownload:ftpPort` | FTP/SFTP port |
| `ftpDownload:ftpUserName` | FTP username |
| `ftpDownload:ftpPassword` | FTP password |
| `ftpDownload:ftpKey` | Path or value of the SFTP private key |
| `ftpDownload:ftpPassPhrase` | Passphrase for the SFTP private key |

---

### 2.2 DecryptionEventFunction

| Property | Value |
|---|---|
| **Function name** | `DecryptionEventFunction` |
| **Trigger type** | Blob trigger |
| **Blob path** | `emislanding/encrypted/{name}` |
| **Connection setting** | `BlobStorage` |

#### 2.2.1 Description

Fires whenever a new file arrives in the `emislanding/encrypted/` container. If the file has no
extension it is ignored. If the extension is `.gpg` the function calls
`IDecryptionClient.DecryptAsync("/encrypted/{name}")` to decrypt the file and write the plaintext
output to the decrypted landing folder. Files with any other extension are silently skipped. Any
unhandled exception is logged and re-thrown.

#### 2.2.2 Configuration

| Key | Description |
|---|---|
| `BlobStorage` (connection string / managed-identity URI) | Connection used by the blob trigger |
| `blobStorage:azureBlobServiceUri` | Azure Blob Service endpoint URI |
| `blobStorage:azureTenantId` | Entra tenant ID |
| `cryptography:privateKey` | GPG private key used for decryption |
| `cryptography:publicKey` | GPG public key |
| `cryptography:passphrase` | Passphrase protecting the GPG private key |

---

### 2.3 RedecryptionTimerFunction

| Property | Value |
|---|---|
| **Function name** | `RedecryptionTimerFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `redecryptionTimerFunction` app setting (default: `0 */15 * * * *` — every 15 minutes) |

#### 2.3.1 Description

Runs every 15 minutes (by default). Calls `IDecryptionClient.RetryDecryptAsync()` to retry
decryption of any files that previously failed. This provides an automatic recovery mechanism for
transient decryption failures without requiring manual intervention. Any unhandled exception is
logged and re-thrown.

#### 2.3.2 Configuration

| Key | Description |
|---|---|
| `redecryptionTimerFunction` | CRON expression for the retry schedule (default: `0 */15 * * * *`) |
| `cryptography:privateKey` | GPG private key |
| `cryptography:publicKey` | GPG public key |
| `cryptography:passphrase` | GPG key passphrase |

---

### 2.4 BatchCompleteTimerFunction

| Property | Value |
|---|---|
| **Function name** | `BatchCompleteTimerFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `batchCompleteTimerFunction` app setting (default: `0 */30 * * * *` — every 30 minutes) |

#### 2.4.1 Description

Runs every 30 minutes (by default). Calls
`IDecryptionClient.ProcessDecryptedItemsForBatchCompleteAsync()` to evaluate whether all files
belonging to an open batch have been decrypted. When all files in a batch are confirmed decrypted,
the batch is marked complete and downstream pipeline stages are unblocked. Any unhandled exception
is logged and re-thrown.

#### 2.4.2 Configuration

| Key | Description |
|---|---|
| `batchCompleteTimerFunction` | CRON expression for the batch-complete check (default: `0 */30 * * * *`) |

---

## 3. Shared Configuration

The following settings apply to all functions in this project and are defined in `appsettings.json`.

### 3.1 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:emislanding` | `emislanding` | EMIS landing container |
| `blobStorage:blobContainers:versioner` | `versioner` | Versioner container |
| `blobStorage:blobContainers:ingress` | `ingress` | General ingress container |
| `blobStorage:blobContainers:optOut` | `optOut` | Opt-Out container |
| `blobStorage:blobContainers:pds` | `pds` | PDS container |
| `blobStorage:blobContainers:tpplanding` | `tpplanding` | TPP landing container |
| `blobStorage:blobContainers:addresses` | `addresses` | Addresses container |

### 3.2 Landing Settings

| Key | Default | Description |
|---|---|---|
| `landingSettings:landingSupplierId` | `c1e98940-6d0a-4a76-bfbe-614c4a47b23e` | Supplier GUID for EMIS |
| `landingSettings:encryptedFolder` | `encrypted` | Blob sub-folder for encrypted files |
| `landingSettings:decryptedFolder` | `inbox` | Blob sub-folder for decrypted files |
| `landingSettings:batchDownloadedFile` | `LDSBatchDownloaded.txt` | Batch-downloaded sentinel filename |
| `landingSettings:batchReadyFile` | `LDSBatchReady.txt` | Batch-ready sentinel filename |
| `landingSettings:lastSeenMinutes` | `60` | Inactivity threshold in minutes |
| `landingSettings:relandIntervalInMinutes` | `360` | Re-land retry interval in minutes |

### 3.3 Timer Schedules

| Key | Default | Description |
|---|---|---|
| `emisLandingTimerInterval` | `0 0 */2 * * *` | FTP poll schedule |
| `batchCompleteTimerFunction` | `0 */30 * * * *` | Batch-complete check schedule |
| `redecryptionTimerFunction` | `0 */15 * * * *` | Decryption retry schedule |
| `reLandTimerInterval` | `0 */15 * * * *` | Re-land retry schedule |

### 3.4 FTP Download

| Key | Default | Description |
|---|---|---|
| `ftpDownload:ftpServer` | _(empty — must be set)_ | FTP/SFTP host |
| `ftpDownload:ftpPort` | _(empty — must be set)_ | FTP/SFTP port |
| `ftpDownload:ftpUserName` | _(empty — must be set)_ | FTP username |
| `ftpDownload:ftpPassword` | _(empty — must be set)_ | FTP password |
| `ftpDownload:ftpKey` | _(empty)_ | SFTP private key |
| `ftpDownload:ftpPassPhrase` | _(empty)_ | SFTP key passphrase |

### 3.5 Cryptography

| Key | Default | Description |
|---|---|---|
| `cryptography:privateKey` | _(empty — must be set)_ | GPG private key for decryption |
| `cryptography:publicKey` | _(empty — must be set)_ | GPG public key |
| `cryptography:passphrase` | _(empty — must be set)_ | GPG key passphrase |

### 3.6 Logging

| Key | Default | Description |
|---|---|---|
| `Logging:LogLevel:Default` | `Information` | Default log level |
| `Logging:LogLevel:Microsoft.AspNetCore.Landings` | `Warning` | Log level for ASP.NET Core framework messages |
