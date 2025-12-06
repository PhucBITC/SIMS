using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Admin")] // STRICTLY ADMIN
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Admin manages the structural existence of courses
        // This links to the CourseController we built earlier
        public IActionResult ManageCourses()
        {
            return RedirectToAction("Index", "Course");
        }

        // Admin manages Users (Creating Instructors/Students)
        public IActionResult ManageUsers()
        {
            return RedirectToAction("Register", "User");
        }
    }
}