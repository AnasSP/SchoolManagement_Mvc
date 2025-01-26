using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models;

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    
    public ICollection<AssignGrade> AssignGrades { get; set; } = new List<AssignGrade>();
    [NotMapped]
    public ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
    public ICollection<SubjectGrade> SubjectGrades { get; set; }
    
}