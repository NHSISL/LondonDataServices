# The Standard React + TypeScript + Vite — Error Handling — Rules

## Broker Layer

**tsr-errors-001** [ERROR] Brokers must expose external failures without converting them into UI messages. Throw the raw or minimally wrapped error so the service layer can localize it meaningfully.

## Foundation Service Layer

**tsr-errors-002** [ERROR] Foundation services must catch broker exceptions and re-throw as domain-specific exceptions with meaningful context. Do not let raw HTTP or network errors propagate to the view layer.

## View Service Layer

**tsr-errors-003** [WARN] View services may convert service errors into view-friendly error models when the page or component needs to render structured error information.

## Page Layer

**tsr-errors-004** [ERROR] Pages own the decision of how to display errors. A page must render an error state explicitly — not silently render nothing.

## Component Layer

**tsr-errors-005** [ERROR] Components must not inspect raw infrastructure errors. A component must not check `error.status`, `error.code`, or HTTP status codes. It must receive a structured error model or display field.

## No Silent Swallowing

**tsr-errors-006** [ERROR] Errors must never be silently swallowed. An empty `catch {}` or `catch { return null; }` without logging or re-throwing is a violation.

## Logging

**tsr-errors-007** [ERROR] Unexpected errors that are not re-thrown must be logged through an approved logging broker or telemetry boundary before being suppressed.
