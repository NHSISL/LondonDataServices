// ---
// skill: the-standard-csharp-methods
// type: template
// source-section: "4. Methods — structure, spacing, parameter breaking"
// ---

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

public async ValueTask<{Entity}> Add{Entity}Async({Entity} {entity})
{
    // validate
    Validate{Entity}OnAdd({entity});

    // act
    {Entity} persisted{Entity} =
        await this.storageBroker.Insert{Entity}Async({entity});

    // ✅ blank line before return
    return persisted{Entity};
}

public async ValueTask<{Entity}> Retrieve{Entity}ByIdAsync(Guid {entity}Id)
{
    // validate
    Validate{Entity}Id({entity}Id);

    // act
    {Entity} maybe{Entity} =
        await this.storageBroker.Select{Entity}ByIdAsync({entity}Id);

    // validate
    ValidateStorage{Entity}(maybe{Entity}, {entity}Id);

    return maybe{Entity};
}
