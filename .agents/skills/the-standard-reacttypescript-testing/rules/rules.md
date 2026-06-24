# The Standard React + TypeScript + Vite — Testing — Rules

## Broker Testing

**tsr-testing-001** [ERROR] Brokers must be tested through replaceable interface contracts or integration tests that verify the external communication boundary.

## Foundation Service Testing

**tsr-testing-002** [ERROR] Foundation services must have unit tests covering: (1) happy path logic, (2) validation failures, and (3) exception localization when a broker throws.

## View Service Testing

**tsr-testing-003** [ERROR] View services must have unit tests covering aggregation, mapping, and view model shaping — verifying that the correct display-ready fields are produced from given foundation model inputs.

## Component Testing

**tsr-testing-004** [ERROR] Components must be tested through visible behavior using React Testing Library. Tests must query by accessible role, label, or text — not by internal implementation details.

## Page Testing

**tsr-testing-005** [ERROR] Pages must be tested for all four route-level states: loading state, error state, empty state, and successful render state.

## No React Internals

**tsr-testing-006** [ERROR] Tests must not target React internals: state variable names, hook call counts, internal refs, or component instance properties.

## No Large Snapshots

**tsr-testing-007** [WARN] Large rendered tree snapshots must not be the primary proof of correctness. Snapshots may supplement behavior tests but must not replace them.

## Mocks at Boundaries

**tsr-testing-008** [ERROR] Mocks must only be placed at architectural boundaries: broker interface mocks for service tests, service interface mocks for component and page tests. Do not mock internal implementation details.
