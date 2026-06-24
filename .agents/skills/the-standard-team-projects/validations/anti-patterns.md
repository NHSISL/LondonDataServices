---
skill: the-standard-team-projects
type: anti-patterns
source-section: "4.1.2 Projects"
---

# The Standard Team — Projects — Anti-Patterns

---

## Flat Project Structure

**Violates:** tst-projects-007

**What happens:** Source files are placed directly in the project root or in a single flat `src/` folder without the required hierarchy.

**Why it's wrong:** A flat structure makes navigation, discovery, and onboarding inconsistent across Standard-compliant codebases. Reviewers and tools that expect the standard hierarchy will not find files in their expected locations.

**Fix:** Organise all files into `Brokers/`, `Models/`, `Services/`, and `Controllers/` with the appropriate sub-folder structure.

```
# ❌ Flat structure
Taarafo.Core/
  Student.cs
  StudentService.cs
  StudentBroker.cs
  StudentController.cs

# ✅ Standard structure
Taarafo.Core/
  Brokers/
    Storages/
      StorageBroker.Students.cs
  Models/
    Foundations/
      Students/
        Student.cs
        Exceptions/
          StudentNotFoundException.cs
  Services/
    Foundations/
      Students/
        StudentService.cs
  Controllers/
    StudentsController.cs
```

---

## Missing Exceptions Folder

**Violates:** tst-projects-008

**What happens:** An entity folder under `Models/[Layer]/[Entity]/` is created without an `Exceptions/` subfolder.

**Why it's wrong:** Exception models are a required part of every entity's model contract. Without the subfolder, exception classes end up in the wrong location, breaking discoverability and Standard compliance.

**Fix:** Add an `Exceptions/` subfolder to every entity folder under `Models/`.

```
# ❌ Missing Exceptions/
Models/
  Foundations/
    Students/
      Student.cs

# ✅ Correct structure
Models/
  Foundations/
    Students/
      Student.cs
      Exceptions/
        StudentNotFoundException.cs
        StudentValidationException.cs
        StudentDependencyException.cs
        StudentServiceException.cs
```
