using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement_Mvc.Data;
using SchoolManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagement_Mvc.Models;

namespace SchoolManagement_Mvc.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subject
        public IActionResult Index()
        {
            // Include Teacher data for display
            var subjects = _context.Subjects
                .Include(s => s.TeacherSubjects)
                .ThenInclude(ts => ts.Teacher) // Include the Teacher navigation property
                .ToList();

            return View(subjects); // Pass the list of subjects to the view
        }

        // GET: Subject/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Populate ViewBag.Grades for the dropdown
            ViewBag.Grades = _context.Grades
                .Select(g => new SelectListItem
                {
                    Value = g.GradeId.ToString(),
                    Text = g.GradeName
                }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add the subject to the database
                    _context.Subjects.Add(subject);
                    _context.SaveChanges();

                    TempData["success"] = "Subject created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Failed to create subject. Error: " + ex.Message;
                }
            }
            else
            {
                // Collect validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
            }

            // Repopulate ViewBag.Grades if the model is invalid
            ViewBag.Grades = _context.Grades
                .Select(g => new SelectListItem
                {
                    Value = g.GradeId.ToString(),
                    Text = g.GradeName
                }).ToList();

            return View(subject);
        }

        // GET: Subject/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subject subject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Update the subject
                    _context.Subjects.Update(subject);
                    _context.SaveChanges();

                    TempData["success"] = "Subject updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Failed to update subject. Error: " + ex.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
            }

            return View(subject);
        }

        // GET: Subject/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var subject = _context.Subjects
                .Include(s => s.TeacherSubjects) // Include related TeacherSubjects
                .FirstOrDefault(s => s.SubjectId == id);

            if (subject == null)
            {
                TempData["error"] = "Subject not found.";
                return RedirectToAction("Index");
            }

            try
            {
                // Remove all related TeacherSubject records
                _context.TeacherSubjects.RemoveRange(subject.TeacherSubjects);

                // Remove the subject
                _context.Subjects.Remove(subject);

                _context.SaveChanges();
                TempData["success"] = $"Subject '{subject.SubjectName}' deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Failed to delete subject. Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }





        // GET: Subject/AssignTeachers/{subjectId}
        [HttpGet]
        public IActionResult AssignTeachers(int? subjectId)
        {
            if (subjectId == null)
            {
                return NotFound();
            }

            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
            {
                return NotFound();
            }

            // Fetch all teachers
            var teachers = _context.Teachers.ToList();

            ViewBag.SubjectId = subjectId;
            ViewBag.Teachers = teachers;

            return View();
        }

        // POST: Subject/AssignTeachers/{subjectId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignTeachers(int subjectId, List<int> teacherIds)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
            {
                TempData["error"] = "Subject not found.";
                return RedirectToAction("Index");
            }

            // Assign selected teachers to the subject
            foreach (var teacherId in teacherIds)
            {
                var teacher = _context.Teachers.Find(teacherId);
                if (teacher != null)
                {
                    subject.TeacherSubjects.Add(new TeacherSubject
                    {
                        TeacherId = teacherId,
                        SubjectId = subjectId
                    });
                }
            }

            _context.SaveChanges();

            TempData["success"] = "Teachers assigned to subject successfully.";
            return RedirectToAction("Details", new { id = subjectId });
        }
    }
}