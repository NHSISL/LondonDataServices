# Copilot Instructions

## Code Style Rules

### Line Length
- All `.cs` source files must adhere to the following rule:
  - No line of code should exceed **120 characters** in length.
  - This includes comments, string literals, and code.
  - Exception: automatically generated files may be ignored if they cannot be reformatted safely.
- **How to measure (raw file characters):**
  - Count based on **raw file characters**, not editor rendering.  
  - Tabs must always be converted to spaces (`indent_style = space`).  
  - Trailing whitespace must be removed.  

### Code Formatting
- Single-line instructions must follow each other with **no blank lines** in between.
- Multi-line instructions must always be preceded by **exactly one blank line**.
- If a multi-line instruction is followed by further instructions, it must also be followed by **exactly one blank line**.
- Any C# `return` statement must be preceded by **exactly one blank line** if it comes after other instructions.
- If a constructor/method name would push a line past **120 columns**, move `new`, the **method call**, or the **arguments** to the **next line**.
- Always format so that **no single physical line exceeds 120 characters**, even when calls span multiple lines.
- **Definition of a blank line**: a line must contain **no characters at all** (no spaces, no tabs). A line with only whitespace does **not** count as a blank line.
- **Method separation**: Method declarations (single-line or multi-line) must be preceded by **exactly one blank line** after the closing brace `}` of the previous member.

### Enforcement
- Copilot should **not generate or suggest code** that exceeds the 120-character line limit (as measured above).
- When writing new C# code, Copilot should:
  - Break up long method/constructor calls across multiple lines.
  - Use string interpolation or verbatim strings with proper line breaks if a literal would otherwise exceed 120 characters.
  - Format long LINQ queries across multiple lines.
  - Suggest wrapping parameters and arguments for readability.
  - Insert a blank line before any `return` statement that follows other instructions.
  - Prefer moving `new` (or method invocation) to the next line if the type or method name is long.

### Review Guidelines
- When reviewing or completing code suggestions, Copilot should:
  - Scan `.cs` files for lines longer than 120 characters based on **raw file characters** (tabs already converted to spaces, trailing whitespace removed).  
  - Only flag a violation if a **single physical line** exceeds 120 characters **under these rules**.
    Do **not** flag lines merely because a statement spans multiple lines or because of on-screen wrapping.
  - When flagging, include the **line number** and the **measured character count** (e.g., “Line 42: 128 chars”),
    and show the exact line being measured (truncated if needed).
  - Recommend a multiline formatting fix for flagged lines (e.g., move `new`, method call, or arguments).
  - Flag missing blank lines before `return` statements.
  - Flag **whitespace-only lines** as invalid blank lines.

### Examples

#### ✅ Correct (constructor moved to next line)
```csharp
createException: () =>
    new InvalidArgumentsDocumentProcessingException(
        message: "Invalid document processing arguments. Please correct the errors and try again."),
```

#### ❌ Incorrect (constructor call too long)
```csharp
createException: () => new InvalidArgumentsDocumentProcessingException(
    message: "Invalid document processing arguments. Please correct the errors and try again."),
```

---

#### ✅ Correct (method call moved to next line)
```csharp
var result = dataProcessor
    .ProcessLargeDataSet(
        source,
        destination,
        cancellationToken);
```

#### ❌ Incorrect (method call too long)
```csharp
var result = dataProcessor.ProcessLargeDataSet(source, destination, cancellationToken, additionalOption, extraConfiguration);
```

---

#### ✅ Correct (arguments wrapped to next line)
```csharp
var message = string.Format(
    "User {0} with ID {1} could not be found in the {2} repository.",
    user.Name,
    user.Id,
    repositoryName);
```

#### ❌ Incorrect (arguments crammed into one line)
```csharp
var message = string.Format("User {0} with ID {1} could not be found in the {2} repository.", user.Name, user.Id, repositoryName);
```

---

### Code Formatting Rule Examples

#### ✅ Correct (return with blank line)
```csharp
var user = users.FirstOrDefault(u => u.Id == id);

return user;
```

#### ❌ Incorrect (missing blank line before return)
```csharp
var user = users.FirstOrDefault(u => u.Id == id);
return user;
```

---

### More Formatting Examples

#### ✅ Correct
```csharp
var activeUsers = users.Where(u => u.IsActive == false).Select(u => new { u.Id, u.Name }).ToList();
var activeUsers = users.Where(u => u.IsActive).Select(u => new { u.Id, u.Name }).ToList();

var filteredUsers = users
    .Where(u => u.IsActive && u.LastLoginDate >= DateTime.UtcNow.AddDays(-30))
    .OrderByDescending(u => u.LastLoginDate)
    .Select(u => new
    {
        u.Id,
        u.Name,
        u.Email,
        LastSeen = u.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss")
    })
    .ToList();

var x = 1 + 2;
var y = 2 + 2;

return y;
```

#### ❌ Incorrect
```csharp
var activeUsers = users.Where(u => u.IsActive == false).Select(u => new { u.Id, u.Name }).ToList();
var activeUsers = users.Where(u => u.IsActive).Select(u => new { u.Id, u.Name }).ToList();
var filteredUsers = users
    .Where(u => u.IsActive && u.LastLoginDate >= DateTime.UtcNow.AddDays(-30))
    .OrderByDescending(u => u.LastLoginDate)
    .Select(u => new
    {
        u.Id,
        u.Name,
        u.Email,
        LastSeen = u.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss")
    })
    .ToList();
var x = 1 + 2;
var y = 2 + 2;
return y;
```

---

### Rationale

- **Line Length**: Matching the editor’s 120-column ruler ensures consistency between what developers see in their IDE and what the style guide enforces. This avoids confusion where tools count characters differently.  
- **Blank Lines**: A *blank line* is defined as truly empty (no spaces, no tabs). This prevents false positives from whitespace-only lines, which some linters misinterpret as valid blank lines.  
- **Method Separation**: Requiring exactly one blank line between member methods improves readability while avoiding inconsistent spacing.  
- **Return Statements**: Forcing a blank line before `return` makes return points visually stand out, helping readability and reducing missed returns in code reviews.  
- **Wrapping Long Calls**: Allowing `new`, method names, or arguments to be moved to the next line ensures long invocations can always be split without exceeding the 120-character limit.  
