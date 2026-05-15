# The Standard React + TypeScript + Vite — Components — Rules

## Rendering Responsibility

**tsr-components-001** [ERROR] A component must render UI. Its primary output is JSX.

**tsr-components-006** [ERROR] A component must be small enough to understand at a glance. A component that simultaneously handles data loading, transformation, event orchestration, and rendering must be split.

## Props-First Data Flow

**tsr-components-002** [ERROR] A component must receive data through props unless it is an approved page-level container or application shell.

**tsr-components-007** [ERROR] Non-trivial components must define a typed prop model (`type {Domain}Props = { ... }`).

**tsr-components-008** [ERROR] A component must not duplicate state that already exists in a parent component, page hook, or view model.

## No Infrastructure Calls

**tsr-components-003** [ERROR] A component must not call brokers or make HTTP requests directly.

**tsr-components-004** [ERROR] A component must not call foundation services directly. Data must flow through view services and page hooks.

## No Business Rules in JSX

**tsr-components-005** [ERROR] A component must not embed business rule conditionals in JSX. Business-meaning values must come from view model fields.

## Accessibility

**tsr-components-009** [ERROR] Interactive elements must use semantic HTML and be keyboard accessible. Do not use `<div>` or `<span>` with `onClick` as substitutes for `<button>` or `<a>`.
