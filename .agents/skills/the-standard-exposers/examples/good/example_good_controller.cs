// ---
// skill: the-standard-exposers
// type: example
// source-section: "3.1.1 RESTful APIs"
// demonstrates: "ts-exposers-001, ts-exposers-002, ts-exposers-003, ts-exposers-004, ts-exposers-006"
// ---

// ✅ GOOD: Plural controller name, correct verb→service mapping, correct status codes, thin action body.

[ApiController]
[Route("api/[controller]")]
public class StudentsController : RESTFulController
{
    private readonly IStudentService studentService;

    public StudentsController(IStudentService studentService) =>
        this.studentService = studentService;

    [HttpPost]
    public async ValueTask<ActionResult<Student>> PostStudentAsync(Student student)
    {
        try
        {
            Student addedStudent = await this.studentService.AddStudentAsync(student);

            return Created(addedStudent);
        }
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(studentValidationException.InnerException);
        }
        catch (StudentDependencyValidationException studentDependencyValidationException)
            when (studentDependencyValidationException.InnerException is AlreadyExistsStudentException)
        {
            return Conflict(studentDependencyValidationException.InnerException);
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

    [HttpGet]
    public async ValueTask<ActionResult<IQueryable<Student>>> GetAllStudentsAsync()
    {
        try
        {
            IQueryable<Student> allStudents = await this.studentService.RetrieveAllStudentsAsync();

            return Ok(allStudents);
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
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(studentValidationException.InnerException);
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

    [HttpPut]
    public async ValueTask<ActionResult<Student>> PutStudentAsync(Student student)
    {
        try
        {
            Student modifiedStudent = await this.studentService.ModifyStudentAsync(student);

            return Ok(modifiedStudent);
        }
        catch (StudentValidationException studentValidationException)
            when (studentValidationException.InnerException is NotFoundStudentException)
        {
            return NotFound(studentValidationException.InnerException);
        }
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(studentValidationException.InnerException);
        }
        catch (StudentDependencyValidationException studentDependencyValidationException)
        {
            return BadRequest(studentDependencyValidationException.InnerException);
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

    [HttpDelete("{studentId}")]
    public async ValueTask<ActionResult<Student>> DeleteStudentByIdAsync(Guid studentId)
    {
        try
        {
            Student deletedStudent =
                await this.studentService.RemoveStudentByIdAsync(studentId);

            return Ok(deletedStudent);
        }
        catch (StudentValidationException studentValidationException)
            when (studentValidationException.InnerException is NotFoundStudentException)
        {
            return NotFound(studentValidationException.InnerException);
        }
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(studentValidationException.InnerException);
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
