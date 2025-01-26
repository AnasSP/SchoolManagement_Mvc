namespace SchoolManagement.Models;

public class TeacherSession
{
    public int TeacherSessionId { get; set; }
    public int? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    public int? SessionId { get; set; }
    public Session? Session { get; set; }
}