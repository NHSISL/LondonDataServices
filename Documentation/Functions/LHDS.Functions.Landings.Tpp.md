# LHDS.Functions.Landings.Tpp

This document describes every Azure Function in the `LHDS.Functions.Landings.Tpp` project,
including what each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The TPP Landings function app manages the ingestion pipeline for files delivered by TPP (The Phoenix
Partnership). When a new file lands in the designated blob container the function validates the
filename against include/exclude patterns and processes it immediately. A separate timer-driven
function periodically re-processes any files that previously failed or were skipped.

All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 TppLandingFunction

| Property | Value |
|---|---|
| **Function name** | `TppLandingFunction` |
| **Trigger type** | Blob trigger |
| **Blob path** | `tpplanding/{name}` |
| **Connection setting** | `BlobStorage` |

#### 2.1.1 Description

Fires whenever a new file is dropped into the `tpplanding/` blob container. Before processing the
file, the function calls `ITppLandingClient.ShouldValidateFileNameAsync` with the configured
`FileNameIncludePattern` and `FileNameExcludePattern` to determine whether the file should be
processed or skipped. Files that do not match the include pattern (or match the exclude pattern) are
logged as `SKIPPING` and returned early. Matching files are forwarded to
`ITppLandingClient.ProcessAsync(fileName, supplierId)` which handles ingestion and registration of
the TPP data. Any unhandled exception is logged and re-thrown.

#### 2.1.2 Configuration

| Key | Description |
|---|---|
| `BlobStorage` (connection string / managed-identity URI) | Connection used by the blob trigger |
| `blobStorage:azureBlobServiceUri` | Azure Blob Service endpoint URI |
| `blobStorage:azureTenantId` | Entra tenant ID |
| `landingSettings:landingSupplierId` | GUID identifying the TPP supplier in the system |
| `landingSettings:fileNameIncludePattern` | Regex/glob pattern — only files matching this pattern are processed |
| `landingSettings:fileNameExcludePattern` | Regex/glob pattern — files matching this pattern are skipped |

---

### 2.2 ReLandTimerFunction

| Property | Value |
|---|---|
| **Function name** | `ReLandTimerFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `reLandTimerInterval` app setting (default: `0 */15 * * * *` — every 15 minutes) |

#### 2.2.1 Description

Runs on the schedule defined by the `reLandTimerInterval` application setting. Calls
`ITppLandingClient.ReProcessAsync(supplierId)` to retry processing of any TPP files that were
previously failed, skipped, or are awaiting re-processing. The `relandIntervalInMinutes` landing
setting controls which files are eligible for re-processing (files older than this interval).
Any unhandled exception is logged and re-thrown.

#### 2.2.2 Configuration

| Key | Description |
|---|---|
| `reLandTimerInterval` | CRON expression for the re-land retry schedule (default: `0 */15 * * * *`) |
| `landingSettings:landingSupplierId` | Supplier GUID used when calling ReProcessAsync |
| `landingSettings:relandIntervalInMinutes` | Minimum age (minutes) of a file before it is eligible for re-processing (default: `120`) |

---

## 3. Shared Configuration

The following settings apply to all functions in this project and are defined in `appsettings.json`.

### 3.1 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:tpplanding` | `tpplanding` | Container watched by the blob trigger |
| `blobStorage:blobContainers:emislanding` | `emislanding` | EMIS landing container (shared reference) |
| `blobStorage:blobContainers:versioner` | `versioner` | Versioner container (shared reference) |
| `blobStorage:blobContainers:ingress` | `ingress` | General ingress container (shared reference) |
| `blobStorage:blobContainers:optOut` | `optOut` | Opt-Out container (shared reference) |
| `blobStorage:blobContainers:pds` | `pds` | PDS container (shared reference) |
| `blobStorage:blobContainers:addresses` | `addresses` | Addresses container (shared reference) |

### 3.2 Landing Settings

| Key | Default | Description |
|---|---|---|
| `landingSettings:landingSupplierId` | `c1e98940-6d0a-4a76-bfbe-614c4a47b23e` | Supplier GUID for TPP |
| `landingSettings:encryptedFolder` | `encrypted` | Blob sub-folder for encrypted files |
| `landingSettings:decryptedFolder` | `inbox` | Blob sub-folder for decrypted files |
| `landingSettings:batchDownloadedFile` | `LDSBatchDownloaded.txt` | Batch-downloaded sentinel filename |
| `landingSettings:batchReadyFile` | `LDSBatchReady.txt` | Batch-ready sentinel filename |
| `landingSettings:fileNameIncludePattern` | _(empty)_ | Filename include pattern (empty = accept all) |
| `landingSettings:fileNameExcludePattern` | _(empty)_ | Filename exclude pattern (empty = exclude none) |
| `landingSettings:relandIntervalInMinutes` | `120` | Minimum age before a file is eligible for re-processing |

### 3.3 Timer Schedules

| Key | Default | Description |
|---|---|---|
| `reLandTimerInterval` | `0 */15 * * * *` | CRON expression for the ReLandTimerFunction |

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
