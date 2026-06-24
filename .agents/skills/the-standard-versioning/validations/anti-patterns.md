# The Standard Versioning — Anti-Patterns

## Breaking Contract Change

**Violates:** ts-versioning-003
**What happens:** A broker method `InsertStudentAsync(Student student)` is renamed to `InsertStudentAsync(StudentDto studentDto)` without any migration path.
**Why it's wrong:** All consumers break immediately with no warning or transition period.
**Fix:** Add `InsertStudentAsync(StudentDto studentDto)` as a new method. Mark the old one `[Obsolete]`. Remove it only after all consumers have migrated. This is the SPAL strategy.

## Query-String Versioning

**Violates:** ts-versioning-004
**What happens:** The API is versioned as `GET /api/students?version=2` or via an `api-version` request header.
**Why it's wrong:** URL path versioning is the Standard's required mechanism. Query-string and header versioning make contracts less visible and harder to cache.
**Fix:** Use `GET /api/v2/students` with a `[Route("api/v2/[controller]")]` attribute.

## Missing Version

**Violates:** ts-versioning-001
**What happens:** A `.csproj` file has `<Version>0.0.0</Version>` or no `<Version>` element at all when the package is published.
**Why it's wrong:** Consumers cannot distinguish releases, and NuGet will not accept `0.0.0` as a meaningful version.
**Fix:** Set `<Version>0.1.0</Version>` for the first pre-release; `<Version>1.0.0</Version>` for the first stable release.
