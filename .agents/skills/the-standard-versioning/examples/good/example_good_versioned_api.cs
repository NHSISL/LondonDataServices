// ---
// skill: the-standard-versioning
// type: example
// source-section: "1.9 Versioning, 3.1.1 RESTful APIs"
// demonstrates: "ts-versioning-004 — correct URL path versioning for API controllers"
// ---

// ✅ GOOD: API versioned via URL path; old version maintained alongside new version.

// V1 controller — kept for backward compatibility
[ApiController]
[Route("api/v1/[controller]")]
public class StudentsController : RESTFulController
{
    private readonly IStudentService studentService;

    public StudentsController(IStudentService studentService) =>
        this.studentService = studentService;

    [HttpGet("{studentId}")]
    public async ValueTask<ActionResult<Student>> GetStudentByIdAsync(Guid studentId)
    {
        try
        {
            Student student = await this.studentService.RetrieveStudentByIdAsync(studentId);

            return Ok(student);
        }
        catch (StudentValidationException studentValidationException)
            when (studentValidationException.InnerException is NotFoundStudentException)
        {
            return NotFound(studentValidationException.InnerException);
        }
        catch (StudentDependencyException studentDependencyException)
        {
            return InternalServerError(studentDependencyException);
        }
        catch (StudentServiceException studentServiceException)
        {
            return InternalServerError(studentServiceException);
        }
    }
}

// V2 controller — new version with extended model
[ApiController]
[Route("api/v2/[controller]")]
public class StudentsV2Controller : RESTFulController
{
    private readonly IStudentViewService studentViewService;

    public StudentsV2Controller(IStudentViewService studentViewService) =>
        this.studentViewService = studentViewService;

    [HttpGet("{studentId}")]
    public async ValueTask<ActionResult<StudentView>> GetStudentByIdAsync(Guid studentId)
    {
        try
        {
            StudentView studentView =
                await this.studentViewService.RetrieveStudentViewByIdAsync(studentId);

            return Ok(studentView);
        }
        catch (StudentViewValidationException studentViewValidationException)
            when (studentViewValidationException.InnerException is NotFoundStudentViewException)
        {
            return NotFound(studentViewValidationException.InnerException);
        }
        catch (StudentViewDependencyException studentViewDependencyException)
        {
            return InternalServerError(studentViewDependencyException);
        }
        catch (StudentViewServiceException studentViewServiceException)
        {
            return InternalServerError(studentViewServiceException);
        }
    }
}
