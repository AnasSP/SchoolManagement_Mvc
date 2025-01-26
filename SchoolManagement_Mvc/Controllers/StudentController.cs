using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement_Mvc.Data;

namespace SchoolManagement_Mvc.Controllers
{
    public class StudentController : Controller
{
    private readonly ApplicationDbContext _db;

    public StudentController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: Student
    public IActionResult Index()
    {
        // Handle NULL values in StudentFirstName
        var students = _db.Students
            .OrderBy(s => s.StudentFirstName ?? string.Empty) // Use ?? to handle NULL values
            .ToList();

        return View(students);
    }

    [HttpGet]
    public IActionResult Create()
    {
        // Populate ViewBag.Grades and ViewBag.Sessions for dropdowns
        ViewBag.Grades = _db.Grades
            .Select(g => new SelectListItem
            {
                Value = g.GradeId.ToString(),
                Text = g.GradeName
            }).ToList();

        ViewBag.Sessions = _db.Sessions
            .Select(s => new SelectListItem
            {
                Value = s.SessionId.ToString(),
                Text = s.SessionName
            }).ToList();
        
        ViewBag.Classes = _db.Classes
            .Select(c => new SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.ClassName
            }).ToList();

        return View();
    }
    

    [HttpPost]
    public IActionResult Create(Student student)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Add the student to the database
                _db.Students.Add(student);
                _db.SaveChanges();

                TempData["success"] = "Student created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to create student. Error: " + ex.Message;
            }
        }
        else
        {
            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
        }

        // Repopulate dropdowns if the model is invalid
        ViewBag.Grades = _db.Grades
            .Select(g => new SelectListItem
            {
                Value = g.GradeId.ToString(),
                Text = g.GradeName
            }).ToList();

        ViewBag.Sessions = _db.Sessions
            .Select(s => new SelectListItem
            {
                Value = s.SessionId.ToString(),
                Text = s.SessionName
            }).ToList();
        
        ViewBag.Classes = _db.Classes
            .Select(c => new SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.ClassName
            }).ToList();

        return View(student);
    }




    // GET: Student/Edit/5
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["error"] = "Student ID is required.";
            return RedirectToAction("Index");
        }

        // Fetch the student from the database
        var student = _db.Students.Find(id);
        if (student == null)
        {
            TempData["error"] = "Student not found.";
            return RedirectToAction("Index");
        }

        // Populate dropdowns (if needed)
        ViewBag.Grades = _db.Grades
            .Select(g => new SelectListItem
            {
                Value = g.GradeId.ToString(),
                Text = g.GradeName,
                Selected = g.GradeId == student.GradeId
            }).ToList();
        
        ViewBag.Classes = _db.Classes
            .Select(c => new SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.ClassName,
                Selected = c.ClassId == student.ClassId
            }).ToList();

        return View(student);
    }
    //edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Student student)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Fetch the existing student from the database
                var existingStudent = _db.Students.Find(student.StudentId);
                if (existingStudent == null)
                {
                    TempData["error"] = "Student not found.";
                    return RedirectToAction("Index");
                }

                // Update the existing student's properties
                existingStudent.StudentFirstName = student.StudentFirstName;
                existingStudent.StudentLastName = student.StudentLastName;
                existingStudent.StudentDateOfBirth = student.StudentDateOfBirth;
                existingStudent.StudentRegistrationDate = student.StudentRegistrationDate;
                existingStudent.StudentKeyId = student.StudentKeyId;
                existingStudent.GradeId = student.GradeId;
                existingStudent.ClassId = student.ClassId;

                // Save changes to the database
                _db.SaveChanges();

                TempData["success"] = "Student updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to update student. Error: " + ex.Message;
            }
        }
        else
        {
            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
        }

        // Repopulate dropdowns if the model is invalid
        ViewBag.Grades = _db.Grades
            .Select(g => new SelectListItem
            {
                Value = g.GradeId.ToString(),
                Text = g.GradeName,
                Selected = g.GradeId == student.GradeId
            }).ToList();
        
        ViewBag.Classes = _db.Classes
            .Select(c => new SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.ClassName,
                Selected = c.ClassId == student.ClassId
            }).ToList();

        return View(student);
    }
    
    
    

    // post: Student/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var student = _db.Students.Find(id);
        if (student == null)
        {
            TempData["error"] = "Student not found.";
            return RedirectToAction("Index");
        }

        try
        {
            _db.Students.Remove(student);
            _db.SaveChanges();
            TempData["success"] = $"Student {student.StudentFirstName} deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Failed to delete student. Error: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
    
    //Details
    public IActionResult Details(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        // Fetch the student with related data (including Grade and Session)
        var student = _db.Students
            .Include(s => s.YearlySessions)
            .ThenInclude(e => e.Session)
            .Include(s => s.YearlySessions)
            .ThenInclude(e => e.Grade)
            .Include(s => s.Grade) // Include the Grade navigation property
            .Include(s => s.Class) // Include the Class navigation property
            .Include(s => s.Session) // Include the Session navigation property
            .FirstOrDefault(s => s.StudentId == id);

        if (student == null)
        {
            return NotFound();
        }

        // Load attendances for the student
        var attendances = _db.Attendances
            .Where(a => a.StudentId == id)
            .Include(a => a.Session)
            .ToList();

        // Load grades for the student with subject information
        var grades = _db.Enrolls
            .Where(e => e.StudentId == id)
            .Join(
                _db.Subjects, // Join with Subjects table
                enroll => enroll.SubjectId, // Enroll.SubjectId
                subject => subject.SubjectId, // Subject.SubjectId
                (enroll, subject) => new EnrollWithSubjectViewModel // Project into the view model
                {
                    Enroll = enroll,
                    Subject = subject
                }
            )
            .ToList();

        // Pass data to the view
        ViewBag.Attendances = attendances ?? new List<Attendance>(); // Ensure it's never null
        ViewBag.Grades = grades ?? new List<EnrollWithSubjectViewModel>(); // Ensure it's never null

        return View(student);
    }
    
    // GET: Student/TrackAttendance
    [HttpGet]
    public IActionResult TrackAttendance(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var student = _db.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }

        // Create a new Attendance object with the current date, StudentId, and SessionId
        var attendance = new Attendance
        {
            StudentId = student.StudentId,
            SessionId = 1, // Replace with the actual SessionId (e.g., from a dropdown or database)
            AttendanceDate = DateTime.Now
        };

        return View(attendance);
    }
    
    // POST: Student/TrackAttendance
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult TrackAttendance(Attendance attendance)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Ensure StudentId is valid
                if (attendance.StudentId == 0)
                {
                    TempData["error"] = "Invalid Student.";
                    return View(attendance);
                }

                // Add the attendance record to the database
                _db.Attendances.Add(attendance);
                _db.SaveChanges();

                TempData["success"] = "Attendance tracked successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to track attendance. Error: " + ex.Message;
            }
        }
        else
        {
            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
        }

        // If the model is invalid, return to the same view with the model
        return View(attendance);
    }
    
    
    //addgrade
    [HttpGet]
    public IActionResult AddGrade(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        // Fetch the student from the database, including their Grade
        var student = _db.Students
            .Include(s => s.Grade) // Include the Grade navigation property
            .FirstOrDefault(s => s.StudentId == id);

        if (student == null)
        {
            return NotFound();
        }

        // Filter subjects by the student's GradeId
        var subjects = _db.Subjects
            .Where(s => s.GradeId == student.GradeId) // Only show subjects with the same GradeId as the student
            .ToList();

        // Populate ViewBag with filtered Subjects for dropdown
        ViewBag.Subjects = subjects
            .Select(s => new SelectListItem
            {
                Value = s.SubjectId.ToString(),
                Text = s.SubjectName
            }).ToList();

        // Create a new Enroll object with the StudentId
        var enroll = new Enroll
        {
            StudentId = student.StudentId
        };

        return View(enroll);
    }
    
    [HttpPost][HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddGrade(Enroll enroll)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Ensure StudentId and SubjectId are valid
                if (enroll.StudentId == 0 || enroll.SubjectId == 0)
                {
                    TempData["error"] = "Invalid Student or Subject.";
                    return View(enroll);
                }

                // Ensure GradeValue is between 0 and 20
                if (enroll.GradeValue < 0 || enroll.GradeValue > 20)
                {
                    TempData["error"] = "Grade must be between 0 and 20.";
                    return View(enroll);
                }

                // Add the enroll record to the database
                _db.Enrolls.Add(enroll);
                _db.SaveChanges();

                TempData["success"] = "Grade added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the inner exception for debugging
                var innerException = ex.InnerException?.Message;
                TempData["error"] = $"Failed to add grade. Error: {ex.Message} Inner Exception: {innerException}";
            }
        }
        else
        {
            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
        }

        // If the model is invalid, repopulate the dropdown and return the view
        var student = _db.Students
            .Include(s => s.Grade)
            .FirstOrDefault(s => s.StudentId == enroll.StudentId);

        if (student != null)
        {
            var subjects = _db.Subjects
                .Where(s => s.GradeId == student.GradeId)
                .ToList();

            ViewBag.Subjects = subjects
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.SubjectName
                }).ToList();
        }

        return View(enroll);
    }
    
}
}