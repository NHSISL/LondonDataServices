// ---
// skill: the-standard-csharp-classes
// type: template
// source-section: "2. Classes and Interfaces — 4.0 Naming, 4.1 Fields"
// ---

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

// {Entity}.cs — domain model
public class {Entity}
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}
