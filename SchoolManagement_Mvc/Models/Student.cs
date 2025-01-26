using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Student
    {
        public Student()
        {
            YearlySessions = new List<Enroll>(); // Initialize the collection
        }

        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public DateTime StudentDateOfBirth { get; set; }
        public DateTime StudentRegistrationDate { get; set; }
        public bool Selected { get; set; }

        [Unique]
        public string StudentKeyId { get; set; }

        // Foreign key for Class
        public int? GradeId { get; set; } // Nullable if the relationship is optional
        public Grade? Grade { get; set; } // Navigation property for Grade

        public int? ClassId { get; set; } // Nullable if the relationship is optional
        public Class? Class { get; set; } // Navigation property for Class
        public ICollection<Enroll> YearlySessions { get; set; } // No [Required] attribute
        
        public int? SessionId { get; set; } // Nullable
        public Session? Session { get; set; } // Navigation property
    }

}