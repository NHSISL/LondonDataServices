# Example Good Review

The following shows Standard-compliant review comments on a pull request.

---

**PR:** Add StudentListComponent and wire up to StudentViewService

---

**Comment 1 — PASS (no comment needed)**
`StudentViewService.retrieveAllStudentsAsync` delegates correctly to `StudentFoundationService`. No violation.

---

**Comment 2 — ERROR (layer bypass)**
```
ERROR tsr-broker-001 / [Component layer] StudentListComponent calls StudentBroker.getStudents directly /
Components must only interact with view services — brokers are reserved for foundation services /
Remove the broker import from StudentListComponent and inject StudentViewService instead
```

---

**Comment 3 — WARNING (missing aria label)**
```
WARNING tsr-accessibility-002 / [Component layer] The search input has no label /
All form inputs must have an accessible label via label[htmlFor] or aria-label /
Add: <label htmlFor="search">Search</label> and id="search" on the input
```

---

**Comment 4 — INFO (educational note)**
```
INFO tsr-typescript-003 / [Component layer] The StudentListComponent props type is defined inline as an object literal /
The Standard prefers a named Props type alias for clarity and reusability /
Consider: type StudentListComponentProps = { students: StudentModel[] }
```

---

**Summary:** 1 blocking ERROR (layer bypass), 1 WARNING (accessibility), 1 INFO (naming). Merge blocked until ERROR is resolved.
