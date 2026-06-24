---
name: the-standard-reacttypescript-pages
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/pages/**/*"]
depends-on: ["the-standard-reacttypescript-view-services", "the-standard-reacttypescript-components"]
---

# The Standard React + TypeScript + Vite — Pages

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/pages/**/*` — route-level page components and their page hooks.
0.1/ Who: Any engineer creating or reviewing pages in a Standard frontend project.
0.2/ What: Governs what pages are allowed to do — coordinate loading/error/empty states, compose components, delegate to view services — and what they must not do — call brokers, own business rules, or perform direct data fetching.
0.3/ Applies to: `src/pages/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-view-services, the-standard-reacttypescript-components

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. A page must represent a route or screen — see rules/rules.md#tsr-pages-001
  2. A page may call a view service through its page hook — see rules/rules.md#tsr-pages-002
  3. A page must coordinate loading, error, and empty states — see rules/rules.md#tsr-pages-005
  4. A page must compose components to render its content — see rules/rules.md#tsr-pages-006
  5. A page must delegate complex data preparation to a view service — see rules/rules.md#tsr-pages-007

1.1/ Don'ts:
  1. Never call brokers directly from a page — see rules/rules.md#tsr-pages-003 and validations/anti-patterns.md#broker-in-page
  2. Never put business rules inside a page — see rules/rules.md#tsr-pages-004

1.2/ Ask:
  - Ask when it is unclear whether logic belongs in the page, the page hook, or the view service.

1.3/ Defaults:
  - Each page has a corresponding `use{Domain}Page.ts` hook that handles state and view service calls.
  - The page body is responsible only for rendering based on state returned by the hook.

1.4/ Examples:
  - ✅ see examples/good/example_good_page.tsx
  - ❌ see examples/bad/example_bad_page.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: React TSX page component that delegates data work to its hook and renders composed components.
2.1/ Outcome: Pages are thin route-level containers — loading/error/empty/success states rendered from hook output.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-pages-003). No prose justification unless asked.
