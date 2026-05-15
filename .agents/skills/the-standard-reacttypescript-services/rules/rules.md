# The Standard React + TypeScript + Vite — Foundation Services — Rules

## Dependencies

**tsr-foundation-001** [ERROR] A foundation service must depend only on brokers, models, and other approved same-layer dependencies. It must not import view services, pages, components, or hooks.

## No React

**tsr-foundation-002** [ERROR] A foundation service must not import React, `useState`, `useEffect`, `useRef`, or any other React hook or API.

## No JSX

**tsr-foundation-003** [ERROR] A foundation service must not return JSX or contain JSX expressions.

## Validate Before Broker

**tsr-foundation-004** [ERROR] A foundation service must validate all inputs before calling a broker. Validation must throw a domain-specific error when inputs are invalid.

## Exception Localization

**tsr-foundation-005** [ERROR] A foundation service must catch external exceptions thrown by brokers and re-throw them as domain-specific exceptions with meaningful context when appropriate.

## No Navigation

**tsr-foundation-006** [ERROR] A foundation service must not perform page navigation. Navigation is an exposure-layer concern.

## No State Ownership

**tsr-foundation-007** [ERROR] A foundation service must not own React state (`useState`, `useReducer`, context mutations). State is managed by hooks and pages.
