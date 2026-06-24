---
skill: the-standard-team-projects
type: example
source-section: "4.1.2.1 Project Structure"
demonstrates: "tst-projects-003, tst-projects-004, tst-projects-005, tst-projects-006 — correct API and test project folder structure"
---

# ✅ Standard Project Folder Structure

## API Project — Taarafo.Core

```
Taarafo.Core/
  Brokers/
    DateTimes/
      IDateTimeBroker.cs
      DateTimeBroker.cs
    Loggings/
      ILoggingBroker.cs
      LoggingBroker.cs
    Storages/
      IStorageBroker.cs
      StorageBroker.cs
      StorageBroker.Posts.cs
      StorageBroker.Comments.cs
  Migrations/
  Models/
    Foundations/
      Posts/
        Post.cs
        Exceptions/
          PostNotFoundException.cs
          PostValidationException.cs
          PostDependencyException.cs
          PostServiceException.cs
      Comments/
        Comment.cs
        Exceptions/
          CommentNotFoundException.cs
          CommentValidationException.cs
          CommentDependencyException.cs
          CommentServiceException.cs
  Services/
    Foundations/
      Posts/
        IPostService.cs
        PostService.cs
        PostService.Validations.cs
        PostService.Exceptions.cs
    Processings/
      Posts/
        IPostProcessingService.cs
        PostProcessingService.cs
  Controllers/
    PostsController.cs
    CommentsController.cs
```

## Test Project — Taarafo.Core.Tests.Acceptance

```
Taarafo.Core.Tests.Acceptance/
  Services/
    Foundations/
      Posts/
        PostServiceTests.cs
    Processings/
      Posts/
        PostProcessingServiceTests.cs
```
