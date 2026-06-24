---
name: the-standard-team-core
version: 0.1.0
standard-team-version: v0.1.0
applies-to: ["*.md", "project docs"]
depends-on: []
---

# The Standard Team — Core

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Any project or team documentation defining the purpose, goals, and scenarios of a software product.
0.1/ Who: Any engineer, lead, or stakeholder writing or reviewing a purpose document or scenario definition.
0.2/ What: Governs how teams define their business nature, goals, future considerations, and BDD-style scenarios that communicate product intent to all stakeholders.
0.3/ Applies to: `*.md`, project documentation, purpose documents, scenario definitions.
0.4/ Version: The Standard Team v0.1.0
0.5/ Depends on: none

---

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

### 1.0/ Dos

| Rule ID | Requirement |
|---|---|
| [tst-core-001](rules/rules.md#tst-core-001) | Every engineering team must define and document an Overall Purpose covering Business Nature, Business Goals, and Future Considerations. |
| [tst-core-002](rules/rules.md#tst-core-002) | Business Nature must state what the business is in plain language. |
| [tst-core-003](rules/rules.md#tst-core-003) | Business Goals must state what the business aims to deliver. |
| [tst-core-004](rules/rules.md#tst-core-004) | Future Considerations must state anticipated growth or evolution of the business. |
| [tst-core-005](rules/rules.md#tst-core-005) | Scenarios must be written in BDD format: GIVEN / WHEN / THEN. |
| [tst-core-006](rules/rules.md#tst-core-006) | Scenarios must be non-technical and written from the end-user's perspective. |
| [tst-core-007](rules/rules.md#tst-core-007) | Scenarios must express outcomes that could be advertised to non-technical stakeholders. |
| [tst-core-008](rules/rules.md#tst-core-008) | Scenarios may chain — a follow-on scenario may build on an established prior scenario. |

### 1.1/ Don'ts

| Rule ID | Prohibition |
|---|---|
| [tst-core-009](validations/anti-patterns.md#technical-scenarios) | Scenarios must not contain API endpoints, UI specifics, or implementation details unless the business itself is technical. |
| [tst-core-010](validations/anti-patterns.md#missing-purpose-sections) | A purpose document must not omit any of the three required sections: Business Nature, Business Goals, Future Considerations. |
| [tst-core-011](validations/anti-patterns.md#missing-actors) | A scenario must not omit the actor (GIVEN), the interaction (WHEN), or the outcome (THEN). |

### 1.2/ Ask

- Ask when it is unclear which role is the actor in a scenario.
- Ask when a future consideration is actually a current business goal.
- Ask when a scenario contains technical terms that may not be understood by non-technical stakeholders.

### 1.3/ Defaults

- When a purpose document section is missing: prompt to add it before proceeding.
- When a scenario outcome is vague: ask for a measurable or advertisable outcome.
- When the actor is ambiguous: default to "a user of {system name}".

### 1.4/ Examples

| | File |
|---|---|
| ✅ | [examples/good/example_good_purpose_document.md](examples/good/example_good_purpose_document.md) |
| ✅ | [examples/good/example_good_scenario.md](examples/good/example_good_scenario.md) |
| ❌ | [examples/bad/example_bad_technical_scenario.md](examples/bad/example_bad_technical_scenario.md) |
| ❌ | [examples/bad/example_bad_incomplete_purpose.md](examples/bad/example_bad_incomplete_purpose.md) |

---

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Markdown purpose documents and BDD scenario definitions.
2.1/ Outcome: Every team has a documented purpose with all three required sections, and every product feature has at least one non-technical scenario expressed in GIVEN / WHEN / THEN format.
2.2/ Tone: Direct. Cite rule IDs. Non-technical language throughout scenarios.
