using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SchoolManagement.Models
{
    public class Class
    {
        public Class()
        {
            Students = new List<Student>(); // Initialize the collection
            Teachers = new List<Teacher>(); // Initialize the collection
            Subjects = new List<Subject>(); // Initialize the collection
        }

        public int ClassId { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Grade is required.")]
        public int GradeId { get; set; }

        [ValidateNever] // Exclude Grade from validation
        public Grade Grade { get; set; }

        [Required(ErrorMessage = "Session is required.")]
        public int SessionId { get; set; }

        [ValidateNever] // Exclude Session from validation
        public Session Session { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();// No [Required] attribute
        public ICollection<Teacher> Teachers { get; set; } // No [Required] attribute
        public ICollection<Subject> Subjects { get; set; } // No [Required] attribute

    }
}