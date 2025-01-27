using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement_Mvc.Data;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectListItem

namespace SchoolManagement_Mvc.Controllers
{
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClassController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Class
        public IActionResult Index()
        {
            // Include Grade and Session data for display
            var classes = _db.Classes
                .Include(c => c.Grade)
                .Include(c => c.Session)
                .ToList();

            return View(classes);
        }

        // GET: Class/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Populate ViewBag with Grades and Sessions for dropdowns
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
                    Text = $"{s.SessionName}"
                }).ToList();

            return View();
        }

        // POST: Class/Create
        [HttpPost]
        public IActionResult Create(Class classModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure collections are initialized (if not already done in the model)
                    classModel.Students = classModel.Students ?? new List<Student>();
                    classModel.Teachers = classModel.Teachers ?? new List<Teacher>();
                    classModel.Subjects = classModel.Subjects ?? new List<Subject>();

                    _db.Classes.Add(classModel);
                    _db.SaveChanges();
                    TempData["success"] = "Class created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine(ex.Message);
                    TempData["error"] = "Failed to create class. Error: " + ex.Message;
                }
            }
            else
            {
                // Collect validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["error"] = "Invalid model state. Errors: " + string.Join(", ", errors);
            }

            // Repopulate ViewBag for dropdowns
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
                    Text = $"{s.SessionName}"
                }).ToList();

            // Return to the Create view with the model and error message
            return View(classModel);
        }

        // GET: Class/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var classItem = _db.Classes.Find(id);
            if (classItem == null)
            {
                return NotFound();
            }

            // Populate ViewBag with Grades and Sessions for dropdowns
            ViewBag.Grades = _db.Grades
                .Select(g => new SelectListItem
                {
                    Value = g.GradeId.ToString(),
                    Text = g.GradeName,
                    Selected = g.GradeId == classItem.GradeId
                }).ToList();

            ViewBag.Sessions = _db.Sessions
                .Select(s => new SelectListItem
                {
                    Value = s.SessionId.ToString(),
                    Text = $"{s.SessionName}",
                    Selected = s.SessionId == classItem.SessionId
                }).ToList();

            return View(classItem);
        }

        [HttpPost]
        public IActionResult Edit(Class classItem)
        {
            if (ModelState.IsValid)
            {
                _db.Classes.Update(classItem);
                _db.SaveChanges();
                TempData["success"] = "Class updated successfully.";
                return RedirectToAction("Index");
            }

            // If the model is invalid, repopulate the dropdowns and return the view
            ViewBag.Grades = _db.Grades
                .Select(g => new SelectListItem
                {
                    Value = g.GradeId.ToString(),
                    Text = g.GradeName,
                    Selected = g.GradeId == classItem.GradeId
                }).ToList();

            ViewBag.Sessions = _db.Sessions
                .Select(s => new SelectListItem
                {
                    Value = s.SessionId.ToString(),
                    Text = $"{s.SessionName}",
                    Selected = s.SessionId == classItem.SessionId
                }).ToList();

            TempData["error"] = "Failed to update class.";
            return View(classItem);
        }

        // GET: Class/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var classItem = _db.Classes
                .Include(c => c.Students) // Include related Students
                .Include(c => c.Teachers) // Include related Teachers
                .Include(c => c.Subjects) // Include related Subjects
                .FirstOrDefault(c => c.ClassId == id);

            if (classItem == null)
            {
                TempData["error"] = "Class not found.";
                return RedirectToAction("Index");
            }

            try
            {
                // Remove related Students
                _db.Students.RemoveRange(classItem.Students);

                // Remove related Teachers
                _db.Teachers.RemoveRange(classItem.Teachers);

                // Remove related Subjects
                _db.Subjects.RemoveRange(classItem.Subjects);

                // Remove the class
                _db.Classes.Remove(classItem);

                _db.SaveChanges();
                TempData["success"] = $"Class {classItem.ClassName} deleted successfully.";
            }
            catch (Exception ex)
            {
                // Log the inner exception for debugging
                var innerException = ex.InnerException?.Message;
                TempData["error"] = $"Failed to delete class. Error: {ex.Message} Inner Exception: {innerException}";
            }

            return RedirectToAction("Index");
        }
        
        // GET: Class/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Fetch the class and include its Grade, Session, and Students
            var classItem = _db.Classes
                .Include(c => c.Grade) // Include the Grade navigation property
                .Include(c => c.Session) // Include the Session navigation property
                .Include(c => c.Students) // Include the Students navigation property
                .FirstOrDefault(c => c.ClassId == id);

            if (classItem == null)
            {
                return NotFound();
            }

            // Pass the class item to the view
            return View(classItem);
        }

    }
}