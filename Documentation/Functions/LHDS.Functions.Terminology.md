# LHDS.Functions.Terminology

This document describes every Azure Function in the `LHDS.Functions.Terminology` project, including
what each function does, how it is triggered, and what configuration is required.

---

## 1. Overview

The Terminology function app is responsible for keeping local terminology artifacts
(CodeSystems, ValueSets, ConceptMaps) in sync with a remote terminology server. One function
retrieves the high-level metadata catalogue nightly; a second function continuously downloads
the full details of any artifacts that have been catalogued but not yet fully fetched.

All functions are implemented as isolated-process Azure Functions using the
`Microsoft.Azure.Functions.Worker` host.

---

## 2. Functions

### 2.1 TerminologyMetadataFunction

| Property | Value |
|---|---|
| **Function name** | `TerminologyMetadataFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | `0 0 23 * * *` — daily at 23:00 UTC |

#### 2.1.1 Description

Runs once per day at 23:00 UTC. Calls
`ITerminologyClient.RetrieveArtifactMetadataAsync(["CodeSystem", "ValueSet", "ConceptMap"])` to
query the remote terminology server for a catalogue of all available artifacts across the three
supported resource types. The metadata records are persisted locally so that
`TerminologyDetailFunction` knows which artifacts still require their full content to be downloaded.
Any unhandled exception is logged and re-thrown.

#### 2.1.2 Configuration

Relies on `ontologySettings` (see Section 3.3). No additional per-function settings.

---

### 2.2 TerminologyDetailFunction

| Property | Value |
|---|---|
| **Function name** | `TerminologyDetailFunction` |
| **Trigger type** | Timer trigger |
| **Schedule (CRON)** | `0 */15 * * * *` — every 15 minutes |

#### 2.2.1 Description

Runs every 15 minutes. Calls `ITerminologyClient.RetrieveArtifactDetailsAsync()` to download the
full content of any terminology artifacts that have been catalogued (by
`TerminologyMetadataFunction`) but whose details have not yet been retrieved. This catch-up
mechanism ensures the local store converges to a fully downloaded state even if the nightly
metadata run produces many new records. Any unhandled exception is logged and re-thrown.

#### 2.2.2 Configuration

Relies on `ontologySettings` and `blobStorage` (see Section 3). No additional per-function
settings.

---

## 3. Shared Configuration

The following settings apply to all functions in this project and are defined in `appsettings.json`.

### 3.1 Blob Storage

| Key | Default | Description |
|---|---|---|
| `blobStorage:azureBlobServiceUri` | _(empty — must be set)_ | Azure Blob Service URI for managed-identity access |
| `blobStorage:azureTenantId` | _(empty — must be set)_ | Entra tenant ID |
| `blobStorage:blobContainers:terminology` | `ingress` | Container used to land downloaded terminology artifacts |
| `blobStorage:blobContainers:emislanding` | `emislanding` | EMIS landing container (shared reference) |
| `blobStorage:blobContainers:versioner` | `versioner` | Versioner container (shared reference) |
| `blobStorage:blobContainers:optOut` | `optOut` | Opt-Out container (shared reference) |
| `blobStorage:blobContainers:pds` | `pds` | PDS container (shared reference) |
| `blobStorage:blobContainers:tpplanding` | `tpplanding` | TPP landing container (shared reference) |
| `blobStorage:blobContainers:addresses` | `addresses` | Addresses container (shared reference) |

### 3.2 MESH Configuration

| Key | Default | Description |
|---|---|---|
| `meshConfiguration:mailboxId` | _(empty — must be set)_ | MESH mailbox identifier |
| `meshConfiguration:password` | _(empty — must be set)_ | MESH mailbox password |
| `meshConfiguration:sharedKey` | _(empty — must be set)_ | MESH shared key for HMAC authentication |
| `meshConfiguration:url` | _(empty — must be set)_ | MESH API base URL |
| `meshConfiguration:mexClientVersion` | _(empty)_ | Client version header |
| `meshConfiguration:mexOSName` | _(empty)_ | OS name header |
| `meshConfiguration:mexOSVersion` | _(empty)_ | OS version header |
| `meshConfiguration:tlsRootCertificates` | `[]` | PEM-encoded root certificates for TLS |
| `meshConfiguration:tlsIntermediateCertificates` | `[]` | PEM-encoded intermediate certificates |
| `meshConfiguration:clientSigningCertificate` | _(empty)_ | Client signing certificate |
| `meshConfiguration:clientSigningCertificatePassword` | _(empty)_ | Signing certificate password |
| `meshConfiguration:maxChunkSizeInMegabytes` | `20` | Maximum MESH chunk size in MB |

### 3.3 Ontology / Terminology Server Settings

| Key | Default | Description |
|---|---|---|
| `ontologySettings:terminologyServerBaseUrl` | _(empty — must be set)_ | Base URL of the remote terminology server |
| `ontologySettings:terminologyServerAuthenticationRelativeUrl` | _(empty — must be set)_ | Relative URL for OAuth token acquisition |
| `ontologySettings:terminologyServerResourceRelativeUrl` | _(empty — must be set)_ | Relative URL for resource queries |
| `ontologySettings:clientId` | _(empty — must be set)_ | OAuth client ID |
| `ontologySettings:clientSecret` | _(empty — must be set)_ | OAuth client secret |
| `ontologySettings:TimeoutInSeconds` | _(empty — must be set)_ | HTTP client timeout in seconds |
| `ontologySettings:MaxResponseContentBufferSizeInMegaBytes` | _(empty — must be set)_ | Maximum response buffer size in MB |
| `ontologySettings:LandingFolder` | `inbox/terminology` | Blob sub-folder where downloaded artifacts are written |

### 3.4 PDS Settings

| Key | Default | Description |
|---|---|---|
| `pdsSettings:inputFolder` | `/in` | Blob folder for incoming PDS files |
| `pdsSettings:outputFolder` | `/out` | Blob folder for outgoing PDS files |
| `pdsSettings:pdsFileHasHeader` | `true` | Whether PDS CSV files have a header row |
| `pdsSettings:pdsFileRequireTrailingComma` | `true` | Whether PDS CSV files require a trailing comma |
| `pdsSettings:to` | _(empty — must be set)_ | MESH destination mailbox ID |
| `pdsSettings:workflowId` | _(empty — must be set)_ | MESH workflow identifier |

### 3.5 Logging

| Key | Default | Description |
|---|---|---|
| `Logging:LogLevel:Default` | `Information` | Default log level |
| `Logging:LogLevel:Microsoft.AspNetCore.OptOuts` | `Warning` | Log level for ASP.NET Core framework messages |
