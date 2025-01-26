using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagement_Mvc.Models;

namespace SchoolManagement.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        
        public int? GradeId { get; set; } // Nullable if the relationship is optional
        public Grade? Grade { get; set; } // Navigation property for Grade

        // Navigation property for the join table
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    }
}