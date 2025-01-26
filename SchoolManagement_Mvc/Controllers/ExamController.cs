using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using SchoolManagement_Mvc.Data;

namespace SchoolManagement_Mvc.Controllers
{
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExamController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Exam
        public IActionResult Index()
        {
            var exams = _db.Exams.ToList();
            return View(exams);
        }

        // GET: Exam/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exam/Create
        [HttpPost]
        public IActionResult Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                _db.Exams.Add(exam);
                _db.SaveChanges();
                TempData["success"] = "Exam created successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Failed to create exam.";
            return View(exam);
        }

        // Other actions (Edit, Delete, Details) can be added as needed.
    }
}