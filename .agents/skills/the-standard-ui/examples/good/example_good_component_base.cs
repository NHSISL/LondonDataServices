// ---
// skill: the-standard-ui
// type: example
// source-section: "3.2.1 Web Applications"
// demonstrates: "ts-ui-001, ts-ui-002, ts-ui-004 — ComponentBase with service dependency"
// ---

// ✅ GOOD: All logic in the base class; service injected via interface.

public class StudentComponentBase : ComponentBase
{
    [Inject]
    public IStudentViewService StudentViewService { get; set; }

    public List<StudentView> StudentViews { get; set; }
    public StudentView SelectedStudentView { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.StudentViews =
            await this.StudentViewService.RetrieveAllStudentViewsAsync();
    }

    public async Task SelectStudentAsync(StudentView studentView)
    {
        this.SelectedStudentView = studentView;
    }
}
