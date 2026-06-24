// ---
// skill: the-standard-exposers
// type: example
// source-section: "3.1.1 RESTful APIs"
// demonstrates: "ts-exposers-001, ts-exposers-006 — singular name, business logic in controller"
// ---

// ❌ BAD: Singular controller name; business logic inside action body.

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase  // ❌ singular — must be StudentsController
{
    private readonly IStudentService studentService;
    private readonly IStorageBroker storageBroker; // ❌ broker injected directly

    [HttpPost]
    public async ValueTask<ActionResult<Student>> PostStudentAsync(Student student)
    {
        // ❌ business logic in controller — validation belongs in service
        if (student is null)
            return BadRequest("Student cannot be null.");

        if (student.Id == Guid.Empty)
            student.Id = Guid.NewGuid(); // ❌ mutation belongs in service

        // ❌ direct broker call — bypasses service layer entirely
        Student addedStudent = await this.storageBroker.InsertStudentAsync(student);

        return Ok(addedStudent); // ❌ should return 201 Created, not 200 OK
    }
}
