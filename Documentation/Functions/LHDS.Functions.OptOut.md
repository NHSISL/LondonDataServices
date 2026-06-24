# LHDS.Functions.OptOut

This document describes every Azure Function in the `LHDS.Functions.OptOut` project, including what
each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The Opt-Out function app manages the complete lifecycle of patient opt-out consent records. It
exchanges opt-out data with NHS England via the MESH network, processes status updates received
from MESH, pushes expired opt-out records back to MESH for renewal, and handles inbound opt-out
files arriving in blob storage. A daily handshake function keeps the MESH mailbox connection
verified.

All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 HandshakeFunction

| Property | Value |
|---|---|
| **Function name** | `HandshakeFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `HandshakeTimerTrigger` app setting (default: `0 0 23 * * *` — daily at 23:00 UTC) |

#### 2.1.1 Description

Runs on the schedule defined by the `HandshakeTimerTrigger` application setting. Calls
`IOptOutClient.ValidateMailboxAccessAsync(cancellationToken)` to perform a MESH handshake and
confirm the mailbox is reachable and correctly authenticated. This acts as a daily connectivity
health check for the Opt-Out MESH mailbox. Any unhandled exception is logged and re-thrown.
Supports cooperative cancellation via `CancellationToken`.

#### 2.1.2 Configuration

| Key | Description |
|---|---|
| `HandshakeTimerTrigger` | CRON expression for the handshake schedule (default: `0 0 23 * * *`) |

---

### 2.2 RetrieveOptOutStatusFunction

| Property | Value |
|---|---|
| **Function name** | `RetrieveOptOutStatusFunction` |
| **Trigger type** | Blob trigger |
| **Blob path** | `optout/in/{name}` |
| **Connection setting** | `BlobStorage` |

#### 2.2.1 Description

Fires whenever a new file appears in the `optout/in/` blob container. Reads the blob stream and
calls `IOptOutClient.RetrieveOptOutStatusAsync(input, fileName, cancellationToken)` to parse and
ingest the opt-out status file into the system. The file is expected to contain opt-out consent
records in the format defined by `optOutSettings`. Any unhandled exception is logged and re-thrown.
Supports cooperative cancellation via `CancellationToken`.

#### 2.2.2 Configuration

| Key | Description |
|---|---|
| `BlobStorage` (connection string / managed-identity URI) | Connection used by the blob trigger |
| `blobStorage:azureBlobServiceUri` | Azure Blob Service endpoint URI |
| `blobStorage:azureTenantId` | Entra tenant ID |
| `optOutSettings:inputFolder` | Blob folder for incoming opt-out files (default: `/in`) |
| `optOutSettings:optOutFileHasHeader` | Whether the opt-out file includes a header row |
| `optOutSettings:optOutFileRequireTrailingComma` | Whether the opt-out file requires a trailing comma |

---

### 2.3 ProcessUpdatedOptOutStatusFunction

| Property | Value |
|---|---|
| **Function name** | `ProcessUpdatedOptOutStatusFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `ProcessUpdatedOptOutTimerTrigger` app setting (default: `0 */15 * * * *` — every 15 minutes) |

#### 2.3.1 Description

Runs every 15 minutes (by default). Calls
`IOptOutClient.RetrieveUpdatedMeshConsentStatusesChangesAsync(cancellationToken)` to poll the MESH
inbox for any opt-out status-change messages sent by NHS England. Retrieved messages are processed
and used to update local consent records. Any unhandled exception is logged and re-thrown. Supports
cooperative cancellation via `CancellationToken`.

#### 2.3.2 Configuration

| Key | Description |
|---|---|
| `ProcessUpdatedOptOutTimerTrigger` | CRON expression for the MESH poll schedule (default: `0 */15 * * * *`) |

---

### 2.4 PushExpiredOptOutsToMeshForRenewalFunction

| Property | Value |
|---|---|
| **Function name** | `PushExpiredOptOutsToMeshForRenewalFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `PushExpiredOptOutsToMeshTimerTrigger` app setting (default: `0 */15 * * * *` — every 15 minutes) |

#### 2.4.1 Description

Runs every 15 minutes (by default). Calls
`IOptOutClient.PushExpiredOptOutsToMeshForRenewalAsync(cancellationToken)` to identify opt-out
records whose cache has expired (based on `optOutSettings:expiredAfterDays`) and re-submit them
to MESH for renewal. This ensures that opt-out consents are periodically reconfirmed with NHS
England and do not silently lapse. Any unhandled exception is logged and re-thrown. Supports
cooperative cancellation via `CancellationToken`.

#### 2.4.2 Configuration

| Key | Description |
|---|---|
| `PushExpiredOptOutsToMeshTimerTrigger` | CRON expression for the expiry push schedule (default: `0 */15 * * * *`) |
| `optOutSettings:expiredAfterDays` | Number of days after which an opt-out record is considered expired (default: `7`) |
| `optOutSettings:to` | Destination MESH mailbox ID for opt-out renewal messages |
| `optOutSettings:workflowId` | MESH workflow identifier for opt-out messages |

---

## 3. Shared Configuration

The following settings apply to all functions in this project and are defined in `appsettings.json`.

### 3.1 Timer Trigger Schedules

| Key | Default | Description |
|---|---|---|
| `HandshakeTimerTrigger` | `0 0 23 * * *` | Schedule for the MESH handshake |
| `ProcessUpdatedOptOutTimerTrigger` | `0 */15 * * * *` | Schedule for polling MESH for status changes |
| `PushExpiredOptOutsToMeshTimerTrigger` | `0 */15 * * * *` | Schedule for pushing expired opt-outs to MESH |

### 3.2 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:optOut` | `optOut` | Opt-Out blob container |
| `blobStorage:blobContainers:emislanding` | `emislanding` | EMIS landing container (shared reference) |
| `blobStorage:blobContainers:versioner` | `versioner` | Versioner container (shared reference) |
| `blobStorage:blobContainers:pds` | `pds` | PDS container (shared reference) |
| `blobStorage:blobContainers:tpplanding` | `tpplanding` | TPP landing container (shared reference) |
| `blobStorage:blobContainers:addresses` | `addresses` | Addresses container (shared reference) |

### 3.3 MESH Configuration

| Key | Default | Description |
|---|---|---|
| `meshConfiguration:mailboxId` | _(empty — must be set)_ | MESH mailbox identifier |
| `meshConfiguration:password` | _(empty — must be set)_ | MESH mailbox password |
| `meshConfiguration:sharedKey` | _(empty — must be set)_ | MESH shared key for HMAC authentication |
| `meshConfiguration:url` | _(empty — must be set)_ | MESH API base URL |
| `meshConfiguration:mexClientVersion` | _(empty)_ | Client version header |
| `meshConfiguration:mexOSName` | _(empty)_ | OS name header |
| `meshConfiguration:mexOSVersion` | _(empty)_ | OS version header |
| `meshConfiguration:tlsRootCertificates` | `[]` | PEM-encoded root certificates |
| `meshConfiguration:tlsIntermediateCertificates` | `[]` | PEM-encoded intermediate certificates |
| `meshConfiguration:clientSigningCertificate` | _(empty)_ | Client signing certificate |
| `meshConfiguration:clientSigningCertificatePassword` | _(empty)_ | Signing certificate password |
| `meshConfiguration:maxChunkSizeInMegabytes` | `20` | Maximum MESH chunk size in MB |

### 3.4 Opt-Out Settings

| Key | Default | Description |
|---|---|---|
| `optOutSettings:inputFolder` | `/in` | Blob folder for incoming opt-out files |
| `optOutSettings:outputFolder` | `/out` | Blob folder for outgoing opt-out files |
| `optOutSettings:expiredAfterDays` | `7` | Days before an opt-out record is considered expired |
| `optOutSettings:optOutFileHasHeader` | `true` | Whether opt-out files contain a header row |
| `optOutSettings:optOutFileRequireTrailingComma` | `true` | Whether opt-out files require a trailing comma |
| `optOutSettings:to` | _(empty — must be set)_ | Destination MESH mailbox ID |
| `optOutSettings:workflowId` | _(empty — must be set)_ | MESH workflow identifier |

### 3.5 Logging

| Key | Default | Description |
|---|---|---|
| `Logging:LogLevel:Default` | `Information` | Default log level |
| `Logging:LogLevel:Microsoft.AspNetCore.OptOuts` | `Warning` | Log level for ASP.NET Core framework messages |
