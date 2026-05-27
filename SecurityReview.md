# Security Review — Pre-Public Repository Checklist

> Generated as part of the security review prior to making this repository public.  
> **Last reviewed:** 2025  
> **Reviewer:** GitHub Copilot assisted review

---

## 🔴 CRITICAL — Secrets Already Committed to Git

### 1. `cypress.config.ts` — MULTIPLE LIVE SECRETS ⚠️ IMMEDIATE ACTION REQUIRED

**File:** `LHDS.AdminPortal.Web\ClientApp\cypress.config.ts`

The following **live credentials are committed in plain text**:

| Secret | Key |
|---|---|
| `AZURE_TENANT_ID` | `8076439c-5b1f-4e15-91ea-...` |
| `AZURE_CLIENT_ID` | `03f4538b-cb92-47d7-897e-...` |
| `AZURE_CLIENT_SECRET` | `JKS8Q~Sq~7cydSHkt...` (**active client secret**) |
| `USERNAME` | `cypressTestGPPremises@londonhds.nhs.uk` |
| `PASSWORD` | `98mqcRLIRMEQA9%C55Nr` (**live password**) |
| `FAKE_REFRESH_TOKEN` | Full JWT token committed |
| `FAKE_JWT` | Full signed JWT token committed |
| `SCOPE` | App registration URI |

> 🚨 These are NHS credentials. Even if "test" accounts, they must be treated as **compromised immediately**.

#### Immediate Actions Required

- [ ] **Rotate** `AZURE_CLIENT_SECRET` in Azure Entra ID (App Registration `03f4538b-cb92-47d7-897e-ac18142988e1`)
- [ ] **Change the password** for `cypressTestGPPremises@londonhds.nhs.uk`
- [ ] **Revoke all active sessions** for that test user in Azure Entra ID
- [ ] **Purge secrets from git history** — deleting the file is NOT enough, the secrets exist in all prior commits (see History Purge section below)
- [ ] **Remove all secrets from `cypress.config.ts`** and move them to `cypress.env.json` (gitignored)

---

## 🟠 HIGH — `.gitignore` Gaps

The `.gitignore` covers many environment files but has gaps for the following:

### Missing `.gitignore` Entries

- [ ] `cypress.env.json` and `**/cypress.env.json`
- [ ] `.env`, `.env.local`, `.env.development`, `.env.production`, `.env.staging`
- [ ] `**/.env` and `**/.env.local`
- [ ] `appsettings.Production.json` and `*/appsettings.Production.json`
- [ ] `appsettings.Staging.json` and `*/appsettings.Staging.json`

### Add the Following to `.gitignore`

```gitignore
# Cypress secrets - NEVER commit
cypress.env.json
**/cypress.env.json

# Environment files
.env
.env.local
.env.development
.env.production
.env.staging
**/.env
**/.env.local

# Additional appsettings environments
appsettings.Production.json
appsettings.Staging.json
*/appsettings.Production.json
*/appsettings.Staging.json
```

- [ ] Add missing entries above to `.gitignore`
- [ ] Verify no currently tracked files match those new patterns (`git ls-files | grep appsettings.Production`)

---

## 🟡 MEDIUM — `LhdsContextFactory.cs` — Localhost Connection String

**File:** `LHDS.Core\LhdsContextFactory.cs`  
**File:** `LHDS.ConfigImportExportTool\LhdsContextFactory.cs`

```csharp
value: "Server=(localdb)\\MSSQLLocalDB;Database=LondonDataServices;" +
    "Trusted_Connection=True;MultipleActiveResultSets=true"
```

This is a `localdb` connection string used for EF Core migrations design-time tooling only. It is **not a production secret**, but it does reveal the database name. A comment should be added confirming this is intentional.

- [ ] Add an explanatory comment to both `LhdsContextFactory.cs` files confirming this is design-time only and safe

---

## 🟢 LOW / INFORMATIONAL

### `authConfig.js` — Uses Environment Variables Correctly

**File:** `LHDS.AdminPortal.Web\ClientApp\src\authConfig.js`

```javascript
clientId: process.env.REACT_APP_CLIENTID || "",
authority: process.env.REACT_APP_AUTHORITY,
```

This correctly uses `process.env` — ✅ **safe as-is**. Ensure `.env` files remain gitignored and that the CI/CD pipeline injects these values at build time.

---

## 📋 Full Pre-Public Checklist

### Phase 1 — Immediate Security Remediation

- [ ] Rotate `AZURE_CLIENT_SECRET` in Azure Entra ID
- [ ] Change password for `cypressTestGPPremises@londonhds.nhs.uk`
- [ ] Revoke all active sessions for the Cypress test user
- [ ] Remove all hardcoded secrets from `cypress.config.ts`
- [ ] Create `cypress.env.json` (gitignored) for local developer use
- [ ] Create `cypress.env.json.example` (committed) as a template showing required keys with empty values

### Phase 2 — Git History Purge

Secrets committed to git remain in history even after a file is edited. The history must be rewritten.

```powershell
# Option A: git-filter-repo (recommended)
# Install: pip install git-filter-repo
git filter-repo --path LHDS.AdminPortal.Web/ClientApp/cypress.config.ts --invert-paths

# Then force-push all branches
git push origin --force --all
git push origin --force --tags
```

> ⚠️ **Coordinate with your team** — everyone must re-clone the repository after a history rewrite.  
> ⚠️ All open Pull Requests will need to be re-created.  
> ⚠️ GitHub support can also assist with removing cached views of the old content.

- [ ] Agree history rewrite date/time with all team members
- [ ] Perform history purge using `git filter-repo`
- [ ] Force-push all branches and tags
- [ ] All team members re-clone the repository
- [ ] Contact GitHub support to purge cached data if the repo was ever public or forks exist

### Phase 3 — `.gitignore` and Configuration

- [ ] Add all missing `.gitignore` entries listed above
- [ ] Create `cypress.env.json.example` template file with empty values
- [ ] Verify `appsettings.Development.json` is not tracked in any project (`git ls-files | grep Development`)
- [ ] Verify `local.settings.json` is not tracked in any Azure Functions project
- [ ] Verify no `.env` files are tracked (`git ls-files | grep "\.env"`)
- [ ] Ensure React app `.env` files are covered by `.gitignore`

### Phase 4 — CI/CD Pipeline

- [ ] Store all Cypress secrets as **GitHub Actions Secrets** (Settings → Secrets and variables → Actions)
- [ ] Store Azure credentials as GitHub Actions Secrets, not inline in workflow YAML
- [ ] Verify no pipeline YAML files (`.github/workflows/`) contain inline secrets or real GUIDs
- [ ] Configure Cypress CI to read from `CYPRESS_` prefixed environment variables
- [ ] Ensure build pipelines inject `REACT_APP_CLIENTID`, `REACT_APP_AUTHORITY` etc. at build time from secrets

### Phase 5 — GitHub Repository Security Settings

- [ ] Enable **Secret Scanning** (Settings → Security → Secret scanning → Enable)
- [ ] Enable **Push Protection** to block future secret commits (Settings → Security → Secret scanning → Push protection)
- [ ] Enable **Dependabot security updates** (already have `dependabot.yml` — verify alerts are enabled)
- [ ] Review and configure **Branch Protection Rules** for `main` (require PR reviews, status checks)
- [ ] Set appropriate **repository visibility** only after all above steps are complete

### Phase 6 — Code and Documentation Review

- [ ] Review all `TODO` and `FIXME` comments for sensitive information
- [ ] Check for any hardcoded internal URLs, server names, or IP addresses
- [ ] Verify `README.md` does not contain internal environment URLs or infrastructure details
- [ ] Add a `SECURITY.md` file describing the responsible disclosure process
- [ ] Confirm `License.txt` (AGPL v3) is the intended licence for public release — seek legal sign-off if required
- [ ] Add contributing guidelines (`CONTRIBUTING.md`) if external contributions are welcome
- [ ] Confirm all third-party licences used by NuGet packages and npm packages are compatible with AGPL v3

### Phase 7 — Data Protection / NHS-Specific

- [ ] Confirm no patient data or PII is present in any test fixtures, seed data, or migration files
- [ ] Confirm no ODS codes, NHS numbers, or practice identifiers used as real data in tests
- [ ] Ensure Data Protection Impact Assessment (DPIA) covers open-sourcing this codebase
- [ ] Confirm NHS/ICB information governance sign-off has been obtained for public release
- [ ] Review the `LHDS.Core.SeedGenerator` to ensure all generated seed data is synthetic

---

## 📁 Cypress Configuration — Recommended Fix

### `cypress.config.ts` (after fix — no secrets)

```typescript
import { defineConfig } from "cypress";

const randomChar = () => String.fromCharCode((Math.random() * 25) + 97);

export default defineConfig({
    // All secrets are loaded from cypress.env.json (gitignored) or CI environment variables.
    // See cypress.env.json.example for the list of required keys.

    e2e: {
        baseUrl: "https://localhost:44405/",
        hideXHRInCommandLog: true,
        setupNodeEvents(on, config) {
            // implement node event listeners here
        },
    },

    component: {
        devServer: {
            framework: "create-react-app",
            bundler: "webpack",
        },
    },

    testdata: {
        practiceName: Array(2).fill('').map(x => randomChar()).join('')
    },
});
```

### `cypress.env.json.example` (safe to commit — empty values only)

```json
{
  "AZURE_TENANT_ID": "",
  "AZURE_CLIENT_ID": "",
  "AZURE_CLIENT_SECRET": "",
  "USERNAME": "",
  "PASSWORD": "",
  "FAKE_REFRESH_SECRET": "",
  "FAKE_REFRESH_TOKEN": "",
  "FAKE_JWT": "",
  "SCOPE": "",
  "AUTH_ENVIRONMENT": "login.windows.net"
}
```

---

## Summary

| Severity | Issue | Status |
|---|---|---|
| 🔴 Critical | Live Azure client secret in `cypress.config.ts` | ❌ Not fixed |
| 🔴 Critical | Live NHS user password in `cypress.config.ts` | ❌ Not fixed |
| 🔴 Critical | Secrets present in git history | ❌ Not fixed |
| 🟠 High | `cypress.env.json` not in `.gitignore` | ❌ Not fixed |
| 🟠 High | `.env` files not globally gitignored | ❌ Not fixed |
| 🟠 High | `appsettings.Production.json` not gitignored | ❌ Not fixed |
| 🟡 Medium | Localhost connection string in `LhdsContextFactory.cs` | ℹ️ Low risk, document intent |
| 🟡 Medium | No `SECURITY.md` present | ❌ Not added |
| 🟡 Medium | IG/DPIA sign-off for open sourcing | ❓ Unknown |
| 🟢 Low | `authConfig.js` uses env vars correctly | ✅ OK |
| 🟢 Low | `dependabot.yml` configured | ✅ OK |
