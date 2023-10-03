using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Students;

namespace LHDS.Core.Services.Foundations.Students
{
    public interface IStudentService
    {
        ValueTask<Student> AddStudentAsync(Student student);
    }
}