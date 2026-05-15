// ---
// skill: the-standard-csharp-files
// type: template
// source-section: "0. Files — 0.0 Naming, 0.1 Partial Class Files"
// ---

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

// File layout for a Standard-compliant service class split into partial files:
//
// {Entity}Service.cs                      ← constructor + interface implementation stubs
// {Entity}Service.Validations.cs          ← all ValidateXxx methods
// {Entity}Service.Validations.Add.cs      ← Add-specific validation helpers (if large)
// {Entity}Service.Validations.Modify.cs   ← Modify-specific validation helpers (if large)
// {Entity}Service.Exceptions.cs           ← TryCatch delegate + exception wrapping
//
// Each file name: PascalCase, dot-separated aspect, .cs extension.

// {Entity}Service.cs
public partial class {Entity}Service : I{Entity}Service
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;
    private readonly IDateTimeBroker dateTimeBroker;

    public {Entity}Service(
        IStorageBroker storageBroker,
        ILoggingBroker loggingBroker,
        IDateTimeBroker dateTimeBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
        this.dateTimeBroker = dateTimeBroker;
    }
}
