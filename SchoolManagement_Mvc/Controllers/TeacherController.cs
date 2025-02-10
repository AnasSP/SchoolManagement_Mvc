using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement_Mvc.Data;
using SchoolManagement_Mvc.Models;

namespace SchoolManagement_Mvc.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Users> _userManager;


        public TeacherController(ApplicationDbContext db, UserManager<Users> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: Teacher
        public IActionResult Index()
        {
            var teachers = _db.Teachers.OrderBy(t => t.TeacherFirstName).ToList();
            return View(teachers);
        }

        // GET: Teacher/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Hash the password before saving it to the database
                    teacher.Password = BCrypt.Net.BCrypt.HashPassword(teacher.Password);

                    _db.Teachers.Add(teacher);
                    _db.SaveChanges();
                    TempData["success"] = "Teacher created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Failed to create teacher. Error: " + ex.Message;
                }
            }

            return View(teacher);
        }

        // GET: Teacher/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var teacher = _db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teacher/Edit/5
        [HttpPost]
        public IActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _db.Teachers.Update(teacher);
                _db.SaveChanges();
                TempData["success"] = "Teacher updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Failed to update teacher.";
            return View(teacher);
        }

        // GET: Teacher/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var teacher = _db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var teacher = _db.Teachers.Find(id);
            if (teacher == null)
            {
                TempData["error"] = "Teacher not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _db.Teachers.Remove(teacher);
                _db.SaveChanges();
                TempData["success"] = $"Teacher {teacher.TeacherFirstName} deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Failed to delete teacher. Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // GET: Teacher/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Fetch the teacher and include their assigned subjects
            var teacher = _db.Teachers
                .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject) // Include the Subject navigation property
                .FirstOrDefault(t => t.TeacherId == id);

            if (teacher == null)
            {
                return NotFound();
            }

            // Pass the teacher and their assigned subjects to the view
            return View(teacher);
        }

        // GET: Teacher/AssignSubject/{teacherId}
        [HttpGet]
        public IActionResult AssignSubject(int? teacherId)
        {
            if (teacherId == null)
            {
                return NotFound();
            }

            // Fetch the teacher
            var teacher = _db.Teachers.Find(teacherId);
            if (teacher == null)
            {
                return NotFound();
            }

            // Fetch all unassigned subjects (subjects not assigned to this teacher)
            var unassignedSubjects = _db.Subjects
                .Where(s => !s.TeacherSubjects.Any(ts => ts.TeacherId == teacherId))
                .ToList();

            // Pass the teacher and unassigned subjects to the view
            ViewBag.TeacherId = teacherId;
            ViewBag.UnassignedSubjects = unassignedSubjects;

            return View();
        }

        // POST: Teacher/AssignSubject/{teacherId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignSubject(int teacherId, int subjectId)
        {
            // Fetch the teacher and subject from the database
            var teacher = _db.Teachers.Find(teacherId);
            var subject = _db.Subjects.Find(subjectId);

            if (teacher == null || subject == null)
            {
                TempData["error"] = "Teacher or Subject not found.";
                return RedirectToAction("Index");
            }

            // Check if the subject is already assigned to the teacher
            var existingAssignment = _db.TeacherSubjects
                .FirstOrDefault(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);

            if (existingAssignment != null)
            {
                TempData["error"] = $"Subject '{subject.SubjectName}' is already assigned to teacher '{teacher.TeacherFirstName} {teacher.TeacherLastName}'.";
                return RedirectToAction("Details", new { id = teacherId });
            }

            // Assign the subject to the teacher using the join table
            var teacherSubject = new TeacherSubject
            {
                TeacherId = teacherId,
                SubjectId = subjectId
            };

            _db.TeacherSubjects.Add(teacherSubject);
            _db.SaveChanges();

            // Display a success message
            TempData["success"] = $"Subject '{subject.SubjectName}' assigned to teacher '{teacher.TeacherFirstName} {teacher.TeacherLastName}' successfully.";

            // Redirect to the teacher's details page
            return RedirectToAction("Details", new { id = teacherId });
        }
        
        [HttpGet]
        public async Task<IActionResult> TeacherDashboard(int id)
        {
            // Fetch the teacher associated with the given TeacherId
            var teacher = await _db.Teachers
                .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
                .FirstOrDefaultAsync(t => t.TeacherId == id);

            if (teacher == null)
            {
                TempData["error"] = "Teacher not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(teacher);
        }
    }
}