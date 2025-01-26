using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement_Mvc.Data;
using SchoolManagement_Mvc.Models;

namespace SchoolManagement_Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Populate ViewBag with data for the dashboard
        ViewBag.TotalStudents = _context.Students.Count();
        ViewBag.TotalTeachers = _context.Teachers.Count();
        ViewBag.TotalClasses = _context.Classes.Count();
        ViewBag.TotalSubjects = _context.Subjects.Count();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}