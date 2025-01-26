using SchoolManagement.Models;

namespace SchoolManagement_Mvc.Models;

public class TeacherSubject
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
}