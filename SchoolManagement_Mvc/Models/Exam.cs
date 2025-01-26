namespace SchoolManagement.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}