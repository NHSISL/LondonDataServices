// ---
// skill: the-standard-csharp-classes
// type: template
// source-section: "2. Classes and Interfaces — 4.0 Naming"
// ---

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

// I{Entity}Service.cs — service interface
public interface I{Entity}Service
{
    ValueTask<{Entity}> Add{Entity}Async({Entity} {entity});
    ValueTask<IQueryable<{Entity}>> RetrieveAll{Entity}s();
    ValueTask<{Entity}> Retrieve{Entity}ByIdAsync(Guid {entity}Id);
    ValueTask<{Entity}> Modify{Entity}Async({Entity} {entity});
    ValueTask<{Entity}> Remove{Entity}ByIdAsync(Guid {entity}Id);
}
