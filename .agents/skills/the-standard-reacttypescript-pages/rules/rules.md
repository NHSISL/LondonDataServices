# The Standard React + TypeScript + Vite — Pages — Rules

## Route Responsibility

**tsr-pages-001** [ERROR] A page must represent a route or screen. It is the entry point for a navigable URL.

## View Service Access

**tsr-pages-002** [ERROR] A page may call a view service only through a dedicated page hook (`use{Domain}Page.ts`). The page must not instantiate or call view services directly in the component body.

## No Broker Calls

**tsr-pages-003** [ERROR] A page must not call brokers or make HTTP requests directly.

## No Business Rules

**tsr-pages-004** [ERROR] A page must not contain business rules. Conditional logic with domain meaning belongs in foundation or view services.

## State Coordination

**tsr-pages-005** [ERROR] A page must handle loading, error, and empty states explicitly. It must not silently render nothing when data is absent.

## Component Composition

**tsr-pages-006** [ERROR] A page must compose named components for its content. A page that contains large inline JSX blocks must extract them into components.

## View Service Delegation

**tsr-pages-007** [ERROR] A page must delegate all complex data preparation to a view service. It must not perform data fetching, transformation, or aggregation in the component body.
