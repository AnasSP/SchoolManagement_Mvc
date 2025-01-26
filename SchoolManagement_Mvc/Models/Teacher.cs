using System;
using System.Collections.Generic;
using SchoolManagement_Mvc.Models;
using ServiceStack.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public DateTime TeacherDateOfBirth { get; set; }
        public DateTime TeacherRegistrationDate { get; set; }
        public string TeacherKeyId { get; set; }

        // Navigation property for Subjects taught by the teacher
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    }
}