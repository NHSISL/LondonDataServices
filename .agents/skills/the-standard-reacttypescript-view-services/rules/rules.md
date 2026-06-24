# The Standard React + TypeScript + Vite — View Services — Rules

## Coordinate Foundation Services

**tsr-viewservices-001** [ERROR] A view service must coordinate one or more foundation services to produce a view model. It must not contain raw infrastructure calls or duplicate business rules from foundation services.

## No JSX

**tsr-viewservices-002** [ERROR] A view service must not return JSX or contain JSX expressions.

## No React

**tsr-viewservices-003** [ERROR] A view service must not import React, `useState`, `useEffect`, or any React hook.

## No Direct Broker Calls

**tsr-viewservices-004** [ERROR] A view service must not call brokers directly unless it is acting as an explicit service boundary for a dependency that has no foundation service. Default flow: View Service → Foundation Service → Broker.

## Data Transformation

**tsr-viewservices-005** [WARN] A view service may aggregate, sort, filter, and map foundation model data into view models. This is its primary responsibility.

## No Rendering Concerns

**tsr-viewservices-006** [ERROR] A view service must not contain CSS class names, Bootstrap class strings, component names, or any other rendering detail. Display-field values (e.g., `"Adult"`) are permitted; class names (e.g., `"text-success"`) are not.
