---
skill: the-standard-team-projects
type: example
source-section: "4.1.2.1 Project Structure"
demonstrates: "tst-projects-007, tst-projects-008 — flat structure and missing Exceptions folder"
---

# ❌ Flat Project Structure — Anti-Pattern

The structure below violates tst-projects-007 (flat layout) and tst-projects-008 (missing Exceptions/).

```
# ❌ Flat structure — all files dumped in project root
Taarafo.Core/
  Post.cs
  PostNotFoundException.cs
  PostService.cs
  PostBroker.cs
  PostsController.cs
  Comment.cs
  CommentService.cs
```

```
# ❌ Partial hierarchy — Models present but missing Exceptions/ subfolder
Taarafo.Core/
  Models/
    Posts/
      Post.cs              ← no Exceptions/ subfolder
  Services/
    PostService.cs         ← not grouped by layer
  PostsController.cs       ← not inside Controllers/
```

```
# ✅ Correct hierarchy
Taarafo.Core/
  Brokers/
    Storages/
      StorageBroker.cs
  Models/
    Foundations/
      Posts/
        Post.cs
        Exceptions/        ← required
          PostNotFoundException.cs
  Services/
    Foundations/
      Posts/
        PostService.cs
  Controllers/
    PostsController.cs
```
