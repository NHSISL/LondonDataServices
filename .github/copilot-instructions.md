# Copilot Instructions

## Code Style Rules

### Line Length
- All `.cs` source files must adhere to the following rule:
  - No line of code should exceed **120 characters** in length.
  - This includes comments, string literals, and code.
  - Exception: automatically generated files may be ignored if they cannot be reformatted safely.
- **How to measure (match the editor ruler):**
  - Enforce the limit **per physical line** (newline-delimited). **Do not** aggregate multi-line statements.
  - Measure **visual columns from column 1**, including leading indentation.
  - Treat tabs using the editor’s `tabSize` (default **4** columns). If unknown, assume 4.
  - Trim **trailing whitespace** before counting.
  - Ignore soft wrapping (on-screen wrapping that doesn’t insert a newline).

### Code Formatting
- Single-line instructions must follow each other with **no blank lines** in between.
- Multi-line instructions must always be preceded by **exactly one blank line**.
- If a multi-line instruction is followed by further instructions, it must also be followed by **exactly one blank line**.
- Any C# `return` statement must be preceded by **exactly one blank line** if it comes after other instructions.
- If a constructor/method name would push a line past **120 columns**, move `new`, the **method call**, or the **arguments** to the **next line**.
- Always format so that **no single physical line exceeds 120 characters**, even when calls span multiple lines.

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
  - Scan `.cs` files for lines longer than 120 characters **after**:
    1) trimming trailing whitespace, and
    2) including leading indentation, and
    3) treating tabs as `tabSize` (default 4).
  - Only flag a violation if a **single physical line** exceeds 120 characters **under these rules**.
    Do **not** flag lines merely because a statement spans multiple lines or because of on-screen wrapping.
  - When flagging, include the **line number** and the **measured character count** (e.g., “Line 42: 128 chars”),
    and show the exact line being measured (truncated if needed).
  - Recommend a multiline formatting fix for flagged lines (e.g., move `new`, method call, or arguments).
  - Flag missing blank lines before `return` statements.

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
    .ProcessLargeDataSet(source, destination, cancellationToken);
```

#### ❌ Incorrect (method call too long)
```csharp
var result = dataProcessor.ProcessLargeDataSet(source, destination, cancellationToken, additionalOption, extraConfiguration);
```

---

#### ✅ Correct (arguments wrapped to next line)
```csharp
var result = dataProcessor.ProcessLargeDataSet(
    source,
    destination,
    cancellationToken);
```

#### ❌ Incorrect (arguments crammed into one line)
```csharp
var result = dataProcessor.ProcessLargeDataSet(source, destination, cancellationToken, additionalOption, extraConfiguration);
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
