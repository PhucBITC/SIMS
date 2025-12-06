using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using SIMS_Project.Interface;
using SIMS_Project.Models;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IUserRepository _userRepo; 

        public CourseController(ICourseRepository courseRepo, IUserRepository userRepo)
        {
            _courseRepo = courseRepo;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            var courses = _courseRepo.GetAllCourses();
            return View(courses);
        }

        public IActionResult Create()
        {
            ViewBag.Instructors = new SelectList(_userRepo.GetInstructors(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepo.AddCourse(course);
                return RedirectToAction("Index");
            }
            // Reload list if validation fails
            ViewBag.Instructors = new SelectList(_userRepo.GetInstructors(), "Id", "FullName");
            return View(course);
        }

        public IActionResult Edit(int id)
        {
            var c = _courseRepo.GetById(id);
            if (c == null) return NotFound();

            ViewBag.Instructors = new SelectList(_userRepo.GetInstructors(), "Id", "FullName", c.InstructorId);
            return View(c);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                // Preserve CreatedAt if it's lost, or the Repo should handle it.
                // ideally, fetch old -> update fields -> save.
                var old = _courseRepo.GetById(course.Id);
                if (old != null) course.CreatedAt = old.CreatedAt;

                _courseRepo.UpdateCourse(course);
                return RedirectToAction("Index");
            }
            ViewBag.Instructors = new SelectList(_userRepo.GetInstructors(), "Id", "FullName", course.InstructorId);
            return View(course);
        }

        public IActionResult Delete(int id)
        {
            var c = _courseRepo.GetById(id);
            return c == null ? NotFound() : View(c);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _courseRepo.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}