# Example Bad Review

The following shows NON-COMPLIANT review comments that violate the review rules.

---

**PR:** Add StudentListComponent and wire up to StudentViewService

---

**❌ Bad Comment 1 — Opinion without rule (tsr-review-001, tsr-review-003)**
```
I'd rewrite this as a named function — arrow functions in components feel harder to read.
```
No rule ID cited. Personal preference. Should be omitted.

---

**❌ Bad Comment 2 — Architectural violation logged as INFO (tsr-review-002)**
```
INFO — It might be nicer to move the axios call into a broker at some point.
```
A component calling axios directly is an ERROR, not an INFO. Merge must be blocked.

---

**❌ Bad Comment 3 — No layer identification (tsr-review-004)**
```
ERROR tsr-async-002 / await not handled / fix the async / add try-catch
```
No layer identified. No clear reason. No actionable suggested fix. Author cannot act on this.

---

**❌ Bad Comment 4 — Oversized suggested fix (tsr-review-005)**
```
WARNING tsr-styles-001 / CSS module class name is wrong /
You should rename this file and restructure the entire component to match the module pattern.
Rewrite:
[... 40 lines of replacement code ...]
```
The Standard requires the smallest fix that satisfies the rule — not a full rewrite.

---

**❌ Bad Comment 5 — Approved generated code without review (tsr-review-008)**
```
LGTM — Copilot generated this, should be fine.
```
Generated code must be reviewed against all Standard rules before approval.
