# The Standard Aggregation Services — Anti-Patterns

## Aggregation with Logic

**Violates:** ts-aggregations-004
**What happens:** `StudentEnrollmentAggregationService.EnrollStudentAsync` checks whether the student is already enrolled before calling the orchestration services.
**Why it's wrong:** Business-rule enforcement belongs at the processing or orchestration layer. Aggregation services must only route and fan out.
**Fix:** Move the existence check into `StudentEnrollmentOrchestrationService.EnsureEnrollmentAsync`.

## Aggregation Foundation Bypass

**Violates:** ts-aggregations-003
**What happens:** The aggregation service injects `IStudentService` (a foundation service) to pre-load a student before calling orchestrations.
**Why it's wrong:** Aggregation services must depend on orchestration services, not foundation services. Mixing layers breaks the dependency hierarchy.
**Fix:** Move the pre-load into the orchestration service; pass the student into the aggregation entry point.

## Aggregation Mapping

**Violates:** ts-aggregations-004
**What happens:** The aggregation service maps the results of two orchestration services into a combined `EnrollmentSummary` DTO.
**Why it's wrong:** Data transformation is not an aggregation responsibility; it belongs in the exposer layer or a dedicated mapping service.
**Fix:** Return the individual orchestration results; let the controller or view model compose the summary.
