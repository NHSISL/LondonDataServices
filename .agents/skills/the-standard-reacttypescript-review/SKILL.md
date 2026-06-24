---
name: the-standard-reacttypescript-review
version: 0.1.0
standard-version: v0.1.0
applies-to: ["pull-requests", "*.tsx", "*.ts"]
depends-on:
  - the-standard-reacttypescript-files
  - the-standard-reacttypescript-typescript
  - the-standard-reacttypescript-models
  - the-standard-reacttypescript-brokers
  - the-standard-reacttypescript-services
  - the-standard-reacttypescript-view-services
  - the-standard-reacttypescript-components
  - the-standard-reacttypescript-pages
  - the-standard-reacttypescript-hooks
  - the-standard-reacttypescript-state
  - the-standard-reacttypescript-routing
  - the-standard-reacttypescript-styling
  - the-standard-reacttypescript-errors
  - the-standard-reacttypescript-async
  - the-standard-reacttypescript-vite
  - the-standard-reacttypescript-testing
  - the-standard-reacttypescript-accessibility
  - the-standard-reacttypescript-performance
---

# The Standard React + TypeScript + Vite — Review

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Pull request reviews for any Standard React + TypeScript + Vite project.
0.1/ Who: Any engineer or AI agent performing code review.
0.2/ What: Governs how review feedback is structured, what architectural issues must be blocked, how AI-generated code is evaluated, and how personal preferences are excluded from reviews.
0.3/ Applies to: Pull request review comments on `*.tsx` and `*.ts` files.
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: All 18 prior React + TypeScript + Vite skills.

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Review Mechanics:
  1. Every comment must cite a rule ID — see rules/rules.md#tsr-review-001
  2. Architectural concerns must be separated from style concerns — see rules/rules.md#tsr-review-002
  3. Personal preference comments must be omitted — see rules/rules.md#tsr-review-003
  4. Each comment must identify the affected layer — see rules/rules.md#tsr-review-004
  5. Suggest the smallest fix that satisfies the rule — see rules/rules.md#tsr-review-005
  6. Block comments that bypass Standard layers — see rules/rules.md#tsr-review-006
  7. Flag React Doctor warnings — see rules/rules.md#tsr-review-007
  8. Reject generated code that violates Standard rules — see rules/rules.md#tsr-review-008

1.1/ Review Output Format (section 24 of the standard):
  `{SEVERITY} {RULE-ID} / {Problem description} / {Why it violates the rule} / {Suggested fix}`
  Example: `ERROR tsr-broker-001 / StudentBroker calls axios directly in a view service / Brokers must be the only layer calling HTTP clients / Move the axios call into a dedicated StudentBroker`

1.2/ Severity Levels:
  - `ERROR` — Blocks merge. Architectural violation or broken Standard rule.
  - `WARNING` — Should be addressed. Style or quality concern that weakens the Standard.
  - `INFO` — Informational. Educational note; does not block merge.

1.3/ Don'ts:
  - Never comment "I prefer X" or "this would be cleaner as Y" without citing a rule.
  - Never approve a PR that bypasses a Standard layer.
  - Never accept AI-generated code at face value — every line must satisfy Standard rules.

1.4/ Examples:
  - ✅ see examples/good/example_good_review.md
  - ❌ see examples/bad/example_bad_review.md

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Review comments in the format `{SEVERITY} {RULE-ID} / Problem / Why / Fix`.
2.1/ Outcome: Every review comment is rule-backed, architectural violations are blocked, and generated code is evaluated against all Standard rules.
2.2/ Tone: Direct. Cite rule IDs in every comment. No personal preference. No prose justification unless asked.
