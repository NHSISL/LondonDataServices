---
skill: the-standard-team-branching
type: anti-patterns
source-section: "4.1.1 Forking and Branching Strategies"
---

# The Standard Team — Branching — Anti-Patterns

---

## Direct Push to Official Repository

**Violates:** tst-branching-007

**What happens:** A contributor clones the official repository directly and pushes a branch to it instead of working through a fork.

**Why it's wrong:** Direct push bypasses the pull request review process. The project maintainer loses control over what enters the official codebase. It can overwrite or conflict with other contributors' work.

**Fix:** Fork the official repository, clone the fork, create branches on the fork, and open a PR from the fork to the official repo.

```
# ❌ Direct clone and push
git clone https://github.com/official/repo.git
git checkout -b my-feature
git push origin my-feature          # pushes to official repo

# ✅ Forking workflow
# 1. Fork on GitHub UI
git clone https://github.com/myusername/repo.git   # clone own fork
git checkout -b users/myusername/FOUNDATIONS-Student-Add
git push origin users/myusername/FOUNDATIONS-Student-Add   # pushes to fork
# 2. Open PR from fork to official repo on GitHub
```

---

## Invalid Branch Name

**Violates:** tst-branching-008

**What happens:** A branch is created with a name that does not follow the `users/[username]/[category]-[entity]-[action]` pattern.

**Why it's wrong:** Non-standard names make it impossible for reviewers to understand scope, category, and ownership at a glance. They break tooling that relies on the naming pattern.

**Fix:** Rename the branch to follow the approved pattern before opening a PR.

```
# ❌ Invalid branch names
feature/add-student
fix-bug-123
john/student
FOUNDATIONS-Student

# ✅ Valid branch names
users/jsmith/FOUNDATIONS-Student-Add
users/jsmith/MINOR-FIX-StudentService-Validation
users/jsmith/DATA-Student-AddMigration
```

---

## Unapproved Category

**Violates:** tst-branching-009

**What happens:** A branch uses a category that is not in the approved category list.

**Why it's wrong:** Unapproved categories make it impossible to correctly score, report, or automate work classification. The approved list exists to cover all recognised types of engineering work.

**Fix:** Replace the unapproved category with the closest approved one from the category list in contracts/contracts.json.

```
# ❌ Unapproved categories
users/jsmith/FEATURE-Student-Add       # FEATURE is not approved
users/jsmith/BUGFIX-Student-Update     # BUGFIX is not approved — use MINOR FIX / MEDIUM FIX / MAJOR FIX

# ✅ Correct approved categories
users/jsmith/FOUNDATIONS-Student-Add
users/jsmith/MINOR-FIX-Student-Validation
```
