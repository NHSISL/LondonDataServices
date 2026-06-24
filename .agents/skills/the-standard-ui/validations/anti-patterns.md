# The Standard UI — Anti-Patterns

## Logic in Razor

**Violates:** ts-ui-001
**What happens:** Business logic, service calls, and state management are written directly inside `@code { }` blocks in a `.razor` file.
**Why it's wrong:** Mixing markup and logic makes the file untestable and violates separation of concerns.
**Fix:** Move all C# logic to a `StudentComponentBase : ComponentBase` class file. The `.razor` file inherits from it with `@inherits StudentComponentBase`.

## Component Broker Access

**Violates:** ts-ui-004
**What happens:** `[Inject] private IStorageBroker StorageBroker { get; set; }` appears in a component base class.
**Why it's wrong:** Components must be technology-agnostic. All data access must go through a service interface.
**Fix:** Inject `IStudentViewService` or equivalent service; remove broker dependency.

## Mixed Page and Component

**Violates:** ts-ui-003
**What happens:** A single `StudentPage.razor` renders the page chrome, a student form, and a student list all in one file.
**Why it's wrong:** Each UI concern must be a separate component for reusability and testability.
**Fix:** Extract `StudentFormComponent.razor` and `StudentListComponent.razor`; compose them in `StudentPage.razor`.
