namespace SchoolManagement.Models;

public class Session
{
    public int SessionId { get; set; }
    public string SessionName { get; set; }
    // public string SessionEnd { get; set; }
    
    public ICollection<Enroll> Enrolls { get; set; }
    public ICollection<TeacherSession> TeacherSessions { get; set; }
}