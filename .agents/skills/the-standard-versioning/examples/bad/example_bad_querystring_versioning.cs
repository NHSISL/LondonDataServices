// ---
// skill: the-standard-versioning
// type: example
// source-section: "3.1.1 RESTful APIs — Versioning"
// demonstrates: "ts-versioning-004 — query-string versioning instead of URL path versioning"
// ---

// ❌ BAD: API version communicated via query string instead of URL path.

[ApiController]
[Route("api/[controller]")]  // ❌ no version in the route
public class StudentsController : ControllerBase
{
    [HttpGet("{studentId}")]
    public async ValueTask<ActionResult<object>> GetStudentByIdAsync(
        Guid studentId,
        [FromQuery] int version = 1)  // ❌ version as query parameter
    {
        if (version == 1)
        {
            // return v1 response
        }
        else if (version == 2)
        {
            // return v2 response
        }

        return BadRequest("Unsupported version.");
    }
}
