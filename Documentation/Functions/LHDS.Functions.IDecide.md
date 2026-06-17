# LHDS.Functions.IDecide

This document describes every Azure Function in the `LHDS.Functions.IDecide` project, including
what each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The IDecide function app is responsible for periodically retrieving patient treatment-decision
records from the IDecide API and landing them into blob storage for downstream processing. IDecide
is an external service that records patient opt-in/opt-out decisions for data sharing; this function
app acts as the ingestion point for those records into the LHDS platform.

All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 GetPatientDecisionsFunction

| Property | Value |
|---|---|
| **Function name** | `GetPatientDecisionsFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | Configured via `getPatientDecisionsFunction` app setting (default: `0 */15 * * * *` — every 15 minutes) |

#### 2.1.1 Description

Runs on the schedule defined by the `getPatientDecisionsFunction` application setting. Calls
`IIDecideClient.GetPatientDecisions()` to query the IDecide API for the latest patient
treatment-decision records. Retrieved records are written to blob storage under the folder and
prefix configured in `IDecide:folderName` / `IDecide:filePrefix`, where they can be picked up by
downstream processing pipelines. NHS numbers may optionally be hashed before storage, controlled
by the `IDecide:HashNhsNumber` flag. Any unhandled exception is logged and re-thrown.

#### 2.1.2 Configuration

| Key | Description |
|---|---|
| `getPatientDecisionsFunction` | CRON expression controlling how often decisions are fetched (default: `0 */15 * * * *`) |
| `IDecide:iDecideBaseUrl` | Base URL of the IDecide API |
| `IDecide:iDecideAuthenticationRelativeUrl` | Relative URL used to obtain an OAuth access token |
| `IDecide:iDecidePatientDecisionsRelativeUrl` | Relative URL for the patient decisions endpoint |
| `IDecide:iDecideRecordAdoptionRelativeUrl` | Relative URL used to acknowledge/adopt retrieved records |
| `IDecide:iDecideScope` | OAuth scope required for IDecide API calls |
| `IDecide:folderName` | Blob folder where retrieved decision files are written (default: `localdataoptout`) |
| `IDecide:filePrefix` | Prefix applied to generated decision filenames (default: `ldoo`) |
| `IDecide:HashNhsNumber` | If `true`, NHS numbers are hashed before being written to storage (default: `false`) |
| `IDecide:hashPepper` | Pepper value appended to NHS numbers before hashing (default: `pepper` — must be changed in production) |
| `IDecide:TimeoutInSeconds` | HTTP client timeout in seconds |
| `IDecide:MaxResponseContentBufferSizeInMegaBytes` | Maximum HTTP response buffer size in MB |

---

## 3. Shared Configuration

The following settings apply to the function in this project and are defined in `appsettings.json`.

### 3.1 Timer Trigger Schedule

| Key | Default | Description |
|---|---|---|
| `getPatientDecisionsFunction` | `0 */15 * * * *` | CRON expression for `GetPatientDecisionsFunction` |

### 3.2 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI for managed-identity access |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:emislanding` | `emislanding` | EMIS landing container (shared reference) |
| `blobStorage:blobContainers:versioner` | `versioner` | Versioner container (shared reference) |
| `blobStorage:blobContainers:optOut` | `optOut` | Opt-Out container (shared reference) |
| `blobStorage:blobContainers:pds` | `pds` | PDS container (shared reference) |
| `blobStorage:blobContainers:tpplanding` | `tpplanding` | TPP landing container (shared reference) |
| `blobStorage:blobContainers:addresses` | `addresses` | Addresses container (shared reference) |

### 3.3 IDecide API Settings

| Key | Default | Description |
|---|---|---|
| `IDecide:hashPepper` | `pepper` | Pepper value for NHS number hashing — **must be overridden in production** |
| `IDecide:folderName` | `localdataoptout` | Blob sub-folder for output files |
| `IDecide:filePrefix` | `ldoo` | Prefix for generated output filenames |
| `IDecide:iDecideBaseUrl` | _(empty — must be set)_ | IDecide API base URL |
| `IDecide:iDecideAuthenticationRelativeUrl` | _(empty — must be set)_ | OAuth token endpoint (relative) |
| `IDecide:iDecidePatientDecisionsRelativeUrl` | _(empty — must be set)_ | Patient decisions endpoint (relative) |
| `IDecide:iDecideRecordAdoptionRelativeUrl` | _(empty — must be set)_ | Record adoption endpoint (relative) |
| `IDecide:iDecideScope` | _(empty — must be set)_ | OAuth scope |
| `IDecide:HashNhsNumber` | `false` | Whether to hash NHS numbers before storage |
| `IDecide:MaxResponseContentBufferSizeInMegaBytes` | _(empty — must be set)_ | Max HTTP response buffer (MB) |
| `IDecide:TimeoutInSeconds` | _(empty — must be set)_ | HTTP client timeout (seconds) |

### 3.4 Logging

| Key | Default | Description |
|---|---|---|
| `Logging:LogLevel:Default` | `Information` | Default log level |
| `Logging:LogLevel:Microsoft.AspNetCore.OptOuts` | `Warning` | Log level for ASP.NET Core framework messages |
