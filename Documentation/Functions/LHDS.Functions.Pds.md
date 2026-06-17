# LHDS.Functions.Pds

This document describes every Azure Function in the `LHDS.Functions.Pds` project, including what
each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The PDS (Personal Demographics Service) function app orchestrates the exchange of patient data with
NHS England's PDS via the MESH (Message Exchange for Social Care and Health) network. It performs a
daily mailbox handshake, picks up outbound PDS query files from blob storage and delivers them to
MESH, and polls MESH for response messages to write back to blob storage.

All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 HandShakeFunction

| Property | Value |
|---|---|
| **Function name** | `HandShakeFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | `0 0 23 * * *` — daily at 23:00 UTC |

#### 2.1.1 Description

Runs once per day at 23:00 UTC. Calls `IPdsClient.ValidateMailboxAccessAsync(cancellationToken)` to
perform a MESH handshake, confirming that the configured mailbox is reachable and correctly
authenticated. This acts as a daily connectivity check and keeps the mailbox session alive. Any
unhandled exception is logged and re-thrown. Supports cooperative cancellation via
`CancellationToken`.

#### 2.1.2 Configuration

Relies entirely on the shared `meshConfiguration` settings (see Section 3.2). No additional
per-function settings.

---

### 2.2 PickupFileAndSendToMeshFunction

| Property | Value |
|---|---|
| **Function name** | `PickupFileAndSendToMeshFunction` |
| **Trigger type** | Blob trigger |
| **Blob path** | `pds/in/{name}` |
| **Connection setting** | `BlobStorage` |

#### 2.2.1 Description

Fires whenever a new file appears in the `pds/in/` blob container. Reads the blob stream and calls
`IPdsClient.PickupFileAndSendToMesh(pdsStream, fileName, cancellationToken)` to chunk and transmit
the file to the MESH mailbox identified by `pdsSettings:to` using workflow
`pdsSettings:workflowId`. Any unhandled exception is logged and re-thrown. Supports cooperative
cancellation via `CancellationToken`.

#### 2.2.2 Configuration

| Key | Description |
|---|---|
| `BlobStorage` (connection string / managed-identity URI) | Connection used by the blob trigger |
| `blobStorage:azureBlobServiceUri` | Azure Blob Service endpoint URI |
| `blobStorage:azureTenantId` | Entra tenant ID |
| `pdsSettings:to` | Destination MESH mailbox ID |
| `pdsSettings:workflowId` | MESH workflow identifier for outbound PDS messages |
| `pdsSettings:pdsFileHasHeader` | Whether the PDS CSV file includes a header row |
| `pdsSettings:pdsFileRequireTrailingComma` | Whether the PDS CSV file requires a trailing comma |

---

### 2.3 RetreiveMessagesFromMeshAndUpdateStorage

| Property | Value |
|---|---|
| **Function name** | `RetreiveMessagesFromMeshAndUpdateStorage` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | `0 */15 * * * *` — every 15 minutes |

#### 2.3.1 Description

Runs every 15 minutes. Calls
`IPdsClient.RetreiveMessagesFromMeshAndUpdateStorage(cancellationToken)` to poll the MESH inbox for
any response messages from PDS. Retrieved messages are written into blob storage (under
`pdsSettings:outputFolder`) and acknowledged/deleted from MESH. Any unhandled exception is logged
and re-thrown. Supports cooperative cancellation via `CancellationToken`.

#### 2.3.2 Configuration

Relies on the shared `meshConfiguration` and `pdsSettings` (see Section 3). No additional
per-function settings. The timer schedule is hard-coded in the `[TimerTrigger]` attribute.

---

## 3. Shared Configuration

The following settings apply to all functions in this project and are defined in `appsettings.json`.

### 3.1 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI for managed-identity access |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:pds` | `pds` | PDS blob container |
| `blobStorage:blobContainers:emislanding` | `emislanding` | EMIS landing container (shared reference) |
| `blobStorage:blobContainers:versioner` | `versioner` | Versioner container (shared reference) |
| `blobStorage:blobContainers:optOut` | `optOut` | Opt-Out container (shared reference) |
| `blobStorage:blobContainers:tpplanding` | `tpplanding` | TPP landing container (shared reference) |
| `blobStorage:blobContainers:addresses` | `addresses` | Addresses container (shared reference) |

### 3.2 MESH Configuration

| Key | Default | Description |
|---|---|---|
| `meshConfiguration:mailboxId` | _(empty — must be set)_ | MESH mailbox identifier |
| `meshConfiguration:password` | _(empty — must be set)_ | MESH mailbox password |
| `meshConfiguration:sharedKey` | _(empty — must be set)_ | MESH shared key for HMAC authentication |
| `meshConfiguration:url` | _(empty — must be set)_ | MESH API base URL |
| `meshConfiguration:mexClientVersion` | _(empty)_ | Client version header sent to MESH |
| `meshConfiguration:mexOSName` | _(empty)_ | OS name header |
| `meshConfiguration:mexOSVersion` | _(empty)_ | OS version header |
| `meshConfiguration:tlsRootCertificates` | `[]` | PEM-encoded root certificates for TLS |
| `meshConfiguration:tlsIntermediateCertificates` | `[]` | PEM-encoded intermediate certificates |
| `meshConfiguration:clientSigningCertificate` | _(empty)_ | Client signing certificate |
| `meshConfiguration:clientSigningCertificatePassword` | _(empty)_ | Signing certificate password |
| `meshConfiguration:maxChunkSizeInMegabytes` | `20` | Maximum MESH message chunk size in MB |

### 3.3 PDS Settings

| Key | Default | Description |
|---|---|---|
| `pdsSettings:inputFolder` | `/in` | Blob folder for incoming PDS query files |
| `pdsSettings:outputFolder` | `/out` | Blob folder for incoming PDS response files |
| `pdsSettings:pdsFileHasHeader` | `true` | Whether PDS CSV files have a header row |
| `pdsSettings:pdsFileRequireTrailingComma` | `true` | Whether PDS CSV files require a trailing comma |
| `pdsSettings:to` | _(empty — must be set)_ | Destination MESH mailbox ID |
| `pdsSettings:workflowId` | _(empty — must be set)_ | MESH workflow identifier |

### 3.4 Logging

| Key | Default | Description |
|---|---|---|
| `Logging:LogLevel:Default` | `Information` | Default log level |
| `Logging:LogLevel:Microsoft.AspNetCore.OptOuts` | `Warning` | Log level for ASP.NET Core framework messages |
