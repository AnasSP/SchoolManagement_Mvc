namespace SchoolManagement.Models;

public class SubjectGrade
{
    public int SubjectGradeId { get; set; }
    public int? GradeId { get; set; }
    public Grade? Grade { get; set; }
    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }
}