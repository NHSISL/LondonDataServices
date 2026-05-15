# The Standard Versioning — Checklist

- [ ] ts-versioning-001: Package version follows MAJOR.MINOR.PATCH format.
- [ ] ts-versioning-002: Version number incremented correctly for the type of change (breaking/feature/fix).
- [ ] ts-versioning-003: Broker contract breaking changes use SPAL strategy (old method kept alongside new until migration complete).
- [ ] ts-versioning-004: API versioned via URL path (`/api/v1/...`), not query string or header.
- [ ] ts-versioning-005: CHANGELOG or release notes updated for this release.
