using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models;

public class Enroll
{
    public int EnrollId { get; set; }
    public int StudentId { get; set; }
    [NotMapped]
    public Student? Student { get; set; }
    public int? SessionId { get; set; }
    [NotMapped]
    public Session? Session { get; set; }
    
    public int SubjectId { get; set; }
    [NotMapped]
    public Subject? Subject { get; set; }

    public int? GradeId { get; set; }
    // [NotMapped]
    public double GradeValue { get; set; } // Add GradeValue (0 to 20)

    public Grade? Grade { get; set; }
    
}