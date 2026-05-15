---
applyTo: "**/*.ts,**/*.tsx"
---

# The Standard React + TypeScript + Vite — Review

## Applies To
Pull request reviews on `*.tsx` and `*.ts` files in any Standard React + TypeScript + Vite project.

## Review Comment Format
Every comment must follow: `{SEVERITY} {RULE-ID} / {Problem} / {Why it violates the rule} / {Suggested fix}`

Example: `ERROR tsr-brokers-001 / StudentBroker calls axios directly in a view service / Brokers must be the only layer calling HTTP clients / Move the axios call into a dedicated StudentBroker`

## Severity Levels
- `ERROR` — Blocks merge. Architectural violation or broken Standard rule.
- `WARNING` — Should be addressed. Style or quality concern.
- `INFO` — Educational note; does not block merge.

## Rules — Do
- Every comment must cite a rule ID (tsr-review-001)
- Architectural concerns must be separated from style concerns (tsr-review-002)
- Each comment must identify the affected layer (tsr-review-004)
- Suggest the smallest fix that satisfies the rule (tsr-review-005)
- Block comments that bypass Standard layers (tsr-review-006)
- Flag React Doctor warnings (tsr-review-007)
- Reject generated code that violates Standard rules (tsr-review-008)

## Rules — Do Not
- Never comment "I prefer X" or "this would be cleaner as Y" without citing a rule (tsr-review-003)
- Never approve a PR that bypasses a Standard layer (tsr-review-006)
- Never accept AI-generated code at face value — every line must satisfy Standard rules (tsr-review-008)

## Defaults
- Personal preference comments are forbidden unless backed by a rule ID.
- Every architectural violation is `ERROR` and blocks merge.
