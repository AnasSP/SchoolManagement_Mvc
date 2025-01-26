using Microsoft.EntityFrameworkCore;
using SchoolManagement_Mvc.Models;
using SchoolManagement.Models;

namespace SchoolManagement_Mvc.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet properties for each entity
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Enroll> Enrolls { get; set; }
    public DbSet<AssignGrade> AssignGrades { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectGrade> SubjectGrades { get; set; }
    public DbSet<TeacherSession> TeacherSessions { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Class> Classes { get; set; } // Add this line for the Class model
    public DbSet<Exam> Exams { get; set; } // Add this line for the Exam model
    public DbSet<TeacherSubject> TeacherSubjects { get; set; } // Add this line


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships and constraints

        // Student - Enroll (One-to-Many)
        modelBuilder.Entity<Student>()
            .HasMany(s => s.YearlySessions)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Grade - Enroll (One-to-Many)
        modelBuilder.Entity<Grade>()
            .HasMany(g => g.Enrolls)
            .WithOne(e => e.Grade)
            .HasForeignKey(e => e.GradeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Session - Enroll (One-to-Many)
        modelBuilder.Entity<Session>()
            .HasMany(s => s.Enrolls)
            .WithOne(e => e.Session)
            .HasForeignKey(e => e.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Grade - AssignGrade (One-to-Many)
        modelBuilder.Entity<Grade>()
            .HasMany(g => g.AssignGrades)
            .WithOne(ag => ag.Grade)
            .HasForeignKey(ag => ag.GradeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Teacher - AssignGrade (One-to-Many)
        // modelBuilder.Entity<Teacher>()
        //     .HasMany(t => t.AssignGrades)
        //     .WithOne(ag => ag.Teacher)
        //     .HasForeignKey(ag => ag.TeacherId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // Grade - SubjectGrade (One-to-Many)
        modelBuilder.Entity<Grade>()
            .HasMany(g => g.SubjectGrades)
            .WithOne(sg => sg.Grade)
            .HasForeignKey(sg => sg.GradeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Subject - SubjectGrade (One-to-Many)
        // modelBuilder.Entity<Subject>()
        //     .HasMany(s => s.SubjectGrades)
        //     .WithOne(sg => sg.Subject)
        //     .HasForeignKey(sg => sg.SubjectId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // Teacher - TeacherSession (One-to-Many)
        // modelBuilder.Entity<Teacher>()
        //     .HasMany(t => t.TeacherSessions)
        //     .WithOne(ts => ts.Teacher)
        //     .HasForeignKey(ts => ts.TeacherId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // Session - TeacherSession (One-to-Many)
        modelBuilder.Entity<Session>()
            .HasMany(s => s.TeacherSessions)
            .WithOne(ts => ts.Session)
            .HasForeignKey(ts => ts.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Attendance - Student (One-to-Many)
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Attendance - Session (One-to-Many)
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Session)
            .WithMany()
            .HasForeignKey(a => a.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Class - Grade (One-to-Many)
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Grade)
            .WithMany()
            .HasForeignKey(c => c.GradeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Class - Session (One-to-Many)
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Session)
            .WithMany()
            .HasForeignKey(c => c.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraints
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.StudentKeyId)
            .IsUnique();

        modelBuilder.Entity<Teacher>()
            .HasIndex(t => t.TeacherKeyId)
            .IsUnique();
        
        // modelBuilder.Entity<Subject>()
        //     .HasOne(s => s.Teacher)
        //     .WithOne(t => t.Subject)
        //     .HasForeignKey<Subject>(s => s.TeacherId) // Subject is the dependent side
        //     .OnDelete(DeleteBehavior.Restrict); // Optional: Configure delete behavior
        //
        // modelBuilder.Entity<Teacher>()
        //     .HasOne(t => t.Subject)
        //     .WithOne(s => s.Teacher)
        //     .HasForeignKey<Teacher>(t => t.SubjectId) // Teacher is the dependent side
        //     .OnDelete(DeleteBehavior.Restrict); // Optional: Configure delete behavior
        
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.GradeId)
            .OnDelete(DeleteBehavior.Restrict); // Use Restrict or NoAction
        
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Class)
            .WithMany()
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<TeacherSubject>()
            .HasKey(ts => new { ts.TeacherId, ts.SubjectId }); // Composite primary key

        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.Teacher)
            .WithMany(t => t.TeacherSubjects)
            .HasForeignKey(ts => ts.TeacherId);

        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.Subject)
            .WithMany(s => s.TeacherSubjects)
            .HasForeignKey(ts => ts.SubjectId);
        
        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.Subject)
            .WithMany(s => s.TeacherSubjects)
            .HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        // Configure cascade delete for other related entities if needed
        // Example: Enrolls
        modelBuilder.Entity<Enroll>()
            .HasOne(e => e.Subject)
            .WithMany()
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete
        
        
        modelBuilder.Entity<TeacherSubject>()
            .HasKey(ts => new { ts.TeacherId, ts.SubjectId }); // Composite primary key

        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.Teacher)
            .WithMany(t => t.TeacherSubjects)
            .HasForeignKey(ts => ts.TeacherId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Teacher

        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.Subject)
            .WithMany(s => s.TeacherSubjects)
            .HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Subject
        
    }
    
    
}