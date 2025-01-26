namespace SchoolManagement.Models;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; } // Make Student nullable
    public int SessionId { get; set; }
    public Session? Session { get; set; } // Make Session nullable
    public DateTime AttendanceDate { get; set; }
    public bool IsPresent { get; set; }
}