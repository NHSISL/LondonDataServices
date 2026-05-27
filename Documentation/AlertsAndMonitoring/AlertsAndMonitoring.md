# Alerts and Monitoring

This document describes every health check in the LHDS Admin Portal API, including what it monitors,
how RAG (Red/Amber/Green) status is determined, what metrics are emitted to Application Insights,
and how thresholds are configured.

---

## 1. How It Works

Health checks run on a repeating schedule controlled by two global settings:

| Configuration Key | Default | Purpose |
|---|---|---|
| `HealthChecks:StartupDelaySeconds` | `10` | Delay before the first run after startup |
| `HealthChecks:PublishIntervalSeconds` | `60` | How frequently checks repeat |

Each check produces a `HealthCheckResult` with a `Status` of **Healthy**, **Degraded**, or **Unhealthy**
and a `Data` dictionary of named values. After every run, the publisher writes both an Application
Insights **Event** (containing string/date properties) and one or more Application Insights **Metrics**
(containing numeric values) per check entry.

### 1.1 Application Insights Metric Naming

All metrics are namespaced with the orchestration entry key to prevent collision across domains:

```
{entryKey}.StatusCode         — 0 = Healthy (🟢), 1 = Degraded (🟡), 2 = Unhealthy (🔴)
{entryKey}.{metricKey}        — e.g. ingestionTrackingHealthChecks.degradedItems
```

The `StatusCode` metric maps directly to the .NET `HealthStatus` enum values and is designed to be used as an
**Application Insights metric alert threshold** (e.g. alert when `StatusCode >= 1` for Amber, or `>= 2` for Red).

### 1.2 RAG Threshold Pattern

Most checks use a two-window model based on `UpdatedDate` (or `CreatedDate` for pipeline-alive checks):

| Status | Condition |
|---|---|
| 🟢 **Healthy** | No stuck/failing records, or all records within normal operating time |
| 🟡 **Degraded** | Records whose age is between `DegradedThreshold` and `UnHealthyThreshold` minutes |
| 🔴 **Unhealthy** | Records whose age exceeds `UnHealthyThreshold` minutes |

Thresholds are in **minutes** unless stated otherwise and are all overridable in `appsettings.json`.

---

## 2. Orchestration Groups

Checks are grouped into six orchestration entries published independently to Application Insights.

| Entry Key (App Insights namespace) | Domain |
|---|---|
| `ingestionTrackingHealthChecks` | Ingestion Tracking |
| `resolvedAddressHealthChecks` | Resolved Address |
| `optOutsHealthChecks` | Opt-Outs |
| `terminologyArtifactsHealthChecks` | Terminology Artifacts |
| `terminologyPollsHealthChecks` | Terminology Polls |
| `pdsAuditHealthChecks` | PDS Audit |

---

## 3. Ingestion Tracking Health Checks

**Entry key:** `ingestionTrackingHealthChecks`  
**Config root:** `HealthChecks:IngestionTracking`

---

### 3.1 Pipeline Alive

**Check name:** `pipelineAlive`  
**Config section:** `HealthChecks:IngestionTracking:PipelineAlive`  
**What it monitors:** Whether any new `IngestionTracking` records have been created recently.
Returns Healthy if the table is empty (pipeline has never run).

#### 3.1.1 RAG Thresholds

| Status | Condition | Default |
|---|---|---|
| 🟢 Healthy | A record with `CreatedDate >= now - DegradedThreshold` exists | — |
| 🟡 Degraded | No record within `DegradedThreshold` but one exists within `UnHealthyThreshold` | 1440 min (24 h) |
| 🔴 Unhealthy | No record within `UnHealthyThreshold` | 2880 min (48 h) |

#### 3.1.2 Metrics Published

| Metric Key | Type | Description |
|---|---|---|
| `StatusCode` | numeric | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |

#### 3.1.3 Event Properties

| Property | Description |
|---|---|
| `description` | "Pipeline Alive" |
| `degradedThresholdMinutes` | Configured degraded threshold |
| `unHealthyThresholdMinutes` | Configured unhealthy threshold |
| `checkedAt` | ISO-8601 timestamp of the check |
| `message` | Human-readable status message |
| `status` | Healthy / Degraded / Unhealthy |

---

### 3.2 Decryption Queue

**Check name:** `decryption`  
**Config section:** `HealthChecks:IngestionTracking:Decryption`  
**What it monitors:** Files that have **not** been decrypted (`Decrypted = false`) and have **not** been deleted (`FileDeleted = false`), aged by `UpdatedDate`.

#### 3.2.1 RAG Thresholds

| Status | Condition | Default |
|---|---|---|
| 🟢 Healthy | No such records | — |
| 🟡 Degraded | Records with `UpdatedDate` between `DegradedThreshold` and `UnHealthyThreshold` ago | 1440 min |
| 🔴 Unhealthy | Records with `UpdatedDate` older than `UnHealthyThreshold` ago | 2880 min |

#### 3.2.2 Metrics Published

| Metric Key | Type | Description |
|---|---|---|
| `StatusCode` | numeric | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | numeric | Count of degraded records |
| `unHealthyItems` | numeric | Count of unhealthy records |
| `unDecryptedItems` | numeric | Total stuck records |

#### 3.2.3 Event Properties

`description`, `unDecryptedItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.3 Download Queue

**Check name:** `downloadQueue`  
**Config section:** `HealthChecks:IngestionTracking:Download`  
**What it monitors:** Files not yet downloaded (`IsDownloaded = false`), not currently processing (`IsProcessing = false`), and not deleted (`FileDeleted = false`), aged by `UpdatedDate`.

#### 3.3.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.3.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `notDownloadedItems` | Total |

#### 3.3.3 Event Properties

`description`, `notDownloadedItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.4 Processing Queue

**Check name:** `processingQueue`  
**Config section:** `HealthChecks:IngestionTracking:Processing`  
**What it monitors:** Files stuck with `IsProcessing = true`, aged by `UpdatedDate`.

#### 3.4.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.4.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `stuckInProcessing` | Total |

#### 3.4.3 Event Properties

`description`, `stuckInProcessing`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.5 Failed To Process

**Check name:** `failedToProcess`  
**Config section:** `HealthChecks:IngestionTracking:FailedToProcess`  
**What it monitors:** Files that have reached or exceeded the maximum retry count (`RetryCount >= RetryCount config`), indicating they have permanently failed. Severity is determined by `UpdatedDate`.

#### 3.5.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.5.2 Additional Config

| Key | Default | Description |
|---|---|---|
| `RetryCount` | `3` | Retry count threshold to classify as permanently failed |

#### 3.5.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `failedToProcess` | Total failed records |

#### 3.5.4 Event Properties

`description`, `failedToProcess`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.6 Retry Warning

**Check name:** `retryWarning`  
**Config section:** `HealthChecks:IngestionTracking:RetryWarning`  
**What it monitors:** Files that have retried at least once but have not yet reached the failure threshold (`RetryCount > 0 && RetryCount < MaxRetryCount`). These are in danger of failing. Severity is determined by `UpdatedDate`.

#### 3.6.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.6.2 Additional Config

| Key | Default | Description |
|---|---|---|
| `MaxRetryCount` | `3` | Upper retry boundary (exclusive) |

#### 3.6.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `retryWarningItems` | Total at-risk records |

#### 3.6.4 Event Properties

`description`, `retryWarningItems`, `degradedItems`, `unHealthyItems`, `maxRetryCount`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.7 Deleted Without Completion

**Check name:** `deletedWithoutCompletion`  
**Config section:** `HealthChecks:IngestionTracking:DeletedWithoutCompletion`  
**What it monitors:** Files that have been deleted (`FileDeleted = true`) before being decrypted (`Decrypted = false`), indicating they were removed mid-pipeline. Severity is determined by how long ago the deletion occurred (`UpdatedDate`).

#### 3.7.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.7.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `deletedWithoutCompletionItems` | Total |

#### 3.7.3 Event Properties

`description`, `deletedWithoutCompletionItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.8 Files Received (Per Supplier)

**Check name:** `filesReceived`  
**Config section:** `HealthChecks:IngestionTracking:FilesReceived`  
**What it monitors:** For each supplier that has ingestion tracking enabled (`IsIngestionTracked = true`),
checks whether new files have been received recently (by `CreatedDate`). Each supplier is reported
independently. If a supplier has never sent a file, it is reported as Healthy (no expectation set yet).

#### 3.8.1 RAG Thresholds

| Status | Condition | Default |
|---|---|---|
| 🟢 Healthy | File received within `DegradedThreshold`, or supplier has no files yet | — |
| 🟡 Degraded | No file within `DegradedThreshold` but one within `UnHealthyThreshold` | 1440 min |
| 🔴 Unhealthy | No file within `UnHealthyThreshold` | 2880 min |

#### 3.8.2 Metrics Published

Per-supplier numeric metric where the metric key is the supplier name:

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `{supplierName}.filesReceived` | Total files received from that supplier |

#### 3.8.3 Event Properties (per supplier entry in the data dictionary)

`description` (supplier name), `filesReceived`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `status`

---

### 3.9 Incomplete Batches

**Check name:** `incompleteBatchesQueue`  
**Config section:** `HealthChecks:IngestionTracking:IncompleteBatches`  
**What it monitors:** Distinct batch identifiers where at least one file has `IsBatchComplete = false`,
aged by `UpdatedDate`. Counts are at the **batch** level (not file level), so one batch with many
incomplete files counts as one degraded/unhealthy batch.

#### 3.9.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.9.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Distinct degraded batch count |
| `unHealthyItems` | Distinct unhealthy batch count |
| `incompleteBatches` | Total distinct incomplete batches |

#### 3.9.3 Event Properties

`description`, `incompleteBatches`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.10 Stale Last Batch Check

**Check name:** `staleLastBatchCheck`  
**Config section:** `HealthChecks:IngestionTracking:StaleLastBatchCheck`  
**What it monitors:** Files where `IsBatchComplete = false` and either `LastBatchCompleteCheck` is
null (never checked) or the check timestamp is older than `DegradedThreshold`. This detects cases
where the batch-completion checker has stopped running or is being skipped.

#### 3.10.1 RAG Thresholds

| Status | Condition | Default |
|---|---|---|
| 🟢 Healthy | `LastBatchCompleteCheck` is within `DegradedThreshold` | — |
| 🟡 Degraded | `LastBatchCompleteCheck` is between `DegradedThreshold` and `UnHealthyThreshold` ago | 1440 min |
| 🔴 Unhealthy | `LastBatchCompleteCheck` is null or older than `UnHealthyThreshold` | 2880 min |

#### 3.10.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `staleLastBatchCheckItems` | Total |

#### 3.10.3 Event Properties

`description`, `staleLastBatchCheckItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 3.11 Last Seen Staleness

**Check name:** `lastSeenStaleness`  
**Config section:** `HealthChecks:IngestionTracking:LastSeenStaleness`  
**What it monitors:** In-progress files (`FileDeleted = false`, `IsBatchComplete = false`) where
`LastSeen` has become stale. A stale `LastSeen` indicates the file is no longer being actively
processed and may be orphaned.

#### 3.11.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 3.11.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `staleLastSeenItems` | Total orphaned-risk files |

#### 3.11.3 Event Properties

`description`, `staleLastSeenItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

## 4. Resolved Address Health Checks

**Entry key:** `resolvedAddressHealthChecks`  
**Config root:** `HealthChecks:ResolvedAddress`

---

### 4.1 Pipeline Alive

**Check name:** `pipelineAlive`  
**Config section:** `HealthChecks:ResolvedAddress:PipelineAlive`  
**What it monitors:** Whether any `ResolvedAddress` records have been created recently (`CreatedDate`).
Returns Healthy if the table is empty.

#### 4.1.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.1.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |

#### 4.1.3 Event Properties

`description`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.2 Queued For Processing

**Check name:** `queuedForProcessing`  
**Config section:** `HealthChecks:ResolvedAddress:Queued`  
**What it monitors:** Records that are neither processing (`IsProcessing = false`) nor processed
(`IsProcessed = false`), stuck in the incoming queue. Aged by `UpdatedDate`.

#### 4.2.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.2.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `queuedItems` | Total queued |

#### 4.2.3 Event Properties

`description`, `queuedItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.3 Processing Queue

**Check name:** `processingQueue`  
**Config section:** `HealthChecks:ResolvedAddress:Processing`  
**What it monitors:** Records stuck with `IsProcessing = true`, aged by `UpdatedDate`.

#### 4.3.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.3.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `stuckInProcessing` | Total stuck |

#### 4.3.3 Event Properties

`description`, `stuckInProcessing`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.4 Address Matching Time

**Check name:** `addressMatchingTime`  
**Config section:** `HealthChecks:ResolvedAddress:AddressMatchingTime`  
**What it monitors:** Audit records where `AuditType = "Resolved Address Match"` and only a start
audit exists (no completion audit for the same `CorrelationId`). These are matches that have been
running longer than expected. Aged by `ProcessStartDateTime` (the audit's `CreatedDate`).

#### 4.4.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.4.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `unacceptableProcessingTime` | Total long-running matches |

#### 4.4.3 Event Properties

`description`, `unacceptableProcessingTime`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.5 Match Quality

**Check name:** `matchQuality`  
**Config section:** `HealthChecks:ResolvedAddress:MatchQuality`  
**What it monitors:** The ratio of `MatchedWithAssign = true` records to all records updated in the
last 24 hours. A drop in match rate indicates data quality or matching-logic issues.
Returns Healthy if no records exist in the window.

#### 4.5.1 RAG Thresholds (percentage — not minutes)

| Status | Condition | Default |
|---|---|---|
| 🟢 Healthy | Match rate > `DegradedThresholdPercentage` | — |
| 🟡 Degraded | Match rate > `UnHealthyThresholdPercentage` but â‰¤ `DegradedThresholdPercentage` | 90% |
| 🔴 Unhealthy | Match rate â‰¤ `UnHealthyThresholdPercentage` | 80% |

#### 4.5.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `averageMatchRate` | Ratio of matched records (0.0â€“1.0) |

#### 4.5.3 Event Properties

`description`, `averageMatchRate`, `isDegraded`, `isUnhealthy`, `degradedThresholdPercentage`, `unHealthyThresholdPercentage`, `checkedAt`, `message`, `status`

---

### 4.6 Queued For Export

**Check name:** `queuedForExport`  
**Config section:** `HealthChecks:ResolvedAddress:Export`  
**What it monitors:** Records that are processed (`IsProcessed = true`) but not yet exported
(`IsExported = false`), stuck in the export queue. Aged by `UpdatedDate`.

#### 4.6.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.6.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `queuedItems` | Total queued for export |

#### 4.6.3 Event Properties

`description`, `queuedItems`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.7 Failed To Process

**Check name:** `failedToProcess`  
**Config section:** `HealthChecks:ResolvedAddress:FailedToProcess`  
**What it monitors:** Records that have reached or exceeded the retry limit (`RetryCount >= RetryCount`)
and are not yet processed (`IsProcessed = false`). Aged by `UpdatedDate`.

#### 4.7.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.7.2 Additional Config

| Key | Default |
|---|---|
| `RetryCount` | `3` |

#### 4.7.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `failedToProcess` | Total permanently failed |

#### 4.7.4 Event Properties

`description`, `failedToProcess`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.8 Failed To Export

**Check name:** `failedToExport`  
**Config section:** `HealthChecks:ResolvedAddress:FailedToExport`  
**What it monitors:** Records that have reached the retry limit, are processed (`IsProcessed = true`),
but are not yet exported (`IsExported = false`). Aged by `UpdatedDate`.

#### 4.8.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.8.2 Additional Config

| Key | Default |
|---|---|
| `RetryCount` | `3` |

#### 4.8.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `failedToExport` | Total permanently failed to export |

#### 4.8.4 Event Properties

`description`, `failedToExport`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 4.9 Retry Warning

**Check name:** `retryWarning`  
**Config section:** `HealthChecks:ResolvedAddress:RetryWarning`  
**What it monitors:** Records with `RetryCount > 0 && RetryCount < MaxRetryCount` — retrying but
not yet permanently failed. Aged by `UpdatedDate`.

#### 4.9.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 4.9.2 Additional Config

| Key | Default |
|---|---|
| `MaxRetryCount` | `3` |

#### 4.9.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `retryWarningItems` | Total at-risk records |

#### 4.9.4 Event Properties

`description`, `retryWarningItems`, `degradedItems`, `unHealthyItems`, `maxRetryCount`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

## 5. Opt-Out Health Checks

**Entry key:** `optOutsHealthChecks`  
**Config root:** `HealthChecks:OptOuts`

---

### 5.1 Pipeline Alive

**Check name:** `pipelineAlive`  
**Config section:** `HealthChecks:OptOuts:PipelineAlive`  
**What it monitors:** Whether any `OptOut` records have been created recently (`CreatedDate`).
Returns Healthy if the table is empty.

#### 5.1.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 5.1.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |

#### 5.1.3 Event Properties

`description`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 5.2 Expired Opt-Outs

**Check name:** `expiredOptOuts`  
**Config section:** `HealthChecks:OptOuts:ExpiredOptOuts`  
**What it monitors:** Opt-out records where **both** the cache has expired (`CacheTime < now - ExpiredAfterDays`)
**and** the last MESH send is outdated (`LastSentToMesh < now - LastSentExpiredAfterDays`). These
records are stale in both dimensions and have not been re-sent. Severity is determined by `UpdatedDate`.

#### 5.2.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 5.2.2 Additional Config

| Key | Default | Description |
|---|---|---|
| `ExpiredAfterDays` | `7` | Age in days before `CacheTime` is considered expired |
| `LastSentExpiredAfterDays` | `2` | Age in days before `LastSentToMesh` is considered overdue |

#### 5.2.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `expiredAndOutdated` | Total expired+outdated records |

#### 5.2.4 Event Properties

`description`, `expiredAndOutdated`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 5.3 Stuck Opt-Outs

**Check name:** `stuckOptOuts`  
**Config section:** `HealthChecks:OptOuts:StuckOptOuts`  
**What it monitors:** Opt-out records whose cache is still valid (`CacheTime >= now - ExpiredAfterDays`)
but which have not been sent to MESH recently (`LastSentToMesh < now - StuckAfterDays`). This is
distinct from *Expired Opt-Outs*: these are records that are still active but are not being dispatched.
Severity is determined by `UpdatedDate`.

#### 5.3.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 5.3.2 Additional Config

| Key | Default | Description |
|---|---|---|
| `ExpiredAfterDays` | `7` | Age in days defining an active (non-expired) record |
| `StuckAfterDays` | `1` | How long since `LastSentToMesh` before the record is considered stuck |

#### 5.3.3 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `stuckOptOuts` | Total stuck records |

#### 5.3.4 Event Properties

`description`, `stuckOptOuts`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

## 6. Terminology Artifacts Health Checks

**Entry key:** `terminologyArtifactsHealthChecks`  
**Config root:** `HealthChecks:TerminologyArtifacts`

All artifact checks break counts down by resource type: `CodeSystem`, `ConceptMap`, and `ValueSet`.

---

### 6.1 Artifact Errors

**Check name:** `artifactErrors`  
**Config section:** `HealthChecks:TerminologyArtifacts:FailedToProcess`  
**What it monitors:** Artifacts with `IsError = true`, grouped by resource type. Aged by `UpdatedDate`.

#### 6.1.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 6.1.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedCodeSystemItems` | Degraded CodeSystem errors |
| `unHealthyCodeSystemItems` | Unhealthy CodeSystem errors |
| `degradedConceptMapItems` | Degraded ConceptMap errors |
| `unHealthyConceptMapItems` | Unhealthy ConceptMap errors |
| `degradedValueSetItems` | Degraded ValueSet errors |
| `unHealthyValueItems` | Unhealthy ValueSet errors |
| `failedToProcess` | Total across all types |

#### 6.1.3 Event Properties

`description`, `failedToProcess`, `degradedCodeSystemItems`, `unHealthyCodeSystemItems`, `degradedConceptMapItems`, `unHealthyConceptMapItems`, `degradedValueSetItems`, `unHealthyValueItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 6.2 Not Downloaded

**Check name:** `notDownloaded`  
**Config section:** `HealthChecks:TerminologyArtifacts:NotDownloaded`  
**What it monitors:** Artifacts that are not yet downloaded (`IsDownloaded = false`) and have no
error (`IsError = false`), grouped by resource type. Aged by `UpdatedDate`.

#### 6.2.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 6.2.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedCodeSystemItems` | Degraded CodeSystem count |
| `unHealthyCodeSystemItems` | Unhealthy CodeSystem count |
| `degradedConceptMapItems` | Degraded ConceptMap count |
| `unHealthyConceptMapItems` | Unhealthy ConceptMap count |
| `degradedValueSetItems` | Degraded ValueSet count |
| `unHealthyValueItems` | Unhealthy ValueSet count |
| `notDownloaded` | Total |

#### 6.2.3 Event Properties

`description`, `notDownloaded`, `degradedCodeSystemItems`, `unHealthyCodeSystemItems`, `degradedConceptMapItems`, `unHealthyConceptMapItems`, `degradedValueSetItems`, `unHealthyValueItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

### 6.3 Core Not Downloaded

**Check name:** `coreNotDownloaded`  
**Config section:** `HealthChecks:TerminologyArtifacts:CoreNotDownloaded`  
**What it monitors:** Artifacts marked as core (`IsCore = true`) that are not yet downloaded
(`IsDownloaded = false`) and have no error (`IsError = false`), grouped by resource type.
This check has **no time-based thresholds** — any missing core artifact is immediately Unhealthy.

#### 6.3.1 RAG Thresholds

| Status | Condition |
|---|---|
| 🟢 Healthy | All core artifacts are downloaded |
| 🔴 Unhealthy | Any core artifact is not downloaded (regardless of age) |

#### 6.3.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 2 = 🔴 Unhealthy (no Degraded state for this check) |
| `codeSystemItems` | Missing core CodeSystem count |
| `conceptMapItems` | Missing core ConceptMap count |
| `valueSetItems` | Missing core ValueSet count |
| `coreNotDownloaded` | Total missing core artifacts |

#### 6.3.3 Event Properties

`description`, `coreNotDownloaded`, `codeSystemItems`, `conceptMapItems`, `valueSetItems`, `checkedAt`, `message`, `status`

---

## 7. Terminology Polls Health Checks

**Entry key:** `terminologyPollsHealthChecks`  
**Config root:** `HealthChecks:TerminologyPolls`

---

### 7.1 Not Polling

**Check name:** `notPolling`  
**Config section:** `HealthChecks:TerminologyPolls:NotPolling`  
**What it monitors:** Terminology poll records where `LastPoll` is stale, grouped by individual
poll record. Returns Unhealthy immediately if the table is empty (polling has never run).

#### 7.1.1 RAG Thresholds

| Status | Condition | Default |
|---|---|---|
| 🟢 Healthy | All `LastPoll` timestamps are within `DegradedThreshold` | — |
| 🟡 Degraded | `LastPoll` is between `DegradedThreshold` and `UnHealthyThreshold` ago | 1440 min |
| 🔴 Unhealthy | `LastPoll` is older than `UnHealthyThreshold`, or table is empty | 2880 min |

#### 7.1.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |
| `degradedItems` | Degraded count |
| `unHealthyItems` | Unhealthy count |
| `notPolling` | Total stale poll records |

#### 7.1.3 Event Properties

`description`, `notPolling`, `degradedItems`, `unHealthyItems`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

## 8. PDS Audit Health Checks

**Entry key:** `pdsAuditHealthChecks`  
**Config root:** `HealthChecks:PdsAudit`

---

### 8.1 Pipeline Alive

**Check name:** `pipelineAlive`  
**Config section:** `HealthChecks:PdsAudit:PipelineAlive`  
**What it monitors:** Whether any `PdsAudit` records have been created recently (`CreatedDate`).
Returns Healthy if the table is empty.

#### 8.1.1 RAG Thresholds

| Status | Default |
|---|---|
| 🟢 Healthy | < 1440 min |
| 🟡 Degraded | 1440 min |
| 🔴 Unhealthy | 2880 min |

#### 8.1.2 Metrics Published

| Metric Key | Description |
|---|---|
| `StatusCode` | 0 = 🟢 Healthy, 1 = 🟡 Degraded, 2 = 🔴 Unhealthy |

#### 8.1.3 Event Properties

`description`, `degradedThresholdMinutes`, `unHealthyThresholdMinutes`, `checkedAt`, `message`, `status`

---

## 9. Configuration Reference

All thresholds can be overridden in `appsettings.json` (or environment-specific overrides). The full
set of configurable keys with their compiled-in defaults is listed below.

```json
{
  "HealthChecks": {
    "StartupDelaySeconds": 10,
    "PublishIntervalSeconds": 60,
    "IngestionTracking": {
      "PipelineAlive":               { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "Decryption":                  { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "Download":                    { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "Processing":                  { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "FailedToProcess":             { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "RetryCount": 3 },
      "RetryWarning":                { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "MaxRetryCount": 3 },
      "DeletedWithoutCompletion":    { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "FilesReceived":               { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "IncompleteBatches":           { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "StaleLastBatchCheck":         { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "LastSeenStaleness":           { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 }
    },
    "ResolvedAddress": {
      "PipelineAlive":               { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "Queued":                      { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "Processing":                  { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "AddressMatchingTime":         { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "MatchQuality":                { "DegradedThresholdPercentage": 0.9, "UnHealthyThresholdPercentage": 0.8 },
      "Export":                      { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "FailedToProcess":             { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "RetryCount": 3 },
      "FailedToExport":              { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "RetryCount": 3 },
      "RetryWarning":                { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "MaxRetryCount": 3 }
    },
    "OptOuts": {
      "PipelineAlive":               { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "ExpiredOptOuts":              { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "ExpiredAfterDays": 7, "LastSentExpiredAfterDays": 2 },
      "StuckOptOuts":                { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880, "ExpiredAfterDays": 7, "StuckAfterDays": 1 }
    },
    "TerminologyArtifacts": {
      "FailedToProcess":             { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "NotDownloaded":               { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 },
      "CoreNotDownloaded":           {}
    },
    "TerminologyPolls": {
      "NotPolling":                  { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 }
    },
    "PdsAudit": {
      "PipelineAlive":               { "DegradedThreshold": 1440, "UnHealthyThreshold": 2880 }
    }
  }
}
```

> **Note:** `HealthChecks:TerminologyArtifacts:CoreNotDownloaded` has no time-based thresholds.
> Any missing core artifact triggers Unhealthy immediately regardless of age.

---

## 10. Summary Table

| Domain | Check Name | Config Key | Degraded Default | Unhealthy Default |
|---|---|---|---|---|
| IngestionTracking | pipelineAlive | `IngestionTracking:PipelineAlive` | 1440 min | 2880 min |
| IngestionTracking | decryption | `IngestionTracking:Decryption` | 1440 min | 2880 min |
| IngestionTracking | downloadQueue | `IngestionTracking:Download` | 1440 min | 2880 min |
| IngestionTracking | processingQueue | `IngestionTracking:Processing` | 1440 min | 2880 min |
| IngestionTracking | failedToProcess | `IngestionTracking:FailedToProcess` | 1440 min | 2880 min |
| IngestionTracking | retryWarning | `IngestionTracking:RetryWarning` | 1440 min | 2880 min |
| IngestionTracking | deletedWithoutCompletion | `IngestionTracking:DeletedWithoutCompletion` | 1440 min | 2880 min |
| IngestionTracking | filesReceived | `IngestionTracking:FilesReceived` | 1440 min | 2880 min |
| IngestionTracking | incompleteBatchesQueue | `IngestionTracking:IncompleteBatches` | 1440 min | 2880 min |
| IngestionTracking | staleLastBatchCheck | `IngestionTracking:StaleLastBatchCheck` | 1440 min | 2880 min |
| IngestionTracking | lastSeenStaleness | `IngestionTracking:LastSeenStaleness` | 1440 min | 2880 min |
| ResolvedAddress | pipelineAlive | `ResolvedAddress:PipelineAlive` | 1440 min | 2880 min |
| ResolvedAddress | queuedForProcessing | `ResolvedAddress:Queued` | 1440 min | 2880 min |
| ResolvedAddress | processingQueue | `ResolvedAddress:Processing` | 1440 min | 2880 min |
| ResolvedAddress | addressMatchingTime | `ResolvedAddress:AddressMatchingTime` | 1440 min | 2880 min |
| ResolvedAddress | matchQuality | `ResolvedAddress:MatchQuality` | < 90% | < 80% |
| ResolvedAddress | queuedForExport | `ResolvedAddress:Export` | 1440 min | 2880 min |
| ResolvedAddress | failedToProcess | `ResolvedAddress:FailedToProcess` | 1440 min | 2880 min |
| ResolvedAddress | failedToExport | `ResolvedAddress:FailedToExport` | 1440 min | 2880 min |
| ResolvedAddress | retryWarning | `ResolvedAddress:RetryWarning` | 1440 min | 2880 min |
| OptOuts | pipelineAlive | `OptOuts:PipelineAlive` | 1440 min | 2880 min |
| OptOuts | expiredOptOuts | `OptOuts:ExpiredOptOuts` | 1440 min | 2880 min |
| OptOuts | stuckOptOuts | `OptOuts:StuckOptOuts` | 1440 min | 2880 min |
| TerminologyArtifacts | artifactErrors | `TerminologyArtifacts:FailedToProcess` | 1440 min | 2880 min |
| TerminologyArtifacts | notDownloaded | `TerminologyArtifacts:NotDownloaded` | 1440 min | 2880 min |
| TerminologyArtifacts | coreNotDownloaded | `TerminologyArtifacts:CoreNotDownloaded` | N/A | Any missing = Unhealthy |
| TerminologyPolls | notPolling | `TerminologyPolls:NotPolling` | 1440 min | 2880 min |
| PdsAudit | pipelineAlive | `PdsAudit:PipelineAlive` | 1440 min | 2880 min |

---

## 11. Example Health Output JSON

The JSON below represents a complete snapshot of all health check entries as they would appear in a
single `HealthReport`. Each entry shows the `status` (from the event property) and the `data`
dictionary whose numeric values are emitted as Application Insights metrics and whose string/date
values are emitted as event properties.

> **Key:** All timestamps are ISO-8601. Numeric metric values appear directly under `data`; string
> values are promoted to event telemetry properties by the publisher.

```json
{
  "status": "Degraded",
  "entries": {

    "ingestionTrackingHealthChecks": {
      "status": "Degraded",
      "description": "One or more ingestion tracking checks are degraded or unhealthy.",
      "data": {
        "pipelineAlive": {
          "description": "Pipeline Alive",
          "status": "Healthy",
          "message": "Pipeline is alive. A record was created within the last 1440 minutes.",
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "decryption": {
          "description": "Decryption Queue",
          "status": "Degraded",
          "message": "3 file(s) have not been decrypted and are between 1440 and 2880 minutes old.",
          "degradedItems": 3,
          "unHealthyItems": 0,
          "unDecryptedItems": 3,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "downloadQueue": {
          "description": "Download Queue",
          "status": "Healthy",
          "message": "No files are stuck in the download queue.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "notDownloadedItems": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "processingQueue": {
          "description": "Processing Queue",
          "status": "Unhealthy",
          "message": "1 file(s) are stuck in processing and are older than 2880 minutes.",
          "degradedItems": 0,
          "unHealthyItems": 1,
          "stuckInProcessing": 1,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "failedToProcess": {
          "description": "Failed To Process",
          "status": "Healthy",
          "message": "No files have permanently failed processing.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "failedToProcess": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "retryWarning": {
          "description": "Retry Warning",
          "status": "Degraded",
          "message": "2 file(s) are retrying and between 1440 and 2880 minutes old.",
          "degradedItems": 2,
          "unHealthyItems": 0,
          "retryWarningItems": 2,
          "maxRetryCount": 3,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "deletedWithoutCompletion": {
          "description": "Deleted Without Completion",
          "status": "Healthy",
          "message": "No files have been deleted before decryption.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "deletedWithoutCompletionItems": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "filesReceived": {
          "description": "Files Received",
          "status": "Healthy",
          "message": "All suppliers have received files within expected thresholds.",
          "SupplierA.filesReceived": 12,
          "SupplierB.filesReceived": 7,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "incompleteBatchesQueue": {
          "description": "Incomplete Batches",
          "status": "Healthy",
          "message": "No incomplete batches detected.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "incompleteBatches": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "staleLastBatchCheck": {
          "description": "Stale Last Batch Check",
          "status": "Healthy",
          "message": "All batch complete checks are current.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "staleLastBatchCheckItems": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "lastSeenStaleness": {
          "description": "Last Seen Staleness",
          "status": "Healthy",
          "message": "No in-progress files have a stale LastSeen timestamp.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "staleLastSeenItems": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        }
      }
    },

    "resolvedAddressHealthChecks": {
      "status": "Healthy",
      "description": "All resolved address checks are healthy.",
      "data": {
        "pipelineAlive": {
          "description": "Pipeline Alive",
          "status": "Healthy",
          "message": "Pipeline is alive. A record was created within the last 1440 minutes.",
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "queuedForProcessing": {
          "description": "Queued For Processing",
          "status": "Healthy",
          "message": "No records are stuck in the processing queue.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "queuedItems": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "processingQueue": {
          "description": "Processing Queue",
          "status": "Healthy",
          "message": "No records are stuck in processing.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "stuckInProcessing": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "addressMatchingTime": {
          "description": "Address Matching Time",
          "status": "Healthy",
          "message": "No address matches are taking longer than expected.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "unacceptableProcessingTime": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "matchQuality": {
          "description": "Match Quality",
          "status": "Healthy",
          "message": "Match rate of 0.95 is above the degraded threshold of 90%.",
          "averageMatchRate": 0.95,
          "isDegraded": "False",
          "isUnhealthy": "False",
          "degradedThresholdPercentage": 0.9,
          "unHealthyThresholdPercentage": 0.8,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "queuedForExport": {
          "description": "Queued For Export",
          "status": "Healthy",
          "message": "No records are stuck in the export queue.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "queuedItems": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "failedToProcess": {
          "description": "Failed To Process",
          "status": "Healthy",
          "message": "No records have permanently failed processing.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "failedToProcess": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "failedToExport": {
          "description": "Failed To Export",
          "status": "Healthy",
          "message": "No records have permanently failed to export.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "failedToExport": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "retryWarning": {
          "description": "Retry Warning",
          "status": "Healthy",
          "message": "No records are at risk of permanent failure.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "retryWarningItems": 0,
          "maxRetryCount": 3,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        }
      }
    },

    "optOutsHealthChecks": {
      "status": "Healthy",
      "description": "All opt-out checks are healthy.",
      "data": {
        "pipelineAlive": {
          "description": "Pipeline Alive",
          "status": "Healthy",
          "message": "Pipeline is alive. A record was created within the last 1440 minutes.",
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "expiredOptOuts": {
          "description": "Expired Opt-Outs",
          "status": "Healthy",
          "message": "No opt-out records are expired and overdue for re-send.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "expiredAndOutdated": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "stuckOptOuts": {
          "description": "Stuck Opt-Outs",
          "status": "Healthy",
          "message": "No active opt-out records are stuck pending MESH dispatch.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "stuckOptOuts": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        }
      }
    },

    "terminologyArtifactsHealthChecks": {
      "status": "Unhealthy",
      "description": "One or more terminology artifact checks are unhealthy.",
      "data": {
        "artifactErrors": {
          "description": "Artifact Errors",
          "status": "Healthy",
          "message": "No artifact errors detected.",
          "degradedCodeSystemItems": 0,
          "unHealthyCodeSystemItems": 0,
          "degradedConceptMapItems": 0,
          "unHealthyConceptMapItems": 0,
          "degradedValueSetItems": 0,
          "unHealthyValueItems": 0,
          "failedToProcess": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "notDownloaded": {
          "description": "Not Downloaded",
          "status": "Degraded",
          "message": "2 artifact(s) are pending download and between 1440 and 2880 minutes old.",
          "degradedCodeSystemItems": 1,
          "unHealthyCodeSystemItems": 0,
          "degradedConceptMapItems": 1,
          "unHealthyConceptMapItems": 0,
          "degradedValueSetItems": 0,
          "unHealthyValueItems": 0,
          "notDownloaded": 2,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        },
        "coreNotDownloaded": {
          "description": "Core Not Downloaded",
          "status": "Unhealthy",
          "message": "1 core artifact(s) are missing and must be downloaded immediately.",
          "codeSystemItems": 0,
          "conceptMapItems": 0,
          "valueSetItems": 1,
          "coreNotDownloaded": 1,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        }
      }
    },

    "terminologyPollsHealthChecks": {
      "status": "Healthy",
      "description": "All terminology poll checks are healthy.",
      "data": {
        "notPolling": {
          "description": "Not Polling",
          "status": "Healthy",
          "message": "All poll records have been updated within the last 1440 minutes.",
          "degradedItems": 0,
          "unHealthyItems": 0,
          "notPolling": 0,
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        }
      }
    },

    "pdsAuditHealthChecks": {
      "status": "Healthy",
      "description": "All PDS audit checks are healthy.",
      "data": {
        "pipelineAlive": {
          "description": "Pipeline Alive",
          "status": "Healthy",
          "message": "Pipeline is alive. A record was created within the last 1440 minutes.",
          "degradedThresholdMinutes": 1440,
          "unHealthyThresholdMinutes": 2880,
          "checkedAt": "2025-07-14T08:00:00.0000000Z"
        }
      }
    }

  }
}
```

### 11.1 Corresponding Application Insights Metrics

For the snapshot above the following metric values would be emitted:

| Metric Name | Value | Meaning |
|---|---|---|
| `ingestionTrackingHealthChecks.StatusCode` | 1 | 🟡 Degraded |
| `ingestionTrackingHealthChecks.decryption.degradedItems` | 3 | 3 files degraded |
| `ingestionTrackingHealthChecks.decryption.unDecryptedItems` | 3 | 3 total un-decrypted |
| `ingestionTrackingHealthChecks.processingQueue.unHealthyItems` | 1 | 1 file unhealthy |
| `ingestionTrackingHealthChecks.processingQueue.stuckInProcessing` | 1 | 1 stuck |
| `ingestionTrackingHealthChecks.retryWarning.degradedItems` | 2 | 2 files at risk |
| `ingestionTrackingHealthChecks.filesReceived.SupplierA.filesReceived` | 12 | 12 files from Supplier A |
| `ingestionTrackingHealthChecks.filesReceived.SupplierB.filesReceived` | 7 | 7 files from Supplier B |
| `resolvedAddressHealthChecks.StatusCode` | 0 | 🟢 Healthy |
| `resolvedAddressHealthChecks.matchQuality.averageMatchRate` | 0.95 | 95% match rate |
| `optOutsHealthChecks.StatusCode` | 0 | 🟢 Healthy |
| `terminologyArtifactsHealthChecks.StatusCode` | 2 | 🔴 Unhealthy |
| `terminologyArtifactsHealthChecks.notDownloaded.degradedCodeSystemItems` | 1 | 1 degraded CodeSystem |
| `terminologyArtifactsHealthChecks.notDownloaded.degradedConceptMapItems` | 1 | 1 degraded ConceptMap |
| `terminologyArtifactsHealthChecks.notDownloaded.notDownloaded` | 2 | 2 total not downloaded |
| `terminologyArtifactsHealthChecks.coreNotDownloaded.valueSetItems` | 1 | 1 missing core ValueSet |
| `terminologyArtifactsHealthChecks.coreNotDownloaded.coreNotDownloaded` | 1 | 1 total missing core |
| `terminologyPollsHealthChecks.StatusCode` | 0 | 🟢 Healthy |
| `pdsAuditHealthChecks.StatusCode` | 0 | 🟢 Healthy |
