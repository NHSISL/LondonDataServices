# LHDS.Functions.Addresses

This document describes every Azure Function in the `LHDS.Functions.Addresses` project, including
what each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The Addresses function app is responsible for loading raw address (UPRN/Ordinance) data into the
system, ingesting addresses that need resolving, running the address-matching process, and exporting
the resolved results. All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 AddressLoaderFunction

| Property | Value |
|---|---|
| **Function name** | `AddressLoaderFunction` |
| **Trigger type** | Blob trigger |
| **Blob path** | `uprn/ordinance/in/{name}` |
| **Connection setting** | `BlobStorage` |

#### 2.1.1 Description

Fires whenever a new file is dropped into the `uprn/ordinance/in/` container path. It reads the
blob stream and forwards it to `IAddressClient.LoadAddressDataAsync`, which loads the raw Ordinance
Survey UPRN address data into the backing store. Any unhandled exception is logged and re-thrown so
the Functions runtime can apply retry/dead-letter behaviour.

#### 2.1.2 Configuration

| Key | Description |
|---|---|
| `BlobStorage` (connection string / managed-identity URI) | Connection used by the blob trigger to watch the container |
| `blobStorage:azureBlobServiceUri` | Azure Blob Service endpoint URI (used when authenticating via managed identity) |
| `blobStorage:azureTenantId` | Entra (AAD) tenant ID for managed-identity authentication |

---

### 2.2 ResolvedAddressLoaderFunction

| Property | Value |
|---|---|
| **Function name** | `ResolvedAddressLoaderFunction` |
| **Trigger type** | Blob trigger |
| **Blob path** | `uprn/in/{name}` |
| **Connection setting** | `BlobStorage` |

#### 2.2.1 Description

Fires whenever a new file appears in `uprn/in/`. The blob stream is passed to
`IAddressClient.LoadAddressesToResolveAsync`, which stages the incoming addresses that require
resolution (i.e. UPRN lookup / matching) into the processing queue. Any unhandled exception is
logged and re-thrown.

#### 2.2.2 Configuration

| Key | Description |
|---|---|
| `BlobStorage` (connection string / managed-identity URI) | Connection used by the blob trigger |
| `blobStorage:azureBlobServiceUri` | Azure Blob Service endpoint URI |
| `blobStorage:azureTenantId` | Entra tenant ID |
| `storageQueueSettings:storageQueueServiceUri` | Azure Storage Queue service URI |
| `storageQueueSettings:azureTenantId` | Tenant ID for queue authentication |
| `storageQueueSettings:storageQueues:resolveAddressQueue` | Name of the queue used to enqueue resolve-address work items (default: `ResolveAddressQueue`) |

---

### 2.3 ResolvedAddressMatchTimerFunction

| Property | Value |
|---|---|
| **Function name** | `ResolvedAddressMatchTimerFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | `0 */30 * * * *` — every 30 minutes |

#### 2.3.1 Description

Runs on a fixed schedule every 30 minutes. Calls `IAddressClient.MatchAddressDataAsync` to process
the queue of addresses waiting for UPRN matching. The function logs the execution time and the next
scheduled run on completion. Any unhandled exception is logged and re-thrown.

#### 2.3.2 Configuration

No additional application settings beyond the shared Blob Storage and Storage Queue settings listed
below. The timer schedule is hard-coded in the `[TimerTrigger]` attribute.

---

### 2.4 ResolvedAddressExporterTimerFunction

| Property | Value |
|---|---|
| **Function name** | `ResolvedAddressExporterTimerFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | `0 15 * * * *` — at 15 minutes past every hour |

#### 2.4.1 Description

Runs at 15 minutes past every hour. Calls `IAddressClient.ExportResolvedAddressesAsync` to push
matched/resolved addresses downstream (e.g. into a blob export or downstream service). Any
unhandled exception is logged and re-thrown.

#### 2.4.2 Configuration

No additional application settings beyond the shared settings listed below. The timer schedule is
hard-coded in the `[TimerTrigger]` attribute.

---

## 3. Shared Configuration

The following settings apply to all functions in this project and are defined in `appsettings.json`.

### 3.1 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI for managed-identity access |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:pds` | _(empty — must be set)_ | Name of the PDS blob container |

### 3.2 Storage Queue

| Key | Default | Description |
|---|---|---|
| `storageQueueSettings:storageQueueServiceUri` | _(empty — must be set)_ | Azure Storage Queue service URI |
| `storageQueueSettings:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `storageQueueSettings:storageQueues:resolveAddressQueue` | `ResolveAddressQueue` | Queue name for address resolution work items |

### 3.3 MESH Configuration

| Key | Default | Description |
|---|---|---|
| `meshConfiguration:mailboxId` | _(empty — must be set)_ | MESH mailbox identifier |
| `meshConfiguration:password` | _(empty — must be set)_ | MESH mailbox password |
| `meshConfiguration:sharedKey` | _(empty — must be set)_ | MESH shared key for HMAC authentication |
| `meshConfiguration:url` | _(empty — must be set)_ | MESH API base URL |
| `meshConfiguration:mexClientVersion` | _(empty)_ | Client version header sent to MESH |
| `meshConfiguration:mexOSName` | _(empty)_ | OS name header sent to MESH |
| `meshConfiguration:mexOSVersion` | _(empty)_ | OS version header sent to MESH |
| `meshConfiguration:tlsRootCertificates` | `[]` | Array of PEM-encoded root certificates for TLS |
| `meshConfiguration:tlsIntermediateCertificates` | `[]` | Array of PEM-encoded intermediate certificates |
| `meshConfiguration:clientSigningCertificate` | _(empty)_ | Path or value of the client signing certificate |
| `meshConfiguration:clientSigningCertificatePassword` | _(empty)_ | Password for the signing certificate |
| `meshConfiguration:maxChunkSizeInMegabytes` | `20` | Maximum MESH message chunk size in MB |

### 3.4 PDS Settings

| Key | Default | Description |
|---|---|---|
| `pdsSettings:inputFolder` | `/in` | Blob folder path for incoming PDS files |
| `pdsSettings:outputFolder` | `/out` | Blob folder path for outgoing PDS files |
| `pdsSettings:pdsFileHasHeader` | `true` | Whether PDS CSV files contain a header row |
| `pdsSettings:pdsFileRequireTrailingComma` | `true` | Whether PDS CSV files require a trailing comma |
| `pdsSettings:to` | _(empty — must be set)_ | MESH destination mailbox ID |
| `pdsSettings:workflowId` | _(empty — must be set)_ | MESH workflow identifier |

### 3.5 Logging

| Key | Default | Description |
|---|---|---|
| `Logging:LogLevel:Default` | `Information` | Default log level |
| `Logging:LogLevel:Microsoft.AspNetCore.OptOuts` | `Warning` | Log level for ASP.NET Core framework messages |
